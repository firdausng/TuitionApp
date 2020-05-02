using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuitionApp.Core.Domain.Entities;

namespace TuitionApp.Core.Common.Exceptions
{
    public class EntityListCountMismatchException<T>: Exception, IAppException where T:BaseEntity
    {
        public string MismatchIdListText { get; set; }
        public EntityListCountMismatchException(ICollection<T> actualList, ICollection<Guid> providedList)
        {
            var actualIdList = actualList.Select(a => a.Id).ToList();
            var mismatchIdList = new List<Guid>();
            if (providedList.Count >= actualIdList.Count)
            {
                mismatchIdList = providedList.Where(l => !actualIdList.Contains(l)).ToList();
            }
            else
            {
                mismatchIdList = actualIdList.Where(l => !providedList.Contains(l)).ToList();
            }
            
            MismatchIdListText = string.Join(",", mismatchIdList);
        }

        public override string Message => $"These ids ({MismatchIdListText}) of type {typeof(T)} are not exist in db";
    }
}
