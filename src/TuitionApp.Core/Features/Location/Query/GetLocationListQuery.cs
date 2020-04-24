using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Features.Common;

namespace TuitionApp.Core.Features.Location
{
    public class GetLocationListQuery : IRequest<GetObjectListVm<GetLocationDto>>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetLocationListQuery, GetObjectListVm<GetLocationDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetLocationDto>> Handle(GetLocationListQuery request, CancellationToken cancellationToken)
            {
                var locations = await context.Locations
                    .ToListAsync(cancellationToken)
                    ;

                var list = locations
                    .Select(x => new GetLocationDto
                    {
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        Name = x.Name
                    }).ToList();


                var dto = new GetObjectListVm<GetLocationDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }

    }
}
