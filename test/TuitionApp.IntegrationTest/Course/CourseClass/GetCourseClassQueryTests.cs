using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.CourseClasses;
using Xunit;

namespace TuitionApp.IntegrationTest.Course.CourseClass
{
    using static SliceFixture;
    public class GetCourseClassQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetStudentItem()
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

            var dto = await SendAsync(new GetCourseClassItemQuery() { Id = createCourseClassDto.Id });

            var created = await ExecuteDbContextAsync(db => db.CourseClasses.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.Name.ShouldBe(created.Name);
            dto.CourseId.ShouldBe(created.CourseId);
        }

        [Fact]
        public async Task ShouldGetStudentList()
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

            var dto = await SendAsync(new GetCourseClassListQuery());

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(createCourseClassDto.Id));
        }
    }
}
