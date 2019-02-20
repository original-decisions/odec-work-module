using System;
using odec.Framework.Generic;

namespace odec.Server.Model.Work.Models.Filters
{
    public class PortfolioFilter: FilterBase
    {
        public int? UserId { get; set; }
        public DateTime? FinishDateStart { get; set; }
        public DateTime? FinishDateEnd { get; set; }
    }
}
