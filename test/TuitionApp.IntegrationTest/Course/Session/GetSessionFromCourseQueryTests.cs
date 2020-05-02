using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.Sessions;
using Xunit;

namespace TuitionApp.IntegrationTest.Course
{
    using static SliceFixture;
    public class GetSessionFromCourseQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetSessionFromCourseItem()
        {
            var createCourseItemCommand = new CreateCourseItemCommand()
            {
                Name = "ShouldCreateSessionFromCourse",
                Rate = 40,
            };
            var CourseDto = await SendAsync(createCourseItemCommand);

            var SessionDto = await SendAsync(new CreateSessionFromCourseCommand()
            {
                CourseId = CourseDto.Id
            });

            var query = new GetSessionItemFromCourseQuery() { Id = SessionDto.Id, CourseId = CourseDto.Id };
            var getClassroomItemdto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Sessions.Where(c => c.Id.Equals(SessionDto.Id)).SingleOrDefaultAsync());

            getClassroomItemdto.ShouldNotBeNull();
            getClassroomItemdto.Id.ShouldBe(created.Id);
        }

        [Fact]
        public async Task ShouldGetSessionFromCourse()
        {
            var createCourseItemCommand = new CreateCourseItemCommand()
            {
                Name = "ShouldCreateSessionFromCourse",
                Rate = 40,
            };
            var CourseDto = await SendAsync(createCourseItemCommand);

            var SessionDto = await SendAsync(new CreateSessionFromCourseCommand()
            {
                CourseId = CourseDto.Id
            });

            var created = await ExecuteDbContextAsync(db =>
            db.Sessions.Where(c => c.Id.Equals(SessionDto.Id)).SingleOrDefaultAsync());

            GetSessionListFromCourseQuery query = new GetSessionListFromCourseQuery() { CourseId= CourseDto.Id };
            GetObjectListVm<GetSessionItemFromCourseDto> dto = await SendAsync(query);

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
