USE BookstoreDB
GO

-- Fix Customers
UPDATE Customers SET FullName = N'Phạm Thị D' WHERE PhoneNumber = '0933445566';
UPDATE Customers SET FullName = N'Hoàng Văn E' WHERE PhoneNumber = '0944556677';
UPDATE Customers SET FullName = N'Nguyễn Văn A' WHERE PhoneNumber = '0901234567';
UPDATE Customers SET FullName = N'Trần Thị B' WHERE PhoneNumber = '0987654321';
UPDATE Customers SET FullName = N'Lê Văn C' WHERE PhoneNumber = '0911223344';

-- Fix Books
UPDATE Books SET Title = N'Đắc Nhân Tâm' WHERE Title LIKE N'Đắc Nhân%';
UPDATE Books SET Title = N'Sapiens: Lược Sử Loài Người' WHERE Title LIKE N'Sapiens%';
UPDATE Books SET Title = N'Nhà Giả Kim' WHERE Title LIKE N'Nhà Giả%';
UPDATE Books SET Title = N'Doraemon Tập 1' WHERE Title LIKE N'Doraemon%';
UPDATE Books SET Title = N'Lập Trình C# Cơ Bản' WHERE Title LIKE N'Lập Trình%';

-- Fix Categories
UPDATE Categories SET CategoryName = N'Khoa Học' WHERE CategoryName LIKE N'Khoa H%';
UPDATE Categories SET CategoryName = N'Văn Học' WHERE CategoryName LIKE N'Văn H%';
UPDATE Categories SET CategoryName = N'Lịch Sử' WHERE CategoryName LIKE N'Lịch S%';
UPDATE Categories SET CategoryName = N'Công Nghệ' WHERE CategoryName LIKE N'Công N%';
UPDATE Categories SET CategoryName = N'Kinh Tế' WHERE CategoryName LIKE N'Kinh T%';
UPDATE Categories SET CategoryName = N'Truyện Tranh' WHERE CategoryName LIKE N'Truyện T%';
GO
