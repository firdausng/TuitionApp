using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;

namespace TuitionApp.Core.Features.Enrollment
{
    public class CreateEnrollmentItemCommand : IRequest<CreateEnrollmentItemDto>
    {
        public DateTime StartDate { get; set; }
        public Guid StudentId { get; set; }
        public Guid SessionId { get; set; }

        public class CommandHandler : IRequestHandler<CreateEnrollmentItemCommand, CreateEnrollmentItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateEnrollmentItemDto> Handle(CreateEnrollmentItemCommand request, CancellationToken cancellationToken)
            {
                var student = await context.Students.SingleOrDefaultAsync(l => l.Id.Equals(request.StudentId));
                if (student == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Student), request.StudentId);
                }

                var Session = await context.Sessions.SingleOrDefaultAsync(l => l.Id.Equals(request.SessionId));
                if (Session == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Session), request.SessionId);
                }


                var entity = new Domain.Entities.Enrollment()
                {
                    StartDate = request.StartDate,
                    Student = student,
                    Session = Session,
                };
                context.Enrollments.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateEnrollmentItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
