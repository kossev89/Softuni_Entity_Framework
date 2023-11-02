using System;
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
        [Key]
        public int TeamId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required, StringLength(1000)]
        public string LogoUrl { get; set; } = null!;

        [Required, StringLength(3, MinimumLength = 3)]
        public string Initials { get; set; } = null!;

        [Required]
        public decimal Budget { get; set; }

        [ForeignKey(nameof(PrimaryKitColorId)), Required]    
        public int PrimaryKitColorId { get; set; }
        public Color PrimaryColor { get; set; }

        [ForeignKey(nameof(SecondaryKitColorId)), Required]
        public int SecondaryKitColorId { get; set; }
        public Color SecondaryColor { get; set; }

        [ForeignKey(nameof(TownId)), Required]
        public int TownId { get; set; }
        public Town Town { get; set; }
    }
}
