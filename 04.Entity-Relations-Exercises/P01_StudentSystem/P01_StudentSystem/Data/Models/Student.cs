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
    public class Student
    {
        public Student()
        {
        }

        [Key]
        public int StudentId { get; set; }

        [Required]
        [StringLength (100)]
        public string Name  { get; set; } = null!;

        [StringLength(10-10)]
        public string? PhoneNumber { get; set; } = null!;

        [Required]
        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday  { get; set; }

        public ICollection<Homework> Homeworks { get; set; }
        public ICollection<StudentCourse> StudentsCourses { get; set; }
    }
}
