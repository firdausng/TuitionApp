using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Course;
using TuitionApp.Core.Features.Enrollment;
using TuitionApp.Core.Features.Student;
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
                Name = "ShouldCreateTimetableFromCourse",
                Rate = 40,
            });

            var TimetableDto = await SendAsync(new CreateTimetableFromCourseCommand
            {
                CourseId = courseDto.Id
            });

            var command = new CreateEnrollmentItemCommand()
            {
               StartDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
               StudentId = studentDto.Id,
               TimetableId = TimetableDto.Id,
            };
            var dto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db =>
                db.Enrollments.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());


            created.ShouldNotBeNull();
            created.StartDate.ShouldBe(command.StartDate);
            created.StudentId.ShouldBe(command.StudentId);
            created.TimetableId.ShouldBe(command.TimetableId);
        }
    }
}
