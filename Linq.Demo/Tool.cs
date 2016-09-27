using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linq.Demo.Entity;
using System.Configuration;
using Mqd.SqlHelper;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using System.Windows.Forms;
using System.Data;

namespace Linq.Demo
{
    public class Tool
    {
        private static readonly string _root;

        static Tool()
        {
            string root = AppDomain.CurrentDomain.BaseDirectory;
            _root = root.Substring(0, root.LastIndexOf("\\bin")) + "\\";
        }

        public static void LoadFormDb<T>() where T : new()
        {
            ConnectionStringSettings css = new ConnectionStringSettings
            {
                ConnectionString = "Server=Mqd-PC\\MSSQL2008;database=Northwind;uid=sa;pwd=hytgku548098",
                Name = "SqlHelperConnName",
                ProviderName = "System.Data.SqlClient"
            };
            Db db = new Db(css);
            List<T> list = db.GetTable("select * from Orders").ToList<T>();
            Serialize<T>(list);
        }

        private static void Serialize<T>(List<T> list)
        {
            string json = JsonConvert.SerializeObject(list);
            byte[] buffer = Encoding.Default.GetBytes(json);
            string path = _root + typeof(T).Name + ".data";
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            fs.SetLength(0);
            fs.Write(buffer, 0, buffer.Length);
            fs.Flush();
            fs.Close();
        }

        public static List<T> LoadFromLocal<T>()
        {
            List<T> list = new List<T>();
            string path = _root + typeof(T).Name + ".data";
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (fs.Length > 0)
            {
                byte[] buffer = new byte[fs.Length];
                StreamReader sr = new StreamReader(fs);
                string json = sr.ReadToEnd();
                list = (List<T>)JsonConvert.DeserializeObject(json, typeof(List<T>));
            }
            return list;
        }

        public static void FillListView(DataTable dt, ListView lv)
        {
            lv.BeginUpdate();
            lv.Items.Clear();
            lv.Columns.Clear();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                lv.Columns.Add(dt.Columns[i].ColumnName);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string[] arr = new string[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    arr[j] = dt.Rows[i][j].ToString();
                }
                lv.Items.Add(new ListViewItem(arr));
            }
            lv.EndUpdate();
        }

        public static void FillListView<T>(IList<T> list, ListView lv)
        {
            lv.BeginUpdate();
            lv.Items.Clear();
            lv.Columns.Clear();
            System.Type t = typeof(T);
            t.GetProperties();
            PropertyInfo[] pi = t.GetProperties();
            for (int i = 0; i < pi.Length; i++)
            {
                lv.Columns.Add(pi[i].Name);
            }
            foreach (T item in list)
            {
                string[] arr = new string[pi.Length];
                for (int j = 0; j < pi.Length; j++)
                {
                    object val = pi[j].GetValue(item);
                    if (val != null)
                    {
                        arr[j] = val.ToString();
                    }
                    else
                    {
                        arr[j] = "";
                    }
                }
                lv.Items.Add(new ListViewItem(arr));
            }
            lv.EndUpdate();
        }
    }
}
