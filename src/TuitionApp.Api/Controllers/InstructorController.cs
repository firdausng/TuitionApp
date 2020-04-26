using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Instructor;

namespace TuitionApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly IMediator mediator;
        public InstructorController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = "GetInstructorList")]
        public async Task<ActionResult<GetObjectListVm<GetInstructorItemDto>>> GetInstructorList()
        {
            var vm = await mediator.Send(new GetInstructorListQuery());
            return Ok(vm);
        }

        [HttpGet("{InstructorId}", Name = "GetInstructorItem")]
        public async Task<ActionResult<GetInstructorItemDto>> GetInstructorItem(Guid InstructorId)
        {
            var vm = await mediator.Send(new GetInstructorItemQuery() { Id = InstructorId });

            return Ok(vm);
        }

        [HttpPost(Name = "CreateInstructorItem")]
        public async Task<ActionResult<Guid>> CreateInstructorItem(CreateInstructorItemCommand itemCommand)
        {
            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetInstructorItem", new { InstructorId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}