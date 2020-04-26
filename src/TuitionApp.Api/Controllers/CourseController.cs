using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Course;

namespace TuitionApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMediator mediator;
        public CourseController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = "GetCourseList")]
        public async Task<ActionResult<GetObjectListVm<GetCourseItemDto>>> GetCourseList()
        {
            var vm = await mediator.Send(new GetCourseListQuery());
            return Ok(vm);
        }

        [HttpGet("{courseId}", Name = "GetCourseItem")]
        public async Task<ActionResult<GetCourseItemDto>> GetCourseItem(Guid courseId)
        {
            var vm = await mediator.Send(new GetCourseItemQuery() { Id = courseId });

            return Ok(vm);
        }

        [HttpPost(Name = "CreatCourseItem")]
        public async Task<ActionResult<Guid>> CreatCourseItem(CreateCourseItemCommand itemCommand)
        {
            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetCourseItem", new { courseId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}