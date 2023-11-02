using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data
{
    public partial class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        { 
        }
        public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
            :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            mb.Entity<Student>()
                .Property(s => s.PhoneNumber)
                .IsUnicode(false);

            mb.Entity<Resource>()
                .Property(r => r.Url)
                .IsUnicode(false);

            mb.Entity<Homework>()
                .Property(h => h.Content)
                .IsUnicode(false);
        }

        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Resource> Resources { get; set; } = null!;
        public virtual DbSet<Homework> Homeworks { get; set; } = null!;
        public virtual DbSet<StudentCourse> StudentsCourses { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=DESKTOP-A4Q93F3\\SQLEXPRESS;Database=StudentSystem;Integrated Security=True; TrustServerCertificate=true");
//            }
//        }


    }
}
