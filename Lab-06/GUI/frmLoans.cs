using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Lab_06.BLL;
using Lab_06.DTO;

namespace Lab_06.GUI
{
    public partial class frmLoans : Form
    {
        private TabControl tabs;
        private ComboBox cboCust, cboBook;
        private TextBox txtQty;
        private DataGridView dgvCart, dgvHistory;
        private List<LoanDetail> cart = new List<LoanDetail>();
        
        private LoanBLL bll = new LoanBLL();
        private BookBLL bookBll = new BookBLL();
        private CustomerBLL custBll = new CustomerBLL();

        public frmLoans()
        {
            InitializeComponent();
            LoadCombos();
            LoadHistory();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản Lý Mượn Trả";
            this.Size = new Size(1000, 700);
            this.BackColor = Color.WhiteSmoke;

            tabs = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10), Padding = new Point(10, 5) };

            // --- Tab 1: New Loan ---
            TabPage pageNew = new TabPage("Mượn Sách Mới") { Padding = new Padding(20) };
            
            // Header
            Label lblHeader = new Label { Text = "TẠO PHIẾU MƯỢN MỚI", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.DarkSlateBlue, Location = new Point(20, 20), AutoSize = true };
            
            // Input Panel
            Panel pnlInput = new Panel { Location = new Point(20, 60), Size = new Size(950, 100), BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            Label lblCust = new Label { Text = "Khách Hàng:", Location = new Point(20, 25), AutoSize = true };
            cboCust = new ComboBox { Location = new Point(120, 22), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cboCust.DisplayMember = "FullName"; cboCust.ValueMember = "CustomerID";

            Label lblBook = new Label { Text = "Chọn Sách:", Location = new Point(20, 65), AutoSize = true };
            cboBook = new ComboBox { Location = new Point(120, 62), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cboBook.DisplayMember = "Title"; cboBook.ValueMember = "BookID";

            Label lblQty = new Label { Text = "Số Lượng:", Location = new Point(450, 65), AutoSize = true };
            txtQty = new TextBox { Location = new Point(530, 62), Width = 60, Text = "1", TextAlign = HorizontalAlignment.Center };

            Button btnAdd = new Button { Text = "➕ Thêm Vào Giỏ", Location = new Point(620, 60), Width = 150, Height = 30, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
            btnAdd.Click += AddToCart;

            pnlInput.Controls.AddRange(new Control[] { lblCust, cboCust, lblBook, cboBook, lblQty, txtQty, btnAdd });

            // Cart Grid with Label
            Label lblCart = new Label { Text = "🛒 Giỏ Hàng Chi Tiết:", Location = new Point(20, 180), AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            
            dgvCart = new DataGridView { Location = new Point(20, 210), Width = 940, Height = 300 };
            GridHelper.StyleGrid(dgvCart);
            dgvCart.Columns.Add("BookID", "ID");
            dgvCart.Columns.Add("Title", "Tên Sách");
            dgvCart.Columns.Add("Qty", "Số Lượng");

            Button btnSave = new Button { Text = "✅ XÁC NHẬN MƯỢN", Location = new Point(760, 530), Width = 200, Height = 50, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, Font = new Font("Segoe UI", 12, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnSave.Click += SaveLoan;

            pageNew.Controls.AddRange(new Control[] { lblHeader, pnlInput, lblCart, dgvCart, btnSave });

            // --- Tab 2: History ---
            TabPage pageHist = new TabPage("Lịch Sử Mượn");
            
            Panel pnlHistTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.White };
            Label lblHistHeader = new Label { Text = "DANH SÁCH PHIẾU MƯỢN", Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(20, 15), AutoSize = true, ForeColor = Color.DarkSlateBlue };
            Button btnRefreshTop = new Button { Text = "Làm Mới", Location = new Point(850, 15), Width = 100, Height = 30, BackColor = Color.LightGray, FlatStyle = FlatStyle.Flat };
            btnRefreshTop.Click += (s, e) => LoadHistory();
            pnlHistTop.Controls.AddRange(new Control[] { lblHistHeader, btnRefreshTop });

            dgvHistory = new DataGridView { Dock = DockStyle.Fill };
            GridHelper.StyleGrid(dgvHistory);
            // Hide some cols if needed
            
            // History Controls Panel
            Panel pnlAction = new Panel { Dock = DockStyle.Bottom, Height = 60, BackColor = Color.White };
            
            Button btnReturn = new Button { Text = "↩️ TRẢ SÁCH", Location = new Point(20, 10), Width = 150, Height = 40, BackColor = Color.FromArgb(230, 126, 34), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnReturn.Click += BtnReturn_Click;

            Button btnPrint = new Button { Text = "🖨️ IN PHIẾU", Location = new Point(190, 10), Width = 150, Height = 40, BackColor = Color.FromArgb(142, 68, 173), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnPrint.Click += BtnPrint_Click;

            Button btnRefresh = new Button { Text = "🔄 Tải Lại", Location = new Point(360, 10), Width = 120, Height = 40, BackColor = Color.Gray, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
            btnRefresh.Click += (s, e) => dgvHistory.DataSource = bll.GetAllLoans();

            pnlAction.Controls.AddRange(new Control[] { btnReturn, btnPrint, btnRefresh });

            pageHist.Controls.Add(dgvHistory);
            pageHist.Controls.Add(pnlAction);
            pageHist.Controls.Add(pnlHistTop);

            tabs.TabPages.Add(pageNew);
            tabs.TabPages.Add(pageHist);
            this.Controls.Add(tabs);
        }

        private void LoadCombos()
        {
            cboCust.DataSource = custBll.GetAllCustomers();
            cboBook.DataSource = bookBll.GetAllBooks();
        }

        private void LoadHistory()
        {
            dgvHistory.DataSource = bll.GetAllLoans();
        }

        private void AddToCart(object sender, EventArgs e)
        {
            try
            {
                int qty = int.Parse(txtQty.Text);
                if (qty <= 0) throw new Exception();

                int bid = (int)cboBook.SelectedValue;
                foreach(var item in cart) {
                    if (item.BookID == bid) {
                        item.Quantity += qty;
                        RefreshCartGrid();
                        return;
                    }
                }

                // Add new
                Book selectedBook = null;
                // Try cast
                if(cboBook.SelectedItem is Book b) selectedBook = b;
                else if (cboBook.SelectedItem is DataRowView drv) // fallback
                    selectedBook = new Book { Title = drv["Title"].ToString() };
                else // Using EF List<Book>
                    selectedBook = (Book)cboBook.SelectedItem;

                cart.Add(new LoanDetail { 
                    BookID = bid, 
                    BookTitle = selectedBook.Title,
                    Quantity = qty 
                });
                RefreshCartGrid();
            }
            catch { MessageBox.Show("Số lượng không hợp lệ hoặc lỗi chọn sách"); }
        }

        private void RefreshCartGrid()
        {
            dgvCart.Rows.Clear();
            foreach(var item in cart)
            {
                dgvCart.Rows.Add(item.BookID, item.BookTitle, item.Quantity);
            }
        }

        private void SaveLoan(object sender, EventArgs e)
        {
            if (cart.Count == 0 || cboCust.SelectedValue == null) 
            {
                MessageBox.Show("Giỏ hàng trống hoặc chưa chọn khách hàng!");
                return;
            }
            try
            {
                Loan loan = new Loan {
                    CustomerID = (int)cboCust.SelectedValue,
                    LoanDate = DateTime.Now
                };
                
                string res = bll.CreateLoan(loan, cart);
                if(res == "Success")
                {
                    MessageBox.Show("Mượn sách thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cart.Clear();
                    RefreshCartGrid();
                    LoadHistory(); // Auto refresh history
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra: " + res);
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }
        private void BtnReturn_Click(object sender, EventArgs e)
        {
            if (dgvHistory.SelectedRows.Count == 0) return;
            var row = dgvHistory.SelectedRows[0];
            int id = (int)row.Cells["LoanID"].Value;
            string status = row.Cells["Status"].Value.ToString();
            
            if (status == "Đã Trả") { MessageBox.Show("Phiếu này đã trả rồi!"); return; }

            if (MessageBox.Show("Xác nhận trả sách cho phiếu này?", "Xác Nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bll.ReturnLoan(id);
                dgvHistory.DataSource = bll.GetAllLoans();
                MessageBox.Show("Đã cập nhật trạng thái trả sách.");
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (dgvHistory.SelectedRows.Count == 0) return;
            int id = (int)dgvHistory.SelectedRows[0].Cells["LoanID"].Value;
            new frmReceipt(id).ShowDialog();
        }
    }
}
