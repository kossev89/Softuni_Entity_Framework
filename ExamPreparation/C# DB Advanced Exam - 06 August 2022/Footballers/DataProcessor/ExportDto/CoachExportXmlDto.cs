using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Coach")]
    public class CoachExportXmlDto
    {
        public string CoachName { get; set; }
        public List<FootbalerExportXmlDto> Footballers { get; set; } = new();
        [XmlAttribute("FootballersCount")]
        public int FootballersCount { get; set; }
    }
}
