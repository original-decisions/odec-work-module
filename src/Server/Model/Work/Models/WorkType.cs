using System.Collections.Generic;
using odec.Framework.Generic;

namespace odec.Server.Model.Work.Models
{
    public class WorkType:Glossary<int>
    {
        public IList<WorkItem> WorkItems { get; set; }
    }
}