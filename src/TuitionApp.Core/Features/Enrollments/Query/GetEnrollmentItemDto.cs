﻿using System;

namespace TuitionApp.Core.Features.Enrollments
{
    public class GetEnrollmentItemDto
    {
        public Guid Id { get; set; }
        public string Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseClassIdId { get; set; }
    }
}
