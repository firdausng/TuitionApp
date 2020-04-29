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
    public class CreateClassroomFromLocationTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateClassroomFromLocation()
        {
            var createLocationDto = await SendAsync(new CreateLocationItemCommand
            {
                Name = "location1",
                IsEnabled = true,
                OpeningTime = new TimeSpan(0, 19, 0),
                ClosingTime = new TimeSpan(0, 21, 0),
            });

            CreateClassroomFromLocationCommand command = new CreateClassroomFromLocationCommand
            {
                IsEnabled = true,
                Name = "Classroom1",
                Capacity = 40,
                LocationId = createLocationDto.Id
            };
            CreateClassroomFromLocationDto dto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Classrooms.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.LocationId.ShouldBe(createLocationDto.Id);
            created.Capacity.ShouldBe(command.Capacity);
            created.Name.ShouldBe(command.Name);
            created.IsEnabled.ShouldBe(command.IsEnabled);
        }
    }
}
