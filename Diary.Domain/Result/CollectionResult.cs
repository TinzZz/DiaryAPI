using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.Domain.Result
{
    public class CollectionResult<T> : BaseResult<IEnumerable<T>>
    {
        /* use this class to return collection of resuls. Use base class to return single item.*/
        public int Count { get; set; }
    }
}
