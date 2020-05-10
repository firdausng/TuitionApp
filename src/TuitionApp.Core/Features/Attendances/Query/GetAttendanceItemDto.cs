using System;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.Attendances
{
    public class GetAttendanceItemDto
    {
        public Guid Id { get; set; }
        public Guid EnrollmentId { get; set; }
        public Guid TimeslotId { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }
    }
}
