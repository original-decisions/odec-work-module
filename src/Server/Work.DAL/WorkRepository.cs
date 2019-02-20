using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using odec.Entity.DAL;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Server.Model.Attachment;
using odec.Server.Model.Category;
using odec.Server.Model.Work.Models;
using odec.Server.Model.Work.Models.Filters;
using odec.Server.Model.Work.Models.Helpers;
using odec.Work.DAL.Interop;

namespace odec.Work.DAL
{
    public class WorkRepository :
        OrmEntityOperationsRepository<int, WorkItem, DbContext>,
        IWorkRepository<int, WorkItem, Category, WorkFeedback, WorkItemTeam, Attachment, WorkType, WorkItemFilter>,
        IContextRepository<DbContext>
    {
        public WorkRepository() { }
        public WorkRepository(DbContext db)
        {
            Db = db;
        }
        public IEnumerable<WorkItem> Get(WorkItemFilter filter)
        {
            try
            {
                var iQuery = filter.IncludeFeedbacks ?
                    Db.Set<WorkItem>()
                    .Include(it => it.CustomerFeedback)
                    .Include(it => it.TeamFeedBack)
                    .Include(it => it.WorkType)
                    .Include(it => it.Customer) :
                    Db.Set<WorkItem>()
                    .Include(it => it.WorkType)
                    .Include(it => it.Customer);


                var query = iQuery.AsQueryable();

                if (filter.Categories != null && filter.Categories.Any())
                    query = filter.CategoryOperation == CategoryOperation.Cross
                        ? (from item in GetWorksByCategoriesCross(filter.Categories)
                           join workItem1 in query on item.Id equals workItem1.Id
                           select workItem1).AsQueryable()
                        : (from item in GetWorksByCategoriesUnion(filter.Categories)
                           join workItem1 in query on item.Id equals workItem1.Id
                           select workItem1).AsQueryable();

                if (filter.CustomerId.HasValue)
                    query = query.Where(it => it.CustomerId == filter.CustomerId.Value);

                if (filter.WorkTypeId.HasValue)
                    query = query.Where(it => it.WorkTypeId == filter.WorkTypeId.Value);
                if (filter.ExecutorId.HasValue)
                    query = from workItem in query
                            join workItemTeam in Db.Set<WorkItemTeam>() on workItem.Id equals workItemTeam.WorkItemId
                            where workItemTeam.ExecutorId == filter.ExecutorId.Value
                            select workItem;

                return query; //;

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<Category> GetWorkItemCategories(int workItemId)
        {
            try
            {
                var query =
                    Db.Set<WorkCategory>()
                        .Where(it => it.WorkItemId == workItemId)
                        .Include(it => it.Category)
                        .Select(it => it.Category);
                return query;

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<Category> GetWorkItemCategories(WorkItem workItem)
        {
            return GetWorkItemCategories(workItem.Id);
        }

        public IEnumerable<WorkItem> GetMilestones(int workItemId)
        {
            try
            {
                return
                    Db.Set<WorkItem>()
                        .Where(it => it.ParentId == workItemId)
                        .Include(it => it.CustomerFeedback)
                        .Include(it => it.TeamFeedBack)
                        .Include(it => it.WorkType);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<WorkItem> GetMilestones(WorkItem workItem)
        {
            return GetMilestones(workItem.Id);
        }

        public Task<IEnumerable<WorkItem>> GetMilestonesAsync(int workItemId)
        {
            return Task<IEnumerable<WorkItem>>.Factory.StartNew(() => GetMilestones(workItemId));
        }

        public Task<IEnumerable<WorkItem>> GetMilestonesAsync(WorkItem workItem)
        {
            return Task<IEnumerable<WorkItem>>.Factory.StartNew(() => GetMilestones(workItem));
        }

        public void AddWorkCategory(WorkItem workItem, Category category)
        {
            Add(new WorkCategory { CategoryId = category.Id, WorkItemId = workItem.Id });
        }

        public void RemoveWorkCategory(WorkItem workItem, Category category)
        {
            try
            {
                Db.Set<WorkCategory>()
                    .Remove(Db.Set<WorkCategory>()
                        .Single(it => it.CategoryId == category.Id && it.WorkItemId == workItem.Id));
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<WorkItem> GetWorksByCategory(Category category)
        {
            try
            {
                return
                    Db.Set<WorkCategory>()
                        .Include(it => it.WorkItem)
                        .Where(it => it.CategoryId == category.Id)
                        .Select(it => it.WorkItem)
                        .Distinct();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<WorkItem> GetWorksByCategoriesUnion(IEnumerable<Category> categories)
        {
            try
            {
                var categoryIds = categories.Select(it => it.Id).ToList();

                return (from categoryId in categoryIds
                    join workCategory in Db.Set<WorkCategory>() on categoryId equals workCategory.CategoryId
                    join workItem in Db.Set<WorkItem>() on workCategory.WorkItemId equals workItem.Id
                    select workItem).Distinct();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<WorkItem> GetWorksByCategoriesCross(IEnumerable<Category> categories)
        {
            try
            {
                var catCount = categories.Count();
                var query = Db.Set<WorkCategory>().AsQueryable();
                var query2 = (from workCategory in query
                              join category in categories on workCategory.CategoryId equals category.Id
                              group new { workCategory } by
                                  new { workCategory.WorkItemId }
                    into tmp
                              where tmp.Count() == catCount
                              select tmp.Key.WorkItemId);
                var query3 = from workCategory1 in query
                             join i in query2 on workCategory1.WorkItemId equals i
                             join wi in Db.Set<WorkItem>() on workCategory1.WorkItemId equals wi.Id
                             select wi;
                var res = query3.Distinct().ToList();
                return res;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }


        public IEnumerable<WorkItem> GetWorkItemsByWorkType(WorkType workType)
        {
            try
            {
                return Db.Set<WorkItem>().Where(it => it.WorkTypeId == workType.Id);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public WorkFeedback GetCustomerFeedback(int workItemId)
        {
            try
            {
                return Db.Set<WorkItem>()
                        .Include(it => it.CustomerFeedback)
                        .Single(it => it.Id == workItemId).CustomerFeedback;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public WorkFeedback GetCustomerFeedback(WorkItem workItem)
        {
            return GetCustomerFeedback(workItem.Id);
        }

        public void AddTeamMember(int teamMemberId, WorkItem workItem, bool isTeamLeader = false)
        {
            try
            {
                if (isTeamLeader)
                {
                    var tl = Db.Set<WorkItemTeam>().SingleOrDefault(it => it.IsTeamLeader && it.WorkItemId == workItem.Id);
                    if (tl != null)
                        tl.IsTeamLeader = false;
                }
                Add(new WorkItemTeam
                {
                    ExecutorId = teamMemberId,
                    IsTeamLeader = isTeamLeader,
                    WorkItemId = workItem.Id
                });
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveTeamMember(int teamMemberId, WorkItem workItem)
        {
            try
            {
                var teamMemb =
                    Db.Set<WorkItemTeam>().Single(it => it.ExecutorId == teamMemberId && it.WorkItemId == workItem.Id);
                Db.Set<WorkItemTeam>().Remove(teamMemb);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void SelectTeamLeader(int teamMemberId, WorkItem workItem)
        {
            try
            {
                var selected =
                Db.Set<WorkItemTeam>().Single(it => it.ExecutorId == teamMemberId && it.WorkItemId == workItem.Id);
                var tl = Db.Set<WorkItemTeam>().SingleOrDefault(it => it.IsTeamLeader && it.WorkItemId == workItem.Id);
                if (tl != null)
                    tl.IsTeamLeader = false;

                selected.IsTeamLeader = true;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public WorkItemTeam GetTeamLeader(WorkItem workItem)
        {
            try
            {
                return Db.Set<WorkItemTeam>().SingleOrDefault(it => it.IsTeamLeader && it.WorkItemId == workItem.Id);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public WorkFeedback GetTeamFeedback(int workItemId)
        {
            try
            {
                return
                    Db.Set<WorkItem>()
                        .Include(it => it.TeamFeedBack)
                        .Single(it => it.Id == workItemId).TeamFeedBack;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public WorkFeedback GetTeamFeedback(WorkItem workItem)
        {
            return GetTeamFeedback(workItem.Id);
        }

        public WorkFeedback GetTeamMemberFeedback(int workItemId, int teamMemberId)
        {
            try
            {
                return
                    Db.Set<WorkItemTeam>()
                        .Include(it => it.Executor)
                        .Include(it => it.ExecutorFeedback)
                        .Single(it => it.WorkItemId == workItemId && it.ExecutorId == teamMemberId)
                        .ExecutorFeedback;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<WorkItemTeam> GetTeams(int workItemId)
        {
            try
            {
                return Db.Set<WorkItemTeam>().Include(it => it.Executor).Where(it => it.WorkItemId == workItemId);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<WorkItemTeam> GetTeams(WorkItem workItem)
        {
            try
            {
                return GetTeams(workItem.Id);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<Attachment> GetDeliverables(int workItemId, bool isOnlyPublic = true)
        {
            try
            {
                var res= (from deliverable in Db.Set<WorkItemDeliverable>()
                    join attachment in Db.Set<Attachment>() on deliverable.DeliverableId equals attachment.Id
                    where deliverable.WorkItemId == workItemId && (isOnlyPublic && deliverable.IsPublic || !isOnlyPublic)
                    select attachment).Distinct();
                //Db.Set<WorkItemDeliverable>()
                //    .Include(it => it.Deliverable)
                //    .Where(it => it.WorkItemId == workItemId && (isOnlyPublic && it.IsPublic || !isOnlyPublic))
                //    .Select(it => it.Deliverable);
                return res;
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<Attachment> GetDeliverables(WorkItem workItem, bool isOnlyPublic = true)
        {
            try
            {
                return GetDeliverables(workItem.Id, isOnlyPublic);

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void AddCustomerFeedBack(WorkItem workItem, WorkFeedback feedback)
        {
            try
            {
                var vi = Db.Set<WorkItem>().SingleOrDefault(it => it.Id == workItem.Id);
                vi.CustomerFeedback = feedback;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveCustomerFeedBack(WorkItem workItem, WorkFeedback feedback)
        {
            try
            {
                var vi = GetById(workItem.Id);
                Db.Set<WorkFeedback>().Remove(vi.CustomerFeedback);
                vi.CustomerFeedbackId = null;
                vi.CustomerFeedback = null;
                Db.SaveChanges();

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void AddTeamFeedback(WorkItem workItem, WorkFeedback feedback)
        {
            try
            {
                var vi = Db.Set<WorkItem>().SingleOrDefault(it => it.Id == workItem.Id);
                vi.TeamFeedBack = feedback;
                Db.SaveChanges();

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveTeamFeedback(WorkItem workItem, WorkFeedback feedback)
        {
            try
            {
                var vi = GetById(workItem.Id);
                Db.Set<WorkFeedback>().Remove(vi.TeamFeedBack);
                vi.TeamFeedBackId = null;
                vi.TeamFeedBack = null;
                Db.SaveChanges();

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void AddTeamMemberFeedback(WorkItem workItem, WorkFeedback feedback, int teamMemberId)
        {
            try
            {
                var teamMemberFeedbackRow = Db.Set<WorkItemTeam>().Single(it => it.ExecutorId == teamMemberId && it.WorkItemId == workItem.Id);
                teamMemberFeedbackRow.ExecutorFeedback = feedback;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveTeamMemberFeedback(WorkItem workItem, WorkFeedback feedback, int teamMemberId)
        {
            try
            {
                var teamMemberFeedbackRow = Db.Set<WorkItemTeam>().Single(it => it.ExecutorId == teamMemberId && it.WorkItemId == workItem.Id);
                Delete(feedback);
                teamMemberFeedbackRow.ExecutorFeedback = null;
                teamMemberFeedbackRow.ExecutorFeedbackId = null;
                Db.SaveChanges();

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }



        public void AddDeliverable(WorkItem workItem, Attachment deliverable, bool isPublic = false)
        {
            try
            {
                Db.Set<Attachment>().Add(deliverable);
                Db.Set<WorkItemDeliverable>().Add(new WorkItemDeliverable { DeliverableId = deliverable.Id, WorkItemId = workItem.Id, IsPublic = isPublic });
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void MakeDeliverablePublic(WorkItem workItem, Attachment deliverable)
        {
            try
            {
                var wiDeliverable = Db.Set<WorkItemDeliverable>().Single(it => it.DeliverableId == deliverable.Id && it.WorkItemId == workItem.Id);
                wiDeliverable.IsPublic = true;
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void MakeDeliverablePrivate(WorkItem workItem, Attachment deliverable)
        {
            var wiDeliverable = Db.Set<WorkItemDeliverable>().Single(it => it.DeliverableId == deliverable.Id && it.WorkItemId == workItem.Id);
            wiDeliverable.IsPublic = false;
            Db.SaveChanges();
        }

        public void RemoveDeliverable(WorkItem workItem, Attachment deliverable)
        {
            try
            {
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public bool IsDeadlineMet(int workItemId)
        {
            try
            {
                var wi = Db.Set<WorkItem>().SingleOrDefault(it => it.Id == workItemId);
                if (!wi.DateStarted.HasValue || !wi.DateEnded.HasValue)
                    return DateTime.Compare(DateTime.Today, wi.DeadLine) <= 0;
                return DateTime.Compare(wi.DateEnded.Value, wi.DeadLine) <= 0;

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void SetConnection(string connection)
        {
            throw new NotImplementedException();
        }

        public void SetContext(DbContext db)
        {
            Db = db;
        }
    }
}
