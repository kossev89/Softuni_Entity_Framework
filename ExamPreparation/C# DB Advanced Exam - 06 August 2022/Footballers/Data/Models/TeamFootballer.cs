using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class TeamFootballer
    {
        [ForeignKey(nameof(TeamId))]
        [Required]
        public int TeamId { get; set; }
        public Team Team { get; set; }
        [ForeignKey(nameof(FootballerId))]
        [Required]
        public int FootballerId { get; set; }
        public Footballer Footballer { get; set; }
    }
}
