using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Linq.Demo.Entity;
using System.Linq;//System.Core.dll

namespace Linq.Demo
{
    public partial class Linq_to_Object : Form
    {
        private readonly List<Orders> _orders;
        private readonly List<Employees> _employees;

        public Linq_to_Object()
        {
            InitializeComponent();
            _orders = Tool.LoadFromLocal<Orders>();
            _employees = Tool.LoadFromLocal<Employees>();
            Tool.FillListView<Orders>(_orders, listView1);
            label1.Text = string.Format("{0}记录数：{1}", typeof(Orders).Name, _orders.Count);
        }

        private void Bind<T>(IEnumerable<T> list)
        {
            List<T> list1 = list.ToList<T>();
            Tool.FillListView<T>(list1, listView2);
            label2.Text = string.Format("{0}记录数：{1}", typeof(T).Name, list1.Count);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = from c in _orders
                         where c.OrderID > 10500
                         select c;
            Bind<Orders>(result);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var result = from c in _orders
                         where c.OrderID > 10500 && c.OrderID < 10800
                         select c;
            Bind<Orders>(result);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = from c in _orders
                         where c.OrderID > 10500 || (c.OrderID > 10200 && c.OrderID < 10300)
                         select c;
            Bind<Orders>(result);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var result = from c in _orders
                         where c.OrderID == 10248
                         select c;
            Bind<Orders>(result);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var result = from c in _orders
                         where c.OrderID < 10300
                         select c;
            Bind<Orders>(result);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var result = from c in _orders
                         where c.OrderID != 10248
                         select c;
            Bind<Orders>(result);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var result = from c in _orders
                         where c.EmployeeID == 
                         (
                            from em in _employees
                            select em.EmployeeID
                         ).FirstOrDefault()
                         select c;
            Bind<Orders>(result);
        }
    }
}
