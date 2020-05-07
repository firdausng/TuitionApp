using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.Courses.ClassSubjects
{
    public class CreateClassSubjectItemCommand : IRequest<CreateClassSubjectItemDto>
    {
        public string Title { get; set; }
        public Guid CourseClassId { get; set; }
        public Guid SubjectAssignmentId { get; set; }

        public class CommandHandler : IRequestHandler<CreateClassSubjectItemCommand, CreateClassSubjectItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateClassSubjectItemDto> Handle(CreateClassSubjectItemCommand request, CancellationToken cancellationToken)
            {
                var courseClass = await context.CourseClasses
                    .SingleOrDefaultAsync(l => l.Id.Equals(request.CourseClassId));
                if (courseClass == null)
                {
                    throw new EntityNotFoundException(nameof(CourseClass), request.CourseClassId);
                }

                var subjectAssignment = await context.SubjectAssignments
                    .SingleOrDefaultAsync(l => l.Id.Equals(request.SubjectAssignmentId));
                if (subjectAssignment == null)
                {
                    throw new EntityNotFoundException(nameof(SubjectAssignment), request.SubjectAssignmentId);
                }

                var entity = new ClassSubject()
                {
                    Title = request.Title,
                    CourseClass = courseClass,
                    SubjectAssignment = subjectAssignment,
                };
                context.ClassSubjects.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateClassSubjectItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
