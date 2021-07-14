using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace WindowsFormsApp
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Report2WithSqlConn();
            //Report2WithEF();
        }

        private void Report2WithSqlConn()
        {
            string quary = "SELECT * FROM [CV] WHERE [CvID] = " + Form1.CvID;
            string connectionString = "Data Source=DESKTOP-SV5OLUK;Initial Catalog=CVDetais;Integrated Security=True";
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(quary, con);
            SqlDataAdapter adap = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds, "CV");
            for (var i = 0; i < ds.Tables["CV"].Rows.Count; i++)
            {
                if (ds.Tables["CV"].Rows[i]["FilePath"] != null)
                {
                    if (!string.IsNullOrEmpty(ds.Tables["CV"].Rows[i]["FilePath"].ToString()))
                    {
                        string strFilePath = Application.StartupPath + ds.Tables["CV"].Rows[i]["FilePath"].ToString();
                        if (File.Exists(strFilePath))
                        {
                            ds.Tables["CV"].Rows[i]["FilePath"] = strFilePath;
                        }
                    }
                }
            }

            CrystalReport1 cr = new CrystalReport1();
            cr.SetDataSource(ds);
            crystalReportViewer1.ReportSource = cr;
            con.Close();
            crystalReportViewer1.Refresh();
        }

    }
}
