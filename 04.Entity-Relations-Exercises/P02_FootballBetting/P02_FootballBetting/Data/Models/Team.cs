﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {

        }

        [Key]
        public int TeamId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(1000)]
        public string LogoUrl { get; set; } = null!;

        [StringLength(3, MinimumLength = 3)]
        public string Initials { get; set; } = null!;

        public decimal Budget { get; set; }

        [ForeignKey(nameof(PrimaryKitColor))]
        public int PrimaryKitColorId { get; set; }
        public virtual Color PrimaryKitColor { get; set; }

        [ForeignKey(nameof(SecondaryKitColor))]
        public int SecondaryKitColorId { get; set; }
        public virtual Color SecondaryKitColor { get; set; }

        [ForeignKey(nameof(Town))]
        public int TownId { get; set; }
        public Town Town { get; set; }

        [InverseProperty("HomeTeam")]
        public ICollection<Game> HomeGames { get; set; }
        [InverseProperty("AwayTeam")]
        public ICollection<Game> AwayGames { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
