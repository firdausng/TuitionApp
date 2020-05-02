using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.CalendarSettings;
using Xunit;

namespace TuitionApp.IntegrationTest.CalendarSetting
{
    using static SliceFixture;
    public class CreateCalendarSettingTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateCalendarSettings()
        {
            var dto = await SendAsync(new CreateCalendarSettingItemCommand()
            {
                FirstDayOfWeek = DayOfWeek.Monday,
                DefaultOpeningTime = new TimeSpan(19,0,0),
                DefaultClosingTime = new TimeSpan(21, 0, 0),
                AllowedTimeslotOverlap = false,
            });

            var created = await ExecuteDbContextAsync(db =>
                db.CalendarSettings.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());


            created.ShouldNotBeNull();
            created.Id.ShouldBe(dto.Id);
        }
    }
}
