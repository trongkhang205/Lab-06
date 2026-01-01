using System;
using System.Drawing;
using System.Windows.Forms;
using Lab_06.BLL;
using Lab_06.DTO;

namespace Lab_06.GUI
{
    public partial class frmSearch : Form
    {
        private ComboBox cboType;
        private TextBox txtKeyword;
        private Button btnSearch;
        private DataGridView dgvResults;
        
        private BookBLL bookBll = new BookBLL();
        private CustomerBLL custBll = new CustomerBLL();

        public frmSearch()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Tra Cứu Thông Tin";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Header Panel
            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.White };
            
            Label lblTitle = new Label { 
                Text = "TRA CỨU DỮ LIỆU", 
                Font = new Font("Segoe UI", 16, FontStyle.Bold), 
                ForeColor = Color.DarkSlateBlue, 
                Location = new Point(20, 15), 
                AutoSize = true 
            };

            Label lblType = new Label { Text = "Tìm Kiếm:", Location = new Point(30, 60), AutoSize = true, Font = new Font("Segoe UI", 10) };
            cboType = new ComboBox { 
                Location = new Point(110, 57), 
                Width = 150, 
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cboType.Items.Add("Sách (Books)");
            cboType.Items.Add("Khách Hàng (Customers)");
            cboType.SelectedIndex = 0; // Default Books

            Label lblKey = new Label { Text = "Từ Khóa:", Location = new Point(280, 60), AutoSize = true, Font = new Font("Segoe UI", 10) };
            txtKeyword = new TextBox { Location = new Point(350, 57), Width = 300, Font = new Font("Segoe UI", 10) };
            txtKeyword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) DoSearch(); };

            btnSearch = new Button { 
                Text = "🔍 Tìm", 
                Location = new Point(670, 55), 
                Width = 100, 
                Height = 32, 
                BackColor = Color.CornflowerBlue, 
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSearch.Click += (s, e) => DoSearch();

            pnlTop.Controls.AddRange(new Control[] { lblTitle, lblType, cboType, lblKey, txtKeyword, btnSearch });

            // Grid
            dgvResults = new DataGridView { Dock = DockStyle.Fill };
            GridHelper.StyleGrid(dgvResults);

            this.Controls.Add(dgvResults);
            this.Controls.Add(pnlTop);
        }

        private void DoSearch()
        {
            string key = txtKeyword.Text.Trim();
            if (cboType.SelectedIndex == 0) // Books
            {
                var books = bookBll.SearchBooks(key);
                dgvResults.DataSource = books;
                dgvResults.Columns["CategoryID"].Visible = false; // Hide if desired
                MessageBox.Show($"Tìm thấy {books.Count} sách.", "Kết Quả");
            }
            else // Customers
            {
                var custs = custBll.SearchCustomers(key);
                dgvResults.DataSource = custs;
                MessageBox.Show($"Tìm thấy {custs.Count} khách hàng.", "Kết Quả");
            }
        }
    }
}
