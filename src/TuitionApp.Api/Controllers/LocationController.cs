using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Location;
using TuitionApp.Core.Features.Location.Query;

namespace TuitionApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IMediator mediator;
        public LocationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{locationId}", Name = "GetLocationItem")]
        public async Task<ActionResult<GetLocationDto>> GetLocationItem(Guid locationId)
        {
            var vm = await mediator.Send(new GetLocationQuery() { Id = locationId });

            return Ok(vm);
        }

        [HttpPost(Name = "NewLocation")]
        public async Task<ActionResult<Guid>> NewLocation(CreateLocationCommand itemCommand)
        {
            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetLocationItem", new { locationId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}