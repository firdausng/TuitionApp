using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Instructors;
using TuitionApp.Core.Features.Locations;
using Xunit;

namespace TuitionApp.IntegrationTest.Location
{
    using static SliceFixture;
    public class CreateLocationTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateLocation()
        {
            var instructorDto1 = await SendAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first",
                LastName = "last",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            });

            var instructorDto2 = await SendAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first1",
                LastName = "last2",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            });


            CreateLocationItemCommand command = new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "location1",
                OpeningTime = new TimeSpan(0, 19, 0),
                ClosingTime = new TimeSpan(0, 21, 0),
                InstructorLists = new System.Collections.Generic.List<Guid>{ instructorDto1.Id, instructorDto2.Id },
            };

            CreateLocationItemDto dto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Locations.Include(l => l.LocationInstructors).ThenInclude(l => l.Instructor).Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Id.ShouldBe(dto.Id);
            created.Name.ShouldBe(command.Name);
            created.OpeningTime.ShouldBe(command.OpeningTime);
            created.ClosingTime.ShouldBe(command.ClosingTime);
            created.IsEnabled.ShouldBe(command.IsEnabled);
            var instructorIdList = created.LocationInstructors.Select(li => li.InstructorId).ToList();
            instructorIdList.ShouldContain(instructorDto1.Id);
            instructorIdList.ShouldContain(instructorDto2.Id);
        }

        [Fact]
        public async Task ShouldNotCreateLocationWhenInstructorIdNotExist()
        {
            var instructorDto1 = await SendAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first",
                LastName = "last",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            });

            var instructorDto2 = await SendAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first1",
                LastName = "last2",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            });


            CreateLocationItemCommand command = new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "location1",
                OpeningTime = new TimeSpan(0, 19, 0),
                ClosingTime = new TimeSpan(0, 21, 0),
                InstructorLists = new System.Collections.Generic.List<Guid> { Guid.NewGuid(), Guid.NewGuid(), instructorDto1.Id, instructorDto2.Id },
            };

            await SendAsync(command).ShouldThrowAsync<EntityListCountMismatchException<Core.Domain.Entities.Instructor>>();
        }
            
    }
}
