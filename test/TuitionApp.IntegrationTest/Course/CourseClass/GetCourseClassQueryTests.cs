using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.CourseClasses;
using TuitionApp.Core.Features.Enrollments;
using TuitionApp.Core.Features.Locations;
using Xunit;

namespace TuitionApp.IntegrationTest.Course.CourseClass
{
    using static SliceFixture;
    public class GetCourseClassQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetCourseClassItem()
        {
            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var createCourseDto = await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());

            var locationDto = await SendWithValidationAsync(new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "location1",
                Address = "address1",
                OpeningTime = new TimeSpan(0, 19, 0),
                ClosingTime = new TimeSpan(0, 21, 0),
            }, new CreateLocationItemCommandValidator());


            var createCourseClassCommand = new CreateCourseClassItemCommand()
            {
                Name = $"{createCourseCommand.Name}-class1",
                CourseId = createCourseDto.Id,
                LocationId = locationDto.Id,
                Capacity = 40,
            };
            var createCourseClassDto = await SendWithValidationAsync(createCourseClassCommand, new CreateCourseClassItemCommandValidator());

            var dto = await SendAsync(new GetCourseClassItemQuery() { Id = createCourseClassDto.Id });

            var created = await ExecuteDbContextAsync(db => db.CourseClasses.Where(c => c.Id.Equals(dto.Id)).Include(cc => cc.Enrollments).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.Name.ShouldBe(created.Name);
            dto.Capacity.ShouldBe(created.Capacity);
            dto.CapacityLeft.ShouldBe(created.Capacity - created.Enrollments.Count);
            dto.CourseId.ShouldBe(created.CourseId);
        }

        [Fact]
        public async Task ShouldGetCourseClassList()
        {
            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var createCourseDto = await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());

            var locationDto = await SendWithValidationAsync(new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "location1",
                Address = "address1",
                OpeningTime = new TimeSpan(0, 19, 0),
                ClosingTime = new TimeSpan(0, 21, 0),
            }, new CreateLocationItemCommandValidator());


            var createCourseClassCommand = new CreateCourseClassItemCommand()
            {
                Name = $"{createCourseCommand.Name}-class1",
                CourseId = createCourseDto.Id,
                LocationId = locationDto.Id,
                Capacity = 40,
            };
            var createCourseClassDto = await SendWithValidationAsync(createCourseClassCommand, new CreateCourseClassItemCommandValidator());

            var dto = await SendAsync(new GetCourseClassListQuery());

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(createCourseClassDto.Id));
        }
    }
}
