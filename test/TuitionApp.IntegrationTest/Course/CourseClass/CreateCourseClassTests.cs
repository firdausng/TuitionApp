using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.CourseClasses;
using Xunit;

namespace TuitionApp.IntegrationTest.Course.CourseClass
{
    using static SliceFixture;
    public class CreateCourseClassTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateCourse()
        {
            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var createCourseDto = await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());

            var createCourseClassCommand = new CreateCourseClassItemCommand()
            {
                Name = $"{createCourseCommand.Name}-class1",
                CourseId = createCourseDto.Id
            };
            var createCourseClassDto = await SendWithValidationAsync(createCourseClassCommand, new CreateCourseClassItemCommandValidator());

            var created = await ExecuteDbContextAsync(db =>
            db.CourseClasses.Where(c => c.Id.Equals(createCourseClassDto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Name.ShouldBe(createCourseClassCommand.Name);
            created.CourseId.ShouldBe(createCourseClassCommand.CourseId);
        }
    }
}
