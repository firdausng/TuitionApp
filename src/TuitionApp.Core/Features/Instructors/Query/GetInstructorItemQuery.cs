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

namespace TuitionApp.Core.Features.Instructors
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
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(Instructor), request.Id);
                }

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
