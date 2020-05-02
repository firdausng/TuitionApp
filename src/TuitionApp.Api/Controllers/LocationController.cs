using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Locations;

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

        [HttpGet(Name = "GetLocationList")]
        public async Task<ActionResult<GetObjectListVm<GetLocationDto>>> GetLocationList()
        {
            var vm = await mediator.Send(new GetLocationListQuery());
            return Ok(vm);
        }

        [HttpGet("{locationId}", Name = "GetLocationItem")]
        public async Task<ActionResult<GetLocationDto>> GetLocationItem(Guid locationId)
        {
            var vm = await mediator.Send(new GetLocationItemQuery() { Id = locationId });

            return Ok(vm);
        }

        [HttpPost(Name = "NewLocationItem")]
        public async Task<ActionResult<Guid>> NewLocationItem(CreateLocationItemCommand itemCommand)
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