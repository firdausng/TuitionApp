using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Features.Common;

namespace TuitionApp.Core.Features.Attendances
{
    public class GetAttendanceListQuery : IRequest<GetObjectListVm<GetAttendanceItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetAttendanceListQuery, GetObjectListVm<GetAttendanceItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetAttendanceItemDto>> Handle(GetAttendanceListQuery request, CancellationToken cancellationToken)
            {
                var calendarSettings = await context.Attendances
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var list = calendarSettings
                    .Select(entity => new GetAttendanceItemDto
                    {
                        Id = entity.Id,
                        AttendanceStatus = entity.AttendanceStatus,
                        EnrollmentId = entity.EnrollmentId,
                        TimeslotId = entity.TimeslotId,
                    }).ToList();


                var dto = new GetObjectListVm<GetAttendanceItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
