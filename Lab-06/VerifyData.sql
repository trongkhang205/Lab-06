USE BookstoreDB
GO

PRINT '--- DATA VERIFICATION REPORT ---'

PRINT '1. Users Table:'
SELECT Count(*) as UserCount FROM Users
SELECT TOP 3 * FROM Users

PRINT '-------------------------------'
PRINT '2. Categories Table:'
SELECT Count(*) as CategoryCount FROM Categories
SELECT TOP 5 * FROM Categories

PRINT '-------------------------------'
PRINT '3. Books Table:'
SELECT Count(*) as BookCount FROM Books
SELECT TOP 5 Title, Author, Price FROM Books

PRINT '-------------------------------'
PRINT '4. Customers Table:'
SELECT Count(*) as CustomerCount FROM Customers
SELECT TOP 5 FullName, PhoneNumber FROM Customers

PRINT '-------------------------------'
PRINT '5. Loans Table:'
SELECT Count(*) as LoanCount FROM Loans
SELECT TOP 5 * FROM Loans

PRINT '-------------------------------'
PRINT '6. LoanDetails Table:'
SELECT Count(*) as LoanDetailCount FROM LoanDetails
SELECT TOP 5 * FROM LoanDetails
GO
