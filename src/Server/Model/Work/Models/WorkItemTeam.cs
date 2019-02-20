using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace odec.Server.Model.Work.Models
{
    public class WorkItemTeam
    {
       
        [Key, Column(Order = 0)]
        public int WorkItemId { get; set; }

        public WorkItem WorkItem { get; set; }

        [Key, Column(Order = 1)]
        public int ExecutorId { get; set; }
        public int? ExecutorFeedbackId { get; set; }
        public WorkFeedback ExecutorFeedback { get; set; }
        public User.User Executor { get; set; }
        public bool IsTeamLeader { get; set; }
    }
}