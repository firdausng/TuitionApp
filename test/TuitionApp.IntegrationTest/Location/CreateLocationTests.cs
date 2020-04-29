using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Location;
using Xunit;

namespace TuitionApp.IntegrationTest.Location
{
    using static SliceFixture;
    public class CreateLocationTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateLocation()
        {
            CreateLocationItemDto dto = null;
            CreateLocationItemCommand command = null;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                //await ctxt.Locations.AddAsync(dept);
                command = new CreateLocationItemCommand()
                {
                    IsEnabled = true,
                    Name = "location1",
                    OpeningTime = new TimeSpan(0, 19, 0),
                    ClosingTime = new TimeSpan(0, 21, 0),
                };
                dto = await mediator.Send(command);
            });

            var created = await ExecuteDbContextAsync(db => db.Locations.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Name.ShouldBe(command.Name);
            created.OpeningTime.ShouldBe(command.OpeningTime);
            created.ClosingTime.ShouldBe(command.ClosingTime);
            created.IsEnabled.ShouldBe(command.IsEnabled);
        }
    }
}
