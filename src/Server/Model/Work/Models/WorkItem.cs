using System;
using System.ComponentModel.DataAnnotations;
using odec.Framework.Generic;

namespace odec.Server.Model.Work.Models
{
    public class WorkItem:Glossary<int>
    {
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public WorkItem Parent { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public DateTime DeadLine { get; set; }
        public int CustomerId { get; set; }
        public User.User Customer { get; set; }
        public int? CustomerFeedbackId { get; set; }
        public int? TeamFeedBackId { get; set; }
        public WorkFeedback CustomerFeedback { get; set; }
        public WorkFeedback TeamFeedBack { get; set; }
        public decimal ActualCost { get; set; }
        public decimal InitialCost { get; set; }
        public int WorkTypeId { get; set; }
        public WorkType WorkType { get; set; }
        //public TYPE Type { get; set; }
    }
}