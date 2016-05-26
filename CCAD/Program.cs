using System;
using System.Windows.Forms;

namespace CCAD
{
    static class Program
    {
        /// <summary>
        /// Main application
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Board());
        }
    }
}
