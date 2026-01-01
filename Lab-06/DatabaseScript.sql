USE master
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'BookstoreDB')
BEGIN
    CREATE DATABASE BookstoreDB
END
GO

USE BookstoreDB
GO

-- 1. Users
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Users](
        [UserID] [int] IDENTITY(1,1) NOT NULL,
        [Username] [nvarchar](50) NOT NULL,
        [Password] [nvarchar](50) NOT NULL,
        [FullName] [nvarchar](100) NULL,
        [Role] [nvarchar](20) DEFAULT 'Staff',
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([UserID] ASC)
    )
    -- Sample Users (3 rows)
    INSERT INTO Users (Username, Password, FullName, Role) VALUES 
    ('admin', '123456', N'Quản Trị Viên', 'Admin'),
    ('staff1', '123', N'Nguyễn Văn Nhân Viên', 'Staff'),
    ('mod', '123', N'Trần Kiểm Duyệt', 'Staff')
END

-- 2. Categories
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Categories](
        [CategoryID] [int] IDENTITY(1,1) NOT NULL,
        [CategoryName] [nvarchar](100) NOT NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([CategoryID] ASC)
    )
    -- Sample Categories (6 rows)
    INSERT INTO Categories (CategoryName) VALUES (N'Khoa Học'), (N'Văn Học'), (N'Lịch Sử'), (N'Công Nghệ'), (N'Kinh Tế'), (N'Truyện Tranh')
END

-- 3. Books
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Books]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Books](
        [BookID] [int] IDENTITY(1,1) NOT NULL,
        [Title] [nvarchar](200) NOT NULL,
        [Author] [nvarchar](100) NULL,
        [CategoryID] [int] NULL,
        [Price] [decimal](18, 2) NOT NULL,
        [StockQuantity] [int] DEFAULT 0,
        CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED ([BookID] ASC),
        CONSTRAINT [FK_Books_Categories] FOREIGN KEY([CategoryID]) REFERENCES [dbo].[Categories] ([CategoryID])
    )
END

-- 4. Customers
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Customers](
        [CustomerID] [int] IDENTITY(1,1) NOT NULL,
        [FullName] [nvarchar](100) NOT NULL,
        [PhoneNumber] [nvarchar](20) NULL,
        [Address] [nvarchar](200) NULL,
        [Email] [nvarchar](100) NULL,
        CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([CustomerID] ASC)
    )
END

-- 5. Loans
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Loans]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Loans](
        [LoanID] [int] IDENTITY(1,1) NOT NULL,
        [CustomerID] [int] NOT NULL,
        [LoanDate] [datetime] DEFAULT GETDATE(),
        [ReturnDate] [datetime] NULL,
        [Status] [nvarchar](20) DEFAULT 'Borrowed',
        CONSTRAINT [PK_Loans] PRIMARY KEY CLUSTERED ([LoanID] ASC),
        CONSTRAINT [FK_Loans_Customers] FOREIGN KEY([CustomerID]) REFERENCES [dbo].[Customers] ([CustomerID])
    )
END

-- 6. LoanDetails
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LoanDetails]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LoanDetails](
        [LoanDetailID] [int] IDENTITY(1,1) NOT NULL,
        [LoanID] [int] NOT NULL,
        [BookID] [int] NOT NULL,
        [Quantity] [int] DEFAULT 1,
        CONSTRAINT [PK_LoanDetails] PRIMARY KEY CLUSTERED ([LoanDetailID] ASC),
        CONSTRAINT [FK_LoanDetails_Loans] FOREIGN KEY([LoanID]) REFERENCES [dbo].[Loans] ([LoanID]),
        CONSTRAINT [FK_LoanDetails_Books] FOREIGN KEY([BookID]) REFERENCES [dbo].[Books] ([BookID])
    )
END

-- --- SAMPLE DATA INSERTION (Upsert Logic to prevent duplicates) ---

-- Books (Ensure at least 5-10 rows)
IF NOT EXISTS (SELECT * FROM Books WHERE Title = N'Clean Code')
    INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES (N'Clean Code', N'Robert C. Martin', 4, 300000, 10);
IF NOT EXISTS (SELECT * FROM Books WHERE Title = N'Đắc Nhân Tâm')
    INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES (N'Đắc Nhân Tâm', N'Dale Carnegie', 5, 80000, 50);
IF NOT EXISTS (SELECT * FROM Books WHERE Title = N'Sapiens: Lược Sử Loài Người')
    INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES (N'Sapiens: Lược Sử Loài Người', N'Yuval Noah Harari', 3, 150000, 20);
IF NOT EXISTS (SELECT * FROM Books WHERE Title = N'Doraemon Tập 1')
    INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES (N'Doraemon Tập 1', N'Fujiko F. Fujio', 6, 20000, 100);
IF NOT EXISTS (SELECT * FROM Books WHERE Title = N'Nhà Giả Kim')
    INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES (N'Nhà Giả Kim', N'Paulo Coelho', 2, 75000, 30);
IF NOT EXISTS (SELECT * FROM Books WHERE Title = N'Lập Trình C# Cơ Bản')
    INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES (N'Lập Trình C# Cơ Bản', N'Phạm Khang', 4, 120000, 15);

-- Customers (Ensure at least 5 rows)
IF NOT EXISTS (SELECT * FROM Customers WHERE PhoneNumber = '0901234567')
    INSERT INTO Customers (FullName, PhoneNumber, Address, Email) VALUES (N'Nguyễn Văn A', N'0901234567', N'123 Lê Lợi, TP.HCM', N'vana@email.com');
IF NOT EXISTS (SELECT * FROM Customers WHERE PhoneNumber = '0987654321')
    INSERT INTO Customers (FullName, PhoneNumber, Address, Email) VALUES (N'Trần Thị B', N'0987654321', N'456 Nguyễn Huệ, Hà Nội', N'btran@gmail.com');
IF NOT EXISTS (SELECT * FROM Customers WHERE PhoneNumber = '0911223344')
    INSERT INTO Customers (FullName, PhoneNumber, Address, Email) VALUES (N'Lê Văn C', N'0911223344', N'789 Đà Nẵng', N'cle@outlook.com');
IF NOT EXISTS (SELECT * FROM Customers WHERE PhoneNumber = '0933445566')
    INSERT INTO Customers (FullName, PhoneNumber, Address, Email) VALUES (N'Phạm Thị D', N'0933445566', N'101 Cần Thơ', N'dpham@yahoo.com');
IF NOT EXISTS (SELECT * FROM Customers WHERE PhoneNumber = '0944556677')
    INSERT INTO Customers (FullName, PhoneNumber, Address, Email) VALUES (N'Hoàng Văn E', N'0944556677', N'202 Hải Phòng', N'ehoang@gmail.com');

-- Loans (Sample transactions)
-- Note: Assuming IDs exist from above, hardcoded for sample data simplicity
IF NOT EXISTS (SELECT * FROM Loans)
BEGIN
    INSERT INTO Loans (CustomerID, LoanDate, Status) VALUES (1, GETDATE(), 'Borrowed');
    INSERT INTO LoanDetails (LoanID, BookID, Quantity) VALUES (1, 1, 1), (1, 4, 2);

    INSERT INTO Loans (CustomerID, LoanDate, Status) VALUES (2, DATEADD(day, -5, GETDATE()), 'Borrowed');
    INSERT INTO LoanDetails (LoanID, BookID, Quantity) VALUES (2, 2, 1);
END
GO
