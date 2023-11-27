using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType("Client")]
    public class ClientImportDto
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength (25)]
        [MinLength(10)]
        public string Name { get; set; } = null!;
        [XmlElement("NumberVat")]
        [Required]
        [MaxLength(15)]
        [MinLength(10)]
        public string NumberVat { get; set; } = null!;
        [XmlArray ("Addresses")]
        public AddressImportDto[] Addresses { get; set; }
    }
}
