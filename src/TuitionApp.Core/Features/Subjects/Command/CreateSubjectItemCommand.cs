using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.Subjects
{
    public class CreateSubjectItemCommand : IRequest<CreateSubjectDto>
    {
        public string Title { get; set; }
        public ICollection<Guid> InstructorList { get; set; } = new List<Guid>();

        public class CommandHandler : IRequestHandler<CreateSubjectItemCommand, CreateSubjectDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateSubjectDto> Handle(CreateSubjectItemCommand request, CancellationToken cancellationToken)
            {
                var entity = new Subject()
                {
                    Title = request.Title,
                };

                if (request.InstructorList.Count > 0)
                {
                    await AddInstructorListAsync(entity, request.InstructorList);
                }


                context.Subjects.Add(entity);
                await context.SaveChangesAsync(cancellationToken);

                return new CreateSubjectDto
                {
                    Id = entity.Id
                };
            }

            private async Task AddInstructorListAsync(Subject entity, ICollection<Guid> instructorList)
            {
                var getInstructorListDbQuery = context.Instructor
                        .OrderBy(i => i.FirstName)
                        .Where(i => instructorList.Contains(i.Id));
                var instructorEntities = await getInstructorListDbQuery.ToListAsync();

                if (instructorEntities.Count != instructorList.Count)
                {
                    throw new EntityListCountMismatchException<Instructor>(instructorEntities, instructorList);
                }

                var subjectAssignmentEntityList = new List<SubjectAssignment>();
                foreach (var instructorEntity in instructorEntities)
                {
                    entity.SubjectAssignments.Add(new SubjectAssignment
                    {
                        Instructor = instructorEntity,
                        Subject = entity,
                    });
                }
            }
        }
    }
}
