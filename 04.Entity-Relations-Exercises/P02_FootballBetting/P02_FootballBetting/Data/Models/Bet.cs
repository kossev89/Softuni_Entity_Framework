using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Bet
    {
        public Bet()
        {

        }

        [Key]
        public int BetId { get; set; }
        [Required]
        public decimal Amount { get; set; } 
        [Required]
        public Result Prediction { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [ForeignKey(nameof(UserId)), Required]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey(nameof(GameId)), Required]
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
