using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Course;
using TuitionApp.Core.Features.Location;
using TuitionApp.Core.Features.WeeklySchedule;
using TuitionApp.Core.Features.WeeklySchedule.Timeslot;
using Xunit;

namespace TuitionApp.IntegrationTest.WeeklySchedule
{
    using static SliceFixture;
    public class GetTimeslotQueryTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldGetWeeklyScheduleItem()
        {
            var weeklyScheduleDto = await CreateWeeklyScheduleAsync();
            var sessionDto = await CreateSessionAsync();


           var command = new CreateTimeslotItemCommand()
            {
                Disabled = false,
                Duration = new TimeSpan(1,0,0),
                StartTime = new TimeSpan(0,22,0),
                WeeklyScheduleId = weeklyScheduleDto.Id,
                SessionId = sessionDto.Id,
           };
            var timeslotDto = await SendAsync(command);
           
            GetTimeslotItemQuery query = new GetTimeslotItemQuery() { Id = timeslotDto.Id };
            GetTimeslotItemDto dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db => db.Timeslots.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            dto.ShouldNotBeNull();
            dto.Id.ShouldBe(created.Id);
            dto.SessionId.ShouldBe(created.SessionId);
            dto.WeeklyScheduleId.ShouldBe(created.WeeklyScheduleId);
            dto.StartTime.ShouldBe(created.StartTime);
            dto.Duration.ShouldBe(created.Duration);
            dto.Disabled.ShouldBe(created.Disabled);
        }


        [Fact]
        public async Task ShouldGetWeeklyScheduleList()
        {
            var weeklyScheduleDto = await CreateWeeklyScheduleAsync();
            var sessionDto = await CreateSessionAsync();


            var command = new CreateTimeslotItemCommand()
            {
                Disabled = false,
                Duration = new TimeSpan(1, 0, 0),
                StartTime = new TimeSpan(0, 22, 0),
                WeeklyScheduleId = weeklyScheduleDto.Id,
                SessionId = sessionDto.Id,
            };
            var timeslotDto = await SendAsync(command);

            GetTimeslotListQuery query = new GetTimeslotListQuery() {  };
            var dto = await SendAsync(query);

            var created = await ExecuteDbContextAsync(db =>
            db.Timeslots.Where(c => c.Id.Equals(timeslotDto.Id)).SingleOrDefaultAsync());


            dto.ShouldNotBeNull();
            dto.Count.ShouldBeGreaterThanOrEqualTo(1);
            dto.Data.ShouldContain(d => d.Id.Equals(created.Id));
        }


        private async Task<CreateWeeklyScheduleItem> CreateWeeklyScheduleAsync()
        {
            var locationDto = await SendAsync(new CreateLocationItemCommand
            {
                Name = "location1",
                IsEnabled = true
            });

            var classroomDto = await SendAsync(new CreateClassroomFromLocationCommand
            {
                IsEnabled = true,
                Name = "Classroom1",
                Capacity = 40,
                LocationId = locationDto.Id
            });

            var scheduleDate = DateTime.UtcNow.Date;
            var weeklyScheduleDto = await SendAsync(new CreateWeeklyScheduleItemCommand()
            {
                DayOfWeek = DayOfWeek.Monday,
                Disabled = false,
                WeekNumber = 3,
                DateSchedule = scheduleDate,
                ClassroomId = classroomDto.Id,
            });

            return weeklyScheduleDto;
        }

        private async Task<CreateSessionFromCourseDto> CreateSessionAsync()
        {
            var createCourseItemCommand = new CreateCourseItemCommand()
            {
                Name = "ShouldCreateSessionFromCourse",
                Rate = 40,
            };
            var CourseDto = await SendAsync(createCourseItemCommand);

            var SessionDto = await SendAsync(new CreateSessionFromCourseCommand()
            {
                CourseId = CourseDto.Id
            });
            return SessionDto;
        }
    }
}
