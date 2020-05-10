using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TuitionApp.Core.Features.Attendances;
using TuitionApp.Core.Domain.Entities;
using TuitionApp.Core.Features.Courses.ClassSubjects;
using TuitionApp.Core.Features.DailySchedules;
using TuitionApp.Core.Features.Locations.Classrooms;
using TuitionApp.Core.Features.Locations;
using TuitionApp.Core.Features.Subjects;
using System.Collections.Generic;
using TuitionApp.Core.Features.Instructors;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Courses.CourseClasses;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.DailySchedules.Timeslots;
using TuitionApp.Core.Features.Enrollments;
using TuitionApp.Core.Features.Students;

namespace TuitionApp.IntegrationTest.Attendances
{
    using static SliceFixture;
    public class UpdateAttendanceTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldUpdateAttendance()
        {
            var instructorDto = await CreateInstructorAsync();
            var studentDto = await CreateStudentAsync();
            var courseDto = await CreateCourseAsync();
            var locationDto = await CreateLocationWithInstructorAsync(instructorDto);

            var courseClassDto = await CreateCourseClassAsync(courseDto, locationDto);
            var subjectDto = await CreateSubjectAsync(instructorDto);
            var classSubjectDto = await CreateClassSubjectAsync(subjectDto, courseClassDto);

            var classroomDto = await CreateClassroomAsync(locationDto);
            var dailyScheduleDto = await CreateDailyScheduleAsync(classroomDto);
            var timeslot = await CreateTimeslotAsync(classSubjectDto, dailyScheduleDto);

            var enrollment = await CreateEnrollmentAsync(courseClassDto, studentDto);

            var enrollmentEntity = await ExecuteDbContextAsync(db => db.Enrollments
                .Include(e => e.Attendances)
                .ThenInclude(a => a.Timeslot)
                .Where(c => c.Id.Equals(enrollment.Id))
                .SingleOrDefaultAsync()
                );

            var attendanceEntity = enrollmentEntity.Attendances.SingleOrDefault(a => a.Timeslot.Id.Equals(timeslot.Id));
            var dto = await SendAsync(new UpdateAttendanceItemCommand()
            {
                Id = attendanceEntity.Id,
                AttendanceStatus = AttendanceStatus.Attend
            });

            var created = await ExecuteDbContextAsync(db => db.Attendances.Where(c => c.EnrollmentId.Equals(enrollment.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
        }
    }
}
