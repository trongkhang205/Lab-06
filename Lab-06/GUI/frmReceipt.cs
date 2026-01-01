using System;
using System.Drawing;
using System.Windows.Forms;
using Lab_06.BLL;
using Lab_06.DTO;
using System.Collections.Generic;

namespace Lab_06.GUI
{
    public partial class frmReceipt : Form
    {
        private int _loanId;
        private LoanBLL bll = new LoanBLL();

        public frmReceipt(int loanId)
        {
            _loanId = loanId;
            InitializeComponent();
            LoadReceipt();
        }

        private void InitializeComponent()
        {
            this.Text = "Chi Tiết Phiếu Mượn";
            this.Size = new Size(400, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
        }

        private void LoadReceipt()
        {
            var loan = bll.GetLoanById(_loanId);
            if (loan == null) return;

            // Simple visualization using a big label or text box
            RichTextBox rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;
            rtb.Font = new Font("Consolas", 11);
            rtb.ReadOnly = true;
            rtb.BorderStyle = BorderStyle.None;
            rtb.Padding = new Padding(20);

            string content = "";
            content += "================================\n";
            content += "       PHIẾU MƯỢN SÁCH          \n";
            content += "================================\n\n";
            content += $"Mã Phiếu:  {loan.LoanID}\n";
            content += $"Ngày Mượn: {loan.LoanDate:dd/MM/yyyy HH:mm}\n";
            content += $"Khách Hàng: {loan.Customer?.FullName}\n";
            content += $"Số ĐT:      {loan.Customer?.PhoneNumber}\n";
            content += "--------------------------------\n";
            content += "SÁCH ĐÃ MƯỢN:\n\n";

            // Now DAL includes LoanDetails and Book
            if (loan.LoanDetails != null)
            {
                foreach (var d in loan.LoanDetails)
                {
                    string title = d.Book?.Title ?? "Unknown Book";
                    if (title.Length > 20) title = title.Substring(0, 17) + "...";
                    content += $"- {title,-20} x{d.Quantity}\n";
                }
            }
            
            content += "--------------------------------\n";
            content += $"Trạng Thái: {loan.Status}\n";
            if (loan.ReturnDate != null)
                content += $"Ngày Trả:   {loan.ReturnDate:dd/MM/yyyy}\n";

            content += "\n================================\n";
            content += "Cảm ơn quý khách!\n";

            rtb.Text = content;
            this.Controls.Add(rtb);
        }
    }
}
