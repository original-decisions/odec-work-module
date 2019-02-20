using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using odec.Server.Model.Work.Abstractions.Interfaces;
using odec.Server.Model.Work.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using odec.Server.Model.User;
using System.Linq;
using odec.Server.Model.Attachment;
using odec.Server.Model.Attachment.Extended;
using CategoryE = odec.Server.Model.Category.Category;
using Usr = odec.Server.Model.User.User;
namespace odec.Server.Model.Work.Contexts
{
    public class WorkContext : DbContext,
        IWorkContext<WorkItem, CategoryE, WorkCategory, WorkFeedback, WorkType, WorkItemTeam, WorkItemDeliverable>
    {
        private string MembershipScheme = "AspNet";
        private string WorkScheme = "work";
        private string AttachmentScheme="attach";
        private string CategoryScheme="dbo";

        public WorkContext(DbContextOptions<WorkContext> options)
            : base(options)
        {

        }
        public DbSet<WorkItem> Works { get; set; }
        public DbSet<CategoryE> Categories { get; set; }
        public DbSet<WorkCategory> WorkCategories { get; set; }
        public DbSet<WorkFeedback> Feedbacks { get; set; }
        public DbSet<WorkType> WorkTypes { get; set; }
        public DbSet<WorkItemTeam> WorkItemTeams { get; set; }
        public DbSet<WorkItemDeliverable> WorkItemDeliverables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Usr>()
                .ToTable("Users", MembershipScheme);
         //   modelBuilder.Entity<Role>().ToTable("Roles", MembershipScheme);
         //   modelBuilder.Entity<UserRole>().ToTable("UserRoles", MembershipScheme);
            //modelBuilder.Entity<UserClaim>().ToTable("UserClaims", MembershipScheme);
            //modelBuilder.Entity<UserLogin>().ToTable("UserLogins", MembershipScheme);
            //modelBuilder.Entity<RoleClaim>().ToTable("RoleClaims", MembershipScheme);
            //modelBuilder.Entity<UserToken>().ToTable("UserTokens", MembershipScheme);

            modelBuilder.Entity<WorkType>()
                .ToTable("WorkTypes", WorkScheme);
            modelBuilder.Entity<WorkCategory>()
                .ToTable("WorkCategories", WorkScheme)
                .HasKey(it => new { it.CategoryId, it.WorkItemId });
            modelBuilder.Entity<WorkItem>()
                .ToTable("Works", WorkScheme)
                .HasOne(wi => wi.WorkType)
                .WithMany(wt => wt.WorkItems)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<WorkFeedback>()
                .ToTable("Feedbacks", WorkScheme);
            modelBuilder.Entity<WorkItemTeam>()
                .ToTable("WorkItemTeams", WorkScheme)
                .HasKey(it => new { it.ExecutorId, it.WorkItemId });
            modelBuilder.Entity<WorkItemDeliverable>()
                .ToTable("WorkItemDeliverables", WorkScheme)
                .HasKey(it=>new {it.DeliverableId,it.WorkItemId});
            modelBuilder.Entity<CategoryE>()
                .ToTable("Categories", CategoryScheme);
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
