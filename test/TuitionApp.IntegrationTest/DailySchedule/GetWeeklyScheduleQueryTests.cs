using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Location;
using TuitionApp.Core.Features.DailySchedule;
using Xunit;

namespace TuitionApp.IntegrationTest.WeeklySchedule
{
    using static SliceFixture;
    public class GetWeeklyScheduleQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetWeeklyScheduleItem()
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
            var dailyScheduleDto = await SendAsync(command);

            GetDailyScheduleItemQuery query = new GetDailyScheduleItemQuery() { Id = dailyScheduleDto.Id };
            GetDailyScheduleItemDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.DailySchedules.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.ClassroomId.ShouldBe(created.ClassroomId);
            dto.DateSchedule.ShouldBe(created.DateSchedule);
            dto.DayOfWeek.ShouldBe(created.DayOfWeek);
            dto.Disabled.ShouldBe(created.Disabled);
        }


        [Fact]
        public async Task ShouldGetWeeklyScheduleList()
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
            var dailyScheduleDto = await SendAsync(command);


            var query = new GetDailyScheduleListQuery() {  };
            var dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db =>
            db.DailySchedules.Where(c => c.Id.Equals(dailyScheduleDto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
