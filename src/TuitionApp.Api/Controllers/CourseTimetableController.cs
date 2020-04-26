using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TuitionApp.Api.Models;
using TuitionApp.Core.Features.Common;
using TuitionApp.Core.Features.Course;

namespace TuitionApp.Api.Controllers
{
    [Route("api/{courseId}/timetable")]
    [ApiController]
    public class CourseTimetableController : ControllerBase
    {
        private readonly IMediator mediator;

        public CourseTimetableController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet(Name = "GetTimetableListFromCourse")]
        public async Task<ActionResult<GetObjectListVm<GetTimetableItemFromCourseDto>>> GetTimetableListFromCourse(Guid courseId)
        {
            var vm = await mediator.Send(new GetTimetableListFromCourseQuery() { CourseId = courseId });
            return Ok(vm);
        }

        [HttpGet("{timetableId}", Name = "GetTimetableItemFromCourse")]
        public async Task<ActionResult<GetTimetableItemFromCourseDto>> GetTimetableItemFromCourse(Guid timetableId, Guid courseId)
        {
            var vm = await mediator.Send(new GetTimetableItemFromCourseQuery() { Id = timetableId, CourseId = courseId });
            return Ok(vm);
        }

        [HttpPost(Name = "CreateTimetableFromCourse")]
        public async Task<ActionResult<Guid>> CreateTimetableFromCourse(Guid courseId)
        {
            var itemCommand = new CreateTimetableFromCourseCommand
            {
                CourseId = courseId
            };

            var vm = await mediator.Send(itemCommand);
            if (vm.Id != null)
            {
                var link = Url.Link("GetTimetableItemFromCourse", new { classroomId = vm.Id, courseId });
                return Created(link, vm);
            }
            else
            {
                return BadRequest(vm);
            }

        }
    }
}