using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicines.DataProcessor.ImportDtos
{
    public class PatientImportDto
    {
        [JsonProperty("FullName")]
        [JsonRequired]
        [MinLength(5)]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;
        [JsonProperty("AgeGroup")]
        [Range(0, 2)]
        [JsonRequired]
        public int AgeGroup { get; set; }
        [JsonProperty("Gender")]
        [JsonRequired]
        [Range(0, 1)]
        public int Gender { get; set; }
        [JsonProperty("Medicines")]
        [JsonRequired]
        public int[]? Medicines { get; set; }
    }
}
