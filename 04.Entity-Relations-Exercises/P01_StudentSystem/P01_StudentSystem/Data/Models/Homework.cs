﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public enum ContentType
    {
        Application,
        Pdf,
        Zip
    }
    public class Homework
    {
        public Homework()
        {

        }


        [Key]
        public int HomeworkId { get; set; } 

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public ContentType ContentType { get; set; } 

        [Required]
        public DateTime SubmissionTime { get; set; } 

        [Required]
        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
