using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.Sessions;
using TuitionApp.Core.Features.Enrollments;
using TuitionApp.Core.Features.Students;
using Xunit;

namespace TuitionApp.IntegrationTest.Enrollment
{
    using static SliceFixture;
    public class CreateEnrollmentTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateEnrollment()
        {
            var studentDto = await SendAsync(new CreateStudentItemCommand()
            {
                FirstName = "first",
                LastName = "last"
            });

            var courseDto = await SendAsync(new CreateCourseItemCommand()
            {
                Name = "ShouldCreateSessionFromCourse",
                Rate = 40,
            });

            var SessionDto = await SendAsync(new CreateSessionFromCourseCommand
            {
                CourseId = courseDto.Id
            });

            var command = new CreateEnrollmentItemCommand()
            {
               StartDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
               StudentId = studentDto.Id,
               SessionId = SessionDto.Id,
            };
            var dto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db =>
                db.Enrollments.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());


            created.ShouldNotBeNull();
            created.StartDate.ShouldBe(command.StartDate);
            created.StudentId.ShouldBe(command.StudentId);
            created.SessionId.ShouldBe(command.SessionId);
        }
    }
}
