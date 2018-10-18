
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/17/2018 11:44:00
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


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[MissingPersons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MissingPersons];
GO
IF OBJECT_ID(N'[dbo].[ContactPersons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContactPersons];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'MissingPersons'
CREATE TABLE [dbo].[MissingPersons] (
    [Id] int  NOT NULL,
    [faceToken] varchar(max)  NOT NULL,
    [firstName] varchar(max)  NULL,
    [lastName] varchar(max)  NULL,
    [lastSeenDate] nvarchar(max)  NOT NULL,
    [lastSeenLocation] nvarchar(max)  NOT NULL,
    [Additional_Information] nvarchar(max)  NOT NULL,
    [dateOfBirth] nvarchar(max)  NOT NULL,
    [faceImg] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ContactPersons'
CREATE TABLE [dbo].[ContactPersons] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [missingPersonId] nvarchar(max)  NOT NULL,
    [firstName] nvarchar(max)  NOT NULL,
    [lastName] nvarchar(max)  NOT NULL,
    [phoneNumber] nvarchar(max)  NOT NULL,
    [emailAddress] nvarchar(max)  NOT NULL
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

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------