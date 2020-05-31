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

namespace TuitionApp.Core.Features.Attendances
{
    public class GetAttendanceItemQuery : IRequest<GetAttendanceItemDto>
    {
        public Guid Id { get; set; }

        public class QueryHandler : IRequestHandler<GetAttendanceItemQuery, GetAttendanceItemDto>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetAttendanceItemDto> Handle(GetAttendanceItemQuery request, CancellationToken cancellationToken)
            {
                var entity = await context.Attendances
                    .AsNoTracking()
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(Attendance), request.Id);
                }

                var dto = new GetAttendanceItemDto
                {
                    Id = entity.Id,
                    AttendanceStatus = entity.AttendanceStatus,
                    EnrollmentId = entity.EnrollmentId,
                    TimeslotId = entity.TimeslotId,
                };

                return dto;
            }
        }

    }
}
