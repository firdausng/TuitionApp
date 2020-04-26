using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Instructor
{
    public class GetInstructorItemQuery : IRequest<GetInstructorItemDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetInstructorItemQuery, GetInstructorItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetInstructorItemDto> Handle(GetInstructorItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Instructor
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                var dto = new GetInstructorItemDto
                {
                    Id = entity.Id,
                    Name = entity.FullName(),
                    HireDate = entity.HireDate,
                };

                return dto;
            }
        }

    }
}
