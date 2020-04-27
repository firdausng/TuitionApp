using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Features.Course;
using TuitionApp.Core.Features.Location;
using TuitionApp.Core.Features.Timeslot;
using Xunit;

namespace TuitionApp.IntegrationTest.Timeslot
{
    using static SliceFixture;
    public class CreateTimeslotTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateTimeslot()
        {
            var classroomDto = await CreateClassroomAsync();
            var timetableDto = await CreateTimetableAsync();

            var command = new CreateTimeslotItemCommand()
            {
                Day = 1,
                Week = 10,
                Time = new TimeSpan(2,0,0),
                roomId = classroomDto.Id,
                TimetableId = timetableDto.Id,
            };
            var dto = await SendAsync(command);
            
            var created = await ExecuteDbContextAsync(db =>
            db.Timeslots.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());


            created.ShouldNotBeNull();
            //created.FirstName.ShouldBe(command.FirstName);
            //created.LastName.ShouldBe(command.LastName);
        }

        [Fact]
        public async Task ShouldNotCreateTimeslotWhenRoomTimeSlotAlreadyTaken()
        {
            var classroomDto = await CreateClassroomAsync();
            var timetableDto = await CreateTimetableAsync();
            var timetableDto2 = await CreateTimetableAsync();


            var dto1st = await SendAsync(new CreateTimeslotItemCommand()
            {
                Day = 1,
                Week = 10,
                Time = new TimeSpan(2, 0, 0),
                roomId = classroomDto.Id,
                TimetableId = timetableDto2.Id,
            });

            var command = new CreateTimeslotItemCommand()
            {
                Day = 1,
                Week = 10,
                Time = new TimeSpan(2, 0, 0),
                roomId = classroomDto.Id,
                TimetableId = timetableDto.Id,
            };

            //assert
            await Should.ThrowAsync<EntityAlreadyExistException>(async () => await SendAsync(command));
        }

        private async Task<CreateClassroomFromLocationDto> CreateClassroomAsync()
        {
            var createLocationDto = await SendAsync(new CreateLocationItemCommand
            {
                Name = "location1",
                IsEnabled = true
            });

            CreateClassroomFromLocationCommand command = new CreateClassroomFromLocationCommand
            {
                IsEnabled = true,
                Name = "Classroom1",
                Capacity = 40,
                LocationId = createLocationDto.Id
            };
            CreateClassroomFromLocationDto dto = await SendAsync(command);
            return dto;
        }

        private async Task<CreateTimetableFromCourseDto> CreateTimetableAsync()
        {
            var createCourseItemDto = await SendAsync(new CreateCourseItemCommand()
            {
                Name = "ShouldCreateTimetableFromCourse",
                Rate = 40,
            });

            CreateTimetableFromCourseDto dto = await SendAsync(new CreateTimetableFromCourseCommand
            {
                CourseId = createCourseItemDto.Id
            });
            return dto;
        }
    }
}
