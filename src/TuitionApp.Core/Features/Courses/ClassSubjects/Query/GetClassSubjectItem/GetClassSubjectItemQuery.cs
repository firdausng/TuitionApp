using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.Courses.ClassSubjects
{
    public class GetClassSubjectItemQuery : IRequest<GetClassSubjectItemDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetClassSubjectItemQuery, GetClassSubjectItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetClassSubjectItemDto> Handle(GetClassSubjectItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.ClassSubjects
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(ClassSubject), request.Id);
                }

                var dto = new GetClassSubjectItemDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    CourseId = entity.CourseClassId,
                    SubjectAssignmentId = entity.SubjectAssignmentId,
                };

                return dto;
            }
        }

    }
}
