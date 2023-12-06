using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Footballers.Shared;

namespace Footballers.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(GlobalConstants.teamNameMax)]
        [RegularExpression(GlobalConstants.nameRegex)]
        public string Name { get; set; }
        [Required]
        [StringLength(GlobalConstants.teamNatMax)]
        public string Nationality { get; set; }
        [Required]
        public int Trophies { get; set; }
        public ICollection<TeamFootballer> TeamsFootballers { get; set; } = new HashSet<TeamFootballer>();
    }
}
