using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                Name = "ShouldGetLocationItem"
            });


            GetLocationDto dto = null;
            GetLocationItemQuery query = null;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                query = new GetLocationItemQuery() { Id = locationDto.Id};
                dto = await mediator.Send(query);
            });

            var created = await ExecuteDbContextAsync(db => db.Locations.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.Name.ShouldBe(created.Name);
            dto.IsEnabled.ShouldBe(created.IsEnabled);
        }

        [Fact]
        public async Task ShouldGetLocationList()
        {
            var locationDto = await SendAsync(new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "ShouldGetLocationList"
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
