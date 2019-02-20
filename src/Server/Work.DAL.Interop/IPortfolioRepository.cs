using System.Collections.Generic;
using odec.Entity.DAL.Interop;

namespace odec.Work.DAL.Interop
{
    public interface IPortfolioRepository<in TKey, TPortfolioItem, TPortfolioVideo, TPortfolioScreenshot, in TPortfolioFilter> : 
        IEntityOperations<TKey,TPortfolioItem>,
        IActivatableEntity<TKey, TPortfolioItem> 
        where TKey : struct 
        where TPortfolioItem : class
    {
        IEnumerable<TPortfolioItem> Get(TPortfolioFilter filter);
        IEnumerable<TPortfolioVideo> GetPortfolioVideos(TKey portfolioItemId);
        void AddVideo(TPortfolioItem portfolioItem,TPortfolioVideo video);
        void RemoveVideo(TPortfolioItem portfolioItem, TPortfolioVideo video);
        void AddScreenshot(TPortfolioItem portfolioItem, TPortfolioScreenshot screenshot);
        void RemoveScreenshot(TPortfolioItem portfolioItem, TPortfolioScreenshot screenshot);
        IEnumerable<TPortfolioScreenshot> GetPortfolioScreenshots(TKey portfolioItemId);
    }
}