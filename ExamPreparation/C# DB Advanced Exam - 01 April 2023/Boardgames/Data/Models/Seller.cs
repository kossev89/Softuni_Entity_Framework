using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data.Models
{
    public class Seller
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(30)]
        public string Address { get; set; } = null!;
        [Required]
        [StringLength(30)]
        public string Country { get; set; } = null!;
        [Required]
        [StringLength(30)]
        public string Website { get; set; } = null!;
        public ICollection<BoardgameSeller>? BoardgamesSellers { get; set; } = new HashSet<BoardgameSeller>();
    }
}
