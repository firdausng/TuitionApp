using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Location.Query
{
    public class GetLocationQuery : IRequest<GetLocationDto>
    {
        public Guid Id { get; set; }

        public class GetProjectItemQueryHandler : IRequestHandler<GetLocationQuery, GetLocationDto>
        {
            private readonly IApplicationDbContext context;

            public GetProjectItemQueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetLocationDto> Handle(GetLocationQuery request, CancellationToken cancellationToken)
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

    public class GetLocationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
