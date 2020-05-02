using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.DailySchedules;
using Xunit;
using TuitionApp.Core.Features.Locations;
using TuitionApp.Core.Features.Locations.Classrooms;

namespace TuitionApp.IntegrationTest.WeeklySchedule
{
    using static SliceFixture;
    public class CreateWeeklyScheduleTests: IntegrationTestBase
    {

        [Fact]
        public async Task ShouldCreateWeeklySchedule()
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

            var created = await ExecuteDbContextAsync(db =>
            db.DailySchedules.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.WeekNumber.ShouldBe(command.WeekNumber);
            created.ClassroomId.ShouldBe(command.ClassroomId);
            created.DateSchedule.ShouldBe(command.DateSchedule);
        }
    }
}
