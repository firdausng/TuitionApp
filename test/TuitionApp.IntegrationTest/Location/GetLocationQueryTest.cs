using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Location;
using Xunit;

namespace TuitionApp.IntegrationTest.Location
{
    using static SliceFixture;
    public class GetLocationQueryTest: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetLocationItem()
        {
            var locationDto = await SendAsync(new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "ShouldGetLocationItem",
                OpeningTime = new TimeSpan(0, 19, 0),
                ClosingTime = new TimeSpan(0, 21, 0),
            });

            GetLocationItemQuery query = new GetLocationItemQuery() { Id = locationDto.Id };
            GetLocationDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Locations.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.Name.ShouldBe(created.Name);
            dto.OpeningTime.ShouldBe(created.OpeningTime);
            dto.ClosingTime.ShouldBe(created.ClosingTime);
            dto.IsEnabled.ShouldBe(created.IsEnabled);
        }

        [Fact]
        public async Task ShouldGetLocationList()
        {
            var locationDto = await SendAsync(new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "ShouldGetLocationList",
                OpeningTime = new TimeSpan(0, 19, 0),
                ClosingTime = new TimeSpan(0, 21, 0),
            });

            var created = await ExecuteDbContextAsync(db => 
            db.Locations.Where(c => c.Id.Equals(locationDto.Id)).SingleOrDefaultAsync());

            GetLocationListQuery query = new GetLocationListQuery();
            GetObjectListVm<GetLocationDto> dto = await SendAsync(query);

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
