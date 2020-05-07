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

namespace TuitionApp.Core.Features.Subjects
{
    public class GetSubjectListQuery : IRequest<GetObjectListVm<GetSubjectItemDto>>
    {
        public class QueryHandler : IRequestHandler<GetSubjectListQuery, GetObjectListVm<GetSubjectItemDto>>
        {
            private readonly IApplicationDbContext context;

            public QueryHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<GetObjectListVm<GetSubjectItemDto>> Handle(GetSubjectListQuery request, CancellationToken cancellationToken)
            {
                var subjectList = await context.Subjects
                    .ToListAsync(cancellationToken);

                var list = subjectList
                    .Select(entity => new GetSubjectItemDto
                    {
                        Id = entity.Id,
                        Title = entity.Title,
                    }).ToList();


                var dto = new GetObjectListVm<GetSubjectItemDto>
                {
                    Count = list.Count,
                    Data = list
                };

                return dto;
            }
        }
    }
}
