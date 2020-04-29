using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Location;
using TuitionApp.Core.Features.WeeklySchedule;
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
            var command = new CreateWeeklyScheduleItemCommand()
            {
                DayOfWeek = DayOfWeek.Monday,
                Disabled = false,
                WeekNumber = 3,
                DateSchedule = scheduleDate,
                ClassroomId = classroomDto.Id,
            };
            var weeklyScheduleDto = await SendAsync(command);


            GetWeeklyScheduleItemQuery query = new GetWeeklyScheduleItemQuery() { Id = weeklyScheduleDto.Id };
            GetWeeklyScheduleItemDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.WeeklySchedules.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

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
            var command = new CreateWeeklyScheduleItemCommand()
            {
                DayOfWeek = DayOfWeek.Monday,
                Disabled = false,
                WeekNumber = 3,
                DateSchedule = scheduleDate,
                ClassroomId = classroomDto.Id,
            };
            var weeklyScheduleDto = await SendAsync(command);


            GetWeeklyScheduleListQuery query = new GetWeeklyScheduleListQuery() {  };
            var dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db =>
            db.WeeklySchedules.Where(c => c.Id.Equals(weeklyScheduleDto.Id)).SingleOrDefaultAsync());


            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
