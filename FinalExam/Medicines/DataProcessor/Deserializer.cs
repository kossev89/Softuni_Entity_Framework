namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.Utilities;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder result = new();
            PatientImportDto[] importedPatients = JsonConvert.DeserializeObject<PatientImportDto[]>(jsonString);
            List<Patient> patientsToImport = new();
            var medIds = context.Medicines
                .Select(e => new
                {
                    e.Id
                })
                .ToArray();

            foreach (var dto in importedPatients)
            {
                if (!IsValid(dto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                Patient patient = new()
                {
                    FullName = dto.FullName,
                    AgeGroup = (AgeGroup)dto.AgeGroup,
                    Gender = (Gender)dto.Gender,
                    PatientsMedicines = new List<PatientMedicine>()
                };

                foreach (var id in dto.Medicines)
                {
                    if (!medIds.Any(x=>x.Id==id)
                        || patient.PatientsMedicines.Any(x => x.MedicineId == id)
                        )
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    PatientMedicine patientMedicine = new()
                    {
                        PatientId = patient.Id,
                        MedicineId = id
                    };

                    patient.PatientsMedicines.Add(patientMedicine);
                }
                patientsToImport.Add(patient);
                result.AppendLine(string.Format(SuccessfullyImportedPatient, patient.FullName, patient.PatientsMedicines.Count()));
            }
            context.Patients.AddRange(patientsToImport);
            context.SaveChanges();
            return result.ToString().Trim();
        }
    

    public static string ImportPharmacies(MedicinesContext context, string xmlString)
    {
        StringBuilder result = new();
        XmlHelper helper = new();
        string rootName = "Pharmacies";
        PharmacyImportDto[] importResult = helper.Decerialize<PharmacyImportDto[]>(xmlString, rootName);
        List<Pharmacy> pharmaciesToImport = new();

        foreach (var dto in importResult)
        {
            bool isNonStop;

            if (!IsValid(dto)
                || !bool.TryParse(dto.IsNonStop, out isNonStop)
                )
            {
                result.AppendLine(ErrorMessage);
                continue;
            }

            Pharmacy pharmacy = new()
            {
                IsNonStop = isNonStop,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Medicines = new List<Medicine>()
            };

            foreach (var medDto in dto.Medicines)
            {
                DateTime productionDate;
                DateTime expiryDate;
                string dateFormat = "yyyy-MM-dd";

                if (!IsValid(medDto)
                    || !DateTime.TryParseExact(medDto.ProductionDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out productionDate)
                    || !DateTime.TryParseExact(medDto.ExpiryDate, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out expiryDate)
                    || productionDate >= expiryDate
                    )
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                Medicine medicine = new()
                {
                    Category = (Category)medDto.Category,
                    Name = medDto.Name,
                    Price = medDto.Price,
                    ProductionDate = productionDate,
                    ExpiryDate = expiryDate,
                    Producer = medDto.Producer
                };

                if (pharmacy.Medicines.Any(x => x.Name == medicine.Name && x.Producer == medicine.Producer))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                pharmacy.Medicines.Add(medicine);
            }
            pharmaciesToImport.Add(pharmacy);
            result.AppendLine(string.Format(SuccessfullyImportedPharmacy, pharmacy.Name, pharmacy.Medicines.Count));
        }
        context.Pharmacies.AddRange(pharmaciesToImport);
        context.SaveChanges();
        return result.ToString().Trim();
    }

    private static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}
}
