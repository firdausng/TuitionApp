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
    public class GetClassroomFromLocationQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetClassroomFromLocationItem()
        {
            var locationDto = await SendAsync(new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "ShouldGetClassroomFromLocationItem_location",
                OpeningTime = new TimeSpan(0, 19, 0),
                ClosingTime = new TimeSpan(0, 21, 0),
            });

            var classroomDto = await SendAsync(new CreateClassroomFromLocationCommand()
            {
                IsEnabled = true,
                Name = "ShouldGetClassroomFromLocationItem_classroom",
                Capacity = 40,
                LocationId = locationDto.Id
            });

            var query = new GetClassroomItemFromLocationQuery() { Id = classroomDto.Id, LocationId = locationDto.Id };
            var getClassroomItemdto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Classrooms.Where(c => c.Id.Equals(classroomDto.Id)).SingleOrDefaultAsync());

            getClassroomItemdto.ShouldNotBeNull();
            getClassroomItemdto.Id.ShouldBe(created.Id);
            getClassroomItemdto.Name.ShouldBe(created.Name);
            getClassroomItemdto.IsEnabled.ShouldBe(created.IsEnabled);
        }

        [Fact]
        public async Task ShouldGetClassroomListFromLocation()
        {
            var locationDto = await SendAsync(new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "ShouldGetClassroomListFromLocation_location"
            });

            var classroomDto = await SendAsync(new CreateClassroomFromLocationCommand()
            {
                IsEnabled = true,
                Name = "ShouldGetClassroomListFromLocation_classroom",
                Capacity = 40,
                LocationId = locationDto.Id
            });

            var created = await ExecuteDbContextAsync(db =>
            db.Classrooms.Where(c => c.Id.Equals(classroomDto.Id)).SingleOrDefaultAsync());

            GetClassroomListFromLocationQuery query = new GetClassroomListFromLocationQuery() { LocationId= locationDto.Id };
            GetObjectListVm<GetClassroomFromLocationDto> dto = await SendAsync(query);

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
