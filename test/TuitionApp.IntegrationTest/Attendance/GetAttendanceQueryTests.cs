using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using Xunit;
using TuitionApp.Core.Features.CalendarSettings;
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
    public class GetAttendanceQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetAttendanceItem()
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
                .Where(c => c.Id.Equals(enrollment.Id))
                .SingleOrDefaultAsync()
                );

            var attendanceEntity = enrollmentEntity.Attendances.First();
            var query = new GetAttendanceItemQuery() { Id = attendanceEntity.Id };
            var dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Attendances.Where(c => c.EnrollmentId.Equals(enrollment.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
        }


        [Fact]
        public async Task ShouldGetAttendanceList()
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
                .Where(c => c.Id.Equals(enrollment.Id))
                .SingleOrDefaultAsync()
                );

            var attendanceEntity = enrollmentEntity.Attendances.First();

            var query = new GetAttendanceListQuery();
            var dto = await SendAsync(query);


            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(attendanceEntity.Id));
        }

        
        
        private async Task<CreateClassSubjectItemDto> CreateClassSubjectAsync(CreateCourseClassItemDto createCourseClassDto)
        {

            var instructorDto = await SendWithValidationAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first",
                LastName = "last",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            }, new CreateInstructorItemCommandValidator());

            var createSubjectItemCommand = new CreateSubjectItemCommand()
            {
                Title = "Subject1",
                InstructorList = new List<Guid> { instructorDto.Id },
            };
            var createSubjectItemDto = await SendWithValidationAsync(createSubjectItemCommand,
                new CreateSubjectItemCommandValidator());

            var getSubjectItemDto = await SendAsync(new GetSubjectItemQuery() { Id = createSubjectItemDto.Id });

            var createClassSubjectCommand = new CreateClassSubjectItemCommand()
            {
                Title = $"{createCourseClassDto.Id.ToString()}-subject1",
                CourseClassId = createCourseClassDto.Id,
                SubjectAssignmentId = getSubjectItemDto.SubjectAssignmentList.First(),
            };

            var createClassSubjectDto = await SendWithValidationAsync(createClassSubjectCommand, new CreateClassSubjectItemCommandValidator());
            return createClassSubjectDto;
        }
    }
}
