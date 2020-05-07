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
using TuitionApp.Core.Features.Courses.ClassSubjects;
using TuitionApp.Core.Features.Instructors;
using TuitionApp.Core.Features.Subjects;
using Xunit;

namespace TuitionApp.IntegrationTest.Course.ClassSubject
{
    using static SliceFixture;
    public class GetClassSubjectQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetClassSubjectItem()
        {
            var createClassSubjectDto = await CreateClassSubjectAsync();

            var dto = await SendAsync(new GetClassSubjectItemQuery() { Id = createClassSubjectDto.Id });

            var created = await ExecuteDbContextAsync(db => db.ClassSubjects.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.Title.ShouldBe(created.Title);
            dto.CourseId.ShouldBe(created.CourseClassId);
            dto.SubjectAssignmentId.ShouldBe(created.SubjectAssignmentId);
        }

        [Fact]
        public async Task ShouldGetClassSubjectList()
        {
            var createClassSubjectDto = await CreateClassSubjectAsync();

            var dto = await SendAsync(new GetClassSubjectListQuery());

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(createClassSubjectDto.Id));
        }


        private async Task<CreateClassSubjectItemDto> CreateClassSubjectAsync()
        {
            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var createCourseDto = await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());

            var createCourseClassCommand = new CreateCourseClassItemCommand()
            {
                Name = $"{createCourseCommand.Name}-class1",
                CourseId = createCourseDto.Id
            };
            var createCourseClassDto = await SendWithValidationAsync(createCourseClassCommand, new CreateCourseClassItemCommandValidator());

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

            var createCourseSubjectCommand = new CreateClassSubjectItemCommand()
            {
                Title = $"{createCourseCommand.Name}-subject1",
                CourseClassId = createCourseClassDto.Id,
                SubjectAssignmentId = getSubjectItemDto.SubjectAssignmentList.First(),
            };

            var createCourseSubjectDto = await SendWithValidationAsync(createCourseSubjectCommand, new CreateClassSubjectItemCommandValidator());
            return createCourseSubjectDto;
        }
    }
}
