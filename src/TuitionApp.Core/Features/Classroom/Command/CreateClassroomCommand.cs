using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Classroom
{
    public class CreateClassroomCommand : IRequest<CreateClassroomItemDto>
    {
        public string Name { get; set; }
        public Guid LocationId { get; set; }
        public Guid SubjectId { get; set; }
        public int Capacity { get; set; }

        public class CommandHandler : IRequestHandler<CreateClassroomCommand, CreateClassroomItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateClassroomItemDto> Handle(CreateClassroomCommand request, CancellationToken cancellationToken)
            {
                var entity = new Domain.Entities.Classroom()
                {
                    Name = request.Name,
                    Capacity = request.Capacity,
                    LocationId = request.LocationId,
                    SubjectId = request.SubjectId,
                };

                context.Classrooms.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateClassroomItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateClassroomItemDto
    {
        public Guid Id { get; set; }
    }
}
