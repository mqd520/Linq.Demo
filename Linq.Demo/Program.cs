using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Linq.Demo.Entity;

namespace Linq.Demo
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Tool.LoadFormDb<Orders>();
            //Tool.LoadFormDb<Employees>();
            Application.Run(new Linq_to_Object());
        }
    }
}
