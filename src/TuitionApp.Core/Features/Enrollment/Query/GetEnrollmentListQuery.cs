using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Features.Common;

namespace TuitionApp.Core.Features.Enrollment
{
    public class GetEnrollmentListQuery : IRequest<GetObjectListVm<GetEnrollmentItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetEnrollmentListQuery, GetObjectListVm<GetEnrollmentItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetEnrollmentItemDto>> Handle(GetEnrollmentListQuery request, CancellationToken cancellationToken)
            {
                var entities = await context.Enrollments
                    .ToListAsync(cancellationToken);

                var list = entities
                    .Select(entity => new GetEnrollmentItemDto
                    {
                        Id = entity.Id,
                        StartDate = entity.StartDate,
                        EndDate = entity.EndDate,
                        Grade = entity.Grade,
                        StudentId = entity.StudentId,
                        SessionId = entity.SessionId,
                    }).ToList();


                var dto = new GetObjectListVm<GetEnrollmentItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
