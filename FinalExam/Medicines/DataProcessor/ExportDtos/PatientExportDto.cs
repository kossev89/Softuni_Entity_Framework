using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class PatientExportDto
    {
        [XmlElement("Name")]
        public string Name { get; set; }
        [XmlElement("AgeGroup")]
        public AgeGroup AgeGroup { get; set; }
        [XmlArray("Medicines")]
        public List<MedicineExportDto> Medicines { get; set; } = new();
        [XmlAttribute("Gender")]
        public string Gender { get; set; }
    }
}
