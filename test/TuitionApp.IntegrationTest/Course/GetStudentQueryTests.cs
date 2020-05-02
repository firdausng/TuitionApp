using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Courses;
using Xunit;

namespace TuitionApp.IntegrationTest.Course
{
    using static SliceFixture;
    public class GetStudentQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetStudentItem()
        {
            var command = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var createCourseItemDto = await SendAsync(command);

            GetCourseItemQuery query = new GetCourseItemQuery() { Id = createCourseItemDto.Id };
            GetCourseItemDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Courses.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.Name.ShouldBe(created.Name);
        }

        [Fact]
        public async Task ShouldGetStudentList()
        {
            var command = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var createCourseItemDto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db =>
            db.Courses.Where(c => c.Id.Equals(createCourseItemDto.Id)).SingleOrDefaultAsync());

            GetCourseListQuery query = new GetCourseListQuery();
            GetObjectListVm<GetCourseItemDto> dto = await SendAsync(query);

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
