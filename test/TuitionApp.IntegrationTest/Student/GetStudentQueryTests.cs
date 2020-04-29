using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Student;
using Xunit;

namespace TuitionApp.IntegrationTest.Student
{
    using static SliceFixture;
    public class GetStudentQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetStudentItem()
        {
            var createStudentItemCommand = new CreateStudentItemCommand()
            {
                FirstName = "first",
                LastName = "last"
            };
            var createStudentDto = await SendAsync(createStudentItemCommand);


            GetStudentItemQuery query = new GetStudentItemQuery() { Id = createStudentDto.Id };
            GetStudentItemDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Students.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.Name.ShouldBe($"{created.FirstName} {created.LastName}");
        }


        [Fact]
        public async Task ShouldGetStudentList()
        {
            var createStudentItemCommand = new CreateStudentItemCommand()
            {
                FirstName = "first",
                LastName = "last"
            };
            var createStudentDto = await SendAsync(createStudentItemCommand);

            var created = await ExecuteDbContextAsync(db =>
            db.Students.Where(c => c.Id.Equals(createStudentDto.Id)).SingleOrDefaultAsync());

            GetWeeklyScheduleListQuery query = new GetWeeklyScheduleListQuery();
            GetObjectListVm<GetStudentItemDto> dto = await SendAsync(query);


            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
