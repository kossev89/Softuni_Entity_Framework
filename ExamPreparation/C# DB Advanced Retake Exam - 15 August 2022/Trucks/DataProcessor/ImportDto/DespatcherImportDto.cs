using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class DespatcherImportDto
    {
        [XmlElement("Name")]
        [MinLength(2)]
        [MaxLength(40)]
        [Required]
        public string Name { get; set; } = null!;
        [XmlElement("Position")]
        public string? Position { get; set; }
        [XmlArray("Trucks")]
        public TruckImportDto[] Trucks { get; set; }
    }
}
