using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.ClassSubjects;
using TuitionApp.Core.Features.Courses.CourseClasses;
using TuitionApp.Core.Features.DailySchedules;
using TuitionApp.Core.Features.DailySchedules.Timeslots;
using TuitionApp.Core.Features.Enrollments;
using TuitionApp.Core.Features.Instructors;
using TuitionApp.Core.Features.Locations;
using TuitionApp.Core.Features.Locations.Classrooms;
using TuitionApp.Core.Features.Students;
using TuitionApp.Core.Features.Subjects;
using Xunit;

namespace TuitionApp.IntegrationTest
{
    using static SliceFixture;
    public abstract class IntegrationTestBase : IAsyncLifetime
    {
        private static readonly AsyncLock _mutex = new AsyncLock();

        private static bool _initialized;

        public virtual async Task InitializeAsync()
        {
            if (_initialized)
                return;

            using (await _mutex.LockAsync())
            {
                if (_initialized)
                    return;

                await SliceFixture.ResetCheckpoint();

                _initialized = true;
            }
        }

        public virtual Task DisposeAsync() => Task.CompletedTask;


        protected async Task<CreateClassroomFromLocationDto> CreateClassroomAsync(CreateLocationItemDto locationDto)
        {
            return await SendAsync(new CreateClassroomFromLocationCommand
            {
                IsEnabled = true,
                Name = "Classroom1",
                Capacity = 40,
                LocationId = locationDto.Id
            });
        }

        protected async Task<CreateDailyScheduleItemDto> CreateDailyScheduleAsync(CreateClassroomFromLocationDto classroomDto)
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

        protected async Task<CreateInstructorItemDto> CreateInstructorAsync()
        {
            return await SendAsync(new CreateInstructorItemCommand()
            {
                FirstName = "first",
                LastName = "last",
                HireDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
            });
        }

        protected async Task<CreateLocationItemDto> CreateLocationWithInstructorAsync(CreateInstructorItemDto instructorDto)
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

        protected async Task<CreateCourseItemDto> CreateCourseAsync()
        {
            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            return await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());
        }

        protected async Task<CreateCourseClassItemDto> CreateCourseClassAsync(CreateCourseItemDto courseItemDto, CreateLocationItemDto locationItemDto)
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

        protected async Task<CreateSubjectDto> CreateSubjectAsync(CreateInstructorItemDto instructorDto)
        {
            var createSubjectItemCommand = new CreateSubjectItemCommand()
            {
                Title = "Subject1",
                InstructorList = new List<Guid> { instructorDto.Id },
            };
            return await SendWithValidationAsync(createSubjectItemCommand,
                new CreateSubjectItemCommandValidator());
        }

        protected async Task<CreateClassSubjectItemDto> CreateClassSubjectAsync(CreateSubjectDto subjectDto, CreateCourseClassItemDto courseClassItemDto)
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

        protected async Task<CreateTimeslotItemDto> CreateTimeslotAsync(CreateClassSubjectItemDto classSubjectItemDto, CreateDailyScheduleItemDto dailyScheduleItemDto)
        {

            var command = new CreateTimeslotItemCommand
            {
                Disabled = false,
                DailyScheduleId = dailyScheduleItemDto.Id,
                ClassSubjectId = classSubjectItemDto.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            var timeslotDto = await SendWithValidationAsync(command, new CreateTimeslotItemCommandValidator());
            return timeslotDto;
        }

        protected async Task<CreateStudentDto> CreateStudentAsync()
        {
            return await SendAsync(new CreateStudentItemCommand()
            {
                FirstName = "first",
                LastName = "last"
            });
        }

        protected async Task<CreateEnrollmentItemDto> CreateEnrollmentAsync(CreateCourseClassItemDto courseClassDto, CreateStudentDto studentDto)
        {
            var dto = await SendAsync(new CreateEnrollmentItemCommand()
            {
                StartDate = DateTime.UtcNow.DateTimeWithoutMilisecond(),
                StudentId = studentDto.Id,
                CourseClassId = courseClassDto.Id,
            });

            return dto;
        }

    }
}

