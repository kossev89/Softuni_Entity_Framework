using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Data.Models.Enums;

namespace Trucks.Data.Models
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }
        [StringLength(8)]
        [Required]
        public string RegistrationNumber { get; set; } = null!;
        [Required]
        [StringLength(17)]
        public string VinNumber { get; set; } = null!;
        public int? TankCapacity { get; set; }
        public int? CargoCapacity { get; set; }
        [Required]
        [JsonConverter(typeof(StringEnumConverter))]
        public CategoryType CategoryType { get; set; }
        [Required]
        public MakeType MakeType { get; set; }
        [ForeignKey(nameof(DespatcherId))]
        [Required]
        public int DespatcherId { get; set; }
        public Despatcher Despatcher { get; set; }
        public ICollection<ClientTruck> ClientsTrucks { get; set; } = new List<ClientTruck>();
    }
}
