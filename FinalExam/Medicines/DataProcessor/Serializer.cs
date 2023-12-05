namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Medicines.Utilities;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System.Globalization;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            string dateFormat = "yyyy-MM-dd";
            DateTime productionDate = DateTime.ParseExact(date, dateFormat, CultureInfo.InvariantCulture);
            List<PatientExportDto> patients = new();

            var patientsToExport = context.Patients
                .Where(x => x.PatientsMedicines.Any(m => m.Medicine.ProductionDate > productionDate))
                .Select(p => new
                {
                    Name = p.FullName,
                    p.AgeGroup,
                    p.Gender,
                    Medicines = p.PatientsMedicines
                    .Where(x => x.Medicine.ProductionDate > productionDate)
                    .Select(e => new
                    {
                        Name = e.Medicine.Name,
                        e.Medicine.Price,
                        e.Medicine.Category,
                        e.Medicine.Producer,
                        e.Medicine.ExpiryDate
                    })
                    .OrderByDescending(e => e.ExpiryDate)
                    .ThenBy(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(c => c.Medicines.Count())
                .ThenBy(n => n.Name)
                .ToArray();

            foreach (var patient in patientsToExport)
            {
                PatientExportDto patientDto = new()
                {
                    Name = patient.Name,
                    AgeGroup = patient.AgeGroup,
                    Gender = patient.Gender.ToString().ToLower()
                };

                foreach (var medicine in patient.Medicines)
                {
                    MedicineExportDto medicineDto = new()
                    {
                        Name = medicine.Name,
                        Price = medicine.Price,
                        Producer = medicine.Producer,
                        ExpiryDate = medicine.ExpiryDate.ToString("yyyy-MM-dd"),
                        Category = medicine.Category.ToString().ToLower()
                    };
                    patientDto.Medicines.Add(medicineDto);
                }
                patients.Add(patientDto);
            }

            string rootName = "Patients";
            string result = XmlSerializationHelper.Serialize(patients, rootName, false);
            return result;
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            Category category = (Category)medicineCategory;
            List<MedicineExportJsonDto> medicines = new();

            var medicinesToExport = context.Medicines
                 .Where(c => c.Category == category && c.Pharmacy.IsNonStop == true)
                 .Select(e => new
                 {
                     Name = e.Name,
                     Price = e.Price,
                     Pharmacy = new
                     {
                         e.Pharmacy.Name,
                         e.Pharmacy.PhoneNumber
                     }
                 })
                 .OrderBy(p => p.Price)
                 .ThenBy(n => n.Name)
                .ToArray();

            foreach (var medicine in medicinesToExport)
            {
                MedicineExportJsonDto medicineExport = new()
                {
                    Name = medicine.Name,
                    Price = medicine.Price.ToString("f2"),
                    Pharmacy = new()
                    {
                        Name = medicine.Pharmacy.Name,
                        PhoneNumber = medicine.Pharmacy.PhoneNumber
                    }
                };
                medicines.Add(medicineExport);
            }


            string result = JsonConvert.SerializeObject(medicines, Formatting.Indented);
            return result;
        }
    }
}
