using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.Courses.CourseClasses
{
    public class CreateCourseClassItemCommand : IRequest<CreateCourseClassItemDto>
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public Guid CourseId { get; set; }
        public Guid LocationId { get; set; }

        public class CommandHandler : IRequestHandler<CreateCourseClassItemCommand, CreateCourseClassItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateCourseClassItemDto> Handle(CreateCourseClassItemCommand request, CancellationToken cancellationToken)
            {
                var course = await context.Courses
                    .SingleOrDefaultAsync(l => l.Id.Equals(request.CourseId));
                if (course == null)
                {
                    throw new EntityNotFoundException(nameof(Course), request.CourseId);
                }

                var location = await context.Locations
                    .SingleOrDefaultAsync(l => l.Id.Equals(request.LocationId));
                if (location == null)
                {
                    throw new EntityNotFoundException(nameof(Location), request.LocationId);
                }

                var entity = new CourseClass()
                {
                    Name = request.Name,
                    CourseId = course.Id,
                    LocationId = location.Id,
                    Capacity = request.Capacity,
                };
                context.CourseClasses.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateCourseClassItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
