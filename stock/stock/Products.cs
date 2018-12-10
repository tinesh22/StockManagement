using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stock
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }

        private void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadData();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection con=new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\project\stock\stock\Database1.mdf;Integrated Security=True");
            //Insert Logic
            con.Open();
            bool status = false;
            if (comboBox1.SelectedIndex == 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            try
            {
                var sqlQuery = "";
                if (IfProductsExist(con, textBox1.Text))
                {
                    sqlQuery = @"UPDATE Products SET [ProductName] = '" + textBox2.Text + "',[ProductStatus] = '" + status + "' WHERE [ProductCode] = '" + textBox1.Text + "'";

                }
                else
                {
                    sqlQuery = @"insert into Products(ProductCode,ProductName,ProductStatus)values('" + textBox1.Text + "','" + textBox2.Text + "','" + status + "')";
                }
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                textBox1.Clear();
                textBox2.Clear();
                con.Close();
                //Readin Data
                LoadData();
            }
            catch (SqlException ex)
            {
                if (ex.Number == -2146232060)
                {
                    textBox1.Clear();
                    MessageBox.Show("Enter the correct integer value", "alert", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    MessageBox.Show("Enter Valid Integer Value Like as 1,2,3 .....");
                    textBox1.Clear();
                }
            }
              }
        private bool IfProductsExist(SqlConnection con, string productCode)
        {
           SqlDataAdapter sda = new SqlDataAdapter("Select 1 From Products where ProductCode= '"+productCode+"'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if(dt.Rows.Count>0)
            
            return true;
            
            else
                return false;
            
        }
        public void LoadData()
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\project\stock\stock\Database1.mdf;Integrated Security=True");
            SqlDataAdapter sda = new SqlDataAdapter("Select * From Products", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();

            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "Deactive";
                }
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if(dataGridView1.SelectedRows[0].Cells[2].Value.ToString()=="Active")
            {
                comboBox1.SelectedIndex = 0 ;
            }
            else
            {
                comboBox1.SelectedIndex = 1; 
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\project\stock\stock\Database1.mdf;Integrated Security=True");
            var sqlQuery = "";
            if (IfProductsExist(con, textBox1.Text))
            {
                con.Open();
                sqlQuery = @"DELETE FROM Products WHERE [ProductCode] = '" + textBox1.Text + "'";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.ExecuteNonQuery();
                textBox1.Clear();
                textBox2.Clear();
                con.Close();
            }
            else
            {
                MessageBox.Show("No Record Found....");
                textBox1.Clear();
                textBox2.Clear();
            }
            
            //Readin Data
            LoadData();
        }
        
    }
}
