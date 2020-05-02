using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Courses;
using TuitionApp.Core.Features.Courses.Sessions;

namespace TuitionApp.Api.Controllers
{
    [Route("api/{courseId}/session")]
    [ApiController]
    public class CourseSessionController : ControllerBase
    {
        private readonly IMediator mediator;

        public CourseSessionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = "GetSessionListFromCourse")]
        public async Task<ActionResult<GetObjectListVm<GetSessionItemFromCourseDto>>> GetSessionListFromCourse(Guid courseId)
        {
            var vm = await mediator.Send(new GetSessionListFromCourseQuery() { CourseId = courseId });
            return Ok(vm);
        }

        [HttpGet("{SessionId}", Name = "GetSessionItemFromCourse")]
        public async Task<ActionResult<GetSessionItemFromCourseDto>> GetSessionItemFromCourse(Guid SessionId, Guid courseId)
        {
            var vm = await mediator.Send(new GetSessionItemFromCourseQuery() { Id = SessionId, CourseId = courseId });
            return Ok(vm);
        }

        [HttpPost(Name = "CreateSessionFromCourse")]
        public async Task<ActionResult<Guid>> CreateSessionFromCourse(Guid courseId)
        {
            var itemCommand = new CreateSessionFromCourseCommand
            {
                CourseId = courseId
            };

            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetSessionItemFromCourse", new { SessionId = vm.Id, courseId });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}