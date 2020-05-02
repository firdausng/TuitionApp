using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Enrollments;

namespace TuitionApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IMediator mediator;
        public EnrollmentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = "GetEnrollmentList")]
        public async Task<ActionResult<GetObjectListVm<GetEnrollmentItemDto>>> GetEnrollmentList()
        {
            var vm = await mediator.Send(new GetEnrollmentListQuery());
            return Ok(vm);
        }

        [HttpGet("{enrollmentId}", Name = "GetEnrollmentItem")]
        public async Task<ActionResult<GetEnrollmentItemDto>> GetEnrollmentItem(Guid enrollmentId)
        {
            var vm = await mediator.Send(new GetEnrollmentItemQuery() { Id = enrollmentId });

            return Ok(vm);
        }

        [HttpPost(Name = "CreatEnrollmentItem")]
        public async Task<ActionResult<Guid>> CreatEnrollmentItem(CreateEnrollmentItemCommand itemCommand)
        {
            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetEnrollmentItem", new { enrollmentId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}