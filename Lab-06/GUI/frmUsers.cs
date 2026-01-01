using System;
using System.Drawing;
using System.Windows.Forms;
using Lab_06.BLL;
using Lab_06.DTO;

namespace Lab_06.GUI
{
    public partial class frmUsers : Form
    {
        private TextBox txtUser, txtName, txtPass;
        private ComboBox cboRole;
        private Button btnAdd, btnEdit, btnDelete;
        private DataGridView dgv;
        private UserBLL bll = new UserBLL();

        public frmUsers()
        {
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Text = "Quản Lý Nhân Viên";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            // Header
            Panel pnlTop = new Panel { Dock = DockStyle.Top, Height = 160, BackColor = Color.White };
            Label lblHeader = new Label { Text = "QUẢN LÝ TÀI KHOẢN", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.DarkSlateBlue, Location = new Point(20, 15), AutoSize = true };

            Label lblUser = new Label { Text = "Username:", Location = new Point(30, 60), AutoSize = true };
            txtUser = new TextBox { Location = new Point(120, 57), Width = 150 };

            Label lblPass = new Label { Text = "Password:", Location = new Point(300, 60), AutoSize = true };
            txtPass = new TextBox { Location = new Point(380, 57), Width = 150, PasswordChar = '●' }; // Hide initially? Or plain for admin. Let's do plain for ease. 

            Label lblName = new Label { Text = "Họ Tên:", Location = new Point(30, 95), AutoSize = true };
            txtName = new TextBox { Location = new Point(120, 92), Width = 250 };

            Label lblRole = new Label { Text = "Vai Trò:", Location = new Point(400, 95), AutoSize = true };
            cboRole = new ComboBox { Location = new Point(460, 92), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };
            cboRole.Items.AddRange(new string[] { "Staff", "Admin" });
            cboRole.SelectedIndex = 0;

            // Buttons
            btnAdd = ButtonHelper("➕ Thêm", Color.SeaGreen, 30);
            btnAdd.Click += (s, e) => AddUser();
            
            btnEdit = ButtonHelper("✏️ Sửa", Color.Orange, 140);
            btnEdit.Click += (s, e) => EditUser();

            btnDelete = ButtonHelper("🗑️ Xóa", Color.IndianRed, 250);
            btnDelete.Click += (s, e) => DeleteUser();

            pnlTop.Controls.AddRange(new Control[] { lblHeader, lblUser, txtUser, lblPass, txtPass, lblName, txtName, lblRole, cboRole, btnAdd, btnEdit, btnDelete });

            // Grid
            dgv = new DataGridView { Dock = DockStyle.Fill };
            GridHelper.StyleGrid(dgv);
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.CellClick += Dgv_CellClick;

            this.Controls.Add(dgv);
            this.Controls.Add(pnlTop);
        }

        private Button ButtonHelper(string text, Color bg, int x)
        {
            return new Button { 
                Text = text, 
                Location = new Point(x, 125), 
                Width = 100, 
                Height = 30, 
                BackColor = bg, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
        }

        private void LoadData()
        {
            dgv.DataSource = bll.GetAllUsers();
            if (dgv.Columns["UserID"] != null) dgv.Columns["UserID"].Visible = false;
            // Maybe hide password column too
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgv.Rows[e.RowIndex];
                txtUser.Text = row.Cells["Username"].Value.ToString();
                txtName.Text = row.Cells["FullName"].Value?.ToString();
                cboRole.SelectedItem = row.Cells["Role"].Value?.ToString();
                txtPass.Text = ""; // Don't show password hash/plain
                txtUser.Tag = row.Cells["UserID"].Value; // Store ID
            }
        }

        private void AddUser()
        {
            var u = new User
            {
                Username = txtUser.Text,
                Password = txtPass.Text,
                FullName = txtName.Text,
                Role = cboRole.SelectedItem.ToString()
            };
            string res = bll.AddUser(u);
            MessageBox.Show(res);
            LoadData();
        }

        private void EditUser()
        {
            if (txtUser.Tag == null) return;
            var u = new User
            {
                UserID = (int)txtUser.Tag,
                Username = txtUser.Text,
                Password = txtPass.Text, // Only update if not empty, handled in BLL logic? Wait, UI Logic? 
                // BLL updated to check IsNullOrEmpty.
                FullName = txtName.Text,
                Role = cboRole.SelectedItem.ToString()
            };
            string res = bll.UpdateUser(u);
            MessageBox.Show(res);
            LoadData();
        }

        private void DeleteUser()
        {
            if (txtUser.Tag == null) return;
            string res = bll.DeleteUser((int)txtUser.Tag);
            MessageBox.Show(res);
            LoadData();
        }
    }
}
