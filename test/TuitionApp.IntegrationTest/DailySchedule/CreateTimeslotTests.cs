using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Locations;
using TuitionApp.Core.Features.DailySchedules;
using Xunit;
using TuitionApp.Core.Features.DailySchedules.Timeslots;
using TuitionApp.Core.Features.Locations.Classrooms;
using TuitionApp.Core.Features.Courses.ClassSubjects;
using TuitionApp.Core.Features.Subjects;
using TuitionApp.Core.Features.Instructors;
using System.Collections.Generic;
using TuitionApp.Core.Common.Extensions;
using TuitionApp.Core.Features.Courses.CourseClasses;

namespace TuitionApp.IntegrationTest.DailySchedule.Timeslot
{
    using static SliceFixture;
    public class CreateTimeslotTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateTimeslot()
        {
            var instructorDto = await CreateInstructorAsync();
            var courseDto = await CreateCourseAsync();
            var locationDto = await CreateLocationWithInstructorAsync(instructorDto);
           
            var courseClassDto = await CreateCourseClassAsync(courseDto, locationDto);
            var subjectDto = await CreateSubjectAsync(instructorDto);
            var classSubjectDto = await CreateClassSubjectAsync(subjectDto, courseClassDto);

            var classroomDto = await CreateClassroomAsync(locationDto);
            var dailyScheduleDto = await CreateDailyScheduleAsync(classroomDto);

            var command = new CreateTimeslotItemCommand
            {
                Disabled = false,
                DailyScheduleId = dailyScheduleDto.Id,
                ClassSubjectId = classSubjectDto.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            var timeslotDto = await SendWithValidationAsync(command, new CreateTimeslotItemCommandValidator());

            var created = await ExecuteDbContextAsync(db =>
                db.Timeslots.Where(c => c.Id.Equals(timeslotDto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.DailyScheduleId.ShouldBe(command.DailyScheduleId);
            created.StartTime.ShouldBe(command.StartTime);
            created.Disabled.ShouldBe(command.Disabled);
            created.Duration.ShouldBe(command.Duration);
        }

        [Fact()]
        public async Task ShouldNotCreateTimeslotWhenRoomTimeSlotAlreadyTaken()
        {
            var instructorDto = await CreateInstructorAsync();
            var courseDto = await CreateCourseAsync();
            var locationDto = await CreateLocationWithInstructorAsync(instructorDto);
            var classroomDto = await CreateClassroomAsync(locationDto);
            var dailyScheduleDto = await CreateDailyScheduleAsync(classroomDto);
            var courseClassDto = await CreateCourseClassAsync(courseDto, locationDto);
            var subjectDto = await CreateSubjectAsync(instructorDto);
            var classSubjectDto = await CreateClassSubjectAsync(subjectDto, courseClassDto);

            var command1st = new CreateTimeslotItemCommand
            {
                Disabled = false,
                DailyScheduleId = dailyScheduleDto.Id,
                ClassSubjectId = classSubjectDto.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            var timeslotDto1st = await SendAsync(command1st);

            var command2nd = new CreateTimeslotItemCommand
            {
                Disabled = false,
                DailyScheduleId = dailyScheduleDto.Id,
                ClassSubjectId = classSubjectDto.Id,
                Duration = new TimeSpan(0, 1, 0),
                StartTime = new TimeSpan(0, 20, 0),
            };
            await SendWithValidationAsync(command2nd, new CreateTimeslotItemCommandValidator()).ShouldThrowAsync<EntityAlreadyExistException>();
        }

    
    }
}
