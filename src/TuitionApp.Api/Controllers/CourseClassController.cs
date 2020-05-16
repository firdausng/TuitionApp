using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Courses.CourseClasses;

namespace TuitionApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseClassController : ControllerBase
    {
        private readonly IMediator mediator;
        public CourseClassController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = "GetCourseClassList")]
        public async Task<ActionResult<GetObjectListVm<GetCourseClassItemDto>>> GetCourseClassList()
        {
            var vm = await mediator.Send(new GetCourseClassListQuery());
            return Ok(vm);
        }

        [HttpGet("{CourseClassId}", Name = "GetCourseClassItem")]
        public async Task<ActionResult<GetCourseClassItemDto>> GetCourseClassItem(Guid CourseClassId)
        {
            var vm = await mediator.Send(new GetCourseClassItemQuery() { Id = CourseClassId });
            return Ok(vm);
        }

        [HttpPost(Name = "CreateCourseClassFromCourse")]
        public async Task<ActionResult<Guid>> CreateCourseClassFromCourse(CreateCourseClassItemCommand itemCommand)
        {
            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetCourseClassItem", new { CourseClassId = vm.Id });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}
