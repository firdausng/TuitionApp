﻿using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Student;
using Xunit;

namespace TuitionApp.IntegrationTest.Student
{
    using static SliceFixture;
    public class CreateStudentTests: IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateStudent()
        {
            var command = new CreateStudentItemCommand()
            {
                FirstName = "first",
                LastName = "last"
            };
            var dto = await SendAsync(command);

            var created = await ExecuteDbContextAsync(db =>
            db.Students.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());


            created.ShouldNotBeNull();
            created.FirstName.ShouldBe(command.FirstName);
            created.LastName.ShouldBe(command.LastName);
        }
    }
}
