using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TuitionApp.Api.Models;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Location;

namespace TuitionApp.Api.Controllers
{
    [Route("api/{locationId}/Classroom")]
    [ApiController]
    public class LocationClassroomController : ControllerBase
    {
        private readonly IMediator mediator;

        public LocationClassroomController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = "GetClassroomList")]
        public async Task<ActionResult<GetObjectListVm<GetClassroomFromLocationDto>>> GetClassroomList(Guid locationId)
        {
            var vm = await mediator.Send(new GetClassroomListFromLocationQuery() { LocationId = locationId });
            return Ok(vm);
        }

        [HttpGet("{classroomId}", Name = "GetClassroomItem")]
        public async Task<ActionResult<GetClassroomFromLocationDto>> GetClassroomItem(Guid classroomId, Guid locationId)
        {
            var vm = await mediator.Send(new GetClassroomItemFromLocationQuery() { Id = classroomId, LocationId = locationId});
            return Ok(vm);
        }

        [HttpPost(Name = "NewClassroomItem")]
        public async Task<ActionResult<Guid>> NewClassroomItem(LocationClassroomCreateRequest request, Guid locationId)
        {
            var itemCommand = new CreateClassroomFromLocationCommand 
            { 
                Capacity = request.Capacity,
                IsEnabled = request. IsEnabled,
                Name = request.Name,
                LocationId = locationId
            };

            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetLocationItem", new { classroomId = vm.Id, locationId });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}