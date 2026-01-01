using System;
using System.Drawing;
using System.Windows.Forms;
using Lab_06.DAL;

namespace Lab_06.GUI
{
    public partial class frmConfig : Form
    {
        private TextBox txtServer;
        private TextBox txtDb;
        private Button btnSave;
        private Button btnTest;

        public frmConfig()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void InitializeComponent()
        {
            this.Text = "Cấu Hình Cơ Sở Dữ Liệu";
            this.Size = new Size(420, 250);
            this.StartPosition = FormStartPosition.CenterParent;

            Label lblServer = new Label { Text = "Tên Server:", Location = new Point(30, 30), AutoSize = true };
            txtServer = new TextBox { Location = new Point(130, 27), Width = 220 };

            Label lblDb = new Label { Text = "Tên CSDL:", Location = new Point(30, 70), AutoSize = true };
            txtDb = new TextBox { Location = new Point(130, 67), Width = 220 };

            btnTest = new Button { Text = "Kiểm Tra Kết Nối", Location = new Point(30, 120), Width = 150 };
            btnTest.Click += BtnTest_Click;

            btnSave = new Button { Text = "Lưu & Đóng", Location = new Point(200, 120), Width = 150 };
            btnSave.Click += BtnSave_Click;

            this.Controls.Add(lblServer);
            this.Controls.Add(lblDb);
            this.Controls.Add(txtServer);
            this.Controls.Add(txtDb);
            this.Controls.Add(btnTest);
            this.Controls.Add(btnSave);
        }

        private void LoadSettings()
        {
            // Try to parse current connection string to fill boxes
            string conn = DatabaseAccess.ConnectionString;
            try
            {
                var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
                txtServer.Text = builder.DataSource;
                txtDb.Text = builder.InitialCatalog;
            }
            catch { }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            string testConn = $"Data Source={txtServer.Text};Initial Catalog={txtDb.Text};Integrated Security=True";
            if (DatabaseAccess.TestConnection(testConn))
                MessageBox.Show("Kết Nối Thành Công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Kết Nối Thất Bại!\nVui lòng kiểm tra lại Tên Server.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            DatabaseAccess.SaveConnectionString(txtServer.Text, txtDb.Text);
            MessageBox.Show("Đã lưu cấu hình. Vui lòng khởi động lại ứng dụng nếu cần.", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
