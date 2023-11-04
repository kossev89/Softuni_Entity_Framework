using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        public Player()
        {

        }

        [Key]
        public int PlayerId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public int SquadNumber { get; set; }

        [ForeignKey(nameof(Team)), Required]
        public int TeamId { get; set; }
        public Team Team { get; set; }

        [ForeignKey(nameof(Position)), Required]
        public int PositionId { get; set; }
        public Position Position { get; set; }

        [Required]
        public bool IsInjured { get; set; }

        public ICollection<Game> PlayersStatistics { get; set; }

    }
}
