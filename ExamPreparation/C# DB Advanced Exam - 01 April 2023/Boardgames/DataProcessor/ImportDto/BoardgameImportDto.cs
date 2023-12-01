using Boardgames.Data.Models.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Boardgame")]
    public class BoardgameImportDto
    {
        [XmlElement("Name")]
        [MinLength(10)]
        [MaxLength(20)]
        [JsonRequired]
        public string Name { get; set; } = null!;
        [XmlElement("Rating")]
        [JsonRequired]
        [Range(1,10)]
        public double Rating { get; set; }
        [XmlElement("YearPublished")]
        [JsonRequired]
        [Range(2018,2023)]
        public int YearPublished { get; set; }
        [XmlElement("CategoryType")]
        [JsonRequired]
        [Range(0,4)]
        public int CategoryType { get; set; }
        [XmlElement("Mechanics")]
        [JsonRequired]
        [MinLength(1)]
        public string Mechanics { get; set; } = null!;
    }
}
