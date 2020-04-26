using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Course;
using Xunit;

namespace TuitionApp.IntegrationTest.Course
{
    using static SliceFixture;
    public class GetTimetableFromCourseQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetClassroomFromLocationItem()
        {
            var createCourseItemCommand = new CreateCourseItemCommand()
            {
                Name = "ShouldCreateTimetableFromCourse",
                Rate = 40,
            };
            var CourseDto = await SendAsync(createCourseItemCommand);

            var timetableDto = await SendAsync(new CreateTimetableFromCourseCommand()
            {
                CourseId = CourseDto.Id
            });

            var query = new GetTimetableItemFromCourseQuery() { Id = timetableDto.Id, CourseId = CourseDto.Id };
            var getClassroomItemdto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Timetables.Where(c => c.Id.Equals(timetableDto.Id)).SingleOrDefaultAsync());

            getClassroomItemdto.ShouldNotBeNull();
            getClassroomItemdto.Id.ShouldBe(created.Id);
        }

        [Fact]
        public async Task ShouldGetClassroomListFromLocation()
        {
            var createCourseItemCommand = new CreateCourseItemCommand()
            {
                Name = "ShouldCreateTimetableFromCourse",
                Rate = 40,
            };
            var CourseDto = await SendAsync(createCourseItemCommand);

            var timetableDto = await SendAsync(new CreateTimetableFromCourseCommand()
            {
                CourseId = CourseDto.Id
            });

            var created = await ExecuteDbContextAsync(db =>
            db.Timetables.Where(c => c.Id.Equals(timetableDto.Id)).SingleOrDefaultAsync());

            GetTimetableListFromCourseQuery query = new GetTimetableListFromCourseQuery() { CourseId= CourseDto.Id };
            GetObjectListVm<GetTimetableItemFromCourseDto> dto = await SendAsync(query);

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }
    }
}
