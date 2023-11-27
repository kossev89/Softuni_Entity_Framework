using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(20, MinimumLength = 10)]
        public string? StreetName { get; set; }
        [Required]
        public int? StreetNumber { get; set; }
        [Required]
        public string? PostCode { get; set; }
        [Required, StringLength(15, MinimumLength = 5)]
        public string? City { get; set; }
        [Required, StringLength(15, MinimumLength = 5)]
        public string? Country { get; set; }
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId)), Required]
        public Client Client { get; set; }
    }
}
