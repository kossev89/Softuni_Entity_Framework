using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ExportDto
{
    public class TruckExportJsonDto
    {
        [JsonProperty("TruckRegistrationNumber")]
        public string? RegistrationNumber { get; set; }
        public string VinNumber { get; set; } = null!;
        public int? TankCapacity { get; set; }
        public int? CargoCapacity { get; set; }
        public string CategoryType { get; set; }
        public string MakeType { get; set; }
    }
}
