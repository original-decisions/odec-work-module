using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Categ = odec.Server.Model.Category.Category;
namespace odec.Server.Model.Work.Models
{
    public class WorkCategory
    {
     //   [Key, Column(Order = 1)]
        public int WorkItemId { get; set; }

        public WorkItem WorkItem { get; set; }
   //     [Key, Column(Order = 0)]
        public int CategoryId { get; set; }

        public Categ Category { get; set; }

    }
}