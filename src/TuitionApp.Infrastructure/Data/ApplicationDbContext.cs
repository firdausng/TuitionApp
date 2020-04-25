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
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Timetable> Timetables { get; set; }
        public DbSet<Timeslot> Timeslots { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // many to many mapping for location and instructor
            modelBuilder.Entity<LocationInstructor>()
                .HasKey(bc => new { bc.LocationId, bc.InstructorId });
            modelBuilder.Entity<LocationInstructor>()
                .HasOne(bc => bc.Location)
                .WithMany(b => b.LocationInstructors)
                .HasForeignKey(bc => bc.InstructorId);
            modelBuilder.Entity<LocationInstructor>()
                .HasOne(bc => bc.Instructor)
                .WithMany(c => c.LocationInstructors)
                .HasForeignKey(bc => bc.LocationId);

            // many to many mapping for Timetable and Instructor
            modelBuilder.Entity<InstructorTimetable>()
                .HasKey(bc => new { bc.InstructorId, bc.TimetableId });
            modelBuilder.Entity<InstructorTimetable>()
                .HasOne(bc => bc.Instructor)
                .WithMany(b => b.InstructorTimetables)
                .HasForeignKey(bc => bc.TimetableId);
            modelBuilder.Entity<InstructorTimetable>()
                .HasOne(bc => bc.Timetable)
                .WithMany(c => c.InstructorTimetables)
                .HasForeignKey(bc => bc.InstructorId);
        }
    }
}
