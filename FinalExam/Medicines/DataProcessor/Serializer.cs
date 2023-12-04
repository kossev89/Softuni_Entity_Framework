namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using Medicines.Data.Models.Enums;
    using System.Diagnostics;
    using System.Xml.Linq;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {
            throw new NotImplementedException();
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            Category category = (Category)medicineCategory;

            var medicinesToExport = context.Medicines
                 .Where(c => c.Category == category && c.Pharmacy.IsNonStop == true)
                 .Select(e => new
                 {
                     Name = e.Name,
                     Price = e.Price,
                     Pharmacy = e.Pharmacy
                 })
                .ToArray();
            ;
            return "mo";
        }
    }
}
