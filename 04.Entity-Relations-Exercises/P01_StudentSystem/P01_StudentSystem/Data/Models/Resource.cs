using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public enum ResourceType
    {
        Video,
        Presentation,
        Document,
        Other 
    }
    public class Resource
    {
        [Key]
        public int RecourceID { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!;

        [Required]
        public ResourceType ResourceType { get; set; }

        [ForeignKey(nameof(CourseId))]
        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
