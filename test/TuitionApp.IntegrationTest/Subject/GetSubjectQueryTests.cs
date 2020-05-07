using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Subjects;
using Xunit;

namespace TuitionApp.IntegrationTest.Subject
{
    using static SliceFixture;
    public class GetSubjectQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetStudentItem()
        {
            var createSubjectItemCommand = new CreateSubjectItemCommand()
            {
                Title = "Subject1"
            };
            var createSubjectItemDto = await SendWithValidationAsync(createSubjectItemCommand, new CreateSubjectItemCommandValidator());

            var query = new GetSubjectItemQuery() { Id = createSubjectItemDto.Id };
            var dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Subjects.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.Title.ShouldBe(created.Title);
        }

        [Fact]
        public async Task ShouldGetStudentList()
        {
            var createSubjectItemCommand = new CreateSubjectItemCommand()
            {
                Title = "Subject1"
            };
            var createSubjectItemDto = await SendWithValidationAsync(createSubjectItemCommand, new CreateSubjectItemCommandValidator());


            var created = await ExecuteDbContextAsync(db =>
            db.Subjects.Where(c => c.Id.Equals(createSubjectItemDto.Id)).SingleOrDefaultAsync());

            var query = new GetSubjectListQuery();
            var dto = await SendAsync(query);


            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
