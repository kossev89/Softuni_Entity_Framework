using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trucks.Data.Models
{
    public class ClientTruck
    {
        [ForeignKey(nameof(ClientId))]
        [Required]
        public int ClientId { get; set; }
        public Client Client { get; set; }
        [ForeignKey(nameof(TruckId))]
        [Required]
        public int TruckId { get; set; }
        public Truck Truck { get; set; }
    }
}
