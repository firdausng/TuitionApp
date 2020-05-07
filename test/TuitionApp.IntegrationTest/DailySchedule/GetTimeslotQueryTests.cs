using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.DailySchedules.Timeslots;
using TuitionApp.Core.Features.Locations;
using TuitionApp.Core.Features.DailySchedules;
using Xunit;
using TuitionApp.Core.Features.Locations.Classrooms;
using TuitionApp.Core.Features.Courses.ClassSubjects;
using TuitionApp.Core.Features.Subjects;
using TuitionApp.Core.Features.Courses.CourseClasses;
using TuitionApp.Core.Features.Instructors;
using TuitionApp.Core.Common.Extensions;
using System.Collections.Generic;

namespace TuitionApp.IntegrationTest.DailySchedule.Timeslot
{
    using static SliceFixture;
    public class GetTimeslotQueryTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetTimeslotItem()
        {
            var dailyScheduleDto = await CreateDailyScheduleAsync();
            var createClassSubjectDto = await CreateClassSubjectAsync();

            var command = new CreateTimeslotItemCommand()
            {
                Disabled = false,
                Duration = new TimeSpan(1, 0, 0),
                StartTime = new TimeSpan(0, 22, 0),
                DailyScheduleId = dailyScheduleDto.Id,
                ClassSubjectId = createClassSubjectDto.Id,
            };
            var timeslotDto = await SendAsync(command);

            GetTimeslotItemQuery query = new GetTimeslotItemQuery() { Id = timeslotDto.Id };
            GetTimeslotItemDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Timeslots.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.DailyScheduleId.ShouldBe(created.DailyScheduleId);
            dto.StartTime.ShouldBe(created.StartTime);
            dto.Duration.ShouldBe(created.Duration);
            dto.Disabled.ShouldBe(created.Disabled);
        }


        [Fact]
        public async Task ShouldGetTimeslotList()
        {
            var createClassSubjectDto = await CreateClassSubjectAsync();
            var dailyScheduleDto = await CreateDailyScheduleAsync();

            var command = new CreateTimeslotItemCommand()
            {
                Disabled = false,
                Duration = new TimeSpan(1, 0, 0),
                StartTime = new TimeSpan(0, 22, 0),
                DailyScheduleId = dailyScheduleDto.Id,
                ClassSubjectId = createClassSubjectDto.Id,
            };
            var timeslotDto = await SendAsync(command);

            GetTimeslotListQuery query = new GetTimeslotListQuery() { };
            var dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db =>
            db.Timeslots.Where(c => c.Id.Equals(timeslotDto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }


        private async Task<CreateDailyScheduleItemDto> CreateDailyScheduleAsync()
        {
            var locationDto = await SendAsync(new CreateLocationItemCommand
            {
                Name = "location1",
                IsEnabled = true
            });

            var classroomDto = await SendAsync(new CreateClassroomFromLocationCommand
            {
                IsEnabled = true,
                Name = "Classroom1",
                Capacity = 40,
                LocationId = locationDto.Id
            });

            var scheduleDate = DateTime.UtcNow.Date;
            var dailyScheduleDto = await SendAsync(new CreateDailyScheduleItemCommand()
            {
                DayOfWeek = DayOfWeek.Monday,
                Disabled = false,
                WeekNumber = 3,
                DateSchedule = scheduleDate,
                ClassroomId = classroomDto.Id,
            });

            return dailyScheduleDto;
        }

        private async Task<CreateClassSubjectItemDto> CreateClassSubjectAsync()
        {
            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var createCourseDto = await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());

            var createCourseClassCommand = new CreateCourseClassItemCommand()
            {
                Name = $"{createCourseCommand.Name}-class1",
                CourseId = createCourseDto.Id
            };
            var createCourseClassDto = await SendWithValidationAsync(createCourseClassCommand, new CreateCourseClassItemCommandValidator());

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
                Title = $"{createCourseCommand.Name}-subject1",
                CourseClassId = createCourseClassDto.Id,
                SubjectAssignmentId = getSubjectItemDto.SubjectAssignmentList.First(),
            };

            var createClassSubjectDto = await SendWithValidationAsync(createClassSubjectCommand, new CreateClassSubjectItemCommandValidator());
            return createClassSubjectDto;
        }
    }
}
