using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class DespatcherExportXmlDto
    {
        public string DespatcherName { get; set; }
        public List<TruckExportXmlDto> Trucks { get; set; } = new();
        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }
    }
}
