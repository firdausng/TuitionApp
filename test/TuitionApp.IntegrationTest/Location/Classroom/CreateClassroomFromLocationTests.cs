using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Location;
using Xunit;

namespace TuitionApp.IntegrationTest.Location.Classroom
{
    using static SliceFixture;
    public class CreateClassroomFromLocationTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateClassroomFromLocation()
        {
            var createLocationDto = await SendAsync(new CreateLocationCommand 
            { 
                Name = "location1",
                IsEnabled = true
            });

            CreateClassroomFromLocationCommand command = null;
            CreateClassroomFromLocationDto dto = null;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                //await ctxt.Departments.AddAsync(dept);
                command = new CreateClassroomFromLocationCommand
                {
                    IsEnabled = true,
                    Name= "Classroom1",
                    Capacity= 40,
                    LocationId = createLocationDto.Id
                };
                dto = await mediator.Send(command);
            });

            var created = await ExecuteDbContextAsync(db => db.Classrooms.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.LocationId.ShouldBe(createLocationDto.Id);
            created.Capacity.ShouldBe(command.Capacity);
            created.Name.ShouldBe(command.Name);
            created.IsEnabled.ShouldBe(command.IsEnabled);
        }
    }
}
