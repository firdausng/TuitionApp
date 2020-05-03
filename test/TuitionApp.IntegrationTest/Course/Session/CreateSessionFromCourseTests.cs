using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.Sessions;
using Xunit;

namespace TuitionApp.IntegrationTest.Course
{
    using static SliceFixture;
    public class CreateSessionFromCourseTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateSessionFromCourse()
        {
            var createCourseItemDto = await SendAsync(new CreateCourseItemCommand()
            {
                Name = "ShouldCreateSessionFromCourse",
                Rate = 40,
            });

            CreateSessionFromCourseDto dto = await SendWithValidationAsync(new CreateSessionFromCourseCommand
            {
                CourseId = createCourseItemDto.Id
            }, new CreateSessionFromCourseCommandValidator());

            var created = await ExecuteDbContextAsync(db => db.Sessions.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.CourseId.ShouldBe(createCourseItemDto.Id);
            created.Id.ShouldBe(created.Id);
        }
    }
}
