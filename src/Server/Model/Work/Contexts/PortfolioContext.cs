using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using odec.Server.Model.Work.Abstractions;
using odec.Server.Model.Work.Models;
using System.Linq;
using odec.Server.Model.Attachment;
using odec.Server.Model.Attachment.Extended;
using Usr = odec.Server.Model.User.User;
namespace odec.Server.Model.Work.Contexts
{
    public class PortfolioContext : DbContext,
        //IdentityDbContext<Usr, Role, int, UserClaim, UserRole, UserLogin, IdentityRoleClaim<int>, UserToken>,
        IPortfolioContext<PortfolioItem, PortfolioScreenshot, PortfolioVideo>
    {
        private string MembershipScheme = "AspNet";
        private string WorkScheme = "work";

        private string AttachmentScheme = "attach";
        public PortfolioContext(DbContextOptions<PortfolioContext> options)
            : base(options)
        {

        }
        public DbSet<PortfolioItem> Portfolio { get; set; }
        public DbSet<PortfolioScreenshot> PortfolioScreenshots { get; set; }
        public DbSet<PortfolioVideo> PortfolioVideos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usr>().ToTable("Users", MembershipScheme);
         //   modelBuilder.Entity<Role>().ToTable("Roles", MembershipScheme);
            //modelBuilder.Entity<UserRole>().ToTable("UserRoles", MembershipScheme);
            //modelBuilder.Entity<UserClaim>().ToTable("UserClaims", MembershipScheme);
            //modelBuilder.Entity<UserLogin>().ToTable("UserLogins", MembershipScheme);
            //modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", MembershipScheme);
            //modelBuilder.Entity<UserToken>().ToTable("UserTokens", MembershipScheme);

            modelBuilder.Entity<PortfolioItem>()
                .ToTable("Portfolio", WorkScheme);
            modelBuilder.Entity<PortfolioScreenshot>()
                .ToTable("PortfolioScreenshots", WorkScheme)
                .HasKey(it=> new { it.ScreenshotId,it.PortfolioItemId});
            modelBuilder.Entity<PortfolioVideo>()
                .ToTable("PortfolioVideos", WorkScheme)
                .HasKey(it => new { it.VideoId, it.PortfolioItemId });
            modelBuilder.Entity<Attachment.Attachment>().ToTable("Attachments", AttachmentScheme);
            modelBuilder.Entity<AttachmentType>().ToTable("AttachmentTypes", AttachmentScheme);
            modelBuilder.Entity<Extension>().ToTable("Extensions", AttachmentScheme);
            modelBuilder.Entity<AttachmentTypeExtension>().ToTable("AttachmentTypeExtentions", AttachmentScheme)
                .HasKey(it => new { it.AttachmentTypeId, it.ExtensionId });
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            base.OnModelCreating(modelBuilder);
        }
    }
}
