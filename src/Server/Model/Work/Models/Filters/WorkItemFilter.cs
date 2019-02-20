using System.Collections.Generic;
using odec.Framework.Generic;
using odec.Server.Model.Work.Models.Helpers;

using Categ = odec.Server.Model.Category.Category;
namespace odec.Server.Model.Work.Models.Filters
{
    public class WorkItemFilter: FilterBase
    {
        public int? CustomerId { get; set; }
        public int? ExecutorId { get; set; }
        public int? WorkTypeId { get; set; }
        public bool IncludeFeedbacks { get; set; }
        public IEnumerable<Categ> Categories { get; set; }
        public CategoryOperation CategoryOperation { get; set; }
    }

}
