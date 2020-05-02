using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TuitionApp.Core.Common.Exceptions;
using TuitionApp.Core.Common.Interfaces;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Features.Location
{
    public class CreateLocationItemCommand : IRequest<CreateLocationItemDto>
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public List<Guid> InstructorLists { get; set; } = new List<Guid>();
        public string Description { get; set; }
        public string Address { get; set; }

        public class CommandHandler : IRequestHandler<CreateLocationItemCommand, CreateLocationItemDto>
        {
            private readonly IApplicationDbContext context;
            public CommandHandler(IApplicationDbContext context)
            {
                this.context = context;
            }

            public async Task<CreateLocationItemDto> Handle(CreateLocationItemCommand request, CancellationToken cancellationToken)
            {
                var entity = new Domain.Entities.Location()
                {
                    Name = request.Name,
                    IsEnabled = request.IsEnabled,
                    OpeningTime = request.OpeningTime,
                    ClosingTime = request.ClosingTime,
                    Description = request.Description,
                    Address = request.Address,

                };
                await context.Locations.AddAsync(entity);


                if (request.InstructorLists.Count > 0)
                {
                    var getInstructorListDbQuery = context.Instructor
                        .Include(i => i.LocationInstructors)
                        .OrderBy(i => i.FirstName)
                        .Where(i => request.InstructorLists.Contains(i.Id));
                    var instructorEntities = await getInstructorListDbQuery.ToListAsync();

                    if (instructorEntities.Count != request.InstructorLists.Count)
                    {
                        throw new EntityListCountMismatchException<Domain.Entities.Instructor>(instructorEntities, request.InstructorLists);
                    }
                    foreach (var instructorEntity in instructorEntities)
                    {
                        var li = new LocationInstructor
                        {
                            Instructor = instructorEntity,
                            Location = entity,
                        };
                        instructorEntity.LocationInstructors.Add(li);
                    }
                }

                await context.SaveChangesAsync(cancellationToken);

                return new CreateLocationItemDto
                {
                    Id = entity.Id
                };
            }
        }
    }
}
