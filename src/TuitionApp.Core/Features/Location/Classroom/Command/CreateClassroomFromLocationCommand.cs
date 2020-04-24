using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Location
{
    public class CreateClassroomFromLocationCommand : IRequest<CreateClassroomFromLocationDto>
    {
        public Guid LocationId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool IsEnabled { get; set; }

        public class CommandHandler : IRequestHandler<CreateClassroomFromLocationCommand, CreateClassroomFromLocationDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateClassroomFromLocationDto> Handle(CreateClassroomFromLocationCommand request, CancellationToken cancellationToken)
            {
                var location = await context.Locations.SingleOrDefaultAsync(l => l.Id.Equals(request.LocationId));
                if (location == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Location), request.LocationId);
                }

                var entity = new Domain.Entities.Classroom()
                {
                    Name = request.Name,
                    IsEnabled = request.IsEnabled,
                    Capacity = request.Capacity,
                    

                    Location = location,
                    
                };
                context.Classrooms.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateClassroomFromLocationDto
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateClassroomFromLocationDto
    {
        public Guid Id { get; set; }
    }
}
