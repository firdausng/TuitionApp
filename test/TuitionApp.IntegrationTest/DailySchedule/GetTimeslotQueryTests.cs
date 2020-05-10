using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.DailySchedules.Timeslots;
using TuitionApp.Core.Features.Locations;
using TuitionApp.Core.Features.DailySchedules;
using Xunit;
using TuitionApp.Core.Features.Locations.Classrooms;
using TuitionApp.Core.Features.Courses.ClassSubjects;
using TuitionApp.Core.Features.Subjects;
using TuitionApp.Core.Features.Courses.CourseClasses;
using TuitionApp.Core.Features.Instructors;
using TuitionApp.Core.Common.Extensions;
using System.Collections.Generic;

namespace TuitionApp.IntegrationTest.DailySchedule.Timeslot
{
    using static SliceFixture;
    public class GetTimeslotQueryTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetTimeslotItem()
        {
            var instructorDto = await CreateInstructorAsync();
            var courseDto = await CreateCourseAsync();
            var locationDto = await CreateLocationWithInstructorAsync(instructorDto);
            var classroomDto = await CreateClassroomAsync(locationDto);
            var dailyScheduleDto = await CreateDailyScheduleAsync(classroomDto);
            var courseClassDto = await CreateCourseClassAsync(courseDto, locationDto);
            var subjectDto = await CreateSubjectAsync(instructorDto);
            var classSubjectDto = await CreateClassSubjectAsync(subjectDto, courseClassDto);

            var command = new CreateTimeslotItemCommand()
            {
                Disabled = false,
                Duration = new TimeSpan(1, 0, 0),
                StartTime = new TimeSpan(0, 22, 0),
                DailyScheduleId = dailyScheduleDto.Id,
                ClassSubjectId = classSubjectDto.Id,
            };
            var timeslotDto = await SendAsync(command);

            GetTimeslotItemQuery query = new GetTimeslotItemQuery() { Id = timeslotDto.Id };
            GetTimeslotItemDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Timeslots.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.DailyScheduleId.ShouldBe(created.DailyScheduleId);
            dto.StartTime.ShouldBe(created.StartTime);
            dto.Duration.ShouldBe(created.Duration);
            dto.Disabled.ShouldBe(created.Disabled);
        }


        [Fact]
        public async Task ShouldGetTimeslotList()
        {
            var instructorDto = await CreateInstructorAsync();
            var courseDto = await CreateCourseAsync();
            var locationDto = await CreateLocationWithInstructorAsync(instructorDto);
            var classroomDto = await CreateClassroomAsync(locationDto);
            var dailyScheduleDto = await CreateDailyScheduleAsync(classroomDto);
            var courseClassDto = await CreateCourseClassAsync(courseDto, locationDto);
            var subjectDto = await CreateSubjectAsync(instructorDto);
            var classSubjectDto = await CreateClassSubjectAsync(subjectDto, courseClassDto);

            var command = new CreateTimeslotItemCommand()
            {
                Disabled = false,
                Duration = new TimeSpan(1, 0, 0),
                StartTime = new TimeSpan(0, 22, 0),
                DailyScheduleId = dailyScheduleDto.Id,
                ClassSubjectId = classSubjectDto.Id,
            };
            var timeslotDto = await SendAsync(command);

            GetTimeslotListQuery query = new GetTimeslotListQuery() { };
            var dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db =>
            db.Timeslots.Where(c => c.Id.Equals(timeslotDto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }

        private async Task<CreateClassroomFromLocationDto> CreateClassroomAsync(CreateLocationItemDto locationDto)
        {
            return await SendAsync(new CreateClassroomFromLocationCommand
            {
                IsEnabled = true,
                Name = "Classroom1",
                Capacity = 40,
                LocationId = locationDto.Id
            });
        }

        private async Task<CreateDailyScheduleItemDto> CreateDailyScheduleAsync(CreateClassroomFromLocationDto classroomDto)
        {
            var scheduleDate = DateTime.UtcNow.Date;
            var command = new CreateDailyScheduleItemCommand()
            {
                DayOfWeek = DayOfWeek.Monday,
                Disabled = false,
                WeekNumber = 3,
                DateSchedule = scheduleDate,
                ClassroomId = classroomDto.Id,
            };
            var dto = await SendAsync(command);
            return dto;
        }

        private async Task<CreateInstructorItemDto> CreateInstructorAsync()
        {
            return await SendAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first",
                LastName = "last",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            });
        }

        private async Task<CreateLocationItemDto> CreateLocationWithInstructorAsync(CreateInstructorItemDto instructorDto)
        {
            var locationDto = await SendWithValidationAsync(new CreateLocationItemCommand()
            {
                IsEnabled = true,
                Name = "location1",
                Address = "address1",
                OpeningTime = new TimeSpan(0, 19, 0),
                ClosingTime = new TimeSpan(0, 21, 0),
                InstructorLists = new List<Guid> { instructorDto.Id },
            }, new CreateLocationItemCommandValidator());
            return locationDto;
        }

        private async Task<CreateCourseItemDto> CreateCourseAsync()
        {
            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            return await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());
        }

        private async Task<CreateCourseClassItemDto> CreateCourseClassAsync(CreateCourseItemDto courseItemDto, CreateLocationItemDto locationItemDto)
        {
            var createCourseClassCommand = new CreateCourseClassItemCommand()
            {
                Name = $"{courseItemDto.Id}-class1",
                CourseId = courseItemDto.Id,
                LocationId = locationItemDto.Id,
                Capacity = 40,
            };
            return await SendWithValidationAsync(createCourseClassCommand, new CreateCourseClassItemCommandValidator());
        }

        private async Task<CreateSubjectDto> CreateSubjectAsync(CreateInstructorItemDto instructorDto)
        {
            var createSubjectItemCommand = new CreateSubjectItemCommand()
            {
                Title = "Subject1",
                InstructorList = new List<Guid> { instructorDto.Id },
            };
            return await SendWithValidationAsync(createSubjectItemCommand,
                new CreateSubjectItemCommandValidator());
        }

        private async Task<CreateClassSubjectItemDto> CreateClassSubjectAsync(CreateSubjectDto subjectDto, CreateCourseClassItemDto courseClassItemDto)
        {
            var getSubjectItemDto = await SendAsync(new GetSubjectItemQuery() { Id = subjectDto.Id });

            var createClassSubjectCommand = new CreateClassSubjectItemCommand()
            {
                Title = $"{courseClassItemDto.Id}-subject1",
                CourseClassId = courseClassItemDto.Id,
                SubjectAssignmentId = getSubjectItemDto.SubjectAssignmentList.First(),
            };

            var createClassSubjectDto = await SendWithValidationAsync(createClassSubjectCommand, new CreateClassSubjectItemCommandValidator());
            return createClassSubjectDto;
        }

    }
}
