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
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
