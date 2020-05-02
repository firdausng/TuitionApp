using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Locations.Classrooms
{
    public class GetClassroomItemFromLocationQuery : IRequest<GetClassroomFromLocationDto>
    {
        public Guid LocationId { get; set; }
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetClassroomItemFromLocationQuery, GetClassroomFromLocationDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetClassroomFromLocationDto> Handle(GetClassroomItemFromLocationQuery request, CancellationToken cancellationToken)
            {
                var location = await context.Locations.SingleOrDefaultAsync(l => l.Id.Equals(request.LocationId));
                if (location == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Location), request.LocationId);
                }

                var classroom = await context.Classrooms
                    .SingleOrDefaultAsync(x => x.LocationId.Equals(request.LocationId));

                if (classroom == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Classroom), request.Id);
                }

                var dto = new GetClassroomFromLocationDto
                {
                    Id = classroom.Id,
                    IsEnabled = classroom.IsEnabled,
                    Name = classroom.Name
                };

                return dto;
            }
        }

    }
}
