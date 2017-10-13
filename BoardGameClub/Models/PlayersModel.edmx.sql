
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/13/2017 14:49:55
-- Generated from EDMX file: C:\Users\pjensen\Documents\GitHub\BoardGameClub\BoardGameClub\Models\PlayersModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BoardGameClub];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AspNetUserLibrary_AspNetUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLibrary] DROP CONSTRAINT [FK_AspNetUserLibrary_AspNetUser];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserLibrary_Library]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLibrary] DROP CONSTRAINT [FK_AspNetUserLibrary_Library];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Players] DROP CONSTRAINT [FK_AspNetUserPlayer];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRole];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUser];
GO
IF OBJECT_ID(N'[dbo].[FK_BoardGamePlaySession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlaySessions] DROP CONSTRAINT [FK_BoardGamePlaySession];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_LibraryBoardGame_BoardGame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LibraryBoardGame] DROP CONSTRAINT [FK_LibraryBoardGame_BoardGame];
GO
IF OBJECT_ID(N'[dbo].[FK_LibraryBoardGame_Library]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LibraryBoardGame] DROP CONSTRAINT [FK_LibraryBoardGame_Library];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerMedal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Medals] DROP CONSTRAINT [FK_PlayerMedal];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerPlaySession_Player]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerPlaySession] DROP CONSTRAINT [FK_PlayerPlaySession_Player];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerPlaySession_PlaySession]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerPlaySession] DROP CONSTRAINT [FK_PlayerPlaySession_PlaySession];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLibrary]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLibrary];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[BoardGames]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BoardGames];
GO
IF OBJECT_ID(N'[dbo].[C__MigrationHistory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[C__MigrationHistory];
GO
IF OBJECT_ID(N'[dbo].[C__MigrationHistory1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[C__MigrationHistory1];
GO
IF OBJECT_ID(N'[dbo].[Libraries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Libraries];
GO
IF OBJECT_ID(N'[dbo].[LibraryBoardGame]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LibraryBoardGame];
GO
IF OBJECT_ID(N'[dbo].[Medals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Medals];
GO
IF OBJECT_ID(N'[dbo].[PlayerPlaySession]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerPlaySession];
GO
IF OBJECT_ID(N'[dbo].[Players]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Players];
GO
IF OBJECT_ID(N'[dbo].[PlaySessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlaySessions];
GO
IF OBJECT_ID(N'[BoardGameClubModelStoreContainer].[database_firewall_rules]', 'U') IS NOT NULL
    DROP TABLE [BoardGameClubModelStoreContainer].[database_firewall_rules];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL,
    [Department] nvarchar(max)  NULL
);
GO

-- Creating table 'BoardGames'
CREATE TABLE [dbo].[BoardGames] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [TotalPlays] int  NOT NULL,
    [BGGID] int  NOT NULL
);
GO

-- Creating table 'C__MigrationHistory'
CREATE TABLE [dbo].[C__MigrationHistory] (
    [MigrationId] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'C__MigrationHistory1'
CREATE TABLE [dbo].[C__MigrationHistory1] (
    [MigrationId] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'Libraries'
CREATE TABLE [dbo].[Libraries] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Medals'
CREATE TABLE [dbo].[Medals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Player_Id] int  NULL
);
GO

-- Creating table 'Players'
CREATE TABLE [dbo].[Players] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [AspNetUser_Id] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'PlaySessions'
CREATE TABLE [dbo].[PlaySessions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DatePlayed] datetime  NOT NULL,
    [PlayTime] time  NOT NULL,
    [BoardGame_Id] int  NOT NULL
);
GO

-- Creating table 'database_firewall_rules'
CREATE TABLE [dbo].[database_firewall_rules] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(128)  NOT NULL,
    [start_ip_address] varchar(45)  NOT NULL,
    [end_ip_address] varchar(45)  NOT NULL,
    [create_date] datetime  NOT NULL,
    [modify_date] datetime  NOT NULL
);
GO

-- Creating table 'AspNetUserLibrary'
CREATE TABLE [dbo].[AspNetUserLibrary] (
    [AspNetUsers_Id] nvarchar(128)  NOT NULL,
    [Libraries_Id] int  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'LibraryBoardGame'
CREATE TABLE [dbo].[LibraryBoardGame] (
    [BoardGames_Id] int  NOT NULL,
    [Libraries_Id] int  NOT NULL
);
GO

-- Creating table 'PlayerPlaySession'
CREATE TABLE [dbo].[PlayerPlaySession] (
    [Players_Id] int  NOT NULL,
    [PlaySessions_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BoardGames'
ALTER TABLE [dbo].[BoardGames]
ADD CONSTRAINT [PK_BoardGames]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory'
ALTER TABLE [dbo].[C__MigrationHistory]
ADD CONSTRAINT [PK_C__MigrationHistory]
    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
GO

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory1'
ALTER TABLE [dbo].[C__MigrationHistory1]
ADD CONSTRAINT [PK_C__MigrationHistory1]
    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
GO

-- Creating primary key on [Id] in table 'Libraries'
ALTER TABLE [dbo].[Libraries]
ADD CONSTRAINT [PK_Libraries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Medals'
ALTER TABLE [dbo].[Medals]
ADD CONSTRAINT [PK_Medals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [PK_Players]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PlaySessions'
ALTER TABLE [dbo].[PlaySessions]
ADD CONSTRAINT [PK_PlaySessions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [id], [name], [start_ip_address], [end_ip_address], [create_date], [modify_date] in table 'database_firewall_rules'
ALTER TABLE [dbo].[database_firewall_rules]
ADD CONSTRAINT [PK_database_firewall_rules]
    PRIMARY KEY CLUSTERED ([id], [name], [start_ip_address], [end_ip_address], [create_date], [modify_date] ASC);
GO

-- Creating primary key on [AspNetUsers_Id], [Libraries_Id] in table 'AspNetUserLibrary'
ALTER TABLE [dbo].[AspNetUserLibrary]
ADD CONSTRAINT [PK_AspNetUserLibrary]
    PRIMARY KEY CLUSTERED ([AspNetUsers_Id], [Libraries_Id] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- Creating primary key on [BoardGames_Id], [Libraries_Id] in table 'LibraryBoardGame'
ALTER TABLE [dbo].[LibraryBoardGame]
ADD CONSTRAINT [PK_LibraryBoardGame]
    PRIMARY KEY CLUSTERED ([BoardGames_Id], [Libraries_Id] ASC);
GO

-- Creating primary key on [Players_Id], [PlaySessions_Id] in table 'PlayerPlaySession'
ALTER TABLE [dbo].[PlayerPlaySession]
ADD CONSTRAINT [PK_PlayerPlaySession]
    PRIMARY KEY CLUSTERED ([Players_Id], [PlaySessions_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [AspNetUser_Id] in table 'Players'
ALTER TABLE [dbo].[Players]
ADD CONSTRAINT [FK_AspNetUserPlayer]
    FOREIGN KEY ([AspNetUser_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserPlayer'
CREATE INDEX [IX_FK_AspNetUserPlayer]
ON [dbo].[Players]
    ([AspNetUser_Id]);
GO

-- Creating foreign key on [BoardGame_Id] in table 'PlaySessions'
ALTER TABLE [dbo].[PlaySessions]
ADD CONSTRAINT [FK_BoardGamePlaySession]
    FOREIGN KEY ([BoardGame_Id])
    REFERENCES [dbo].[BoardGames]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BoardGamePlaySession'
CREATE INDEX [IX_FK_BoardGamePlaySession]
ON [dbo].[PlaySessions]
    ([BoardGame_Id]);
GO

-- Creating foreign key on [Player_Id] in table 'Medals'
ALTER TABLE [dbo].[Medals]
ADD CONSTRAINT [FK_PlayerMedal]
    FOREIGN KEY ([Player_Id])
    REFERENCES [dbo].[Players]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerMedal'
CREATE INDEX [IX_FK_PlayerMedal]
ON [dbo].[Medals]
    ([Player_Id]);
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserLibrary'
ALTER TABLE [dbo].[AspNetUserLibrary]
ADD CONSTRAINT [FK_AspNetUserLibrary_AspNetUsers]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Libraries_Id] in table 'AspNetUserLibrary'
ALTER TABLE [dbo].[AspNetUserLibrary]
ADD CONSTRAINT [FK_AspNetUserLibrary_Libraries]
    FOREIGN KEY ([Libraries_Id])
    REFERENCES [dbo].[Libraries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserLibrary_Libraries'
CREATE INDEX [IX_FK_AspNetUserLibrary_Libraries]
ON [dbo].[AspNetUserLibrary]
    ([Libraries_Id]);
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- Creating foreign key on [BoardGames_Id] in table 'LibraryBoardGame'
ALTER TABLE [dbo].[LibraryBoardGame]
ADD CONSTRAINT [FK_LibraryBoardGame_BoardGames]
    FOREIGN KEY ([BoardGames_Id])
    REFERENCES [dbo].[BoardGames]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Libraries_Id] in table 'LibraryBoardGame'
ALTER TABLE [dbo].[LibraryBoardGame]
ADD CONSTRAINT [FK_LibraryBoardGame_Libraries]
    FOREIGN KEY ([Libraries_Id])
    REFERENCES [dbo].[Libraries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LibraryBoardGame_Libraries'
CREATE INDEX [IX_FK_LibraryBoardGame_Libraries]
ON [dbo].[LibraryBoardGame]
    ([Libraries_Id]);
GO

-- Creating foreign key on [Players_Id] in table 'PlayerPlaySession'
ALTER TABLE [dbo].[PlayerPlaySession]
ADD CONSTRAINT [FK_PlayerPlaySession_Players]
    FOREIGN KEY ([Players_Id])
    REFERENCES [dbo].[Players]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PlaySessions_Id] in table 'PlayerPlaySession'
ALTER TABLE [dbo].[PlayerPlaySession]
ADD CONSTRAINT [FK_PlayerPlaySession_PlaySessions]
    FOREIGN KEY ([PlaySessions_Id])
    REFERENCES [dbo].[PlaySessions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerPlaySession_PlaySessions'
CREATE INDEX [IX_FK_PlayerPlaySession_PlaySessions]
ON [dbo].[PlayerPlaySession]
    ([PlaySessions_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------