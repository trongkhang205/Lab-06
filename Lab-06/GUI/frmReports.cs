using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;
using Lab_06.BLL;

namespace Lab_06.GUI
{
    public partial class frmReports : Form
    {
        private Chart chart;
        private BookBLL bll = new BookBLL();

        public frmReports()
        {
            InitializeComponent();
            LoadChart();
        }

        private void InitializeComponent()
        {
            this.Text = "Báo Cáo Thống Kê";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            chart = new Chart();
            chart.Dock = DockStyle.Fill;

            ChartArea area = new ChartArea("MainArea");
            chart.ChartAreas.Add(area);

            Legend legend = new Legend("MainLegend");
            chart.Legends.Add(legend);

            Series series = new Series("Books");
            series.ChartType = SeriesChartType.Column; // Column Chart
            series.IsValueShownAsLabel = true;
            chart.Series.Add(series);

            Label lblHeader = new Label { Text = "THỐNG KÊ SỐ LƯỢNG SÁCH THEO THỂ LOẠI", Dock = DockStyle.Top, Font = new Font("Arial", 14, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter, Height = 50 };

            this.Controls.Add(chart);
            this.Controls.Add(lblHeader);
        }

        private void LoadChart()
        {
            var books = bll.GetAllBooks();
            // Group by CategoryName
            var data = books.GroupBy(b => b.Category != null ? b.Category.CategoryName : "Unknown")
                            .Select(g => new { Category = g.Key, Count = g.Count() })
                            .ToList();

            foreach(var item in data)
            {
                chart.Series["Books"].Points.AddXY(item.Category, item.Count);
            }
        }
    }
}
