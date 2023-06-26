USE [master];
GO
-- Create the new database if it does not exist already
IF NOT EXISTS
(
    SELECT [databases].[name]
    FROM [sys].[databases]
    WHERE [databases].[name] = N'sneakerShop'
)
    CREATE DATABASE [sneakerShop];
GO

USE [sneakerShop];
GO

IF OBJECT_ID('[dbo].[CartItem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CartItem];
GO
CREATE TABLE [dbo].[CartItem]
(
    [cartItemId] [NVARCHAR](50) NOT NULL,
    [quantity] [INT] NULL,
    [unitPrice] [FLOAT] NULL,
    [productId] [INT] NULL,
    [cartId] [NVARCHAR](50) NULL,
    CONSTRAINT [PK_CartItem]
        PRIMARY KEY ([cartItemId])
);
GO

