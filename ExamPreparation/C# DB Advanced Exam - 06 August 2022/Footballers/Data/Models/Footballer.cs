using Footballers.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Footballers.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Footballers.Data.Models
{
    public class Footballer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(GlobalConstants.footballerNameMax)]
        public string Name { get; set; }
        [Required]
        public DateTime ContractStartDate { get; set; }
        [Required]
        public DateTime ContractEndDate { get; set; }
        [Required]
        public PositionType PositionType { get; set; }
        [Required]
        public BestSkillType BestSkillType { get; set; }
        [ForeignKey(nameof(CoachId))]
        [Required]
        public int CoachId { get; set; }
        public Coach Coach { get; set; }
        public ICollection<TeamFootballer> TeamsFootballers { get; set; } = new HashSet<TeamFootballer>();

    }
}
