using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Features.Common;

namespace TuitionApp.Core.Features.Subject
{
    public class GetSubjectListQuery : IRequest<GetObjectListVm<GetSubjectDto>>
    {
    }

    public class GetClassroomListHandler : IRequestHandler<GetSubjectListQuery, GetObjectListVm<GetSubjectDto>>
    {
        private readonly IApplicationDbContext context;

        public GetClassroomListHandler(IApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<GetObjectListVm<GetSubjectDto>> Handle(GetSubjectListQuery request, CancellationToken cancellationToken)
        {
            var entities = await context.Subjects.ToListAsync();

            var list = entities.Select(c =>
                {
                    return new GetSubjectDto
                    {
                        Id = c.Id,
                        Title = c.Title,
                    };
                })
                .ToList();

            var dto = new GetObjectListVm<GetSubjectDto>
            {
                Data = list,
                Count = list.Count
            };

            return dto;
        }
    }
}
