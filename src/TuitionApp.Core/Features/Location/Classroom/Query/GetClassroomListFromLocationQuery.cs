using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Features.Common;

namespace TuitionApp.Core.Features.Location
{
    public class GetClassroomListFromLocationQuery : IRequest<GetObjectListVm<GetClassroomFromLocationDto>>
    {
        public Guid LocationId { get; set; }

        public class QueryHandler : IRequestHandler<GetClassroomListFromLocationQuery, GetObjectListVm<GetClassroomFromLocationDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetClassroomFromLocationDto>> Handle(GetClassroomListFromLocationQuery request, CancellationToken cancellationToken)
            {
                var location = await context.Locations.SingleOrDefaultAsync(l => l.Id.Equals(request.LocationId));
                if (location == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Location), request.LocationId);
                }

                var classroom = await context.Classrooms
                    .Where(x => x.LocationId.Equals(request.LocationId))
                    .ToListAsync(cancellationToken)
                    ;

                var list = classroom
                    .Select(x => new GetClassroomFromLocationDto
                    {
                        Id = x.Id,
                        IsEnabled = x.IsEnabled,
                        Name = x.Name
                    }).ToList();

                var dto = new GetObjectListVm<GetClassroomFromLocationDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }

    }
}
