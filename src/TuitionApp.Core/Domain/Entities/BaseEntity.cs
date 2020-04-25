using System;

namespace TuitionApp.Core.Domain.Entities
{
    public abstract class BaseEntity: IEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
    }
}
