using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Subject
{
    public class GetSubjectQuery : IRequest<GetSubjectDto>
    {
        public Guid Id { get; set; }
        public class QueryHandler : IRequestHandler<GetSubjectQuery, GetSubjectDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetSubjectDto> Handle(GetSubjectQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Subjects
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetSubjectDto
                {
                    Id = entity.Id,
                    Title = entity.Title,
                };

                return dto;
            }
        }
    }
}
