using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Course;
using Xunit;

namespace TuitionApp.IntegrationTest.Course
{
    using static SliceFixture;
    public class CreateTimetableFromCourseTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateTimetableFromCourse()
        {
            var createCourseItemCommand = new CreateCourseItemCommand()
            {
                Name = "ShouldCreateTimetableFromCourse",
                Rate = 40,
            };
            var createCourseItemDto = await SendAsync(createCourseItemCommand);

            CreateTimetableFromCourseCommand command = new CreateTimetableFromCourseCommand
            {
                CourseId = createCourseItemDto.Id
            };
            CreateTimetableFromCourseDto dto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db => db.Timetables.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.CourseId.ShouldBe(createCourseItemDto.Id);
            created.Id.ShouldBe(created.Id);
        }
    }
}
