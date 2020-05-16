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
            var instructorDto = await CreateInstructorAsync();
            var studentDto = await CreateStudentAsync();
            var courseDto = await CreateCourseAsync();
            var locationDto = await CreateLocationWithInstructorAsync(instructorDto);


            var createCourseClassCommand = new CreateCourseClassItemCommand()
            {
                Name = $"{courseDto.Id}-class1",
                CourseId = courseDto.Id,
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
            var instructorDto = await CreateInstructorAsync();
            var studentDto = await CreateStudentAsync();
            var courseDto = await CreateCourseAsync();
            var locationDto = await CreateLocationWithInstructorAsync(instructorDto);


            var createCourseClassCommand = new CreateCourseClassItemCommand()
            {
                Name = $"{courseDto.Id}-class1",
                CourseId = courseDto.Id,
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
