using System;
using System.Collections.Generic;
using System.Text;

namespace TuitionApp.Core.Domain.Entities
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
