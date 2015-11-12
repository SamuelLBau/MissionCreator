using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MissionCreator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (GV.debug == true)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new mainForm());
            }
            else
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new mainForm());
                }
                catch
                {
                    Application.Run(new errorForm());
                }
            }
        }
    }
}
