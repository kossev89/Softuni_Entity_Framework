using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.DataProcessor.ExportDto
{
    public class TeamExportJsonDto
    {
        public string Name { get; set; }
        public List<FootballerExportJsonDto> Footballers { get; set; } = new();
    }
}
