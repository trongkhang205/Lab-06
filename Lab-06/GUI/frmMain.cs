using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Lab_06.BLL;
using Lab_06.DTO;
using Lab_06.DAL;

namespace Lab_06.GUI
{
    public partial class frmMain : Form
    {
        private User _currentUser;
        
        public frmMain(User user)
        {
            _currentUser = user;
            InitializeComponent();
            LoadDashboard();
            ApplyPermissions();
        }

        // Default constructor for designer support (optional)
        public frmMain() 
        { 
            // Mock user for testing if needed or just blank
            InitializeComponent(); 
        }

        private void ApplyPermissions()
        {
             if (_currentUser != null)
             {
                 this.Text += $" - Xin Chào: {_currentUser.FullName} ({_currentUser.Role})";
                 // If not admin, hide management menu? Or just limit?
                 // Let's create specific menu item for Staff Management and hide it if not Admin
             }
        }
        
        private void InitializeComponent()
        {
            this.Text = "Hệ Thống Quản Lý Nhà Sách";
            this.Size = new Size(1100, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.IsMdiContainer = true;
            this.FormClosing += FrmMain_FormClosing; // Add closing event

            this.MainMenuStrip = new MenuStrip();

            // Menu Styling - Modern Color
            this.MainMenuStrip.BackColor = Color.FromArgb(41, 57, 85); // Dark Navy Blue
            this.MainMenuStrip.ForeColor = Color.White;
            this.MainMenuStrip.Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            this.MainMenuStrip.Padding = new Padding(6, 4, 0, 4);

            // System Menu
            ToolStripMenuItem sysMenu = new ToolStripMenuItem("🛠️ Hệ Thống");
            sysMenu.DropDownItems.Add("🔍 Tra Cứu Thông Tin", null, (s, e) => OpenChildForm(new frmSearch()));
            sysMenu.DropDownItems.Add("-", null, null); // Separator
            
            // New Features
            if (_currentUser != null && _currentUser.Role == "Admin")
            {
                sysMenu.DropDownItems.Add("👥 Quản Lý Nhân Viên", null, (s, e) => OpenChildForm(new frmUsers()));
            }
            sysMenu.DropDownItems.Add("🔒 Đổi Mật Khẩu", null, (s, e) => new frmChangePassword(_currentUser.UserID).ShowDialog());
            sysMenu.DropDownItems.Add("-", null, null);
            
            sysMenu.DropDownItems.Add("🚪 Đăng Xuất", null, (s, e) => { 
                this.Hide(); 
                new frmLogin().ShowDialog(); 
                this.Close(); 
            });
            sysMenu.DropDownItems.Add("❌ Thoát Chương Trình", null, (s, e) => Application.Exit());

            // Management Menu
            ToolStripMenuItem manageMenu = new ToolStripMenuItem("📂 Quản Lý");
            manageMenu.DropDownItems.Add("📚 Quản Lý Sách", null, (s, e) => OpenChildForm(new frmBooks()));
            manageMenu.DropDownItems.Add("👥 Quản Lý Khách Hàng", null, (s, e) => OpenChildForm(new frmCustomers()));
            manageMenu.DropDownItems.Add("📝 Quản Lý Mượn Trả", null, (s, e) => OpenChildForm(new frmLoans()));

            // Statistics Menu
            ToolStripMenuItem statsMenu = new ToolStripMenuItem("📊 Thống Kê");
            statsMenu.DropDownItems.Add("📈 Báo Cáo Doanh Thu", null, (s, e) => OpenChildForm(new frmReports()));

            this.MainMenuStrip.Items.AddRange(new ToolStripItem[] { sysMenu, manageMenu, statsMenu });
            this.Controls.Add(this.MainMenuStrip);

            // MDI Background color
            MdiClient ctlMDI;
            foreach (Control ctl in this.Controls)
            {
                try
                {
                    ctlMDI = (MdiClient)ctl;
                    ctlMDI.BackColor = Color.FromArgb(240, 242, 245); // Soft Gray-Blue
                }
                catch { }
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult res = MessageBox.Show(
                    "Bạn có chắc chắn muốn thoát chương trình không?", 
                    "Xác Nhận Thoát", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question
                );
                
                if (res == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void LoadDashboard()
        {
            // Simple Dashboard Logic: Create a Form that opens by default
            frmDashboard dash = new frmDashboard();
            dash.MdiParent = this;
            dash.StartPosition = FormStartPosition.CenterScreen;
            dash.Show();
        }

        private void OpenChildForm(Form childForm)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == childForm.GetType())
                {
                    f.Activate();
                    return;
                }
            }
            childForm.MdiParent = this;
            childForm.StartPosition = FormStartPosition.CenterScreen; 
            childForm.Show();
        }
    }

    // Inner Dashboard Form
    public class frmDashboard : Form
    {
        public frmDashboard()
        {
            this.Text = "Tổng Quan";
            this.Size = new Size(800, 500);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.AliceBlue;

            Label lblWelcome = new Label { 
                Text = "CHÀO MỪNG ĐẾN VỚI HỆ THỐNG QUẢN LÝ NHÀ SÁCH", 
                Font = new Font("Segoe UI", 20, FontStyle.Bold), 
                ForeColor = Color.DarkSlateBlue,
                AutoSize = true,
                Location = new Point(100, 50) 
            };
            
            this.Controls.Add(lblWelcome);
            
            // Stats Panels
            BookBLL bllBook = new BookBLL();
            CustomerBLL bllCust = new CustomerBLL();
            LoanBLL bllLoan = new LoanBLL();

            CreateStatCard("Tổng Sách", bllBook.GetAllBooks().Count.ToString(), Color.CornflowerBlue, 100, 150);
            CreateStatCard("Khách Hàng", bllCust.GetAllCustomers().Count.ToString(), Color.SeaGreen, 320, 150);
            CreateStatCard("Phiếu Mượn", bllLoan.GetAllLoans().Count.ToString(), Color.Orange, 540, 150);
        }

        private void CreateStatCard(string title, string value, Color color, int x, int y)
        {
            Panel p = new Panel { Location = new Point(x, y), Size = new Size(200, 120), BackColor = color };
            Label lblVal = new Label { Text = value, Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = Color.White, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
            Label lblTitle = new Label { Text = title, Font = new Font("Segoe UI", 12), ForeColor = Color.White, Dock = DockStyle.Top, TextAlign = ContentAlignment.MiddleCenter, Height = 30 };
            
            p.Controls.Add(lblVal);
            p.Controls.Add(lblTitle);
            this.Controls.Add(p);
        }
    }
}
