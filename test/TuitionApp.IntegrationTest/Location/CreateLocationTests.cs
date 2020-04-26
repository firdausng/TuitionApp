using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Api;
using TuitionApp.Core.Features.Location;
using Xunit;

namespace TuitionApp.IntegrationTest.LocationItem
{
    using static SliceFixture;
    public class CreateLocationTests : IntegrationTestBase
    {
        [Fact]
        public async Task ShouldCreateLocation()
        {
            CreateLocationItemDto dto = null;
            CreateLocationItemCommand command = null;

            await ExecuteDbContextAsync(async (ctxt, mediator) =>
            {
                //await ctxt.Locations.AddAsync(dept);
                command = new CreateLocationItemCommand()
                {
                    IsEnabled = true,
                    Name = "location1"
                };
                dto = await mediator.Send(command);
            });

            var created = await ExecuteDbContextAsync(db => db.Locations.Where(c => c.Id.Equals(dto.Id)).SingleOrDefaultAsync());

            created.ShouldNotBeNull();
            created.Name.ShouldBe(command.Name);
            created.IsEnabled.ShouldBe(command.IsEnabled);
        }
    }
}
