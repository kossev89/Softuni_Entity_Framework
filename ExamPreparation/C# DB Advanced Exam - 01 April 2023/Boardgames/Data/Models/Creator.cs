using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data.Models
{
    public class Creator
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(7)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(7)]
        public string LastName { get; set; }
        public ICollection<Boardgame>? Boardgames { get; set; } = new HashSet<Boardgame>();
    }
}
