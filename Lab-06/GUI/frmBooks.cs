using System;
using System.Drawing;
using System.Windows.Forms;
using Lab_06.BLL;
using Lab_06.DTO;

namespace Lab_06.GUI
{
    public partial class frmBooks : Form
    {
        private DataGridView dgv;
        private TextBox txtTitle, txtAuthor, txtPrice, txtStock;
        private ComboBox cboCategory;
        private Button btnAdd, btnEdit, btnDelete, btnExport;
        private BookBLL bll = new BookBLL();
        
        public frmBooks()
        {
            InitializeComponent();
            LoadData();
            LoadCategories();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản Lý Sách";
            this.Size = new Size(1000, 600);
            this.BackColor = Color.WhiteSmoke;

            // Top Panel for Inputs
            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 180, BackColor = Color.White, Padding = new Padding(20) };
            
            Label lblHeader = new Label { Text = "THÔNG TIN SÁCH", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.DarkSlateBlue, Location = new Point(20, 10), AutoSize = true };
            
            // Input Group 1
            Label lblTitle = new Label { Text = "Tên Sách:", Location = new Point(30, 50), AutoSize = true };
            txtTitle = new TextBox { Location = new Point(110, 47), Width = 300 };

            Label lblAuthor = new Label { Text = "Tác Giả:", Location = new Point(450, 50), AutoSize = true };
            txtAuthor = new TextBox { Location = new Point(520, 47), Width = 200 };

            // Input Group 2
            Label lblCat = new Label { Text = "Thể Loại:", Location = new Point(30, 90), AutoSize = true };
            cboCategory = new ComboBox { Location = new Point(110, 87), Width = 180, DropDownStyle = ComboBoxStyle.DropDownList };
            cboCategory.DisplayMember = "CategoryName";
            cboCategory.ValueMember = "CategoryID";

            Label lblPrice = new Label { Text = "Giá Bán:", Location = new Point(320, 90), AutoSize = true };
            txtPrice = new TextBox { Location = new Point(380, 87), Width = 120 };

            Label lblStock = new Label { Text = "Tồn Kho:", Location = new Point(550, 90), AutoSize = true };
            txtStock = new TextBox { Location = new Point(620, 87), Width = 100 };

            // Buttons
            btnAdd = CreateButton("➕ Thêm", Color.FromArgb(46, 204, 113), new Point(30, 130)); // Emerald Green
            btnAdd.Click += (s, e) => AddBook();
            
            btnEdit = CreateButton("✏️ Sửa", Color.FromArgb(243, 156, 18), new Point(140, 130)); // Flat Orange
            btnEdit.Click += (s, e) => EditBook();

            btnDelete = CreateButton("🗑️ Xóa", Color.FromArgb(231, 76, 60), new Point(250, 130)); // Flat Red
            btnDelete.Click += (s, e) => DeleteBook();

            btnExport = CreateButton("📤 Xuất Excel", Color.FromArgb(52, 152, 219), new Point(360, 130)); // Flat Blue
            btnExport.Click += (s, e) => ExcelExporter.ExportToCSV(dgv);

            pnlTop.Controls.AddRange(new Control[] { lblHeader, lblTitle, txtTitle, lblAuthor, txtAuthor, lblCat, cboCategory, lblPrice, txtPrice, lblStock, txtStock, btnAdd, btnEdit, btnDelete, btnExport });

            // Grid
            dgv = new DataGridView { Dock = DockStyle.Fill };
            GridHelper.StyleGrid(dgv);
            dgv.CellClick += Dgv_CellClick;

            // Main Container
            this.Controls.Add(dgv);
            this.Controls.Add(pnlTop);
        }

        private Button CreateButton(string text, Color color, Point loc)
        {
            return new Button {
                Text = text,
                Location = loc,
                Width = 100,
                Height = 35,
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
        }

        private void LoadData()
        {
            dgv.DataSource = bll.GetAllBooks();
        }

        private void LoadCategories()
        {
            cboCategory.DataSource = bll.GetCategories();
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                txtTitle.Text = row.Cells["Title"].Value.ToString();
                txtAuthor.Text = row.Cells["Author"].Value?.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
                txtStock.Text = row.Cells["StockQuantity"].Value.ToString();
                try 
                { 
                    if(row.Cells["CategoryID"].Value != null)
                        cboCategory.SelectedValue = Convert.ToInt32(row.Cells["CategoryID"].Value); 
                } 
                catch {}
            }
        }

        private void AddBook()
        {
            try
            {
                Book b = new Book {
                    Title = txtTitle.Text,
                    Author = txtAuthor.Text,
                    CategoryID = (int)cboCategory.SelectedValue,
                    Price = decimal.Parse(txtPrice.Text),
                    StockQuantity = int.Parse(txtStock.Text)
                };
                MessageBox.Show(bll.AddBook(b));
                LoadData();
            } 
            catch { MessageBox.Show("Dữ liệu nhập không hợp lệ"); }
        }

        private void EditBook()
        {
            if (dgv.SelectedRows.Count == 0) return;
            try
            {
                Book b = new Book {
                    BookID = Convert.ToInt32(dgv.SelectedRows[0].Cells["BookID"].Value),
                    Title = txtTitle.Text,
                    Author = txtAuthor.Text,
                    CategoryID = (int)cboCategory.SelectedValue,
                    Price = decimal.Parse(txtPrice.Text),
                    StockQuantity = int.Parse(txtStock.Text)
                };
                MessageBox.Show(bll.UpdateBook(b));
                LoadData();
            }
            catch { MessageBox.Show("Dữ liệu nhập không hợp lệ"); }
        }

        private void DeleteBook()
        {
            if (dgv.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgv.SelectedRows[0].Cells["BookID"].Value);
                if(MessageBox.Show("Bạn có chắc muốn xóa sách này?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    MessageBox.Show(bll.DeleteBook(id));
                    LoadData();
                }
            }
        }
    }
}
