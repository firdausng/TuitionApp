using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuitionApp.Core.Domain.Entities
{
    public class Attendance : BaseEntity
    {
        public AttendanceStatus AttendanceStatus { get; set; }
        // note to fill if not attend
        public string Description { get; set; }
        public Timeslot Timeslot { get; set; }
        public Guid TimeslotId { get; set; }
        public Enrollment Enrollment { get; set; }
        public Guid EnrollmentId { get; set; }

    }

    public enum AttendanceStatus
    {
        Attend,
        NotAttend,
        Created
    }
}
