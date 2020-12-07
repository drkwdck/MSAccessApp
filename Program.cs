using MSAccessApp.Forms;
using MSAccessApp.Persistence;
using System;
using System.Windows.Forms;

namespace MSAccessApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var dbProvider = DatabaseProvider.Get();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartMenu(dbProvider));
        }

    }
}
