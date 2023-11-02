using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Town
    {
        [Key]
        public int TownId { get; set; }

        [StringLength(100), Required]
        public string Name { get; set; } = null!;

        [ForeignKey(nameof(CountryId)), Required]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
