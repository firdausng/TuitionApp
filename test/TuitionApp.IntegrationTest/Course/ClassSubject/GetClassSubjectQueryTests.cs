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
using TuitionApp.Core.Features.Locations;

namespace TuitionApp.IntegrationTest.Course.ClassSubject
{
    using static SliceFixture;
    public class GetClassSubjectQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetClassSubjectItem()
        {
            var instructorDto = await CreateInstructorAsync();
            var courseDto = await CreateCourseAsync();
            var locationDto = await CreateLocationWithInstructorAsync(instructorDto);

            var courseClassDto = await CreateCourseClassAsync(courseDto, locationDto);
            var subjectDto = await CreateSubjectAsync(instructorDto);
            var classSubjectDto = await CreateClassSubjectAsync(subjectDto, courseClassDto);

            var dto = await SendAsync(new GetClassSubjectItemQuery() { Id = classSubjectDto.Id });

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
            var instructorDto = await CreateInstructorAsync();
            var courseDto = await CreateCourseAsync();
            var locationDto = await CreateLocationWithInstructorAsync(instructorDto);

            var courseClassDto = await CreateCourseClassAsync(courseDto, locationDto);
            var subjectDto = await CreateSubjectAsync(instructorDto);
            var classSubjectDto = await CreateClassSubjectAsync(subjectDto, courseClassDto);

            var dto = await SendAsync(new GetClassSubjectListQuery());

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(classSubjectDto.Id));
        }
    }
}
