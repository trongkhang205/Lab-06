// Updated DAL implementation - Lab 06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Lab_06.DTO;

namespace Lab_06.DAL
{
    public class CustomerDAL
    {
        public List<Customer> GetAllCustomers()
        {
            using (var db = new BookstoreContext())
            {
                return db.Customers.ToList();
            }
        }

        public bool AddCustomer(Customer customer)
        {
            using (var db = new BookstoreContext())
            {
                db.Customers.Add(customer);
                return db.SaveChanges() > 0;
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            using (var db = new BookstoreContext())
            {
                var existing = db.Customers.Find(customer.CustomerID);
                if (existing != null)
                {
                    existing.FullName = customer.FullName;
                    existing.PhoneNumber = customer.PhoneNumber;
                    existing.Address = customer.Address;
                    existing.Email = customer.Email;
                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }

        public bool DeleteCustomer(int id)
        {
            using (var db = new BookstoreContext())
            {
                var c = db.Customers.Find(id);
                if(c != null)
                {
                     db.Customers.Remove(c);
                     return db.SaveChanges() > 0;
                }
                return false;
            }
        }
    }

    public class LoanDAL
    {
        public List<Loan> GetAllLoans()
        {
            using (var db = new BookstoreContext())
            {
                 // Include Customer AND LoanDetails.Book for full receipt info
                 return db.Loans
                          .Include(l => l.Customer)
                          .Include(l => l.LoanDetails.Select(ld => ld.Book))
                          .ToList();
            }
        }
        
        public bool CreateLoan(Loan loan, List<LoanDetail> details)
        {
            using (var db = new BookstoreContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        loan.Status = "Borrowed";
                        db.Loans.Add(loan);
                        db.SaveChanges(); // Get LoanID

                        foreach(var item in details)
                        {
                            item.LoanID = loan.LoanID;
                            db.LoanDetails.Add(item); // Note: Make sure BookID is set
                            
                            // Decrease Stock
                            var book = db.Books.Find(item.BookID);
                            if (book != null)
                            {
                                book.StockQuantity -= item.Quantity;
                            }
                        }
                        db.SaveChanges();
                        trans.Commit();
                        return true;
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool ReturnLoan(int loanId)
        {
            using (var db = new BookstoreContext())
            {
                var loan = db.Loans.Find(loanId);
                if (loan != null && loan.Status != "Đã Trả") // Ensure we check Vietnamese status if we set it as such
                {
                    loan.Status = "Đã Trả";
                    loan.ReturnDate = DateTime.Now;
                    
                    // Optional: Return stock logic could be added here if we want to be strict.
                    // For now, just mark return is simpler for MVP.
                    
                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }
    }
}
