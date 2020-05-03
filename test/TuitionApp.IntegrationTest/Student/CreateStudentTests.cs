using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Students;
using Xunit;

namespace TuitionApp.IntegrationTest.Student
{
    using static SliceFixture;
    public class CreatStudentTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateStudent()
        {
            var command = new CreateStudentItemCommand()
            {
                FirstName = "first",
                LastName = "last"
            };
            var dto = await SendWithValidationAsync(command, new CreateStudentItemCommandValidator());

            var created = await ExecuteDbContextAsync(db =>
            db.Students.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.FirstName.ShouldBe(command.FirstName);
            created.LastName.ShouldBe(command.LastName);
        }
    }
}
