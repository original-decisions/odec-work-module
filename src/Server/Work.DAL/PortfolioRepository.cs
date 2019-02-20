using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using odec.Entity.DAL;
using odec.Entity.DAL.Interop;
using odec.Framework.Logging;
using odec.Server.Model.Attachment;
using odec.Server.Model.Work.Models;
using odec.Server.Model.Work.Models.Filters;
using odec.Work.DAL.Interop;

namespace odec.Work.DAL
{
    public class PortfolioRepository : OrmEntityOperationsRepository<int, PortfolioItem, DbContext>, 
        IPortfolioRepository<int, PortfolioItem, PortfolioVideo, PortfolioScreenshot,PortfolioFilter>, IContextRepository<DbContext>
    {

        public PortfolioRepository() { }
        public PortfolioRepository(DbContext db)
        {
            Db = db;
        }
        public IEnumerable<PortfolioItem> Get(PortfolioFilter filter)
        {
            try
            {
                var query = Db.Set<PortfolioItem>().Where(it => it.UserId == filter.UserId);

                if (!filter.FinishDateEnd.HasValue && !filter.FinishDateStart.HasValue)
                    return query;

                if (filter.FinishDateStart.HasValue && !filter.FinishDateEnd.HasValue)
                    return
                        query.Where(
                            it =>
                                it.ProjectFinishDate <= DateTime.Today &&
                                it.ProjectFinishDate >= filter.FinishDateStart.Value);


                if (!filter.FinishDateStart.HasValue && filter.FinishDateEnd.HasValue)
                {
                    var dateStart = filter.FinishDateEnd.Value.AddMonths(-1);
                    return
                        query.Where(
                            it =>
                                it.ProjectFinishDate <= filter.FinishDateEnd.Value &&
                                it.ProjectFinishDate >= dateStart);
                }
                    

                return query.Where(
                            it =>
                                it.ProjectFinishDate <= filter.FinishDateEnd.Value &&
                                it.ProjectFinishDate >= filter.FinishDateStart.Value).ToList();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<PortfolioVideo> GetPortfolioVideos(int portfolioItemId)
        {
            try
            {
                return
                    Db.Set<PortfolioVideo>().Include(it => it.Video).Where(it => it.PortfolioItemId == portfolioItemId);
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void AddVideo(PortfolioItem portfolioItem, PortfolioVideo video)
        {
            try
            {
                if (video.Video == null)
                    throw new ArgumentNullException("video", "You should provide a valid attachment");
                video.PortfolioItemId = portfolioItem.Id;
                if (video.PortfolioItemId == 0 && video.PortfolioItem != null)
                    video.PortfolioItemId = video.PortfolioItem.Id;
                if (video.VideoId == 0 && video.Video != null && video.Video.Id != 0)
                    video.VideoId = video.Video.Id;
                if (video.VideoId == 0 && video.Video != null && video.Video.Id == 0)
                {
                    Db.Set<Attachment>().Add(video.Video);
                    video.VideoId = video.Video.Id;
                }

                Add(video);
                Db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveVideo(PortfolioItem portfolioItem, PortfolioVideo video)
        {
            try
            {
                Db.Set<PortfolioVideo>()
                    .Remove(
                        Db.Set<PortfolioVideo>()
                            .Single(it => it.VideoId == video.VideoId && it.PortfolioItemId == portfolioItem.Id));
                Db.SaveChanges();

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void AddScreenshot(PortfolioItem portfolioItem, PortfolioScreenshot screenshot)
        {
            try
            {
                if (screenshot.Screenshot == null)
                    throw new ArgumentNullException("screenshot", "You should provide a valid attachment");
                screenshot.PortfolioItemId = portfolioItem.Id;
                if (screenshot.PortfolioItemId == 0 && screenshot.PortfolioItem != null)
                    screenshot.PortfolioItemId = screenshot.PortfolioItem.Id;
                if (screenshot.ScreenshotId == 0 && screenshot.Screenshot != null && screenshot.Screenshot.Id != 0) 
                    screenshot.ScreenshotId = screenshot.Screenshot.Id;

                if (screenshot.ScreenshotId == 0 && screenshot.Screenshot != null && screenshot.Screenshot.Id == 0)
                {
                    Db.Set<Attachment>().Add(screenshot.Screenshot);
                    screenshot.ScreenshotId = screenshot.Screenshot.Id;
                }
                
                Add(screenshot);
                Db.SaveChanges();

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public void RemoveScreenshot(PortfolioItem portfolioItem, PortfolioScreenshot screenshot)
        {
            try
            {
                Db.Set<PortfolioScreenshot>()
                    .Remove(
                        Db.Set<PortfolioScreenshot>()
                            .Single(it => it.ScreenshotId == screenshot.ScreenshotId && it.PortfolioItemId == portfolioItem.Id));
                Db.SaveChanges();

            }
            catch (Exception ex)
            {
                LogEventManager.Logger.Error(ex.Message, ex);
                throw;
            }
        }

        public IEnumerable<PortfolioScreenshot> GetPortfolioScreenshots(int portfolioItemId)
        {
            try
            {
                return
                    Db.Set<PortfolioScreenshot>()
                        .Include(it => it.Screenshot)
                        .Where(it => it.PortfolioItemId == portfolioItemId);
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
