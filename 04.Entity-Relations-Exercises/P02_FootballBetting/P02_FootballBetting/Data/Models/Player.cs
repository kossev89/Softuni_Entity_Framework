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
            IsInjured = false;
        }

        [Key]
        public int PlayerId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public int SquadNumber { get; set; }

        [ForeignKey(nameof(TeamId)), Required]
        public int TeamId { get; set; }
        public Team Team { get; set; }

        [ForeignKey(nameof(PositionId)), Required]
        public int PositionId { get; set; }
        public Position Position { get; set; }

        [Required]
        public bool IsInjured { get; set; }

    }
}
