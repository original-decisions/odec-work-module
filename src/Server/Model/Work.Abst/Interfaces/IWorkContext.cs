using Microsoft.EntityFrameworkCore;

namespace odec.Server.Model.Work.Abstractions.Interfaces
{
    public interface IWorkContext<TWork, TCategory, TWorkCategory, TWorkFeedback, TWorkType, TWorkItemTeam, TWorkItemDeliverable> 
        where TCategory : class 
        where TWork : class 
        where TWorkCategory : class 
        where TWorkFeedback : class 
        where TWorkType : class 
        where TWorkItemTeam : class where TWorkItemDeliverable : class
    {
        DbSet<TWork> Works { get; set; }
        DbSet<TCategory> Categories { get; set; }
        DbSet<TWorkCategory> WorkCategories { get; set; }
        DbSet<TWorkFeedback> Feedbacks { get; set; }
        DbSet<TWorkType> WorkTypes { get; set; }
        DbSet<TWorkItemTeam>  WorkItemTeams { get; set; }
        DbSet<TWorkItemDeliverable> WorkItemDeliverables { get; set; } 

    }
}