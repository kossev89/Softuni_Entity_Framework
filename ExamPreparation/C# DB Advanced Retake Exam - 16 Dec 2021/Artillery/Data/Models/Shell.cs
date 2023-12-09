using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Artillery.Shared;
{

}

namespace Artillery.Data.Models
{
    public class Shell
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double ShellWeight { get; set; }
        [Required]
        [StringLength(GlobalConstants.shellCaliberMax)]
        public string Caliber { get; set; }
        public ICollection<Gun> Guns { get; set; } = new HashSet<Gun>();
    }
}
