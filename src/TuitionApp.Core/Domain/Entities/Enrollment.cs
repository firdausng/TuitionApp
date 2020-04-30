using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public string Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid SessionId { get; set; }
        public Session Session { get; set; }
    }
}
