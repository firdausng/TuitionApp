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
        [Fact(Skip ="WIP - need to create dayslot test 1st")]
        public async Task ShouldCreateTimeslot()
        {
            
        }

        [Fact(Skip = "WIP - need to create dayslot test 1st")]
        public async Task ShouldNotCreateTimeslotWhenRoomTimeSlotAlreadyTaken()
        {
            
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

        private async Task<CreateSessionFromCourseDto> CreateSessionAsync()
        {
            var createCourseItemDto = await SendAsync(new CreateCourseItemCommand()
            {
                Name = "ShouldCreateSessionFromCourse",
                Rate = 40,
            });

            CreateSessionFromCourseDto dto = await SendAsync(new CreateSessionFromCourseCommand
            {
                CourseId = createCourseItemDto.Id
            });
            return dto;
        }
    }
}
