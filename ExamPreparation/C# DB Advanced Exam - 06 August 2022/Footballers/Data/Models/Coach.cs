using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Footballers.Shared;

namespace Footballers.Data.Models
{
    public class Coach
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(GlobalConstants.coachNameMax)]
        public string Name { get; set; }
        [Required]
        public string Nationality { get; set; }
        public ICollection<Footballer> Footballers { get; set; } = new HashSet<Footballer>();
    }
}
