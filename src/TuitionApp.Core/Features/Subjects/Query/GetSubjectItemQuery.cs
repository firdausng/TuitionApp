using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Subjects
{
    public class GetSubjectItemQuery : IRequest<GetSubjectItemDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetSubjectItemQuery, GetSubjectItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetSubjectItemDto> Handle(GetSubjectItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Subjects
                    .Include(s => s.SubjectAssignments)
                    .SingleOrDefaultAsync(t => t.Id.Equals(request.Id), cancellationToken);

                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(Subjects), request.Id);
                }

                var dto = new GetSubjectItemDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                    SubjectAssignmentList = entity.SubjectAssignments.Select(sa => sa.Id).ToList(),
                };

                return dto;
            }
        }

    }
}
