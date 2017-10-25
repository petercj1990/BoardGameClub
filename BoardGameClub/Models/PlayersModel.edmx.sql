
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/19/2017 15:43:11
-- Generated from EDMX file: C:\Users\pjensen\Documents\GitHub\BoardGameClub\BoardGameClub\Models\PlayersModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [bghq];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AspNetUserLibrary_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLibrary] DROP CONSTRAINT [FK_AspNetUserLibrary_AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserLibrary_Libraries]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLibrary] DROP CONSTRAINT [FK_AspNetUserLibrary_Libraries];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserPlayer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Players] DROP CONSTRAINT [FK_AspNetUserPlayer];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetUsers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers];
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
IF OBJECT_ID(N'[dbo].[FK_LibraryBoardGame_BoardGames]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LibraryBoardGame] DROP CONSTRAINT [FK_LibraryBoardGame_BoardGames];
GO
IF OBJECT_ID(N'[dbo].[FK_LibraryBoardGame_Libraries]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LibraryBoardGame] DROP CONSTRAINT [FK_LibraryBoardGame_Libraries];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerMedal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Medals] DROP CONSTRAINT [FK_PlayerMedal];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerPlaySession_Players]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerPlaySession] DROP CONSTRAINT [FK_PlayerPlaySession_Players];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerPlaySession_PlaySessions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlayerPlaySession] DROP CONSTRAINT [FK_PlayerPlaySession_PlaySessions];
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

-- Creating table 'AspNetUserLibrary'
CREATE TABLE [dbo].[AspNetUserLibrary] (
    [AspNetUsers_Id] nvarchar(128)  NOT NULL,
    [Libraries_Id] int  NOT NULL
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

-- Creating table 'AspNetUserRoles1'
CREATE TABLE [dbo].[AspNetUserRoles1] (
    [AspNetUserRoles1_AspNetUser_Id] nvarchar(128)  NOT NULL,
    [AspNetUserRoles1_AspNetRole_Id] nvarchar(128)  NOT NULL
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

-- Creating primary key on [UserId], [LoginProvider], [ProviderKey] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([UserId], [LoginProvider], [ProviderKey] ASC);
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

-- Creating primary key on [AspNetUsers_Id], [Libraries_Id] in table 'AspNetUserLibrary'
ALTER TABLE [dbo].[AspNetUserLibrary]
ADD CONSTRAINT [PK_AspNetUserLibrary]
    PRIMARY KEY CLUSTERED ([AspNetUsers_Id], [Libraries_Id] ASC);
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

-- Creating primary key on [AspNetUserRoles1_AspNetUser_Id], [AspNetUserRoles1_AspNetRole_Id] in table 'AspNetUserRoles1'
ALTER TABLE [dbo].[AspNetUserRoles1]
ADD CONSTRAINT [PK_AspNetUserRoles1]
    PRIMARY KEY CLUSTERED ([AspNetUserRoles1_AspNetUser_Id], [AspNetUserRoles1_AspNetRole_Id] ASC);
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

-- Creating foreign key on [AspNetUserRoles1_AspNetUser_Id] in table 'AspNetUserRoles1'
ALTER TABLE [dbo].[AspNetUserRoles1]
ADD CONSTRAINT [FK_AspNetUserRoles1_AspNetRole]
    FOREIGN KEY ([AspNetUserRoles1_AspNetUser_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUserRoles1_AspNetRole_Id] in table 'AspNetUserRoles1'
ALTER TABLE [dbo].[AspNetUserRoles1]
ADD CONSTRAINT [FK_AspNetUserRoles1_AspNetUser]
    FOREIGN KEY ([AspNetUserRoles1_AspNetRole_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles1_AspNetUser'
CREATE INDEX [IX_FK_AspNetUserRoles1_AspNetUser]
ON [dbo].[AspNetUserRoles1]
    ([AspNetUserRoles1_AspNetRole_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------