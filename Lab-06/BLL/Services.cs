using System;
using System.Collections.Generic;
using Lab_06.DAL;
using Lab_06.DTO;

namespace Lab_06.BLL
{
    public class BookBLL
    {
        BookDAL dal = new BookDAL();
        CategoryDAL catDal = new CategoryDAL();

        public List<Book> GetAllBooks()
        {
            return dal.GetAllBooks();
        }
        
        public List<Category> GetCategories()
        {
            return catDal.GetAllCategories();
        }

        public string AddBook(Book b)
        {
            if (string.IsNullOrWhiteSpace(b.Title)) return "Title is required";
            if (b.Price < 0) return "Price must be positive";
            
            return dal.AddBook(b) ? "Success" : "Failed to add book";
        }

        public string UpdateBook(Book b)
        {
            return dal.UpdateBook(b) ? "Success" : "Failed to update book";
        }

        public string DeleteBook(int id)
        {
            return dal.DeleteBook(id) ? "Success" : "Failed to delete book";
        }

        public List<Book> SearchBooks(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAllBooks();
            keyword = keyword.ToLower();
            return GetAllBooks().FindAll(b => b.Title.ToLower().Contains(keyword) || (b.Author != null && b.Author.ToLower().Contains(keyword)));
        }
    }

    public class UserBLL
    {
        UserDAL dal = new UserDAL();

        public List<User> GetAllUsers() { return dal.GetAllUsers(); }
        public string AddUser(User u) { return dal.AddUser(u) ? "Success" : "Failed"; }
        public string UpdateUser(User u) { return dal.UpdateUser(u) ? "Success" : "Failed"; }
        public string DeleteUser(int id) { return dal.DeleteUser(id) ? "Success" : "Failed"; }
        
        public string ChangePassword(int uid, string oldPass, string newPass)
        {
            return dal.ChangePassword(uid, oldPass, newPass) ? "Success" : "Wrong old password or Error";
        }
    }

    public class CustomerBLL
    {
        CustomerDAL dal = new CustomerDAL();

        public List<Customer> GetAllCustomers()
        {
            return dal.GetAllCustomers();
        }

        public string AddCustomer(Customer c)
        {
            if(string.IsNullOrEmpty(c.FullName)) return "Name required";
            return dal.AddCustomer(c) ? "Success" : "Failed";
        }
        
        public string UpdateCustomer(Customer c)
        {
             return dal.UpdateCustomer(c) ? "Success" : "Failed";
        }
        
        public string DeleteCustomer(int id)
        {
             return dal.DeleteCustomer(id) ? "Success" : "Failed";
        }

        public List<Customer> SearchCustomers(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return GetAllCustomers();
            keyword = keyword.ToLower();
            return GetAllCustomers().FindAll(c => c.FullName.ToLower().Contains(keyword) || (c.PhoneNumber != null && c.PhoneNumber.Contains(keyword)));
        }
    }

    public class LoanBLL
    {
        LoanDAL dal = new LoanDAL();
        
        public List<Loan> GetAllLoans()
        {
            return dal.GetAllLoans();
        }
        
        public string CreateLoan(Loan loan, List<LoanDetail> details)
        {
            if (details.Count == 0) return "No books selected";
            try
            {
                dal.CreateLoan(loan, details);
                return "Success";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string ReturnLoan(int loanId)
        {
            return dal.ReturnLoan(loanId) ? "Success" : "Failed";
        }

        public Loan GetLoanById(int id)
        {
            // Simple helper to get loan details for printing
            return dal.GetAllLoans().Find(l => l.LoanID == id);
        }
    }
}
