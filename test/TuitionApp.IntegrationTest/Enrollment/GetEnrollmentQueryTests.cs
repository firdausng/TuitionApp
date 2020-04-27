﻿using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Course;
using TuitionApp.Core.Features.Enrollment;
using TuitionApp.Core.Features.Student;
using Xunit;

namespace TuitionApp.IntegrationTest.Enrollment
{
    using static SliceFixture;
    public class GetEnrollmentQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetEnrollmentItem()
        {
            var enrollmentDto = await CreateEnrollmentDtoAsync();


            GetEnrollmentItemQuery query = new GetEnrollmentItemQuery() { Id = enrollmentDto.Id };
            GetEnrollmentItemDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Enrollments.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.Grade.ShouldBe(created.Grade);
            dto.StudentId.ShouldBe(created.StudentId);
            dto.TimetableId.ShouldBe(created.TimetableId);
            dto.StartDate.ShouldBe(created.StartDate);
            dto.EndDate.ShouldBe(created.EndDate);
        }


        [Fact]
        public async Task ShouldGetEnrollmentList()
        {
            var enrollmentDto = await CreateEnrollmentDtoAsync();

            var created = await ExecuteDbContextAsync(db =>
            db.Enrollments.Where(c => c.Id.Equals(enrollmentDto.Id)).SingleOrDefaultAsync());

            GetEnrollmentListQuery query = new GetEnrollmentListQuery();
            GetObjectListVm<GetEnrollmentItemDto> dto = await SendAsync(query);


            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }


        private async Task<CreateEnrollmentItem>  CreateEnrollmentDtoAsync()
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
            return dto;
        }
    }
}