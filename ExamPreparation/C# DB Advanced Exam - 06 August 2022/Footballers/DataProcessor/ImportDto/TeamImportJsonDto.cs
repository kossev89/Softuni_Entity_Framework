using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Footballers.Shared;

namespace Footballers.DataProcessor.ImportDto
{
    public class TeamImportJsonDto
    {
        [Required]
        [JsonProperty("Name")]
        [MinLength(GlobalConstants.teamNameMin)]
        [MaxLength(GlobalConstants.teamNameMax)]
        [RegularExpression(GlobalConstants.nameRegex)]
        public string Name { get; set; }
        [Required]
        [JsonProperty("Nationality")]
        [MinLength(GlobalConstants.teamNatMin)]
        [MaxLength(GlobalConstants.teamNatMax)]
        public string Nationality { get; set; }
        [Required]
        [JsonProperty("Trophies")]
        public int Trophies { get; set; }
        [JsonProperty("Footballers")]
        public HashSet<int> Footballers { get; set; } = new HashSet<int>();
    }
}
