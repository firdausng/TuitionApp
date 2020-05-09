using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.ClassSubjects;
using TuitionApp.Core.Features.Courses.CourseClasses;
using TuitionApp.Core.Features.Instructors;
using TuitionApp.Core.Features.Subjects;
using Xunit;

namespace TuitionApp.IntegrationTest.Course.ClassSubject
{
    using static SliceFixture;
    public class CreateClassSubjectTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateClassSubject()
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
                CourseId = createCourseDto.Id,
                Capacity = 40,
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

            var createSubjectClassCommand = new CreateClassSubjectItemCommand()
            {
                Title = $"{createCourseCommand.Name}-subject1",
                CourseClassId = createCourseClassDto.Id,
                SubjectAssignmentId = getSubjectItemDto.SubjectAssignmentList.First(),
            };
            var createSubjectClassDto = await SendWithValidationAsync(createSubjectClassCommand, new CreateClassSubjectItemCommandValidator());

            var created = await ExecuteDbContextAsync(db =>
            db.ClassSubjects.Where(c => c.Id.Equals(createSubjectClassDto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Title.ShouldBe(createSubjectClassCommand.Title);
            created.CourseClassId.ShouldBe(createSubjectClassCommand.CourseClassId);
        }
    }
}
