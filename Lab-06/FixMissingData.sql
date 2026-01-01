USE BookstoreDB
GO

-- 1. FIX CATEGORIES (Ensure 5 & 6 exist)
IF NOT EXISTS (SELECT * FROM Categories WHERE CategoryName = N'Kinh Tế')
    INSERT INTO Categories (CategoryName) VALUES (N'Kinh Tế');
IF NOT EXISTS (SELECT * FROM Categories WHERE CategoryName = N'Truyện Tranh')
    INSERT INTO Categories (CategoryName) VALUES (N'Truyện Tranh');
GO

-- 2. FIX BOOKS (Insert missing books using subquery for CategoryID)
IF NOT EXISTS (SELECT * FROM Books WHERE Title = N'Đắc Nhân Tâm')
BEGIN
    DECLARE @CatEconomy INT = (SELECT TOP 1 CategoryID FROM Categories WHERE CategoryName = N'Kinh Tế')
    IF @CatEconomy IS NOT NULL
        INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) 
        VALUES (N'Đắc Nhân Tâm', N'Dale Carnegie', @CatEconomy, 80000, 50);
END

IF NOT EXISTS (SELECT * FROM Books WHERE Title = N'Doraemon Tập 1')
BEGIN
    DECLARE @CatComic INT = (SELECT TOP 1 CategoryID FROM Categories WHERE CategoryName = N'Truyện Tranh')
    IF @CatComic IS NOT NULL
        INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) 
        VALUES (N'Doraemon Tập 1', N'Fujiko F. Fujio', @CatComic, 20000, 100);
END
GO

-- 3. FIX USERS
IF NOT EXISTS (SELECT * FROM Users WHERE Username = 'staff1')
    INSERT INTO Users (Username, Password, FullName, Role) VALUES ('staff1', '123', N'Nguyễn Văn Nhân Viên', 'Staff');
GO

-- 4. FIX LOANS & DETAILS
-- Ensure we have at least one loan and details
IF NOT EXISTS (SELECT * FROM LoanDetails)
BEGIN
    -- Get some IDs
    DECLARE @Uid INT = (SELECT TOP 1 UserID FROM Users);
    DECLARE @Cid INT = (SELECT TOP 1 CustomerID FROM Customers);
    DECLARE @Bid1 INT = (SELECT TOP 1 BookID FROM Books); 
    DECLARE @Bid2 INT = (SELECT TOP 1 BookID FROM Books ORDER BY BookID DESC);
    
    -- Create Loan if none
    DECLARE @Lid INT;
    IF NOT EXISTS (SELECT * FROM Loans)
    BEGIN
        INSERT INTO Loans (CustomerID, LoanDate, Status) VALUES (@Cid, GETDATE(), 'Borrowed');
        SET @Lid = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        SET @Lid = (SELECT TOP 1 LoanID FROM Loans);
    END

    -- Insert Details
    INSERT INTO LoanDetails (LoanID, BookID, Quantity) VALUES (@Lid, @Bid1, 1);
    IF @Bid1 <> @Bid2
        INSERT INTO LoanDetails (LoanID, BookID, Quantity) VALUES (@Lid, @Bid2, 2);
    
    PRINT 'Inserted missing LoanDetails';
END
GO

-- VERIFY FINAL COUNTS
PRINT '=== FINAL COUNTS ==='
SELECT 'Categories' as TableName, Count(*) as Count FROM Categories
UNION ALL
SELECT 'Books', Count(*) FROM Books
UNION ALL
SELECT 'LoanDetails', Count(*) FROM LoanDetails
