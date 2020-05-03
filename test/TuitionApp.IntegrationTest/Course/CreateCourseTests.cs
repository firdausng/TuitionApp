using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Courses;
using Xunit;

namespace TuitionApp.IntegrationTest.Course
{
    using static SliceFixture;
    public class CreateCourseTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateCourse()
        {
            var command = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var dto = await SendWithValidationAsync(command, new CreateCourseItemCommandValidator());

            var created = await ExecuteDbContextAsync(db =>
            db.Courses.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Name.ShouldBe(command.Name);
            created.Rate.ShouldBe(command.Rate);
        }
    }
}
