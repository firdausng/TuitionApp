using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Instructor;
using Xunit;

namespace TuitionApp.IntegrationTest.Instructor
{
    using static SliceFixture;
    public class GetCalendarSettingQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetInstructorItem()
        {
            var instructorDto = await SendAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first",
                LastName = "last",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            });


            GetInstructorItemQuery query = new GetInstructorItemQuery() { Id = instructorDto.Id };
            GetInstructorItemDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Instructor.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.HireDate.ShouldBe(created.HireDate);
            dto.Name.ShouldBe(created.FullName());
        }


        [Fact]
        public async Task ShouldGetInstructorList()
        {
            var instructorDto = await SendAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first",
                LastName = "last",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            });


            var created = await ExecuteDbContextAsync(db =>
                db.Instructor.Where(c => c.Id.Equals(instructorDto.Id)).SingleOrDefaultAsync());

            GetInstructorListQuery query = new GetInstructorListQuery();
            GetObjectListVm<GetInstructorItemDto> dto = await SendAsync(query);


            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
