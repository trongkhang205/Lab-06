USE BookstoreDB
GO

-- 1. DELETE ALL DATA (Order matters due to Foreign Keys)
DELETE FROM LoanDetails;
DELETE FROM Loans;
DELETE FROM Books;
DELETE FROM Categories;
DELETE FROM Customers;
DELETE FROM Users;

-- 2. RESET IDENTITY COUNTERS (So IDs start from 1 again)
DBCC CHECKIDENT ('LoanDetails', RESEED, 0);
DBCC CHECKIDENT ('Loans', RESEED, 0);
DBCC CHECKIDENT ('Books', RESEED, 0);
DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('Customers', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);
GO

-- 3. INSERT CLEAN DATA (With N prefix for Unicode)

-- Users
INSERT INTO Users (Username, Password, FullName, Role) VALUES 
('admin', '123456', N'Quản Trị Viên', 'Admin'),
('staff1', '123', N'Nguyễn Văn Nhân Viên', 'Staff'),
('mod', '123', N'Trần Kiểm Duyệt', 'Staff');

-- Categories
INSERT INTO Categories (CategoryName) VALUES 
(N'Khoa Học'), 
(N'Văn Học'), 
(N'Lịch Sử'), 
(N'Công Nghệ'), 
(N'Kinh Tế'), 
(N'Truyện Tranh');

-- Customers
INSERT INTO Customers (FullName, PhoneNumber, Address, Email) VALUES 
(N'Nguyễn Văn A', '0901234567', N'123 Lê Lợi, TP.HCM', 'vana@email.com'),
(N'Trần Thị B', '0987654321', N'456 Nguyễn Huệ, Hà Nội', 'btran@gmail.com'),
(N'Lê Văn C', '0911223344', N'789 Đường 2/9, Đà Nẵng', 'cle@outlook.com'),
(N'Phạm Thị D', '0933445566', N'101 Ninh Kiều, Cần Thơ', 'dpham@yahoo.com'),
(N'Hoàng Văn E', '0944556677', N'202 Lạch Tray, Hải Phòng', 'ehoang@gmail.com');

-- Books (Note: Explicitly using IDs is risky if Identity is on, but since we reseed, it should be sequential. 
-- However, safer to rely on Identity and lookups, OR just insert plain values and let DB assign IDs 1,2,3... which we know will happen after reseed)

-- Cat 1: Khoa Học
INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES 
(N'Sapiens: Lược Sử Loài Người', N'Yuval Noah Harari', 3, 150000, 20); -- History actually

-- Cat 4: Công Nghệ
INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES 
(N'Clean Code', N'Robert C. Martin', 4, 300000, 10),
(N'Lập Trình C# Cơ Bản', N'Phạm Khang', 4, 120000, 15);

-- Cat 2: Văn Học
INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES 
(N'Nhà Giả Kim', N'Paulo Coelho', 2, 75000, 30);

-- Cat 5: Kinh Tế
INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES 
(N'Đắc Nhân Tâm', N'Dale Carnegie', 5, 80000, 50);

-- Cat 6: Truyện Tranh
INSERT INTO Books (Title, Author, CategoryID, Price, StockQuantity) VALUES 
(N'Doraemon Tập 1', N'Fujiko F. Fujio', 6, 20000, 100);

-- 4. INSERT LOANS (Sample Transactions)
-- Loan 1: Customer 1 borrows Book 1 (Sapiens) and Book 4 (Clean Code)
INSERT INTO Loans (CustomerID, LoanDate, Status) VALUES (1, GETDATE(), N'Đang Mượn');
-- Get last ID
DECLARE @L1 INT = SCOPE_IDENTITY();
INSERT INTO LoanDetails (LoanID, BookID, Quantity) VALUES 
(@L1, 1, 1), 
(@L1, 2, 1);

-- Loan 2: Customer 2 borrows Book 6 (Doraemon)
INSERT INTO Loans (CustomerID, LoanDate, Status) VALUES (2, DATEADD(day, -5, GETDATE()), N'Đang Mượn');
DECLARE @L2 INT = SCOPE_IDENTITY();
INSERT INTO LoanDetails (LoanID, BookID, Quantity) VALUES 
(@L2, 6, 5);

GO
