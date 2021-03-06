﻿using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.CourseClasses;
using TuitionApp.Core.Features.Locations;
using Xunit;

namespace TuitionApp.IntegrationTest.Course.CourseClass
{
    using static SliceFixture;
    public class CreateCourseClassTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateCourseClass()
        {
            var createCourseCommand = new CreateCourseItemCommand()
            {
                Name = "Course1",
                Rate = 40,
            };
            var createCourseDto = await SendWithValidationAsync(createCourseCommand, new CreateCourseItemCommandValidator());

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
                CourseId = createCourseDto.Id,
                LocationId = locationDto.Id,
                Capacity = 40,
            };
            var createCourseClassDto = await SendWithValidationAsync(createCourseClassCommand, new CreateCourseClassItemCommandValidator());

            var created = await ExecuteDbContextAsync(db =>
            db.CourseClasses.Where(c => c.Id.Equals(createCourseClassDto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Name.ShouldBe(createCourseClassCommand.Name);
            created.Capacity.ShouldBe(createCourseClassCommand.Capacity);
            created.CourseId.ShouldBe(createCourseClassCommand.CourseId);
        }
    }
}
