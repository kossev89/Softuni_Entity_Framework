using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Footballers.Data.Models;
using Footballers.Shared;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class CoachImportXmlDto
    {
        [Required]
        [MinLength(GlobalConstants.coachNameMin)]
        [MaxLength(GlobalConstants.coachNameMax)]
        public string Name { get; set; }
        [Required]
        public string Nationality { get; set; }
        [XmlArray("Footballers")]
        public FootbalerImportXmlDto[] Footballers { get; set; }
    }
}
