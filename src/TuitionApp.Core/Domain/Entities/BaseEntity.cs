using System;

namespace TuitionApp.Core.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
    }
}
