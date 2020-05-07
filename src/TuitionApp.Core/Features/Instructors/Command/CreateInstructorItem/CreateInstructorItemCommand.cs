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

namespace TuitionApp.Core.Features.Instructors
{
    public class CreateInstructorItemCommand : IRequest<CreateInstructorItemDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HireDate { get; set; }
        public ICollection<Guid> SubjectList { get; set; } = new List<Guid>();
        public ICollection<Guid> LocationList { get; set; } = new List<Guid>();

        public class CommandHandler : IRequestHandler<CreateInstructorItemCommand, CreateInstructorItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateInstructorItemDto> Handle(CreateInstructorItemCommand request, CancellationToken cancellationToken)
            {
                var entity = new Instructor()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    HireDate = request.HireDate,

                };
                context.Instructor.Add(entity);

                if (request.SubjectList.Count > 0)
                {
                    await AddSubjectListAsync(entity, request.SubjectList);
                }

                if (request.LocationList.Count > 0)
                {
                    await AddLocationListAsync(entity, request.LocationList);
                }

                await context.SaveChangesAsync(cancellationToken);

                return new CreateInstructorItemDto
                {
                    Id = entity.Id
                };
            }

            private async Task AddSubjectListAsync(Instructor entity, ICollection<Guid> subjectList)
            {
                var getSubjectListDbQuery = context.Subjects
                        .OrderBy(i => i.Title)
                        .Where(i => subjectList.Contains(i.Id));
                var subjectEntities = await getSubjectListDbQuery.ToListAsync();

                if (subjectEntities.Count != subjectList.Count)
                {
                    throw new EntityListCountMismatchException<Subject>(subjectEntities, subjectList);
                }
                foreach (var subjectEntity in subjectEntities)
                {

                    entity.SubjectAssignments.Add(new SubjectAssignment
                    {
                        Instructor = entity,
                        Subject = subjectEntity,
                    });
                }
            }

            private async Task AddLocationListAsync(Instructor entity, ICollection<Guid> locationList)
            {
                var getLocationListDbQuery = context.Locations
                        .Include(i => i.LocationInstructors)
                        .OrderBy(i => i.Name)
                        .Where(i => locationList.Contains(i.Id));
                var locationEntities = await getLocationListDbQuery.ToListAsync();

                if (locationEntities.Count != locationList.Count)
                {
                    throw new EntityListCountMismatchException<Location>(locationEntities, locationList);
                }
                foreach (var locationEntity in locationEntities)
                {
                    var ic = new LocationInstructor
                    {
                        Instructor = entity,
                        Location = locationEntity,
                    };
                    locationEntity.LocationInstructors.Add(ic);
                }
            }
        }
    }
}
