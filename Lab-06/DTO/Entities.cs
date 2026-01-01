using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab_06.DTO
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        
        [StringLength(100)]
        public string FullName { get; set; }
        
        [StringLength(20)]
        public string Role { get; set; }
    }

    [Table("Categories")]
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }
    }

    [Table("Books")]
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        [StringLength(100)]
        public string Author { get; set; }
        
        public int? CategoryID { get; set; }
        
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        
        // NotMapped property for Grid display if needed, but EF can use navigation property
        [NotMapped]
        public string CategoryName => Category?.CategoryName;

        public decimal Price { get; set; }
        
        public int StockQuantity { get; set; }
    }

    [Table("Customers")]
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        
        [StringLength(200)]
        public string Address { get; set; }
        
        [StringLength(100)]
        public string Email { get; set; }
    }

    [Table("Loans")]
    public class Loan
    {
        [Key]
        public int LoanID { get; set; }
        
        public int CustomerID { get; set; }
        
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        
        public DateTime LoanDate { get; set; }
        
        public DateTime? ReturnDate { get; set; }
        
        [StringLength(20)]
        public string Status { get; set; }
        
        [NotMapped]
        public string CustomerName => Customer?.FullName;

        public virtual ICollection<LoanDetail> LoanDetails { get; set; }
    }

    [Table("LoanDetails")]
    public class LoanDetail
    {
        [Key]
        public int LoanDetailID { get; set; }
        
        public int LoanID { get; set; }
        
        public int BookID { get; set; }
        
        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }
        
        public int Quantity { get; set; }
        
        [NotMapped]
        public string BookTitle { get; set; }
    }
}
