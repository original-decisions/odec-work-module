begin tran
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
	begin
	CREATE TABLE [dbo].[Categories] (
		[Id] [int] NOT NULL IDENTITY,
		[Name] [nvarchar](max) NOT NULL,
		[IsApproved] [bit] NOT NULL,
		[Code] [nvarchar](128) NOT NULL,
		[IsActive] [bit] NOT NULL,
		[SortOrder] [int] NOT NULL,
		[DateUpdated] [datetime] NOT NULL,
		[DateCreated] [datetime] NOT NULL,
		CONSTRAINT [PK_dbo.Categories] PRIMARY KEY ([Id])
	)
	CREATE INDEX [ix_Categories_Name] ON [dbo].[Categories]([Name], [IsApproved])
	CREATE INDEX [ix_Categories_IsApproved] ON [dbo].[Categories]([IsApproved])
end
IF schema_id('work') IS NULL
    EXECUTE('CREATE SCHEMA [work]')
IF schema_id('attach') IS NULL
    EXECUTE('CREATE SCHEMA [attach]')
	IF schema_id('AspNet') IS NULL
    EXECUTE('CREATE SCHEMA [AspNet]')
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[work].[Feedbacks]') AND type in (N'U'))
	begin
CREATE TABLE [work].[Feedbacks] (
    [Id] [int] NOT NULL IDENTITY,
    [Rating] [decimal](18, 2) NOT NULL,
    [Text] [nvarchar](max),
    CONSTRAINT [PK_work.Feedbacks] PRIMARY KEY ([Id])
)
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[Roles]') AND type in (N'U'))
begin
CREATE TABLE [AspNet].[Roles] (
    [Id] [int] NOT NULL IDENTITY,
    [InRoleId] [int],
    [Scope] [nvarchar](max),
    [Name] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_AspNet.Roles] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_InRoleId] ON [AspNet].[Roles]([InRoleId])
CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNet].[Roles]([Name])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[UserRoles]') AND type in (N'U'))
begin
CREATE TABLE [AspNet].[UserRoles] (
    [UserId] [int] NOT NULL,
    [RoleId] [int] NOT NULL,
    CONSTRAINT [PK_AspNet.UserRoles] PRIMARY KEY ([UserId], [RoleId])
)
CREATE INDEX [IX_UserId] ON [AspNet].[UserRoles]([UserId])
CREATE INDEX [IX_RoleId] ON [AspNet].[UserRoles]([RoleId])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[Users]') AND type in (N'U'))
begin
CREATE TABLE [AspNet].[Users] (
    [Id] [int] NOT NULL IDENTITY,
    [Rating] [decimal](18, 2) NOT NULL,
    [ProfilePicturePath] [nvarchar](max),
    [FirstName] [nvarchar](max),
    [LastName] [nvarchar](max),
    [Patronymic] [nvarchar](max),
    [DateUpdated] [datetime],
    [LastActivityDate] [datetime],
    [LastLogin] [datetime],
    [RemindInDays] [int] NOT NULL,
    [DateRegistration] [datetime] NOT NULL,
    [Email] [nvarchar](256),
    [EmailConfirmed] [bit] NOT NULL,
    [PasswordHash] [nvarchar](max),
    [SecurityStamp] [nvarchar](max),
    [PhoneNumber] [nvarchar](max),
    [PhoneNumberConfirmed] [bit] NOT NULL,
    [TwoFactorEnabled] [bit] NOT NULL,
    [LockoutEndDateUtc] [datetime],
    [LockoutEnabled] [bit] NOT NULL,
    [AccessFailedCount] [int] NOT NULL,
    [UserName] [nvarchar](256) NOT NULL,
    CONSTRAINT [PK_AspNet.Users] PRIMARY KEY ([Id])
)
CREATE UNIQUE INDEX [UserNameIndex] ON [AspNet].[Users]([UserName])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[UserClaims]') AND type in (N'U'))
begin
CREATE TABLE [AspNet].[UserClaims] (
    [Id] [int] NOT NULL IDENTITY,
    [UserId] [int] NOT NULL,
    [ClaimType] [nvarchar](max),
    [ClaimValue] [nvarchar](max),
    CONSTRAINT [PK_AspNet.UserClaims] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_UserId] ON [AspNet].[UserClaims]([UserId])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[AspNet].[UserLogins]') AND type in (N'U'))
begin
CREATE TABLE [AspNet].[UserLogins] (
    [LoginProvider] [nvarchar](128) NOT NULL,
    [ProviderKey] [nvarchar](128) NOT NULL,
    [UserId] [int] NOT NULL,
    CONSTRAINT [PK_AspNet.UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey], [UserId])
)
CREATE INDEX [IX_UserId] ON [AspNet].[UserLogins]([UserId])
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[work].[WorkCategories]') AND type in (N'U'))
begin
CREATE TABLE [work].[WorkCategories] (
    [CategoryId] [int] NOT NULL,
    [WorkItemId] [int] NOT NULL,
    CONSTRAINT [PK_work.WorkCategories] PRIMARY KEY ([CategoryId], [WorkItemId])
)
CREATE INDEX [IX_CategoryId] ON [work].[WorkCategories]([CategoryId])
CREATE INDEX [IX_WorkItemId] ON [work].[WorkCategories]([WorkItemId])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[work].[Works]') AND type in (N'U'))
begin
CREATE TABLE [work].[Works] (
    [Id] [int] NOT NULL IDENTITY,
    [Description] [nvarchar](max) NOT NULL,
    [ParentId] [int],
    [DateStarted] [datetime],
    [DateEnded] [datetime],
    [DeadLine] [datetime] NOT NULL,
    [CustomerId] [int] NOT NULL,
    [CustomerFeedbackId] [int],
    [TeamFeedBackId] [int],
    [ActualCost] [decimal](18, 2) NOT NULL,
    [InitialCost] [decimal](18, 2) NOT NULL,
    [WorkTypeId] [int] NOT NULL,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_work.Works] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_ParentId] ON [work].[Works]([ParentId])
CREATE INDEX [IX_CustomerId] ON [work].[Works]([CustomerId])
CREATE INDEX [IX_CustomerFeedbackId] ON [work].[Works]([CustomerFeedbackId])
CREATE INDEX [IX_TeamFeedBackId] ON [work].[Works]([TeamFeedBackId])
CREATE INDEX [IX_WorkTypeId] ON [work].[Works]([WorkTypeId])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[work].[WorkTypes]') AND type in (N'U'))
begin
CREATE TABLE [work].[WorkTypes] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_work.WorkTypes] PRIMARY KEY ([Id])
)
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[work].[WorkItemDeliverables]') AND type in (N'U'))
begin
CREATE TABLE [work].[WorkItemDeliverables] (
    [WorkItemId] [int] NOT NULL,
    [DeliverableId] [int] NOT NULL,
    [IsPublic] [bit] NOT NULL,
    CONSTRAINT [PK_work.WorkItemDeliverables] PRIMARY KEY ([WorkItemId], [DeliverableId])
)
CREATE INDEX [IX_DeliverableId] ON [work].[WorkItemDeliverables]([DeliverableId])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[attach].[Attachments]') AND type in (N'U'))
begin
CREATE TABLE [attach].[Attachments] (
    [Id] [int] NOT NULL IDENTITY,
    [AttachmentTypeId] [int] NOT NULL,
    [ExtensionId] [int] NOT NULL,
    [Content] [varbinary](max),
    [IsShared] [bit] NOT NULL,
    [PublicUri] [nvarchar](max),
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_attach.Attachments] PRIMARY KEY ([Id])
)
CREATE INDEX [IX_AttachmentTypeId] ON [attach].[Attachments]([AttachmentTypeId])
CREATE INDEX [IX_ExtensionId] ON [attach].[Attachments]([ExtensionId])
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[attach].[AttachmentTypes]') AND type in (N'U'))
begin
CREATE TABLE [attach].[AttachmentTypes] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_attach.AttachmentTypes] PRIMARY KEY ([Id])
)
end

IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[attach].[Extensions]') AND type in (N'U'))
begin
CREATE TABLE [attach].[Extensions] (
    [Id] [int] NOT NULL IDENTITY,
    [Name] [nvarchar](max) NOT NULL,
    [Code] [nvarchar](128) NOT NULL,
    [IsActive] [bit] NOT NULL,
    [SortOrder] [int] NOT NULL,
    [DateUpdated] [datetime] NOT NULL,
    [DateCreated] [datetime] NOT NULL,
    CONSTRAINT [PK_attach.Extensions] PRIMARY KEY ([Id])
)
end
IF  NOT EXISTS (SELECT * FROM sys.objects 
	WHERE object_id = OBJECT_ID(N'[work].[WorkItemTeams]') AND type in (N'U'))
begin
CREATE TABLE [work].[WorkItemTeams] (
    [WorkItemId] [int] NOT NULL,
    [ExecutorId] [int] NOT NULL,
    [ExecutorFeedbackId] [int],
    [IsTeamLeader] [bit] NOT NULL,
    CONSTRAINT [PK_work.WorkItemTeams] PRIMARY KEY ([WorkItemId], [ExecutorId])
)
CREATE INDEX [IX_WorkItemId] ON [work].[WorkItemTeams]([WorkItemId])
CREATE INDEX [IX_ExecutorId] ON [work].[WorkItemTeams]([ExecutorId])
CREATE INDEX [IX_ExecutorFeedbackId] ON [work].[WorkItemTeams]([ExecutorFeedbackId])
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.Roles_AspNet.Roles_InRoleId')
		begin
ALTER TABLE [AspNet].[Roles] ADD CONSTRAINT [FK_AspNet.Roles_AspNet.Roles_InRoleId] FOREIGN KEY ([InRoleId]) REFERENCES [AspNet].[Roles] ([Id])
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserRoles_AspNet.Roles_RoleId')
		begin
ALTER TABLE [AspNet].[UserRoles] ADD CONSTRAINT [FK_AspNet.UserRoles_AspNet.Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNet].[Roles] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserRoles_AspNet.Users_UserId')
		begin
ALTER TABLE [AspNet].[UserRoles] ADD CONSTRAINT [FK_AspNet.UserRoles_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserClaims_AspNet.Users_UserId')
		begin
ALTER TABLE [AspNet].[UserClaims] ADD CONSTRAINT [FK_AspNet.UserClaims_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_AspNet.UserLogins_AspNet.Users_UserId')
		begin
ALTER TABLE [AspNet].[UserLogins] ADD CONSTRAINT [FK_AspNet.UserLogins_AspNet.Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNet].[Users] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.WorkCategories_dbo.Categories_CategoryId')
		begin
ALTER TABLE [work].[WorkCategories] ADD CONSTRAINT [FK_work.WorkCategories_dbo.Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.WorkCategories_work.Works_WorkItemId')
		begin
ALTER TABLE [work].[WorkCategories] ADD CONSTRAINT [FK_work.WorkCategories_work.Works_WorkItemId] FOREIGN KEY ([WorkItemId]) REFERENCES [work].[Works] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.Works_AspNet.Users_CustomerId')
		begin
ALTER TABLE [work].[Works] ADD CONSTRAINT [FK_work.Works_AspNet.Users_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNet].[Users] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.Works_work.Feedbacks_CustomerFeedbackId')
		begin
ALTER TABLE [work].[Works] ADD CONSTRAINT [FK_work.Works_work.Feedbacks_CustomerFeedbackId] FOREIGN KEY ([CustomerFeedbackId]) REFERENCES [work].[Feedbacks] ([Id])
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.Works_work.Works_ParentId')
		begin
ALTER TABLE [work].[Works] ADD CONSTRAINT [FK_work.Works_work.Works_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [work].[Works] ([Id])
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.Works_work.Feedbacks_TeamFeedBackId')
		begin
ALTER TABLE [work].[Works] ADD CONSTRAINT [FK_work.Works_work.Feedbacks_TeamFeedBackId] FOREIGN KEY ([TeamFeedBackId]) REFERENCES [work].[Feedbacks] ([Id])
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.Works_work.WorkTypes_WorkTypeId')
		begin
ALTER TABLE [work].[Works] ADD CONSTRAINT [FK_work.Works_work.WorkTypes_WorkTypeId] FOREIGN KEY ([WorkTypeId]) REFERENCES [work].[WorkTypes] ([Id]) 
end
 
if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.WorkItemDeliverables_attach.Attachments_DeliverableId')
		begin
ALTER TABLE [work].[WorkItemDeliverables] ADD CONSTRAINT [FK_work.WorkItemDeliverables_attach.Attachments_DeliverableId] FOREIGN KEY ([DeliverableId]) REFERENCES [attach].[Attachments] ([Id]) 
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_attach.Attachments_attach.AttachmentTypes_AttachmentTypeId')
		begin
ALTER TABLE [attach].[Attachments] ADD CONSTRAINT [FK_attach.Attachments_attach.AttachmentTypes_AttachmentTypeId] FOREIGN KEY ([AttachmentTypeId]) REFERENCES [attach].[AttachmentTypes] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_attach.Attachments_attach.Extensions_ExtensionId')
		begin
ALTER TABLE [attach].[Attachments] ADD CONSTRAINT [FK_attach.Attachments_attach.Extensions_ExtensionId] FOREIGN KEY ([ExtensionId]) REFERENCES [attach].[Extensions] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.WorkItemTeams_AspNet.Users_ExecutorId')
		begin
ALTER TABLE [work].[WorkItemTeams] ADD CONSTRAINT [FK_work.WorkItemTeams_AspNet.Users_ExecutorId] FOREIGN KEY ([ExecutorId]) REFERENCES [AspNet].[Users] ([Id]) 
end 

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.WorkItemTeams_work.Feedbacks_ExecutorFeedbackId')
		begin
ALTER TABLE [work].[WorkItemTeams] ADD CONSTRAINT [FK_work.WorkItemTeams_work.Feedbacks_ExecutorFeedbackId] FOREIGN KEY ([ExecutorFeedbackId]) REFERENCES [work].[Feedbacks] ([Id])
end

if not exists (SELECT  name
                FROM    sys.foreign_keys
                WHERE   name = 'FK_work.WorkItemTeams_work.Works_WorkItemId')
		begin
ALTER TABLE [work].[WorkItemTeams] ADD CONSTRAINT [FK_work.WorkItemTeams_work.Works_WorkItemId] FOREIGN KEY ([WorkItemId]) REFERENCES [work].[Works] ([Id]) 
end

commit tran