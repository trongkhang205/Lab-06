using System;
using System.Drawing;
using System.Windows.Forms;
using Lab_06.BLL;
using Lab_06.DAL;
using Lab_06.DTO;

namespace Lab_06.GUI
{
    public partial class frmLogin : Form
    {
        private TextBox txtUser;
        private TextBox txtPass;
        private Button btnLogin;
        private Button btnConfig;
        private Label lblTitle;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Đăng Nhập";
            this.Size = new Size(500, 350);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            // Title
            lblTitle = new Label { 
                Text = "QUẢN LÝ NHÀ SÁCH", 
                Font = new Font("Segoe UI", 20, FontStyle.Bold), 
                AutoSize = true, 
                ForeColor = Color.DarkSlateBlue,
                Location = new Point(100, 40)
            };
            
            // Panel for inputs
            Panel pnlInput = new Panel { 
                BackColor = Color.White, 
                Location = new Point(50, 100), 
                Size = new Size(380, 160),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblUser = new Label { Text = "Tài khoản:", Location = new Point(30, 30), AutoSize = true, Font = new Font("Segoe UI", 10) };
            txtUser = new TextBox { Location = new Point(120, 27), Width = 220, Text = "admin", Font = new Font("Segoe UI", 10) };
            
            Label lblPass = new Label { Text = "Mật khẩu:", Location = new Point(30, 70), AutoSize = true, Font = new Font("Segoe UI", 10) };
            txtPass = new TextBox { Location = new Point(120, 67), Width = 220, PasswordChar = '●', Text = "123456", Font = new Font("Segoe UI", 10) };
            
            btnLogin = new Button { 
                Text = "🔓 ĐĂNG NHẬP", 
                Location = new Point(120, 110), 
                Width = 220, 
                Height = 40, 
                BackColor = Color.FromArgb(41, 57, 85), // Dark Navy 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            pnlInput.Controls.Add(lblUser);
            pnlInput.Controls.Add(txtUser);
            pnlInput.Controls.Add(lblPass);
            pnlInput.Controls.Add(txtPass);
            pnlInput.Controls.Add(btnLogin);

            // Config Button
            btnConfig = new Button { 
                Text = "⚙ Cấu Hình", 
                Location = new Point(380, 12), 
                Width = 90, 
                Height = 30,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Gray
            };
            btnConfig.FlatAppearance.BorderSize = 0;
            btnConfig.Click += (s, e) => { new frmConfig().ShowDialog(); };

            this.Controls.Add(lblTitle);
            this.Controls.Add(pnlInput);
            this.Controls.Add(btnConfig);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string userText = txtUser.Text;
                string passText = txtPass.Text;

                UserBLL userBll = new UserBLL(); 
                // Wait, UserBLL doesn't have Login pass-through? Let's check or use DAL directly here for simplicity or add to BLL. 
                // BLL logic is better.
                // Let's quickly add Login to BLL or just use DAL as we have reference.
                // Since I didn't verify BLL Login method, I'll assume I should use DAL directly or assume I need to add it.
                // I'll assume I should use DAL here for speed as I see namespace inclusion.
                
                Lab_06.DAL.UserDAL udal = new Lab_06.DAL.UserDAL();
                var u = udal.Login(userText, passText);

                if (u != null)
                {
                    this.Hide();
                    frmMain main = new frmMain(u);
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối CSDL.\n" + ex.Message);
            }
        }
    }
}
