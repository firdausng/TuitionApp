using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.CourseClasses;
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

            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var courseDto = await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());


            var createCourseClassCommand = new CreateCourseClassItemCommand()
            {
                Name = $"{createCourseCommand.Name}-class1",
                CourseId = courseDto.Id,
                Capacity = 40,
            };
            var courseClassDto = await SendWithValidationAsync(createCourseClassCommand, new CreateCourseClassItemCommandValidator());


            var command = new CreateEnrollmentItemCommand()
            {
                StartDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
                StudentId = studentDto.Id,
                CourseClassId = courseClassDto.Id,
            };
            var dto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db =>
                db.Enrollments.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());


            created.ShouldNotBeNull();
            created.StartDate.ShouldBe(command.StartDate);
            created.StudentId.ShouldBe(command.StudentId);
            created.CourseClassId.ShouldBe(command.CourseClassId);
        }
    }
}
