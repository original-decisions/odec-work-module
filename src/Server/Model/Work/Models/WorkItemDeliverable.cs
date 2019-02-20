using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Attach = odec.Server.Model.Attachment.Attachment;
namespace odec.Server.Model.Work.Models
{
    public class WorkItemDeliverable
    {
        [Key, Column(Order = 1)]
        public int DeliverableId { get; set; }
        public Attach Deliverable { get; set; }
        [Key, Column(Order = 0)]
        public int WorkItemId { get; set; }
        public bool IsPublic { get; set; }
    }
}