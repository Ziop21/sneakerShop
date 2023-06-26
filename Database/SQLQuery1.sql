USE [master];
GO

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

CREATE TABLE [dbo].[UserRoles]
(
    [roleId] [INT] IDENTITY(1, 1) NOT NULL,
    [roleName] [NVARCHAR](50) NULL,
    CONSTRAINT [PK_UserRoles]
        PRIMARY KEY ([roleId])
);
GO

CREATE TABLE [dbo].[Users]
(
    [userId] [INT] IDENTITY(1, 1) NOT NULL,
    [username] [NVARCHAR](50) NULL,
    [email] [NVARCHAR](100) NULL,
    [fullname] [NVARCHAR](50) NULL,
    [password] [NVARCHAR](50) NULL,
    [images] [NVARCHAR](500) NULL,
    [phone] [NVARCHAR](20) NULL,
    [status] [INT] NULL,
    [roleId] [INT] NULL,
    [defaultAddress] NVARCHAR(500) NULL,
    [paypalNumber] VARCHAR(50) NULL,
    CONSTRAINT [PK_Users]
        PRIMARY KEY ([userId])
);
GO

CREATE TABLE [dbo].[Category]
(
    [categoryId] [INT] IDENTITY(1, 1) NOT NULL,
    [categoryName] [NVARCHAR](200) NULL,
    [status] [INT] NULL,
    CONSTRAINT [PK_Category]
        PRIMARY KEY ([categoryId])
);
GO

CREATE TABLE [dbo].[Product]
(
    [productId] [INT] IDENTITY(1, 1) NOT NULL,
    [productName] [NVARCHAR](200) NULL,
    [categoryId] [INT] NULL,
    [description] [NVARCHAR](500) NULL,
    [price] [FLOAT] NULL,
    [amount] INT NULL, --số sản phẩm đã bán được
    [status] TINYINT NULL,
    [createDate] [DATE] NULL,
    [updateDate] DATE,
    CONSTRAINT [PK_Product]
        PRIMARY KEY ([productId] ASC)
);
GO

CREATE TABLE [Stock]
(
    [productId] [INT] NOT NULL,
    [inStock] INT NOT NULL CHECK ([inStock] >= 0),
    [lastUpdate] DATE NULL,
    CONSTRAINT [PK_ProductInStock]
        PRIMARY KEY ([productId] ASC)
);

CREATE TABLE [imagesProduct]
(
    [productId] [INT] NOT NULL,
    [images] [VARCHAR](255) NOT NULL,
    CONSTRAINT [PK_imagesProduce]
        PRIMARY KEY (
                        [productId],
                        [images]
                    )
);
GO

CREATE TABLE [dbo].[Cart]
(
    [cartId] [INT] IDENTITY(1, 1) NOT NULL,
    [userId] [INT] NULL,
    [buyDate] [DATETIME] NULL,
    [status] [INT] NULL,
    CONSTRAINT [PK_Cart]
        PRIMARY KEY ([cartId])
);
GO

CREATE TABLE [dbo].[CartItem]
(
    [cartId] [INT] IDENTITY(1, 1) NOT NULL,
    [productId] [INT],
    [quantity] [INT] NULL,
    [unitPrice] [FLOAT] NULL,
    CONSTRAINT [PK_CartItem]
        PRIMARY KEY (
                        [cartId],
                        [productId]
                    )
);
GO

CREATE TABLE [dbo].[Order]
(
    [orderID] INT IDENTITY(1, 1) NOT NULL,
    [userID] [INT] NOT NULL,
    [cartID] INT NOT NULL,
    [orderDate] DATE,
    [status] TINYINT,
    [shipping] TINYINT,
    [totalPay] BIGINT,
    [paymentType] INT,
    [address] NVARCHAR(500),
    CONSTRAINT [PK_order]
        PRIMARY KEY ([orderID])
);
GO

CREATE TABLE [paymentType]
(
    [paymentTypeID] INT IDENTITY NOT NULL,
    [paymentTypeName] NVARCHAR(50),
    CONSTRAINT [PK_pmType]
        PRIMARY KEY ([paymentTypeID])
);
GO

CREATE TABLE [Shipment]
(
    [shipmentID] INT IDENTITY NOT NULL,
    [shipperID] INT NOT NULL,
    [orderID] INT NOT NULL,
    CONSTRAINT [PK_shipment]
        PRIMARY KEY ([shipmentID])
);
GO

ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_userrole]
    FOREIGN KEY ([roleId])
    REFERENCES [dbo].[UserRoles] ([roleId]);
GO

ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [FK_category]
    FOREIGN KEY ([categoryId])
    REFERENCES [dbo].[Category] ([categoryId]);
GO

ALTER TABLE [dbo].[Stock]
ADD CONSTRAINT [FK_stock]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[Product] ([productId]);
GO

ALTER TABLE [dbo].[imagesProduct]
ADD CONSTRAINT [FK_images]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[Product] ([productId]);
GO

ALTER TABLE [dbo].[Cart]
ADD CONSTRAINT [FK_cartUsers]
    FOREIGN KEY ([userId])
    REFERENCES [dbo].[Users] ([userId]);
GO

ALTER TABLE [dbo].[CartItem]
ADD CONSTRAINT [FK_cartitemCart]
    FOREIGN KEY ([cartId])
    REFERENCES [dbo].[Cart] ([cartId]);
GO

ALTER TABLE [dbo].[CartItem]
ADD CONSTRAINT [FK_cartitemProduct]
    FOREIGN KEY ([productId])
    REFERENCES [dbo].[Product] ([productId]);
GO

ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [FK_orderUser]
    FOREIGN KEY ([userID])
    REFERENCES [dbo].[Users] ([userId]);
GO

ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [FK_orderCart]
    FOREIGN KEY ([cartID])
    REFERENCES [dbo].[Cart] ([cartId]);
GO

ALTER TABLE [dbo].[Order]
ADD CONSTRAINT [FK_orderPaymentType]
    FOREIGN KEY ([paymentType])
    REFERENCES [dbo].[paymentType] ([paymentTypeID]);
GO

ALTER TABLE [dbo].[Shipment]
ADD CONSTRAINT [FK_shipper]
    FOREIGN KEY ([shipperID])
    REFERENCES [dbo].[Users] ([userId]);
GO

ALTER TABLE [dbo].[Shipment]
ADD CONSTRAINT [FK_shipOrder]
    FOREIGN KEY ([orderID])
    REFERENCES [dbo].[Order] ([orderID]);
GO
