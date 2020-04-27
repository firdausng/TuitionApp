﻿using MediatR;
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
    public class CreateEnrollmentItemCommand : IRequest<CreateEnrollmentItem>
    {
        public DateTime StartDate { get; set; }
        public Guid StudentId { get; set; }
        public Guid TimetableId { get; set; }

        public class CommandHandler : IRequestHandler<CreateEnrollmentItemCommand, CreateEnrollmentItem>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateEnrollmentItem> Handle(CreateEnrollmentItemCommand request, CancellationToken cancellationToken)
            {
                var student = await context.Students.SingleOrDefaultAsync(l => l.Id.Equals(request.StudentId));
                if (student == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Student), request.StudentId);
                }

                var timetable = await context.Timetables.SingleOrDefaultAsync(l => l.Id.Equals(request.TimetableId));
                if (timetable == null)
                {
                    throw new EntityNotFoundException(nameof(Domain.Entities.Timetable), request.TimetableId);
                }


                var entity = new Domain.Entities.Enrollment()
                {
                    StartDate = request.StartDate,
                    Student = student,
                    Timetable = timetable,
                };
                context.Enrollments.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateEnrollmentItem
                {
                    Id = entity.Id
                };
            }
        }
    }

    public class CreateEnrollmentItem
    {
        public Guid Id { get; set; }
    }
}