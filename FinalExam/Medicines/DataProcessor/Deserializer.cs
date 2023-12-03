namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        //public static string ImportPatients(MedicinesContext context, string jsonString)
        //{
        //    throw new NotImplementedException();
        //}

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
