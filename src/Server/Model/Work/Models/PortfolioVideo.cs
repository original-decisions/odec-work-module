using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Attach = odec.Server.Model.Attachment.Attachment;
namespace odec.Server.Model.Work.Models
{
    public class PortfolioVideo
    {
      //  [Key, Column(Order = 0)]
        public int PortfolioItemId { get; set; }
        public PortfolioItem PortfolioItem { get; set; }
     //   [Key, Column(Order = 1)]
        public int VideoId { get; set; }
        public Attach Video { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string Description { get; set; }
    }
}