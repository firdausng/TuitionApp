using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.Locations
{
    public class GetLocationItemQuery : IRequest<GetLocationDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetLocationItemQuery, GetLocationDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetLocationDto> Handle(GetLocationItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Locations
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(Location), request.Id);
                }

                var dto = new GetLocationDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    IsEnabled = entity.IsEnabled,
                    OpeningTime = entity.OpeningTime,
                    ClosingTime = entity.ClosingTime,
                };

                return dto;
            }
        }

    }
}
