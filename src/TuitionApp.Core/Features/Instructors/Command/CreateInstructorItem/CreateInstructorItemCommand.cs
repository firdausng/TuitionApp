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
        public ICollection<Guid> CourseList { get; set; } = new List<Guid>();
        public ICollection<Guid> SessionList { get; set; } = new List<Guid>();
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

                if (request.CourseList.Count > 0)
                {
                    await AddCourseListAsync(entity, request.CourseList);
                }

                if (request.SessionList.Count > 0)
                {
                    await AddSessionListAsync(entity, request.SessionList);
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

            private async Task AddCourseListAsync(Instructor entity, ICollection<Guid> courseList)
            {
                var getCourseListDbQuery = context.Courses
                        .Include(i => i.InstructorCourses)
                        .OrderBy(i => i.Name)
                        .Where(i => courseList.Contains(i.Id));
                var courseEntities = await getCourseListDbQuery.ToListAsync();

                if (courseEntities.Count != courseList.Count)
                {
                    throw new EntityListCountMismatchException<Course>(courseEntities, courseList);
                }
                foreach (var courseEntity in courseEntities)
                {
                    var ic = new InstructorCourse
                    {
                        Instructor = entity,
                        Course = courseEntity,
                    };
                    courseEntity.InstructorCourses.Add(ic);
                }
            }

            private async Task AddSessionListAsync(Instructor entity, ICollection<Guid> sessionList)
            {
                var getSessionListDbQuery = context.Sessions
                        .Include(i => i.InstructorSessions)
                        //.OrderBy(i => i.)
                        .Where(i => sessionList.Contains(i.Id));
                var sessionEntities = await getSessionListDbQuery.ToListAsync();

                if (sessionEntities.Count != sessionList.Count)
                {
                    throw new EntityListCountMismatchException<Session>(sessionEntities, sessionList);
                }
                foreach (var sessionEntity in sessionEntities)
                {
                    var ic = new InstructorSession
                    {
                        Instructor = entity,
                        Session = sessionEntity,
                    };
                    sessionEntity.InstructorSessions.Add(ic);
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
