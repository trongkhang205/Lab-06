USE master
GO

-- 1. KILL CONNECTIONS & DROP DATABASE
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'BookstoreDB')
BEGIN
    ALTER DATABASE [BookstoreDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [BookstoreDB];
END
GO

-- 2. CREATE DATABASE
CREATE DATABASE [BookstoreDB] COLLATE Latin1_General_100_CI_AS_SC_UTF8; -- Try UTF8 collation if supported, or just standard
GO

USE [BookstoreDB]
GO

-- 3. CREATE TABLES
CREATE TABLE [dbo].[Users](
    [UserID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Username] [nvarchar](50) NOT NULL,
    [Password] [nvarchar](50) NOT NULL,
    [FullName] [nvarchar](100) NULL,
    [Role] [nvarchar](20) DEFAULT 'Staff'
)

CREATE TABLE [dbo].[Categories](
    [CategoryID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CategoryName] [nvarchar](100) NOT NULL
)

CREATE TABLE [dbo].[Books](
    [BookID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] [nvarchar](200) NOT NULL,
    [Author] [nvarchar](100) NULL,
    [CategoryID] [int] NULL FOREIGN KEY REFERENCES [dbo].[Categories]([CategoryID]),
    [Price] [decimal](18, 2) NOT NULL,
    [StockQuantity] [int] DEFAULT 0
)

CREATE TABLE [dbo].[Customers](
    [CustomerID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [FullName] [nvarchar](100) NOT NULL,
    [PhoneNumber] [nvarchar](20) NULL,
    [Address] [nvarchar](200) NULL,
    [Email] [nvarchar](100) NULL
)

CREATE TABLE [dbo].[Loans](
    [LoanID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CustomerID] [int] NOT NULL FOREIGN KEY REFERENCES [dbo].[Customers]([CustomerID]),
    [LoanDate] [datetime] DEFAULT GETDATE(),
    [ReturnDate] [datetime] NULL,
    [Status] [nvarchar](50) DEFAULT N'Đang Mượn'
)

CREATE TABLE [dbo].[LoanDetails](
    [LoanDetailID] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [LoanID] [int] NOT NULL FOREIGN KEY REFERENCES [dbo].[Loans]([LoanID]),
    [BookID] [int] NOT NULL FOREIGN KEY REFERENCES [dbo].[Books]([BookID]),
    [Quantity] [int] DEFAULT 1
)
GO

-- 4. INSERT DATA (Crucial: Save this file as UTF-8/Unicode)

INSERT INTO Users (Username, Password, FullName, Role) VALUES 
('admin', '123456', N'Quản Trị Viên', 'Admin'),
('staff', '123', N'Nhân Viên Bán Hàng', 'Staff');

INSERT INTO Categories (CategoryName) VALUES 
(N'Khoa Học'), 
(N'Văn Học'), 
(N'Lịch Sử'), 
(N'Công Nghệ'), 
(N'Kinh Tế'), 
(N'Truyện Tranh');

INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES 
(N'Sapiens: Lược Sử Loài Người', N'Yuval Noah Harari', 3, 150000, 20),
(N'Clean Code', N'Robert C. Martin', 4, 300000, 10),
(N'Nhà Giả Kim', N'Paulo Coelho', 2, 75000, 30),
(N'Đắc Nhân Tâm', N'Dale Carnegie', 5, 80000, 50),
(N'Doraemon Tập 1', N'Fujiko F. Fujio', 6, 20000, 100),
(N'Mắt Biếc', N'Nguyễn Nhật Ánh', 2, 90000, 40);

INSERT INTO Customers (FullName, PhoneNumber, Address, Email) VALUES 
(N'Nguyễn Văn An', '0901234567', N'123 Lê Lợi, TP.HCM', 'an@email.com'),
(N'Trần Thị Bình', '0987654321', N'456 Nguyễn Huệ, Hà Nội', 'binh@gmail.com'),
(N'Lê Văn Cường', '0911223344', N'789 Đường 2/9, Đà Nẵng', 'cuong@outlook.com');

INSERT INTO Loans (CustomerID, LoanDate, Status) VALUES 
(1, GETDATE(), N'Đang Mượn'),
(2, DATEADD(day, -2, GETDATE()), N'Đã Trả');

INSERT INTO LoanDetails (LoanID, BookID, Quantity) VALUES 
(1, 1, 1), 
(1, 2, 1),
(2, 5, 5);
GO

-- 5. VERIFY DATA IMMEDIATELY
PRINT '====== CHECKING DATA ======'
SELECT Title As [Book Title (Kiem Tra TV)] FROM Books
SELECT FullName As [Customer Name (Kiem Tra TV)] FROM Customers
SELECT CategoryName As [Category (Kiem Tra TV)] FROM Categories
GO
