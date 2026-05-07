using System;
using System.Windows.Forms;

namespace PUPAcadPortal
{
    internal static class Program
    {
        /// <summary>
        /// Main entry point. Swap the comment to switch between portals.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            //Application.Run(new InstructorPortal());
            Application.Run(new StudentPortal());
        }
    }
}