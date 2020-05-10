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
    public class UpdateAttendanceItemCommand : IRequest<CreateAttendanceDto>
    {
        public Guid Id { get; set; }
        public AttendanceStatus AttendanceStatus { get; set; }

        public class CommandHandler : IRequestHandler<UpdateAttendanceItemCommand, CreateAttendanceDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateAttendanceDto> Handle(UpdateAttendanceItemCommand request, CancellationToken cancellationToken)
            {
                var entity = await context.Attendances
                    .SingleOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

                if (entity == null)
                {
                    throw new EntityNotFoundException(nameof(Attendance), request.Id);
                }

                entity.AttendanceStatus = request.AttendanceStatus;

                context.Attendances.Update(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateAttendanceDto
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateAttendanceDto
    {
        public Guid Id { get; set; }
    }
}
