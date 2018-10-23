
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/23/2018 21:35:19
-- Generated from EDMX file: C:\Users\tomas\Source\Repos\ProtingaVizija\SmartVision\Api\Models\Tables.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [pstop2018];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MissingPersonContactPerson_ContactPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MissingPersonContactPerson] DROP CONSTRAINT [FK_MissingPersonContactPerson_ContactPerson];
GO
IF OBJECT_ID(N'[dbo].[FK_MissingPersonContactPerson_MissingPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MissingPersonContactPerson] DROP CONSTRAINT [FK_MissingPersonContactPerson_MissingPerson];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ContactPersons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContactPersons];
GO
IF OBJECT_ID(N'[dbo].[MissingPersonContactPerson]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MissingPersonContactPerson];
GO
IF OBJECT_ID(N'[dbo].[MissingPersons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MissingPersons];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'MissingPersons'
CREATE TABLE [dbo].[MissingPersons] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [faceToken] varchar(max)  NULL,
    [firstName] varchar(max)  NULL,
    [lastName] varchar(max)  NULL,
    [lastSeenDate] nvarchar(max)  NULL,
    [lastSeenLocation] nvarchar(max)  NULL,
    [Additional_Information] nvarchar(max)  NULL,
    [dateOfBirth] nvarchar(max)  NULL,
    [faceImg] nvarchar(max)  NULL
);
GO

-- Creating table 'ContactPersons'
CREATE TABLE [dbo].[ContactPersons] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [firstName] nvarchar(max)  NOT NULL,
    [lastName] nvarchar(max)  NOT NULL,
    [phoneNumber] nvarchar(max)  NOT NULL,
    [emailAddress] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MissingPersonContactPerson'
CREATE TABLE [dbo].[MissingPersonContactPerson] (
    [ContactPersons_Id] int  NOT NULL,
    [MissingPersons_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'MissingPersons'
ALTER TABLE [dbo].[MissingPersons]
ADD CONSTRAINT [PK_MissingPersons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ContactPersons'
ALTER TABLE [dbo].[ContactPersons]
ADD CONSTRAINT [PK_ContactPersons]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ContactPersons_Id], [MissingPersons_Id] in table 'MissingPersonContactPerson'
ALTER TABLE [dbo].[MissingPersonContactPerson]
ADD CONSTRAINT [PK_MissingPersonContactPerson]
    PRIMARY KEY CLUSTERED ([ContactPersons_Id], [MissingPersons_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ContactPersons_Id] in table 'MissingPersonContactPerson'
ALTER TABLE [dbo].[MissingPersonContactPerson]
ADD CONSTRAINT [FK_MissingPersonContactPerson_ContactPerson]
    FOREIGN KEY ([ContactPersons_Id])
    REFERENCES [dbo].[ContactPersons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [MissingPersons_Id] in table 'MissingPersonContactPerson'
ALTER TABLE [dbo].[MissingPersonContactPerson]
ADD CONSTRAINT [FK_MissingPersonContactPerson_MissingPerson]
    FOREIGN KEY ([MissingPersons_Id])
    REFERENCES [dbo].[MissingPersons]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MissingPersonContactPerson_MissingPerson'
CREATE INDEX [IX_FK_MissingPersonContactPerson_MissingPerson]
ON [dbo].[MissingPersonContactPerson]
    ([MissingPersons_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------