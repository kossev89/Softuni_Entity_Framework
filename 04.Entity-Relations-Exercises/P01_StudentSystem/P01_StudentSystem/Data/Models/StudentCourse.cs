﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class StudentCourse
    {
        [Required]
        [ForeignKey(nameof(StudentId))]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        [Required]
        [ForeignKey (nameof(CourseId))]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
