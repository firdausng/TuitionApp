using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Features.Common;

namespace TuitionApp.Core.Features.Classroom
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
            var classroomsFromDb = await context.Classrooms.ToListAsync();

            var classrooms = classroomsFromDb.Select(c =>
                {
                    return new GetSubjectDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Capacity = c.Capacity
                    };
                })
                .ToList();

            var dto = new GetObjectListVm<GetSubjectDto>
            {
                Data = classrooms,
                Count = classrooms.Count
            };

            return dto;
        }
    }
}
