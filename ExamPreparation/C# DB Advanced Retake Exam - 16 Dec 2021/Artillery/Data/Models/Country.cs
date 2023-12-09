using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artillery.Shared;

namespace Artillery.Data.Models
{
    public class Country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(GlobalConstants.countryNameMax)]
        public string CountryName { get; set; }
        [Required]
        public int ArmySize { get; set; }
        public ICollection<CountryGun> CountriesGuns { get; set; } = new HashSet<CountryGun>();
    }
}
