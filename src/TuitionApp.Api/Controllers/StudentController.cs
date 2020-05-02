using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Students;

namespace TuitionApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMediator mediator;
        public StudentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = "GetStudentList")]
        public async Task<ActionResult<GetObjectListVm<GetStudentItemDto>>> GetStudentList()
        {
            var vm = await mediator.Send(new GetStudentListQuery());
            return Ok(vm);
        }

        [HttpGet("{studentId}", Name = "GetStudentItem")]
        public async Task<ActionResult<GetStudentItemDto>> GetStudentItem(Guid studentId)
        {
            var vm = await mediator.Send(new GetStudentItemQuery() { Id = studentId });

            return Ok(vm);
        }

        [HttpPost(Name = "CreateStudentItem")]
        public async Task<ActionResult<Guid>> CreateStudentItem(CreateStudentItemCommand itemCommand)
        {
            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetStudentItem", new { studentId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}