using System;
using System.Drawing;
using System.Windows.Forms;
using Lab_06.BLL;
using Lab_06.DTO;

namespace Lab_06.GUI
{
    public partial class frmCustomers : Form
    {
        private DataGridView dgv;
        private TextBox txtName, txtPhone, txtAddr, txtEmail;
        private Button btnAdd, btnEdit, btnDelete, btnExport;
        private CustomerBLL bll = new CustomerBLL();

        public frmCustomers()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản Lý Khách Hàng";
            this.Size = new Size(1000, 600);
            this.BackColor = Color.WhiteSmoke;

            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 180, BackColor = Color.White };

            Label lblHeader = new Label { Text = "THÔNG TIN KHÁCH HÀNG", Font = new Font("Segoe UI", 14, FontStyle.Bold), ForeColor = Color.DarkSlateBlue, Location = new Point(20, 10), AutoSize = true };

            Label lblName = new Label { Text = "Họ Tên:", Location = new Point(30, 50), AutoSize = true };
            txtName = new TextBox { Location = new Point(100, 47), Width = 250 };

            Label lblPhone = new Label { Text = "SĐT:", Location = new Point(380, 50), AutoSize = true };
            txtPhone = new TextBox { Location = new Point(430, 47), Width = 150 };

            Label lblEmail = new Label { Text = "Email:", Location = new Point(620, 50), AutoSize = true };
            txtEmail = new TextBox { Location = new Point(670, 47), Width = 200 };

            Label lblAddr = new Label { Text = "Địa Chỉ:", Location = new Point(30, 90), AutoSize = true };
            txtAddr = new TextBox { Location = new Point(100, 87), Width = 770 };
            
            // Buttons
            btnAdd = CreateButton("➕ Thêm", Color.FromArgb(46, 204, 113), new Point(30, 130));
            btnAdd.Click += (s, e) => AddCustomer();
            
            btnEdit = CreateButton("✏️ Sửa", Color.FromArgb(243, 156, 18), new Point(140, 130));
            btnEdit.Click += (s, e) => EditCustomer();

            btnDelete = CreateButton("🗑️ Xóa", Color.FromArgb(231, 76, 60), new Point(250, 130));
            btnDelete.Click += (s, e) => DeleteCustomer();
            
            btnExport = CreateButton("📤 Xuất Excel", Color.FromArgb(52, 152, 219), new Point(360, 130));
            btnExport.Click += (s, e) => ExcelExporter.ExportToCSV(dgv);

            pnlTop.Controls.AddRange(new Control[] { lblHeader, lblName, txtName, lblPhone, txtPhone, lblAddr, txtAddr, lblEmail, txtEmail, btnAdd, btnEdit, btnDelete, btnExport });

            dgv = new DataGridView { Dock = DockStyle.Fill };
            GridHelper.StyleGrid(dgv);
            dgv.CellClick += Dgv_CellClick;

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
            dgv.DataSource = bll.GetAllCustomers();
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                txtName.Text = row.Cells["FullName"].Value.ToString();
                txtPhone.Text = row.Cells["PhoneNumber"].Value?.ToString();
                txtAddr.Text = row.Cells["Address"].Value?.ToString();
                txtEmail.Text = row.Cells["Email"].Value?.ToString();
            }
        }

        private void AddCustomer()
        {
            Customer c = new Customer { FullName = txtName.Text, PhoneNumber = txtPhone.Text, Address = txtAddr.Text, Email = txtEmail.Text };
            MessageBox.Show(bll.AddCustomer(c));
            LoadData();
        }

        private void EditCustomer()
        {
            if (dgv.SelectedRows.Count == 0) return;
            Customer c = new Customer { 
                CustomerID = Convert.ToInt32(dgv.SelectedRows[0].Cells["CustomerID"].Value),
                FullName = txtName.Text, PhoneNumber = txtPhone.Text, Address = txtAddr.Text, Email = txtEmail.Text 
            };
            MessageBox.Show(bll.UpdateCustomer(c));
            LoadData();
        }

        private void DeleteCustomer()
        {
            if (dgv.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dgv.SelectedRows[0].Cells["CustomerID"].Value);
                if(MessageBox.Show("Xóa khách hàng này?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    MessageBox.Show(bll.DeleteCustomer(id));
                    LoadData();
                }
            }
        }
    }
}
