using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Course
{
    public class CreateSessionFromCourseCommand : IRequest<CreateSessionFromCourseDto>
    {
        public Guid CourseId { get; set; }

        public class CommandHandler : IRequestHandler<CreateSessionFromCourseCommand, CreateSessionFromCourseDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateSessionFromCourseDto> Handle(CreateSessionFromCourseCommand request, CancellationToken cancellationToken)
            {
                var course = await context.Courses.SingleOrDefaultAsync(l => l.Id.Equals(request.CourseId));
                if (course == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Course), request.CourseId);
                }

                var entity = new Domain.Entities.Session()
                {
                    Course = course,
                };
                context.Sessions.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateSessionFromCourseDto
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateSessionFromCourseDto
    {
        public Guid Id { get; set; }
    }
}
