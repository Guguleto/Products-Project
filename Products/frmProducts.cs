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
    public partial class frmProducts : frmInheritance
    {
        string strProductName;
        string strProductDescription;
        double dblProductPrice;
        bool boolProductExists = false;
        int intProductID = 0;

        string strAccessConnectionString="Driver={Microsoft Access Driver (*.mdb)}; Dbq=Products.mdb; Uid=Admin; Pwd=;";
        public frmProducts()
        {
            InitializeComponent();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            frmMain frmMain = new frmMain();
            frmMain.Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmProducts_Load(object sender, EventArgs e)
        {
            controlsLoad();
            loadProducts();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if(btnCreate.Text=="Save")
            {
                if (txtProductName.Text == "")
                {
                    MessageBox.Show("Product name field cannot be left empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(txtProductDescription.Text=="")
                {
                    MessageBox.Show("Product description field cannot be left empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if(txtProductPrice.Text=="")
                {
                    MessageBox.Show("Product price field cannot be left empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    checkIfProductExist();
                    if (boolProductExists == false)
                    {
                        createProduct();
                        controlsLoad();
                        clearTextBox();
                        loadProducts();
                    }
                    else if (boolProductExists == true)
                    {
                        MessageBox.Show("Product already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if(btnCreate.Text=="Create")
            {
                controlsCreate();
            }
            
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            editProduct();
            controlsEdit();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            updateProduct();
            controlsLoad();
            clearTextBox();
            loadProducts();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteProduct();
            controlsLoad();
            clearTextBox();
            loadProducts();
        }

        private void controlsLoad()
        {
            txtProductDescription.Enabled = false;
            txtProductName.Enabled = false;
            txtProductPrice.Enabled = false;

            cboProducts.Enabled = true;

            btnCreate.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = true;
            btnReturn.Enabled = true;
            btnUpdate.Enabled = false;

            btnCreate.Text = "Create";

        }

        private void controlsCreate()
        {
            txtProductDescription.Enabled = true;
            txtProductName.Enabled = true;
            txtProductPrice.Enabled = true;

            cboProducts.Enabled = false;

            btnCreate.Enabled = true;
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            btnReturn.Enabled = false;
            btnUpdate.Enabled = false;

            btnCreate.Text = "Save";
        }

        private void controlsEdit()
        {
            txtProductDescription.Enabled = true;
            txtProductName.Enabled = true;
            txtProductPrice.Enabled = true;

            cboProducts.Enabled = false;

            btnCreate.Enabled = false;
            btnDelete.Enabled = true;
            btnEdit.Enabled = false;
            btnReturn.Enabled = false;
            btnUpdate.Enabled = true;

        }
        private void clearTextBox()
        {
            txtProductDescription.Text = "";
            txtProductName.Text = "";
            txtProductPrice.Text = "";

        }

        private void loadProducts()
        {
            cboProducts.DataSource = null;
            cboProducts.Items.Clear();

            OdbcConnection OdbcConnection = new OdbcConnection();
            OdbcConnection.ConnectionString = strAccessConnectionString;

            string qeury = "select ProductName from Products";

            OdbcCommand cmd = new OdbcCommand(qeury, OdbcConnection);

            OdbcConnection.Open();
            OdbcDataReader dr = cmd.ExecuteReader();
            AutoCompleteStringCollection ProductCollection = new AutoCompleteStringCollection();

            while(dr.Read())
            {
                ProductCollection.Add(dr.GetString(0));
            }
            cboProducts.DataSource=ProductCollection;
            OdbcConnection.Close();
        }

        private void createProduct()
        {

            string qeury = "select * from products where ID=0";

            OdbcConnection OdbcConnection = new OdbcConnection();
            OdbcDataAdapter da = new OdbcDataAdapter(qeury, OdbcConnection);

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;

            OdbcConnection.ConnectionString = strAccessConnectionString;

            da.Fill(ds, "Products");
            dt = ds.Tables["Products"];

            try
            {
                dr = dt.NewRow();
                dr["ProductName"] = txtProductName.Text;
                dr["ProductDescription"] = txtProductDescription.Text;
                dr["Price"] = txtProductPrice.Text;

                dt.Rows.Add(dr);
                OdbcCommandBuilder cmd = new OdbcCommandBuilder(da);

                da.Update(ds, "Products");
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message.ToString());
            }
            finally
            {
                OdbcConnection.Close();
                OdbcConnection.Dispose();
            }
        }
        private void checkIfProductExist()
        {
            string query = "select * from products where productname='"+ txtProductName.Text +"'";

            OdbcConnection OdbcConnection = new OdbcConnection();
            OdbcCommand cmd;
            OdbcDataReader dr;

            OdbcConnection.ConnectionString = strAccessConnectionString;

            OdbcConnection.Open();

            cmd = new OdbcCommand(query, OdbcConnection);
            dr = cmd.ExecuteReader();
            if(dr.Read())
            {
                boolProductExists = true;
            }

            dr.Close();
            OdbcConnection.Close(); 
            dr.Dispose();
            OdbcConnection.Dispose();
        }

        private void editProduct()
        {
            string query = "select * from Products where productName='" + cboProducts.Text + "'";

            OdbcConnection OdbcConnection = new OdbcConnection();
            OdbcCommand cmd;
            OdbcDataReader dr;

            OdbcConnection.ConnectionString=strAccessConnectionString;

            OdbcConnection.Open();

            cmd = new OdbcCommand(query, OdbcConnection);
            dr = cmd.ExecuteReader();

            if(dr.Read())
            {
                intProductID = dr.GetInt32(0);
                txtProductName.Text = dr.GetString(1);
                txtProductDescription.Text = dr.GetString(2);
                txtProductPrice.Text = dr.GetString(3);

            }
            dr.Close();
            OdbcConnection.Close();
            dr.Dispose();
            OdbcConnection.Dispose();
        }

        private void updateProduct()
        {
            string query = "select * from products where id="+ intProductID;
            OdbcConnection OdbcConnection = new OdbcConnection();

            OdbcConnection.ConnectionString = strAccessConnectionString;

            OdbcDataAdapter da = new OdbcDataAdapter(query,OdbcConnection);
            DataSet ds = new DataSet("Products");

            da.FillSchema(ds, SchemaType.Source, "Products");
            da.Fill(ds, "Products");
            DataTable dt;

            dt = ds.Tables["Products"];
            DataRow dr;
            dr = dt.NewRow();

            try
            {
                dr = dt.Rows.Find(intProductID);
                dr.BeginEdit();

                dr["ProductName"] = txtProductName.Text;
                dr["ProductDescription"] = txtProductDescription.Text;
                dr["Price"] = txtProductPrice.Text;

                dr.EndEdit();

                OdbcCommandBuilder cmd = new OdbcCommandBuilder(da);
                da.Update(ds, "Products");
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message.ToString());
            }
            finally
            {
                OdbcConnection.Close();
                OdbcConnection.Dispose();
            }
        }

        private void deleteProduct()
        {
            string query = "delete from products where id=" + intProductID;

            OdbcConnection OdbcConnection = new OdbcConnection();
            OdbcCommand cmd;
            OdbcDataReader dr;

            OdbcConnection.ConnectionString = strAccessConnectionString;

            OdbcConnection.Open();

            cmd = new OdbcCommand(query, OdbcConnection);
            dr = cmd.ExecuteReader();

            if(dr.Read())
            {
            }

            dr.Close();
            OdbcConnection.Close();
            dr.Dispose();
            OdbcConnection.Dispose();
        }
    
    }

}
