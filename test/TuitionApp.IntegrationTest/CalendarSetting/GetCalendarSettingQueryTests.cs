using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using Xunit;
using TuitionApp.Core.Features.CalendarSettings;

namespace TuitionApp.IntegrationTest.CalendarSetting
{
    using static SliceFixture;
    public class GetCalendarSettingQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetCalendarSettingItem()
        {
            var CalendarSettingDto = await SendAsync(new CreateCalendarSettingItemCommand()
            {
                FirstDayOfWeek = DayOfWeek.Monday,
                DefaultOpeningTime = new TimeSpan(19, 0, 0),
                DefaultClosingTime = new TimeSpan(21, 0, 0),
                AllowedTimeslotOverlap = false,
            });


            GetCalendarSettingItemQuery query = new GetCalendarSettingItemQuery() { Id = CalendarSettingDto.Id };
            GetCalendarSettingItemDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.CalendarSettings.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.DefaultOpeningTime.ShouldBe(created.DefaultOpeningTime);
            dto.DefaultClosingTime.ShouldBe(created.DefaultClosingTime);
            dto.FirstDayOfWeek.ShouldBe(created.FirstDayOfWeek);
            dto.AllowedTimeslotOverlap.ShouldBe(created.AllowedTimeslotOverlap);
        }


        [Fact]
        public async Task ShouldGetCalendarSettingList()
        {
            var CalendarSettingDto = await SendAsync(new CreateCalendarSettingItemCommand()
            {
                FirstDayOfWeek = DayOfWeek.Monday,
                DefaultOpeningTime = new TimeSpan(19, 0, 0),
                DefaultClosingTime = new TimeSpan(21, 0, 0),
                AllowedTimeslotOverlap = false,
            });


            var created = await ExecuteDbContextAsync(db =>
                db.CalendarSettings.Where(c => c.Id.Equals(CalendarSettingDto.Id)).SingleOrDefaultAsync());

            GetCalendarSettingListQuery query = new GetCalendarSettingListQuery();
            GetObjectListVm<GetCalendarSettingItemDto> dto = await SendAsync(query);


            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
