using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity; // Ensure this is using System.Data.Entity for EF6
using Lab_06.DTO;

namespace Lab_06.DAL
{
    public class UserDAL
    {
        public User Login(string username, string password)
        {
            using (var db = new BookstoreContext())
            {
                return db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            }
        }

        public List<User> GetAllUsers()
        {
            using (var db = new BookstoreContext()) { return db.Users.ToList(); }
        }

        public bool AddUser(User user)
        {
            using (var db = new BookstoreContext())
            {
                if (db.Users.Any(u => u.Username == user.Username)) return false;
                db.Users.Add(user);
                return db.SaveChanges() > 0;
            }
        }

        public bool UpdateUser(User user)
        {
            using (var db = new BookstoreContext())
            {
                var existing = db.Users.Find(user.UserID);
                if (existing != null)
                {
                    existing.FullName = user.FullName;
                    // Password update is separate for security preference, or we can allow reset here.
                    // This is simple admin update
                    if (!string.IsNullOrEmpty(user.Password)) existing.Password = user.Password; 
                    existing.Role = user.Role;
                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }

        public bool DeleteUser(int id)
        {
            using (var db = new BookstoreContext())
            {
                var u = db.Users.Find(id);
                if (u != null) { db.Users.Remove(u); return db.SaveChanges() > 0; }
                return false;
            }
        }

        public bool ChangePassword(int uid, string oldPass, string newPass)
        {
            using (var db = new BookstoreContext())
            {
                var u = db.Users.Find(uid);
                if (u != null && u.Password == oldPass)
                {
                    u.Password = newPass;
                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }
    }

    public class BookDAL
    {
        public List<Book> GetAllBooks()
        {
            using (var db = new BookstoreContext())
            {
                // Include Category to eager load navigation property
                return db.Books.Include(b => b.Category).ToList();
            }
        }

        public bool AddBook(Book book)
        {
            using (var db = new BookstoreContext())
            {
                db.Books.Add(book);
                return db.SaveChanges() > 0;
            }
        }

        public bool UpdateBook(Book book)
        {
            using (var db = new BookstoreContext())
            {
                var existing = db.Books.Find(book.BookID);
                if (existing != null)
                {
                    existing.Title = book.Title;
                    existing.Author = book.Author;
                    existing.CategoryID = book.CategoryID;
                    existing.Price = book.Price;
                    existing.StockQuantity = book.StockQuantity;
                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }

        public bool DeleteBook(int bookId)
        {
            using (var db = new BookstoreContext())
            {
                var book = db.Books.Find(bookId);
                if (book != null)
                {
                    db.Books.Remove(book);
                    return db.SaveChanges() > 0;
                }
                return false;
            }
        }
    }
    
    public class CategoryDAL
    {
        public List<Category> GetAllCategories()
        {
            using (var db = new BookstoreContext())
            {
                return db.Categories.ToList();
            }
        }
    }
}
