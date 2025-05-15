using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PharmacyQueue
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Create the admin form first
            Home adminForm = new Home();
            // Create the customer form with a reference to the admin form
            customerForm customerForm = new customerForm(adminForm);
            
            // Run the application with the customer form
            Application.Run(customerForm);
        }
    }
}
