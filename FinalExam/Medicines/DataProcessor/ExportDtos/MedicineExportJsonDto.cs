using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ExportDtos
{
    public class MedicineExportJsonDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("Price")]
        public string Price { get; set; }
        [JsonProperty("Pharmacy")]
        public PharmacyExportJsonDto Pharmacy { get; set; }
    }
}
