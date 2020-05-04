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

namespace TuitionApp.IntegrationTest.DailySchedule.Timeslot
{
    using static SliceFixture;
    public class GetTimeslotQueryTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetWeeklyScheduleItem()
        {
            var dailyScheduleDto = await CreateDailyScheduleAsync();

            var command = new CreateTimeslotItemCommand()
            {
                Disabled = false,
                Duration = new TimeSpan(1, 0, 0),
                StartTime = new TimeSpan(0, 22, 0),
                DailyScheduleId = dailyScheduleDto.Id,
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
        public async Task ShouldGetWeeklyScheduleList()
        {
            var dailyScheduleDto = await CreateDailyScheduleAsync();

            var command = new CreateTimeslotItemCommand()
            {
                Disabled = false,
                Duration = new TimeSpan(1, 0, 0),
                StartTime = new TimeSpan(0, 22, 0),
                DailyScheduleId = dailyScheduleDto.Id,
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
    }
}
