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

namespace TuitionApp.Core.Features.Instructors
{
    public class GetInstructorListQuery : IRequest<GetObjectListVm<GetInstructorItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetInstructorListQuery, GetObjectListVm<GetInstructorItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetInstructorItemDto>> Handle(GetInstructorListQuery request, CancellationToken cancellationToken)
            {
                var instructors = await context.Instructor
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var list = instructors
                    .Select(entity => new GetInstructorItemDto
                    {
                        Id = entity.Id,
                        Name = entity.FullName(),
                    }).ToList();


                var dto = new GetObjectListVm<GetInstructorItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
