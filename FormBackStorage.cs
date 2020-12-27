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

namespace CaffeBar
{
    public partial class FormBackStorage : Form
    {
        public FormBackStorage()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormBackStorage_Load(object sender, EventArgs e)
        {
            UpdateStorageView();
        }

        private void UpdateStorageView(String filter = "")
        {
            SuspendLayout();
            SqlConnection connection = DB.getConnection();
            DataSet dataset = new DataSet();

            using(SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM dbo.Storage WHERE Deleted = 0", connection))
            {
                adapter.Fill(dataset);//, "Storage");
            }

            var dv = dataset.Tables[0].DefaultView;
            dv.RowFilter = filter;
            //var newDS = new DataSet();
            //var newDT = dv.ToTable();
            //newDS.Tables.Add(newDT);


            dataGridView1.DataSource = dv;//dataset.Tables[0];
            //            dataGridView1.DataMember = "Storage";



            DB.closeConnection();
            ResumeLayout();
        }

        private void buttonNewItem_Click(object sender, EventArgs e)
        {
            FormItemStorage formItem = new FormItemStorage();
            formItem.ShowDialog();
            UpdateStorageView();
        }

        private void buttonEditItem_Click(object sender, EventArgs e)
        {

            FormItemStorage form = new FormItemStorage();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                form.textBoxId.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                form.textBoxItem.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                form.textBoxPrice.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString().Replace(",", ".");
                form.textBoxCooler.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                form.textBoxBackstorage.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                form.ShowDialog();
                UpdateStorageView();
            }
            else
                MessageBox.Show("Please select a row!");
        }

        private void buttonDeleteItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                SqlConnection connection = DB.getConnection();
                SqlCommand sqlcommand = new SqlCommand("update dbo.Storage set Deleted = 1 where Id =@id", connection);

                sqlcommand.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells[0].Value.ToString());

                try
                {
                    sqlcommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR:" + ex.Message);
                }
                finally
                {
                    DB.closeConnection();
                }
                UpdateStorageView();
            }
            else
                MessageBox.Show("Please select a row!");


        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            if (textBoxFilter.Text != "")
                UpdateStorageView("Item LIKE '*" + textBoxFilter.Text + "*'");
            else
                UpdateStorageView();
        }
    }
}
