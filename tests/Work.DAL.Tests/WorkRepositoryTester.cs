using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using odec.Framework.Logging;
using odec.Server.Model.Attachment;
using odec.Server.Model.Attachment.Extended;
using odec.Server.Model.Category;
using odec.Server.Model.User;
using odec.Server.Model.Work.Contexts;
using odec.Server.Model.Work.Models;
using odec.Server.Model.Work.Models.Filters;
using odec.Server.Model.Work.Models.Helpers;
using odec.Work.DAL;
using odec.Work.DAL.Interop;
using IWorkRepo = odec.Work.DAL.Interop.IWorkRepository<int, odec.Server.Model.Work.Models.WorkItem, odec.Server.Model.Category.Category, odec.Server.Model.Work.Models.WorkFeedback, odec.Server.Model.Work.Models.WorkItemTeam, odec.Server.Model.Attachment.Attachment, odec.Server.Model.Work.Models.WorkType, odec.Server.Model.Work.Models.Filters.WorkItemFilter>;
namespace Work.DAL.Tests
{
    public class WorkRepositoryTester : Tester<WorkContext>
    {


        private WorkItem GenerateModel()
        {
            return new WorkItem
            {
                Name = "My Finished Work",
                Code = "My Finished Work",
                ActualCost = 1000,
                IsActive = true,
                DeadLine = DateTime.Now.AddDays(14),
                DateStarted = DateTime.Today.AddDays(-7),
                CustomerId = 1,
                Description = "LONLONNGLONGLONG description",
                InitialCost = 500,
                WorkTypeId = 1,
                DateCreated = DateTime.Now,
                DateEnded = DateTime.Today,
                SortOrder = 0

            };
        }

        [Test]
        public void Save()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository =
                        new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.Greater(item.Id, 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void Delete()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void DeleteById()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Delete(item.Id));
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void Deactivate()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    item.IsActive = true;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Deactivate(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsFalse(item.IsActive);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void DeactivateById()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    item.IsActive = true;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => item = repository.Deactivate(item.Id));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsFalse(item.IsActive);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void Activate()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    item.IsActive = false;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Activate(item));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsTrue(item.IsActive);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void ActivateById()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    item.IsActive = false;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => item = repository.Activate(item.Id));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.IsTrue(item.IsActive);
                }

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetById()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));

                    Assert.DoesNotThrow(() => item = repository.GetById(item.Id));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.NotNull(item);
                    Assert.Greater(item.Id, 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void AddCustomerFeedBack()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));

                    Assert.DoesNotThrow(() => repository.AddCustomerFeedBack(item, new WorkFeedback { Rating = 5, Text = "He was good!" }));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.NotNull(item);
                    Assert.Greater(item.Id, 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void RemoveCustomerFeedBack()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));

                    var feedback = new WorkFeedback { Rating = 5, Text = "He was good!" };
                    Assert.DoesNotThrow(() => repository.AddCustomerFeedBack(item, feedback));
                    Assert.DoesNotThrow(() => repository.RemoveCustomerFeedBack(item, feedback));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetCustomerFeedback()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    WorkFeedback fb = null;
                    Assert.DoesNotThrow(() => fb = repository.GetCustomerFeedback(item));
                    Assert.Null(fb);
                    fb = new WorkFeedback { Rating = 5, Text = "He was good!" };
                    Assert.DoesNotThrow(() => repository.AddCustomerFeedBack(item, fb));
                    Assert.DoesNotThrow(() => fb = repository.GetCustomerFeedback(item));
                    Assert.NotNull(fb);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetCustomerFeedback2()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    WorkFeedback fb = null;
                    Assert.DoesNotThrow(() => fb = repository.GetCustomerFeedback(item.Id));
                    Assert.Null(fb);
                    fb = new WorkFeedback { Rating = 5, Text = "He was good!" };
                    Assert.DoesNotThrow(() => repository.AddCustomerFeedBack(item, fb));
                    Assert.DoesNotThrow(() => fb = repository.GetCustomerFeedback(item.Id));
                    Assert.NotNull(fb);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void AddTeamFeedback()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));

                    Assert.DoesNotThrow(() => repository.AddTeamFeedback(item, new WorkFeedback { Rating = 5, Text = "He was good!" }));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.NotNull(item);
                    Assert.Greater(item.Id, 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void RemoveTeamFeedback()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var feedback = new WorkFeedback { Rating = 5, Text = "He was good!" };
                    Assert.DoesNotThrow(() => repository.AddTeamFeedback(item, feedback));
                    Assert.DoesNotThrow(() => repository.RemoveTeamFeedback(item, feedback));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetTeamFeedback()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    WorkFeedback fb = null;
                    Assert.DoesNotThrow(() => fb = repository.GetTeamFeedback(item));
                    Assert.Null(fb);
                    fb = new WorkFeedback { Rating = 5, Text = "He was good!" };
                    Assert.DoesNotThrow(() => repository.AddTeamFeedback(item, fb));
                    Assert.DoesNotThrow(() => fb = repository.GetTeamFeedback(item));
                    Assert.NotNull(fb);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetTeamFeedback2()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    WorkFeedback fb = null;
                    Assert.DoesNotThrow(() => fb = repository.GetTeamFeedback(item.Id));
                    Assert.Null(fb);
                    fb = new WorkFeedback { Rating = 5, Text = "He was good!" };
                    Assert.DoesNotThrow(() => repository.AddTeamFeedback(item, fb));
                    Assert.DoesNotThrow(() => fb = repository.GetTeamFeedback(item.Id));
                    Assert.NotNull(fb);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void AddDeliverable()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));

                    Assert.DoesNotThrow(() => repository.AddDeliverable(item, new Attachment
                    {
                        Name = "Test",
                        Code = "TEST",
                        IsActive = true,
                        DateCreated = DateTime.Now,
                        SortOrder = 0,
                        Extension = new Extension
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        },
                        PublicUri = string.Empty,
                        IsShared = false,
                        Content = new byte[] {1,3,3,3,3},
                        AttachmentType = new AttachmentType
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        }
                    }));
                    Assert.DoesNotThrow(() => repository.Delete(item));
                    Assert.NotNull(item);
                    Assert.Greater(item.Id, 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void RemoveDeliverable()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var attachment = new Attachment
                    {
                        Name = "Test",
                        Code = "TEST",
                        IsActive = true,
                        DateCreated = DateTime.Now,
                        SortOrder = 0,
                        Extension = new Extension
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        },
                        PublicUri = string.Empty,
                        IsShared = false,
                        Content = new byte[] { 1, 3, 3, 3, 3 },
                        AttachmentType = new AttachmentType
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        }
                    };
                    Assert.DoesNotThrow(() => repository.AddDeliverable(item, attachment));
                    Assert.DoesNotThrow(() => repository.RemoveDeliverable(item, attachment));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetDeliverables()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    IEnumerable<Attachment> resultDeliverables = null;
                    var attachment = new Attachment
                    {
                        Name = "Test",
                        Code = "TEST",
                        IsActive = true,
                        DateCreated = DateTime.Now,
                        SortOrder = 0,
                        Extension = new Extension
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        },
                        PublicUri = string.Empty,
                        IsShared = false,
                        Content = new byte[] { 1, 3, 3, 3, 3 },
                        AttachmentType = new AttachmentType
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        }
                    };
                    Assert.DoesNotThrow(() => resultDeliverables = repository.GetDeliverables(item, false));
                    Assert.True(resultDeliverables ==null ||!resultDeliverables.Any());
                    Assert.DoesNotThrow(() => repository.AddDeliverable(item, attachment));
                    Assert.DoesNotThrow(() => resultDeliverables = repository.GetDeliverables(item,false));
                    Assert.NotNull(resultDeliverables);
                    Assert.Greater(resultDeliverables.Count(), 0);

                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetDeliverables2()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    IEnumerable<Attachment> resultDeliverables = null;
                    var attachment = new Attachment
                    {
                        Name = "Test",
                        Code = "TEST",
                        IsActive = true,
                        DateCreated = DateTime.Now,
                        SortOrder = 0,
                        Extension = new Extension
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        },
                        PublicUri = string.Empty,
                        IsShared = false,
                        Content = new byte[] { 1, 3, 3, 3, 3 },
                        AttachmentType = new AttachmentType
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        }
                    };
                    Assert.DoesNotThrow(() => resultDeliverables = repository.GetDeliverables(item.Id,false));
                    Assert.True(resultDeliverables == null || !resultDeliverables.Any());
                    Assert.DoesNotThrow(() => repository.AddDeliverable(item, attachment));
                    Assert.DoesNotThrow(() => resultDeliverables = repository.GetDeliverables(item.Id,false));
                    Assert.NotNull(resultDeliverables);
                    Assert.Greater(resultDeliverables.Count(), 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void MakeDeliverablePublic()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    IEnumerable<Attachment> resultDeliverables = null;
                    var attachment = new Attachment
                    {
                        Name = "Test",
                        Code = "TEST",
                        IsActive = true,
                        DateCreated = DateTime.Now,
                        SortOrder = 0,
                        Extension = new Extension
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        },
                        PublicUri = string.Empty,
                        IsShared = false,
                        Content = new byte[] { 1, 3, 3, 3, 3 },
                        AttachmentType = new AttachmentType
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        }
                    };

                    int countPublicBefore = 0;
                    int countPublicAfter = 0;
                    Assert.DoesNotThrow(() => countPublicBefore = repository.GetDeliverables(item.Id, isOnlyPublic: true).Count());
                    Assert.DoesNotThrow(() => repository.AddDeliverable(item, attachment));
                    Assert.DoesNotThrow(() => repository.MakeDeliverablePublic(item, attachment));
                    Assert.DoesNotThrow(() => countPublicAfter = repository.GetDeliverables(item.Id, isOnlyPublic: true).Count());
                    Assert.Greater(countPublicAfter, countPublicBefore);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void MakeDeliverablePrivate()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    IEnumerable<Attachment> resultDeliverables = null;
                    var attachment = new Attachment
                    {
                        Name = "Test",
                        Code = "TEST",
                        IsActive = true,
                        DateCreated = DateTime.Now,
                        SortOrder = 0,
                        Extension = new Extension
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        },
                        PublicUri = string.Empty,
                        IsShared = false,
                        Content = new byte[] { 1, 3, 3, 3, 3 },
                        AttachmentType = new AttachmentType
                        {
                            Name = "Test",
                            Code = "TEST",
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            SortOrder = 0
                        }
                    };

                    int countPublicBefore = 0;
                    int countPublicAfter = 0;
                    Assert.DoesNotThrow(() => repository.AddDeliverable(item, attachment, true));
                    Assert.DoesNotThrow(() => countPublicBefore = repository.GetDeliverables(item.Id, isOnlyPublic: true).Count());
                    Assert.DoesNotThrow(() => repository.MakeDeliverablePrivate(item, attachment));
                    Assert.DoesNotThrow(() => countPublicAfter = repository.GetDeliverables(item.Id, isOnlyPublic: true).Count());
                    Assert.Greater(countPublicBefore, countPublicAfter);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetMilestones()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    Assert.DoesNotThrow(() => repository.Save(item1));
                    item2.ParentId = item1.Id;
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    item3.ParentId = item1.Id;
                    Assert.DoesNotThrow(() => repository.Save(item3));
                    IEnumerable<WorkItem> result = null;

                    Assert.DoesNotThrow(() => result = repository.GetMilestones(item1));
                    Assert.NotNull(result);
                    Assert.Greater(result.Count(), 0);
                    Assert.True(result.Count() == 2);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetMilestones2()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    Assert.DoesNotThrow(() => repository.Save(item1));
                    item2.ParentId = item1.Id;
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    item3.ParentId = item1.Id;
                    Assert.DoesNotThrow(() => repository.Save(item3));
                    IEnumerable<WorkItem> result = null;

                    Assert.DoesNotThrow(() => result = repository.GetMilestones(item1.Id));
                    Assert.NotNull(result);
                    Assert.Greater(result.Count(), 0);
                    Assert.True(result.Count() == 2);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetMilestonesAsync()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    Assert.DoesNotThrow(() => repository.Save(item1));
                    item2.ParentId = item1.Id;
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    item3.ParentId = item1.Id;
                    Assert.DoesNotThrow(() => repository.Save(item3));
                    IEnumerable<WorkItem> result = null;

                    Assert.DoesNotThrow(() => result = repository.GetMilestonesAsync(item1).Result);
                    Assert.NotNull(result);
                    Assert.Greater(result.Count(), 0);
                    Assert.True(result.Count() == 2);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetMilestonesAsync2()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    Assert.DoesNotThrow(() => repository.Save(item1));
                    item2.ParentId = item1.Id;
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    item3.ParentId = item1.Id;
                    Assert.DoesNotThrow(() => repository.Save(item3));
                    IEnumerable<WorkItem> result = null;

                    Assert.DoesNotThrow(() => result = repository.GetMilestonesAsync(item1.Id).Result);
                    Assert.NotNull(result);
                    Assert.Greater(result.Count(), 0);
                    Assert.True(result.Count() == 2);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void IsDeadlineMet()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    Assert.DoesNotThrow(() => repository.Save(item1));
                    item2.ParentId = item1.Id;
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    item3.ParentId = item1.Id;
                    Assert.DoesNotThrow(() => repository.Save(item3));
                    bool result = false;

                    Assert.DoesNotThrow(() => result = repository.IsDeadlineMet(item1.Id));
                    Assert.True(result);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void AddWorkCategory()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();

                    Assert.DoesNotThrow(() => repository.Save(item1));

                    var cat = db.Set<Category>().First();
                    Assert.DoesNotThrow(() => repository.AddWorkCategory(item1, cat));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void RemoveWorkCategory()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();

                    Assert.DoesNotThrow(() => repository.Save(item1));

                    var cat = db.Set<Category>().First();
                    Assert.DoesNotThrow(() => repository.AddWorkCategory(item1, cat));
                    Assert.DoesNotThrow(() => repository.RemoveWorkCategory(item1, cat));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetWorksByCategory()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();

                    Assert.DoesNotThrow(() => repository.Save(item1));

                    var cat = db.Set<Category>().First();
                    IEnumerable<WorkItem> result = null;
                    Assert.DoesNotThrow(() => repository.AddWorkCategory(item1, cat));
                    Assert.DoesNotThrow(() => result = repository.GetWorksByCategory(cat));
                    Assert.NotNull(result);
                    Assert.Greater(result.Count(), 0);
                    Assert.DoesNotThrow(() => repository.RemoveWorkCategory(item1, cat));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetWorksByCategoriesCross()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    Assert.DoesNotThrow(() => repository.Save(item1));
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    Assert.DoesNotThrow(() => repository.Save(item3));

                    var cats = db.Set<Category>();
                    var cat = cats.First();
                    foreach (var category in cats)
                        Assert.DoesNotThrow(() => repository.AddWorkCategory(item1, category));
                    IEnumerable<WorkItem> result = null;
                    Assert.DoesNotThrow(() => repository.AddWorkCategory(item2, cat));
                    Assert.DoesNotThrow(() => result = repository.GetWorksByCategoriesCross(cats));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 1);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetWorksByCategoriesUnion()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    Assert.DoesNotThrow(() => repository.Save(item1));
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    Assert.DoesNotThrow(() => repository.Save(item3));

                    var cats = db.Set<Category>();
                    var cat = cats.First();
                    foreach (var category in cats)
                        Assert.DoesNotThrow(() => repository.AddWorkCategory(item1, category));
                    IEnumerable<WorkItem> result = null;
                    Assert.DoesNotThrow(() => repository.AddWorkCategory(item2, cat));
                    Assert.DoesNotThrow(() => result = repository.GetWorksByCategoriesUnion(cats));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 2);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetWorkItemsByWorkType()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    IEnumerable<WorkItem> result = null;

                    Assert.DoesNotThrow(() => result = repository.GetWorkItemsByWorkType(new WorkType { Id = 1 }));
                    Assert.NotNull(result);
                    Assert.Greater(result.Count(), 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void AddTeamMember()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    IEnumerable<WorkItemTeam> result = null;
                    var user = db.Set<User>().First();
                    Assert.DoesNotThrow(() => repository.AddTeamMember(user.Id, item));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void RemoveTeamMember()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var user = db.Set<User>().First();
                    Assert.DoesNotThrow(() => repository.AddTeamMember(user.Id, item));
                    Assert.DoesNotThrow(() => repository.RemoveTeamMember(user.Id, item));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetTeams()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    IEnumerable<WorkItemTeam> result = null;
                    var user = db.Set<User>().First();
                    Assert.DoesNotThrow(() => repository.AddTeamMember(user.Id, item));
                    Assert.DoesNotThrow(() => result = repository.GetTeams(item));
                    Assert.NotNull(result);
                    Assert.Greater(result.Count(), 0);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void SelectTeamLeader()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var user = db.Set<User>().First();
                    Assert.DoesNotThrow(() => repository.AddTeamMember(user.Id, item));
                    Assert.DoesNotThrow(() => repository.SelectTeamLeader(user.Id, item));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void GetTeamLeader()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    WorkItemTeam result = null;
                    var user = db.Set<User>().First();
                    Assert.DoesNotThrow(() => repository.AddTeamMember(user.Id, item));
                    Assert.DoesNotThrow(() => repository.SelectTeamLeader(user.Id, item));
                    Assert.DoesNotThrow(() => result = repository.GetTeamLeader(item));
                    Assert.NotNull(result);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
        [Test]
        public void AddTeamMemberFeedback()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var user = db.Set<User>().First();
                    Assert.DoesNotThrow(() => repository.AddTeamMember(user.Id, item));
                    Assert.DoesNotThrow(() => repository.AddTeamMemberFeedback(item,new WorkFeedback {Rating = 5,Text = "123123123"}, user.Id));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void RemoveTeamMemberFeedback()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    WorkItemTeam result = null;
                    var user = db.Set<User>().First();
                    var fb = new WorkFeedback {Rating = 5, Text = "123123123"};
                    Assert.DoesNotThrow(() => repository.AddTeamMember(user.Id, item));
                    Assert.DoesNotThrow(() => repository.AddTeamMemberFeedback(item, fb, user.Id));
                    Assert.DoesNotThrow(() => repository.RemoveTeamMemberFeedback(item, fb, user.Id));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetTeamMemberFeedback()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    WorkItemTeam result = null;
                    var user = db.Set<User>().First();
                    var fb = new WorkFeedback { Rating = 5, Text = "123123123" };
                    Assert.DoesNotThrow(() => repository.AddTeamMember(user.Id, item));
                    Assert.DoesNotThrow(() => repository.AddTeamMemberFeedback(item, fb, user.Id));
                    Assert.DoesNotThrow(() => fb=repository.GetTeamMemberFeedback(item.Id,user.Id));
                    Assert.NotNull(fb);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void Get_Work()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    Assert.DoesNotThrow(() => repository.Save(item1));
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    Assert.DoesNotThrow(() => repository.Save(item3));

                    var cats = db.Set<Category>();
                    var cat = cats.First();
                    foreach (var category in cats)
                        Assert.DoesNotThrow(() => repository.AddWorkCategory(item1, category));
                    IEnumerable<WorkItem> result = null;
                    Assert.DoesNotThrow(() => repository.AddWorkCategory(item2, cat));
                    Assert.DoesNotThrow(() => result = repository.Get(new WorkItemFilter
                    {
                        Categories = cats,
                        CategoryOperation = CategoryOperation.Cross
                    }));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 1);
                    Assert.DoesNotThrow(() => result = repository.Get(new WorkItemFilter
                    {
                        Categories = cats,
                        CategoryOperation = CategoryOperation.Union
                    }));
                    Assert.NotNull(result);
                    Assert.True(result.Count()==2);

                    Assert.DoesNotThrow(() => result = repository.Get(new WorkItemFilter
                    {
                        Categories = cats,
                        CategoryOperation = CategoryOperation.Union,
                        IncludeFeedbacks = true
                    }));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 2);
                    Assert.DoesNotThrow(() => result = repository.Get(new WorkItemFilter
                    {
                        Categories = cats,
                        CategoryOperation = CategoryOperation.Union,
                        IncludeFeedbacks = true,
                        WorkTypeId = 1
                    }));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 2);

                    Assert.DoesNotThrow(() => result = repository.Get(new WorkItemFilter
                    {
                        Categories = cats,
                        CategoryOperation = CategoryOperation.Union,
                        IncludeFeedbacks = true,
                        WorkTypeId = 1,
                        ExecutorId = 1
                    }));
                    Assert.NotNull(result);
                    Assert.True(!result.Any());
                    Assert.DoesNotThrow(() => result = repository.Get(new WorkItemFilter
                    {
                        Categories = cats,
                        CategoryOperation = CategoryOperation.Union,
                        IncludeFeedbacks = true,
                        WorkTypeId = 1,
                        CustomerId = 1
                    }));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 2);

                    db.Set<WorkItemTeam>().Add(new WorkItemTeam
                    {
                        ExecutorId = 1,
                        WorkItemId = item1.Id,
                        IsTeamLeader = true
                    });
                    db.SaveChanges();
                    Assert.DoesNotThrow(() => result = repository.Get(new WorkItemFilter
                    {
                        ExecutorId = 1
                    }));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 1);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetWorkItemCategories()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    Assert.DoesNotThrow(() => repository.Save(item1));
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    Assert.DoesNotThrow(() => repository.Save(item3));

                    var cats = db.Set<Category>();
                    var cat = cats.First();
                    foreach (var category in cats)
                        Assert.DoesNotThrow(() => repository.AddWorkCategory(item1, category));
                    IEnumerable<Category> result = null;
                    Assert.DoesNotThrow(() => repository.AddWorkCategory(item2, cat));
                    Assert.DoesNotThrow(() => result = repository.GetWorkItemCategories(item1));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 2);
                    Assert.DoesNotThrow(() => result = repository.GetWorkItemCategories(item2));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 1);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetWorkItemCategories2()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new WorkContext(options))
                {
                    var repository = new WorkRepository(db);
                    WorkTestHelper.PopulateDefaultDataWorkCtx(db);
                    var item1 = GenerateModel();
                    var item2 = GenerateModel();
                    item2.Code = "test2";
                    Assert.DoesNotThrow(() => repository.Save(item1));
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    var item3 = GenerateModel();
                    item3.Code = "test3";
                    Assert.DoesNotThrow(() => repository.Save(item3));

                    var cats = db.Set<Category>();
                    var cat = cats.First();
                    foreach (var category in cats)
                        Assert.DoesNotThrow(() => repository.AddWorkCategory(item1, category));
                    IEnumerable<Category> result = null;
                    Assert.DoesNotThrow(() => repository.AddWorkCategory(item2, cat));
                    Assert.DoesNotThrow(() => result = repository.GetWorkItemCategories(item1.Id));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 2);
                    Assert.DoesNotThrow(() => result = repository.GetWorkItemCategories(item2.Id));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 1);
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }
    }
}
