using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Instructors;
using Xunit;

namespace TuitionApp.IntegrationTest.Instructor
{
    using static SliceFixture;
    public class CreateInstructorTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateInstructor()
        {
            var dto = await SendAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first",
                LastName = "last",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            });

            var created = await ExecuteDbContextAsync(db =>
                db.Instructor.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Id.ShouldBe(dto.Id);
        }
    }
}
