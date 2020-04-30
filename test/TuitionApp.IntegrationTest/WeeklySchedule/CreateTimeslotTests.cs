using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Features.Course;
using TuitionApp.Core.Features.WeeklySchedule.Timeslot;
using TuitionApp.Core.Features.Location;
using TuitionApp.Core.Features.WeeklySchedule;
using Xunit;

namespace TuitionApp.IntegrationTest.Timeslot
{
    using static SliceFixture;
    public class CreateTimeslotTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateTimeslot()
        {
            var weeklySchedule = await CreateWeeklyScheduleAsync();
            var session = await CreateSessionAsync();

            var command = new CreateTimeslotItemCommand
            {
                Disabled = false,
                SessionId = session.Id,
                WeeklyScheduleId = weeklySchedule.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            var timeslotDto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db =>
                db.Timeslots.Where(c => c.Id.Equals(timeslotDto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.SessionId.ShouldBe(command.SessionId);
            created.WeeklyScheduleId.ShouldBe(command.WeeklyScheduleId);
            created.StartTime.ShouldBe(command.StartTime);
            created.Disabled.ShouldBe(command.Disabled);
            created.Duration.ShouldBe(command.Duration);
        }

        [Fact()]
        public async Task ShouldNotCreateTimeslotWhenRoomTimeSlotAlreadyTaken()
        {
            var weeklySchedule = await CreateWeeklyScheduleAsync();
            var session = await CreateSessionAsync();

            var command1st = new CreateTimeslotItemCommand
            {
                Disabled = false,
                SessionId = session.Id,
                WeeklyScheduleId = weeklySchedule.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            var timeslotDto1st = await SendAsync(command1st);

            var command2nd = new CreateTimeslotItemCommand
            {
                Disabled = false,
                SessionId = session.Id,
                WeeklyScheduleId = weeklySchedule.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            await SendAsync(command2nd).ShouldThrowAsync<EntityAlreadyExistException>();
        }

        private async Task<CreateWeeklyScheduleItem> CreateWeeklyScheduleAsync()
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
            var command = new CreateWeeklyScheduleItemCommand()
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
