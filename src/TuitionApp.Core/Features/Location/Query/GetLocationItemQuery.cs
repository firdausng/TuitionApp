using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Location
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
                var location = await context.Locations
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken)
                    ;

                var dto = new GetLocationDto
                {
                    Id = location.Id,
                    Name = location.Name,
                    IsEnabled = location.IsEnabled
                };

                return dto;
            }
        }

    }
}
