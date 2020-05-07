using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.Courses.CourseSubjects
{
    public class CreateCourseSubjectItemCommand : IRequest<CreateCourseSubjectItemDto>
    {
        public string Title { get; set; }
        public Guid CourseId { get; set; }
        public Guid SubjectAssignmentId { get; set; }

        public class CommandHandler : IRequestHandler<CreateCourseSubjectItemCommand, CreateCourseSubjectItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateCourseSubjectItemDto> Handle(CreateCourseSubjectItemCommand request, CancellationToken cancellationToken)
            {
                var course = await context.Courses
                    .SingleOrDefaultAsync(l => l.Id.Equals(request.CourseId));
                if (course == null)
                {
                    throw new EntityNotFoundException(nameof(Course), request.CourseId);
                }

                var subjectAssignment = await context.SubjectAssignments
                    .SingleOrDefaultAsync(l => l.Id.Equals(request.SubjectAssignmentId));
                if (subjectAssignment == null)
                {
                    throw new EntityNotFoundException(nameof(SubjectAssignment), request.SubjectAssignmentId);
                }

                var entity = new CourseSubject()
                {
                    Title = request.Title,
                    Course = course,
                    SubjectAssignment = subjectAssignment,
                };
                context.CourseSubjects.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateCourseSubjectItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
