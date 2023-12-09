using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artillery.Shared;

namespace Artillery.Data.Models
{
    public class Manufacturer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(GlobalConstants.manufacturerNameMax)]
        public string ManufacturerName { get; set; }
        [Required]
        [StringLength(GlobalConstants.manufacturerFoundedMax)]
        public string Founded { get; set; }
        public ICollection<Gun> Guns { get; set; } = new HashSet<Gun>();
    }
}
