using System;
using System.Drawing;
using System.Windows.Forms;
using Lab_06.BLL;
using Lab_06.DTO;

namespace Lab_06.GUI
{
    public partial class frmChangePassword : Form
    {
        private TextBox txtOld, txtNew, txtConfirm;
        private Button btnSave;
        private UserBLL bll = new UserBLL();
        
        // We'll simplisticly store logged in user ID in a static somewhere or pass it.
        // For now, let's assume we can pass it or use a global. 
        // Better: Pass in constructor.
        private int _userId;

        public frmChangePassword(int userId)
        {
            _userId = userId;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Đổi Mật Khẩu";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblOld = new Label { Text = "Mật khẩu cũ:", Location = new Point(30, 30), AutoSize = true };
            txtOld = new TextBox { Location = new Point(150, 27), Width = 200, PasswordChar = '●' };

            Label lblNew = new Label { Text = "Mật khẩu mới:", Location = new Point(30, 80), AutoSize = true };
            txtNew = new TextBox { Location = new Point(150, 77), Width = 200, PasswordChar = '●' };

            Label lblConf = new Label { Text = "Xác nhận:", Location = new Point(30, 130), AutoSize = true };
            txtConfirm = new TextBox { Location = new Point(150, 127), Width = 200, PasswordChar = '●' };

            btnSave = new Button { 
                Text = "💾 Lưu Thay Đổi", 
                Location = new Point(100, 190), 
                Width = 150, 
                Height = 40, 
                BackColor = Color.SeaGreen, 
                ForeColor = Color.White, 
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.Click += BtnSave_Click;

            this.Controls.AddRange(new Control[] { lblOld, txtOld, lblNew, txtNew, lblConf, txtConfirm, btnSave });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (txtNew.Text != txtConfirm.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string result = bll.ChangePassword(_userId, txtOld.Text, txtNew.Text);
            if (result == "Success")
            {
                MessageBox.Show("Đổi mật khẩu thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Đổi mật khẩu thất bại. Kiểm tra lại mật khẩu cũ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
