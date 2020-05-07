using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Locations;
using TuitionApp.Core.Features.DailySchedules;
using Xunit;
using TuitionApp.Core.Features.DailySchedules.Timeslots;
using TuitionApp.Core.Features.Locations.Classrooms;
using TuitionApp.Core.Features.Courses.ClassSubjects;
using TuitionApp.Core.Features.Subjects;
using TuitionApp.Core.Features.Instructors;
using System.Collections.Generic;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Courses.CourseClasses;

namespace TuitionApp.IntegrationTest.DailySchedule.Timeslot
{
    using static SliceFixture;
    public class CreateTimeslotTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateTimeslot()
        {
            var dailySchedule = await CreateDailyScheduleAsync();
            var createClassSubjectDto = await CreateClassSubjectAsync();

            var command = new CreateTimeslotItemCommand
            {
                Disabled = false,
                DailyScheduleId = dailySchedule.Id,
                ClassSubjectId = createClassSubjectDto.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            var timeslotDto = await SendWithValidationAsync(command, new CreateTimeslotItemCommandValidator());

            var created = await ExecuteDbContextAsync(db =>
                db.Timeslots.Where(c => c.Id.Equals(timeslotDto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.DailyScheduleId.ShouldBe(command.DailyScheduleId);
            created.StartTime.ShouldBe(command.StartTime);
            created.Disabled.ShouldBe(command.Disabled);
            created.Duration.ShouldBe(command.Duration);
        }

        [Fact()]
        public async Task ShouldNotCreateTimeslotWhenRoomTimeSlotAlreadyTaken()
        {
            var dailySchedule = await CreateDailyScheduleAsync();
            var createClassSubjectDto = await CreateClassSubjectAsync();

            var command1st = new CreateTimeslotItemCommand
            {
                Disabled = false,
                DailyScheduleId = dailySchedule.Id,
                ClassSubjectId = createClassSubjectDto.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            var timeslotDto1st = await SendAsync(command1st);

            var command2nd = new CreateTimeslotItemCommand
            {
                Disabled = false,
                DailyScheduleId = dailySchedule.Id,
                ClassSubjectId = createClassSubjectDto.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            await SendWithValidationAsync(command2nd, new CreateTimeslotItemCommandValidator()).ShouldThrowAsync<EntityAlreadyExistException>();
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
            var command = new CreateDailyScheduleItemCommand()
            {
                DayOfWeek = DayOfWeek.Monday,
                Disabled = false,
                WeekNumber = 3,
                DateSchedule = scheduleDate,
                ClassroomId = classroomDto.Id,
            };
            var dto = await SendAsync(command);
            return dto;
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
