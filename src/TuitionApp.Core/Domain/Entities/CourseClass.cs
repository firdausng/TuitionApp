﻿using System;
using System.Collections.Generic;

namespace TuitionApp.Core.Domain.Entities
{
    public class CourseClass : BaseEntity
    {
        public string Name { get; set; }
        public Course Course { get; set; }
        public Guid CourseId { get; set; }
        public ICollection<Timeslot> Timeslots { get; set; } = new List<Timeslot>();
    }
}
