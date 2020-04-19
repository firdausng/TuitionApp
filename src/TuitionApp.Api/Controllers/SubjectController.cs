using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TuitionApp.Core.Features.Subject;

namespace TuitionApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly IMediator mediator;
        public SubjectController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{subjectId}", Name = "GetSubjectItem")]
        public async Task<ActionResult<GetSubjectDto>> GetLocationItem(Guid subjectId)
        {
            var vm = await mediator.Send(new GetSubjectQuery() { Id = subjectId });

            return Ok(vm);
        }

        [HttpPost(Name = "NewSubject")]
        public async Task<ActionResult<Guid>> NewSubject(CreateSubjectCommand itemCommand)
        {
            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetSubjectItem", new { subjectId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}