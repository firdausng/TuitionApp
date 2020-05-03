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
using TuitionApp.Core.Features.Courses.Sessions;

namespace TuitionApp.IntegrationTest.DailySchedule.Timeslot
{
    using static SliceFixture;
    public class CreateTimeslotTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateTimeslot()
        {
            var dailySchedule = await CreateDailyScheduleAsync();
            var session = await CreateSessionAsync();

            var command = new CreateTimeslotItemCommand
            {
                Disabled = false,
                DailyScheduleId = dailySchedule.Id,
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
            var weeklySchedule = await CreateDailyScheduleAsync();
            var session = await CreateSessionAsync();

            var command1st = new CreateTimeslotItemCommand
            {
                Disabled = false,
                DailyScheduleId = weeklySchedule.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            var timeslotDto1st = await SendAsync(command1st);

            var command2nd = new CreateTimeslotItemCommand
            {
                Disabled = false,
                DailyScheduleId = weeklySchedule.Id,
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
        private async Task<CreateSessionFromCourseDto> CreateSessionAsync()
        {
            var createCourseItemDto = await SendAsync(new CreateCourseItemCommand()
            {
                Name = "ShouldCreateSessionFromCourse",
                Rate = 40,
            });

            CreateSessionFromCourseDto dto = await SendAsync(new CreateSessionFromCourseCommand
            {
                CourseId = createCourseItemDto.Id
            });
            return dto;
        }
    }
}
