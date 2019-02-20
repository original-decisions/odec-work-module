using System.Collections.Generic;
using System.Threading.Tasks;
using odec.Entity.DAL.Interop;

namespace odec.Work.DAL.Interop
{
    public interface IWorkRepository<TKey, TWorkItem, TCategory, TFeedback, TTeamMember, TWorkItemDeliverable, TWorkType, TWorkItemFilter> : IEntityOperations<TKey, TWorkItem>, IActivatableEntity<TKey, TWorkItem>
        where TKey : struct
        where TWorkItem : class
    {
        IEnumerable<TWorkItem> Get(TWorkItemFilter filter);
        IEnumerable<TCategory> GetWorkItemCategories(TKey workItemId);
        IEnumerable<TCategory> GetWorkItemCategories(TWorkItem workItem);
        IEnumerable<TWorkItem> GetMilestones(TKey workItemId);
        IEnumerable<TWorkItem> GetMilestones(TWorkItem workItem);
        Task<IEnumerable<TWorkItem>> GetMilestonesAsync(TKey workItemId);
        Task<IEnumerable<TWorkItem>> GetMilestonesAsync(TWorkItem workItem);
        void AddWorkCategory(TWorkItem workItem, TCategory category);
        void RemoveWorkCategory(TWorkItem workItem, TCategory category);
        IEnumerable<TWorkItem> GetWorksByCategory(TCategory category);
        IEnumerable<TWorkItem> GetWorksByCategoriesUnion(IEnumerable<TCategory> categories);
        IEnumerable<TWorkItem> GetWorksByCategoriesCross(IEnumerable<TCategory> categories);
        IEnumerable<TWorkItem> GetWorkItemsByWorkType(TWorkType workType);
        TFeedback GetCustomerFeedback(TKey workItemId);
        TFeedback GetCustomerFeedback(TWorkItem workItem);
        void AddTeamMember(TKey teamMemberId, TWorkItem workItem, bool isTeamLeader = false);
        void RemoveTeamMember(TKey teamMemberId, TWorkItem workItem);
        void SelectTeamLeader(TKey teamMemberId, TWorkItem workItem);
        TTeamMember GetTeamLeader(TWorkItem workItem);
        TFeedback GetTeamFeedback(TKey workItemId);
        TFeedback GetTeamFeedback(TWorkItem workItem);
        TFeedback GetTeamMemberFeedback(TKey workItemId, TKey teamMemberId);
        IEnumerable<TTeamMember> GetTeams(TKey workItemId);
        IEnumerable<TTeamMember> GetTeams(TWorkItem workItem);
        IEnumerable<TWorkItemDeliverable> GetDeliverables(TKey workItemId, bool isOnlyPublic=true);
        IEnumerable<TWorkItemDeliverable> GetDeliverables(TWorkItem workItem, bool isOnlyPublic = true);
        void AddCustomerFeedBack(TWorkItem workItem, TFeedback feedback);
        void RemoveCustomerFeedBack(TWorkItem workItem, TFeedback feedback);
        void AddTeamFeedback(TWorkItem workItem, TFeedback feedback);
        void RemoveTeamFeedback(TWorkItem workItem, TFeedback feedback);
        void AddTeamMemberFeedback(TWorkItem workItem, TFeedback feedback, TKey teamMemberId);
        void RemoveTeamMemberFeedback(TWorkItem workItem, TFeedback feedback, TKey teamMemberId);
        void AddDeliverable(TWorkItem workItem, TWorkItemDeliverable deliverable, bool isPublic = false);
        void MakeDeliverablePublic(TWorkItem workItem, TWorkItemDeliverable deliverable);
        void MakeDeliverablePrivate(TWorkItem workItem, TWorkItemDeliverable deliverable);
        void RemoveDeliverable(TWorkItem workItem, TWorkItemDeliverable deliverable);
        bool IsDeadlineMet(TKey workItemId);


    }
}