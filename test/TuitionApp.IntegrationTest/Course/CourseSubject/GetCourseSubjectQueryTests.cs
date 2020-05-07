using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.CourseClasses;
using TuitionApp.Core.Features.Courses.CourseSubjects;
using TuitionApp.Core.Features.Instructors;
using TuitionApp.Core.Features.Subjects;
using Xunit;

namespace TuitionApp.IntegrationTest.Course.CourseSubject
{
    using static SliceFixture;
    public class GetCourseSubjectQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetCourseSubjectItem()
        {
            var createCourseSubjectDto = await CreateCourseSubjectAsync();

            var dto = await SendAsync(new GetCourseSubjectItemQuery() { Id = createCourseSubjectDto.Id });

            var created = await ExecuteDbContextAsync(db => db.CourseSubjects.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.Title.ShouldBe(created.Title);
            dto.CourseId.ShouldBe(created.CourseId);
            dto.SubjectAssignmentId.ShouldBe(created.SubjectAssignmentId);
        }

        [Fact]
        public async Task ShouldGetCourseSubjectList()
        {
            var createCourseSubjectDto = await CreateCourseSubjectAsync();

            var dto = await SendAsync(new GetCourseSubjectListQuery());

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(createCourseSubjectDto.Id));
        }


        private async Task<CreateCourseSubjectItemDto> CreateCourseSubjectAsync()
        {
            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var createCourseDto = await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());

            var instructorDto = await SendWithValidationAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first",
                LastName = "last",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            }, new CreateInstructorItemCommandValidator());

            var createSubjectItemCommand = new CreateSubjectItemCommand()
            {
                Title = "Subject1",
                InstructorList = new List<Guid> { instructorDto.Id },
            };
            var createSubjectItemDto = await SendWithValidationAsync(createSubjectItemCommand,
                new CreateSubjectItemCommandValidator());

            var getSubjectItemDto = await SendAsync(new GetSubjectItemQuery() { Id = createSubjectItemDto.Id });

            var createCourseSubjectCommand = new CreateCourseSubjectItemCommand()
            {
                Title = $"{createCourseCommand.Name}-subject1",
                CourseId = createCourseDto.Id,
                SubjectAssignmentId = getSubjectItemDto.SubjectAssignmentList.First(),
            };

            var createCourseSubjectDto = await SendWithValidationAsync(createCourseSubjectCommand, new CreateCourseSubjectItemCommandValidator());
            return createCourseSubjectDto;
        }
    }
}
