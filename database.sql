USE [master]
GO
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'PRN221_PRJ_FoodOrder')
BEGIN
	ALTER DATABASE PRN221_PRJ_FoodOrder SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE PRN221_PRJ_FoodOrder SET ONLINE;
	DROP DATABASE PRN221_PRJ_FoodOrder;
END
GO
CREATE DATABASE PRN221_PRJ_FoodOrder
GO
USE PRN221_PRJ_FoodOrder
GO

CREATE TABLE [user] (
  [user_id] int PRIMARY KEY IDENTITY(1, 1),
  [username] varchar(50) UNIQUE NOT NULL,
  [password] varchar(50) NOT NULL,
  [email] nvarchar(255) UNIQUE NOT NULL,
  [email_confirm] bit NOT NULL,
  [role] nvarchar(255) NOT NULL CHECK ([role] IN ('customer', 'admin')),
  [avatar] nvarchar(255),
  [user_status] bit not null default 1
)
GO

CREATE TABLE [customer] (
  [customer_id] int PRIMARY KEY IDENTITY(1, 1),
  [user_id] int UNIQUE,
  [customer_name] nvarchar(100),
  [customer_address] nvarchar(255),
  [customer_phone] varchar(20)
)
GO

CREATE TABLE [product] (
  [product_id] int PRIMARY KEY IDENTITY(1, 1),
  [product_name] nvarchar(100),
  [product_type] nvarchar(100),
  [unit_price] decimal NOT NULL,
  [description] nvarchar(max),
  [rating] decimal(18,2),
  [product_image] nvarchar(255),
  [status] bit default 1
  )
GO

CREATE TABLE [shopping_cart] (
  [customer_id] int,
  [product_id] int,
  [amount] int NOT NULL DEFAULT (1),
  PRIMARY KEY ([customer_id], [product_id])
)
GO

CREATE TABLE [order] (
  [order_id] int PRIMARY KEY IDENTITY(1, 1),
  [receiver_id] int,
  [receiver_name] nvarchar(100),
  [receiver_phone] varchar(20),
  [order_date] datetime DEFAULT (GETDATE()),
  [order_status] nvarchar(255) NOT NULL CHECK ([order_status] IN ('new', 'hold', 'shipped', 'delivered', 'closed')),
  [delivery_address] nvarchar(max),
  [total] decimal
)
GO

CREATE TABLE [order_item] (
  [order_item_id] int PRIMARY KEY IDENTITY(1, 1),
  [order_id] int,
  [item_id] int,
  [item_name] nvarchar(100),
  [item_type] nvarchar(100),
  [unit_cost] decimal,
  [amount] int,
  [subtotal] decimal
)
GO

CREATE TABLE [comment] (
  [comment_id] int PRIMARY KEY IDENTITY(1, 1),
  [customer_id] int,
  [product_id] int,
  [comment_content] nvarchar(max),
  [created_at] datetime DEFAULT (GETDATE())
)
GO

CREATE TABLE [rate] (
  [customer_id] int,
  [product_id] int,
  [rate_star] int NOT NULL CHECK ([rate_star] IN (1, 2, 3, 4, 5)),
  PRIMARY KEY ([customer_id], [product_id])
)
GO

CREATE UNIQUE INDEX [order_item_index_0] ON [order_item] ("order_id", "item_id")
GO

ALTER TABLE [customer] ADD FOREIGN KEY ([user_id]) REFERENCES [user] ([user_id])
GO

ALTER TABLE [shopping_cart] ADD FOREIGN KEY ([customer_id]) REFERENCES [customer] ([customer_id])
GO

ALTER TABLE [shopping_cart] ADD FOREIGN KEY ([product_id]) REFERENCES [product] ([product_id])
ON DELETE CASCADE
GO

ALTER TABLE [order] ADD FOREIGN KEY ([receiver_id]) REFERENCES [customer] ([customer_id])
GO

ALTER TABLE [order_item] ADD FOREIGN KEY ([order_id]) REFERENCES [order] ([order_id])
GO

ALTER TABLE [order_item] ADD FOREIGN KEY ([item_id]) REFERENCES [product] ([product_id])
ON DELETE SET NULL
GO

ALTER TABLE [comment] ADD FOREIGN KEY ([customer_id]) REFERENCES [customer] ([customer_id])
GO

ALTER TABLE [comment] ADD FOREIGN KEY ([product_id]) REFERENCES [product] ([product_id])
ON DELETE CASCADE
GO

ALTER TABLE [rate] ADD FOREIGN KEY ([customer_id]) REFERENCES [customer] ([customer_id])
GO

ALTER TABLE [rate] ADD FOREIGN KEY ([product_id]) REFERENCES [product] ([product_id])
ON DELETE CASCADE
GO
