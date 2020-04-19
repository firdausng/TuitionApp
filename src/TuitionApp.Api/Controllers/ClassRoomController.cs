using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Classroom;

namespace TuitionApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassRoomController : ControllerBase
    {
        private readonly IMediator mediator;
        public ClassRoomController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = "GetClassrooms")]
        public async Task<IActionResult> GetClassroomList()
        {
            var vm = await mediator.Send(new GetSubjectListQuery());
            return Ok(vm);
        }

        [HttpGet("{classroomId}", Name = "GetClassroomItem")]
        public async Task<ActionResult<GetSubjectDto>> GetClassroomItem(Guid classroomId)
        {
            var vm = await mediator.Send(new GetSubjectQuery() { Id = classroomId });

            return Ok(vm);
        }

        [HttpPost(Name = "NewClassroom")]
        public async Task<ActionResult<Guid>> NewClassroom(CreateClassroomCommand itemCommand)
        {
            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetClassroomItem", new { classroomId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}