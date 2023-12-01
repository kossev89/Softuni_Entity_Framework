using Boardgames.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ImportDto
{
    public class SellerImportDto
    {
        [JsonProperty("Name")]
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Name { get; set; } = null!;
        [JsonProperty("Address")]
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Address { get; set; } = null!;
        [JsonProperty("Country")]
        [MinLength(2)]
        [Required]
        public string Country { get; set; } 
        [JsonProperty("Website")]
        [Required]
        [RegularExpression(@"^www\.[A-Za-z0-9-]{1,}\.com$")]
        public string Website { get; set; }
        [JsonProperty("Boardgames")]
        public HashSet<int>? Boardgames { get; set; }
    }
}
