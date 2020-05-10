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
using TuitionApp.Core.Features.Locations;
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
            var instructorDto = await CreateInstructorAsync();
            var courseDto = await CreateCourseAsync();
            var locationDto = await CreateLocationWithInstructorAsync(instructorDto);

            var courseClassDto = await CreateCourseClassAsync(courseDto, locationDto);
            var subjectDto = await CreateSubjectAsync(instructorDto);
            var classSubjectDto = await CreateClassSubjectAsync(subjectDto, courseClassDto);

            
            var created = await ExecuteDbContextAsync(db =>
            db.ClassSubjects.Where(c => c.Id.Equals(classSubjectDto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            //created.Title.ShouldBe(createSubjectClassCommand.Title);
            created.CourseClassId.ShouldBe(courseClassDto.Id);
        }
    }
}
