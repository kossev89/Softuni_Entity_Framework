using Boardgames.Data.Models.Enums;
using Boardgames.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Boardgame")]
    public class BoardgameExportDto
    {
        [XmlElement("BoardgameName")]
        public string Name { get; set; }
        [XmlElement("BoardgameYearPublished")]
        public int YearPublished { get; set; }
    }
}
