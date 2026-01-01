using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Lab_06.BLL
{
    public static class ExcelExporter
    {
        // Simple CSV Export to avoid Office Dependencies
        public static void ExportToCSV(DataGridView dgv)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV (*.csv)|*.csv";
            sfd.FileName = "Export.csv";
            
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    // Headers
                    string[] columnNames = new string[dgv.Columns.Count];
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        columnNames[i] = dgv.Columns[i].HeaderText.Replace(",", " ");
                    }
                    sb.AppendLine(string.Join(",", columnNames));

                    // Rows
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string[] cells = new string[dgv.Columns.Count];
                            for (int i = 0; i < dgv.Columns.Count; i++)
                            {
                                var val = row.Cells[i].Value;
                                cells[i] = val == null ? "" : val.ToString().Replace(",", " ");
                            }
                            sb.AppendLine(string.Join(",", cells));
                        }
                    }

                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Exported successfully to " + sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting: " + ex.Message);
                }
            }
        }
    }
}
