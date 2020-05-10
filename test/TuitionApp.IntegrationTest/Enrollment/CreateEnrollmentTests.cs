using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.CourseClasses;
using TuitionApp.Core.Features.Enrollments;
using TuitionApp.Core.Features.Locations;
using TuitionApp.Core.Features.Students;
using Xunit;

namespace TuitionApp.IntegrationTest.Enrollment
{
    using static SliceFixture;
    public class CreateEnrollmentTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateEnrollment()
        {
            var studentDto = await CreateStudentAsync();
            var courseClassDto = await CreateCourseClassAsync("ShouldCreateEnrollment");

            var command = new CreateEnrollmentItemCommand()
            {
                StartDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
                StudentId = studentDto.Id,
                CourseClassId = courseClassDto.Id,
            };
            var dto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db =>
                db.Enrollments.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());


            created.ShouldNotBeNull();
            created.StartDate.ShouldBe(command.StartDate);
            created.StudentId.ShouldBe(command.StudentId);
            created.CourseClassId.ShouldBe(command.CourseClassId);
        }

        [Fact]
        public async Task ShouldNotCreateEnrollmentWhenCapacityFull()
        {
            var capacity = 10;
            var studentDto = await CreateStudentAsync();
            var courseClassDto = await CreateCourseClassAsync("ShouldNotCreateEnrollmentWhenCapacityFull", capacity);

            for (int i = 0; i < capacity; i++)
            {
                await SendAsync(new CreateEnrollmentItemCommand()
                {
                    StartDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
                    StudentId = studentDto.Id,
                    CourseClassId = courseClassDto.Id,
                });
            }

            // assert
            await SendAsync(new CreateEnrollmentItemCommand()
            {
                StartDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
                StudentId = studentDto.Id,
                CourseClassId = courseClassDto.Id,
            }).ShouldThrowAsync<InvalidAppOperationException>();
        }

        private async Task<CreateCourseClassItemDto> CreateCourseClassAsync(string courseName, int classCapacity = 40)
        {
            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = courseName,
                Rate = 40,
            };
            var courseDto = await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());

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
                CourseId = courseDto.Id,
                LocationId = locationDto.Id,
                Capacity = classCapacity,
            };
            var courseClassDto = await SendWithValidationAsync(createCourseClassCommand, new CreateCourseClassItemCommandValidator());

            return courseClassDto;
        }

        private async Task<CreateStudentDto> CreateStudentAsync()
        {
            var studentDto = await SendAsync(new CreateStudentItemCommand()
            {
                FirstName = "first",
                LastName = "last"
            });
            return studentDto;
        }
    }
}
