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
        public DbSet<CourseClass> CourseClasses { get; set; }
        public DbSet<ClassSubject> ClassSubjects { get; set; }
        public DbSet<SubjectAssignment> SubjectAssignments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Timeslot> Timeslots { get; set; }
        public DbSet<DailySchedule> DailySchedules { get; set; }
        public DbSet<CalendarSetting> CalendarSettings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // many to many mapping for location and instructor
            modelBuilder.Entity<LocationInstructor>()
                .HasKey(bc => new { bc.LocationId, bc.InstructorId });
            modelBuilder.Entity<LocationInstructor>()
                .HasOne(bc => bc.Location)
                .WithMany(b => b.LocationInstructors)
                .HasForeignKey(bc => bc.LocationId);
            modelBuilder.Entity<LocationInstructor>()
                .HasOne(bc => bc.Instructor)
                .WithMany(c => c.LocationInstructors)
                .HasForeignKey(bc => bc.InstructorId);

            // many to many mapping for course and subject
            modelBuilder.Entity<CourseSubject>()
                .HasKey(bc => new { bc.CourseId, bc.SubjectId });
            modelBuilder.Entity<CourseSubject>()
                .HasOne(bc => bc.Course)
                .WithMany(b => b.CourseSubjects)
                .HasForeignKey(bc => bc.CourseId);
            modelBuilder.Entity<CourseSubject>()
                .HasOne(bc => bc.Subject)
                .WithMany(c => c.CourseSubjects)
                .HasForeignKey(bc => bc.SubjectId);


            modelBuilder.Entity<Person>()
                .HasDiscriminator<string>("PersonRole");
        }
    }
}
