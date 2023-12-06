using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.DataProcessor.ExportDto
{
    public class ClientExportJsonDto
    {
        public string Name { get; set; }
        public List<TruckExportJsonDto> Trucks { get; set; } = new();
    }
}
