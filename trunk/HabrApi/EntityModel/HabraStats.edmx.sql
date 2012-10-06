
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/05/2012 23:30:45
-- Generated from EDMX file: D:\Tps\Source\HabraStatsService\HabrApi\EntityModel\HabraStats.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [habrastats];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Id] int  NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [Url] nvarchar(100)  NOT NULL,
    [PostId] int  NOT NULL,
    [PostUrl] nvarchar(100)  NOT NULL,
    [PostTitle] nvarchar(1000)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Score] int  NOT NULL,
    [ScorePlus] int  NOT NULL,
    [ScoreMinus] int  NOT NULL,
    [UserName] nvarchar(200)  NOT NULL,
    [Avatar] nvarchar(100)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------