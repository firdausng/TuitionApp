using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseClass> CourseClasses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Timeslot> Timeslots { get; set; }
        public DbSet<DailySchedule> DailySchedules { get; set; }
        public DbSet<CalendarSetting> CalendarSettings { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbSet<TEntity> Set<TEntity>(string name) where TEntity:class;
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
