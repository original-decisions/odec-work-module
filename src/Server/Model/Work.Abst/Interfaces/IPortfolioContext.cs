using Microsoft.EntityFrameworkCore;

namespace odec.Server.Model.Work.Abstractions
{
    public interface IPortfolioContext<TPortfolioItem, TPortfolioScreenshot, TPortfolioVideo> where TPortfolioItem : class where TPortfolioScreenshot : class where TPortfolioVideo : class
    {
        DbSet<TPortfolioItem> Portfolio { get; set; } 
        DbSet<TPortfolioScreenshot> PortfolioScreenshots { get; set; }
        DbSet<TPortfolioVideo> PortfolioVideos { get; set; }  
    }
}