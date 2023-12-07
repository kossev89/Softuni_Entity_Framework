using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Footballers.Shared;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Footballer")]
    public class FootbalerImportXmlDto
    {
        [Required]
        [MinLength(GlobalConstants.footballerNameMin)]
        [MaxLength(GlobalConstants.footballerNameMax)]
        public  string Name { get; set; }
        [Required]
        public  string ContractStartDate { get; set; }
        [Required]
        public string ContractEndDate { get; set; }
        [Required]
        [Range(0,4)]
        public  int BestSkillType { get; set; }
        [Required]
        [Range(0, 3)]
        public  int PositionType { get; set; }
    }
}
