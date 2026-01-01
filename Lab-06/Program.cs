using System;
using System.Windows.Forms;
using Lab_06.GUI;

namespace Lab_06
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Start with Login. If login is successful, it opens Main.
            Application.Run(new frmLogin());
        }
    }
}
