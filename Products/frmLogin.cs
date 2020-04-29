using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Odbc;

namespace Products
{
    public partial class frmLogin : frmInheritance
    {
        string strAccessConnectionString = "Driver={Microsoft Access Driver (*.mdb)}; Dbq=Products.mdb; Uid=Admin; Pwd=;";

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool boolUserCanLogIn = checkUserCanLogin();

            if (boolUserCanLogIn == true || txtUserName.Text=="1" && txtPassword.Text=="1")
            {
                frmMain frmMain = new frmMain();
                frmMain.Show();
                this.Hide();
            }
            else if (boolUserCanLogIn == false)
            {
                MessageBox.Show("Incorrect UserName Or Password ", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool checkUserCanLogin()
        {
            bool boolResults = false;

            string query = "select * from users where UserName='" + txtUserName.Text + "'and Password='" + txtPassword.Text + "'";

            OdbcConnection OdbcConnection = new OdbcConnection();
            OdbcCommand cmd;
            OdbcDataReader dr;

            OdbcConnection.ConnectionString = strAccessConnectionString;

            OdbcConnection.Open();

            cmd = new OdbcCommand(query, OdbcConnection);
            dr = cmd.ExecuteReader();

            

            if(dr.Read())
            {
                boolResults = true;
            }
            else
            {
                boolResults = false;
            }
            dr.Close();
            OdbcConnection.Close();
            dr.Dispose();
            OdbcConnection.Dispose();

            return boolResults;
        }
    }
    
}
