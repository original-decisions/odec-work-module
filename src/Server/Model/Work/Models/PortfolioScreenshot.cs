using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Attach = odec.Server.Model.Attachment.Attachment;
namespace odec.Server.Model.Work.Models
{
    public class PortfolioScreenshot
    {
     //   [Key,Column(Order = 0)]
        public int PortfolioItemId { get; set; }
        public PortfolioItem PortfolioItem { get; set; }
       // [Key, Column(Order = 1)]
        public int ScreenshotId { get; set; }
        public Attach Screenshot { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
        public bool IsMain { get; set; }
    }
}