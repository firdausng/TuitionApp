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
            CreateLocationItemCommand command = new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "location1",
                OpeningTime = new TimeSpan(0, 19, 0),
                ClosingTime = new TimeSpan(0, 21, 0),
            };
            CreateLocationItemDto dto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Locations.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Name.ShouldBe(command.Name);
            created.OpeningTime.ShouldBe(command.OpeningTime);
            created.ClosingTime.ShouldBe(command.ClosingTime);
            created.IsEnabled.ShouldBe(command.IsEnabled);
        }
    }
}
