using System;
using System.Collections.Generic;
using System.Text;

namespace TuitionApp.Core.Domain.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public string Address { get; set; }
        public ICollection<Classroom> Classrooms { get; set; } = new List<Classroom>();
        public ICollection<LocationInstructor> LocationInstructors { get; set; } = new List<LocationInstructor>();
    }

    public abstract class Person: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }

        public string FullName()
        {
            return $"{FirstName} {LastName}";
        }
    }

    public class Classroom : BaseEntity
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public int Capacity { get; set; }
        public Guid LocationId { get; set; }
        public Location Location { get; set; }
        public ICollection<Timeslot> Timeslots { get; set; } = new List<Timeslot>();
    }

    public class Instructor : Person
    {
        public DateTime HireDate { get; set; }
        public ICollection<LocationInstructor> LocationInstructors { get; set; } = new List<LocationInstructor>();
        public ICollection<InstructorTimetable> InstructorTimetables { get; set; } = new List<InstructorTimetable>();
    }

    public class LocationInstructor
    {
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public Guid LocationId { get; set; }
        public Location Location { get; set; }
    }

    public class Timetable : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<InstructorTimetable> InstructorTimetables { get; set; } = new List<InstructorTimetable>();
    }

    public class Timeslot: BaseEntity
    {
        public int WeekNumber { get; set; }
        public int Day { get; set; }
        public TimeSpan Time { get; set; }
        public bool Disabled { get; set; } 
        public Guid ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
        public Guid TimetableId { get; set; }
        public Timetable Timetable { get; set; }
    }

    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public int Rate { get; set; }
        public ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
    }

    public class InstructorTimetable
    {
        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; }
        public Guid TimetableId { get; set; }
        public Timetable Timetable { get; set; }
    }

    public class Student: Person
    {
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }

    public class Enrollment : BaseEntity
    {
        public string Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid TimetableId { get; set; }
        public Timetable Timetable { get; set; }
    }
}
