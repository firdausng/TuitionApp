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
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Timeslot> Timeslots { get; set; }
        public DbSet<WeeklySchedule> WeeklySchedules { get; set; }
        public DbSet<CalendarSetting> CalendarSettings { get; set; }

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

            // many to many mapping for Session and Instructor
            modelBuilder.Entity<InstructorSession>()
                .HasKey(bc => new { bc.InstructorId, bc.SessionId });
            modelBuilder.Entity<InstructorSession>()
                .HasOne(bc => bc.Instructor)
                .WithMany(b => b.InstructorSessions)
                .HasForeignKey(bc => bc.SessionId);
            modelBuilder.Entity<InstructorSession>()
                .HasOne(bc => bc.Session)
                .WithMany(c => c.InstructorSessions)
                .HasForeignKey(bc => bc.InstructorId);


            modelBuilder.Entity<Person>()
                .HasDiscriminator<string>("PersonRole");
        }
    }
}
