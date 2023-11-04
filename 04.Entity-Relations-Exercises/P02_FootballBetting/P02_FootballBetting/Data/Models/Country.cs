using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Country
    {
        public Country()
        {

        }

        [Key]
        public int CountryId { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; } = null!;
        public ICollection<Town> Towns { get; set; }
    }
}
