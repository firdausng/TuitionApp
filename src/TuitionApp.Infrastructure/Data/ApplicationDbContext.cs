using Microsoft.EntityFrameworkCore;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // many to many mapping for test location and teacher
            modelBuilder.Entity<LocationTeacher>()
                .HasKey(bc => new { bc.LocationId, bc.TeacherId });
            modelBuilder.Entity<LocationTeacher>()
                .HasOne(bc => bc.Location)
                .WithMany(b => b.LocationTeachers)
                .HasForeignKey(bc => bc.TeacherId);
            modelBuilder.Entity<LocationTeacher>()
                .HasOne(bc => bc.Teacher)
                .WithMany(c => c.LocationTeachers)
                .HasForeignKey(bc => bc.LocationId);

            // many to many mapping for test subject and teacher
            modelBuilder.Entity<TeacherSubject>()
                .HasKey(bc => new { bc.SubjectId, bc.TeacherId });
            modelBuilder.Entity<TeacherSubject>()
                .HasOne(bc => bc.Teacher)
                .WithMany(b => b.TeacherSubjects)
                .HasForeignKey(bc => bc.SubjectId);
            modelBuilder.Entity<TeacherSubject>()
                .HasOne(bc => bc.Subject)
                .WithMany(c => c.TeacherSubjects)
                .HasForeignKey(bc => bc.TeacherId);
        }
    }
}
