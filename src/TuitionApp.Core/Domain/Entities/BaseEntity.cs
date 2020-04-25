using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class BaseEntity: IEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
    }
}
