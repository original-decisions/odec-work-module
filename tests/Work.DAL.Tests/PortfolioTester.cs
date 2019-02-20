using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using odec.Framework.Logging;
using odec.Server.Model.Attachment;
using odec.Server.Model.Attachment.Extended;
using odec.Server.Model.Work.Contexts;
using odec.Server.Model.Work.Models;
using odec.Server.Model.Work.Models.Filters;
using odec.Work.DAL;
//using IPortfolioRepo = odec.Work.DAL.Interop.IPortfolioRepository<int,odec.Server.Model.Work.Models.PortfolioItem,odec.Server.Model.Work.Models.PortfolioVideo,odec.Server.Model.Work.Models.PortfolioScreenshot,odec.Server.Model.Work.Models.Filters.PortfolioFilter>;

namespace Work.DAL.Tests
{
    public class PortfolioTester:Tester<PortfolioContext>
    {

        private PortfolioItem GenerateModel()
        {
            return new PortfolioItem
            {
                Name = "My Finished Work",
                Code = "My Finished Work",
                IsActive = true,
                Description = "LONLONNGLONGLONG description",
                DateCreated = DateTime.Now,
                UserId = 1,
                SortOrder = 0,
                ProjectFinishDate = DateTime.Today,
            };
        }

        private Attachment GenerateAttachment()
        {
            return new Attachment
            {
                Name = "Test",
                Code = Guid.NewGuid().ToString(),
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
                Content = new byte[] {1,1,1,1,1,1,1,1},
                AttachmentType = new AttachmentType
                {
                    Name = "Test",
                    Code = "TEST",
                    IsActive = true,
                    DateCreated = DateTime.Now,
                    SortOrder = 0
                }
            };
        }

        [Test]
        public void Save()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new PortfolioContext(options))
                {
                    var repository =
                        new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
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
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
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
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
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
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
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
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
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
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
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
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
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
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
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
        public void Get_Portfolio()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
                    var item = GenerateModel();
                    var item2 = GenerateModel();
                    item2.UserId = 2;
                    item2.Code = "code2";
                    IEnumerable<PortfolioItem> result = null;
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.Save(item2));
                    Assert.DoesNotThrow(() => result = repository.Get(new PortfolioFilter
                    {
                        UserId = 1
                    }));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 1);
                    Assert.DoesNotThrow(() => result = repository.Get(new PortfolioFilter
                    {
                        UserId = 1,
                        FinishDateStart = DateTime.Now.AddDays(-1),
                        FinishDateEnd = DateTime.Now
                    }));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 1);
                    Assert.DoesNotThrow(() => result = repository.Get(new PortfolioFilter
                    {
                        UserId = 1,
                        FinishDateStart = DateTime.Now.AddDays(-1),
                    }));
                    Assert.NotNull(result);
                    Assert.True(result.Count() == 1);

                    Assert.DoesNotThrow(() => result = repository.Get(new PortfolioFilter
                    {
                        UserId = 1,
                        FinishDateEnd = DateTime.Now.AddDays(1),
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
        public void GetPortfolioVideos()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));

                    var portfolioVideo = new PortfolioVideo
                    {
                        Video = GenerateAttachment(),
                        Description = "myNew Video"
                    };
                    IEnumerable<PortfolioVideo> result = null;
                    Assert.DoesNotThrow(() => result = repository.GetPortfolioVideos(item.Id));
                    Assert.True(result == null || !result.Any());
                    Assert.DoesNotThrow(() => repository.AddVideo(item, portfolioVideo));
                    Assert.DoesNotThrow(() => repository.AddVideo(item, new PortfolioVideo
                    {
                        Video = GenerateAttachment(),
                        Description = "myNew Video"
                    }));
                    Assert.DoesNotThrow(() => result = repository.GetPortfolioVideos(item.Id));
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
        public void AddVideo()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));

                    Assert.DoesNotThrow(() => repository.AddVideo(item,new PortfolioVideo
                    {
                        Video = GenerateAttachment(),
                        Description = "myNew Video"
                    }));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void RemoveVideo()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var portfolioVideo = new PortfolioVideo
                    {
                        Video = GenerateAttachment(),
                        Description = "myNew Video"
                    };
                    Assert.DoesNotThrow(() => repository.AddVideo(item, portfolioVideo));
                    Assert.DoesNotThrow(() => repository.RemoveVideo(item, portfolioVideo));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void AddScreenshot()
        {
            try
            {
                var options = CreateNewContextOptions();
                using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    Assert.DoesNotThrow(() => repository.AddScreenshot(item, new PortfolioScreenshot
                    {
                        Screenshot = GenerateAttachment(),
                        Description = "myNew Video"
                    }));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void RemoveScreenShot()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));

                    var portfolioVideo = new PortfolioScreenshot
                    {
                        Screenshot = GenerateAttachment(),
                        Description = "myNew Video"
                    };
                    Assert.DoesNotThrow(() => repository.AddScreenshot(item, portfolioVideo));
                    Assert.DoesNotThrow(() => repository.RemoveScreenshot(item, portfolioVideo));
                }
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex);
                throw;
            }
        }

        [Test]
        public void GetPortfolioScreenshots()
        {
            try
            {
                var options = CreateNewContextOptions();using (var db = new PortfolioContext(options))
                {
                    var repository = new PortfolioRepository(db);
                    WorkTestHelper.PopulateDefaultDataPortfolioCtx(db);
                    var item = GenerateModel();
                    Assert.DoesNotThrow(() => repository.Save(item));
                    var portfolioVideo = new PortfolioScreenshot
                    {
                        Screenshot = GenerateAttachment(),
                        Description = "myNew Video"
                    };
                    IEnumerable<PortfolioScreenshot> result = null;
                    Assert.DoesNotThrow(() => result = repository.GetPortfolioScreenshots(item.Id));
                    Assert.True(result == null || !result.Any());
                    Assert.DoesNotThrow(() => repository.AddScreenshot(item, portfolioVideo));
                    Assert.DoesNotThrow(() => repository.AddScreenshot(item, new PortfolioScreenshot
                    {
                        Screenshot = GenerateAttachment(),
                        Description = "myNew Video"
                    }));
                    Assert.DoesNotThrow(() => result = repository.GetPortfolioScreenshots(item.Id));
                    Assert.NotNull(result);
                    Assert.True(result.Count()==2);
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
