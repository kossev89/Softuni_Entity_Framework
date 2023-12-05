using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models.Enums;
using Trucks.Data.Models;
using System.Xml.Serialization;
using AutoMapper.Configuration.Annotations;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Truck")]
    public class TruckImportDto
    {
        [XmlElement("RegistrationNumber")]
        [MaxLength(8)]
        [MinLength(8)]
        [RegularExpression(@"^[A-Z]{2}\d{4}[A-Z]{2}$")]
        public string? RegistrationNumber { get; set; }
        [XmlElement("VinNumber")]
        [MaxLength(17)]
        [MinLength(17)]
        [Required]
        public string VinNumber { get; set; } = null!;
        [XmlElement("TankCapacity")]
        [Range(950,1420)]
        public int? TankCapacity { get; set; }
        [XmlElement("CargoCapacity")]
        [Range(5000, 29000)]
        public int? CargoCapacity { get; set; }
        [XmlElement("CategoryType")]
        [Range(0,3)]
        [Required]
        public int CategoryType { get; set; }
        [XmlElement("MakeType")]
        [Range(0, 4)]
        [Required]
        public int MakeType { get; set; }
    }
}
