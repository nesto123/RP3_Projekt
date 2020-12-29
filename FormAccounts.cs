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
    /// <summary>
    /// Sorage display form.
    /// </summary>
    public partial class FormAccounts : Form
    {
        public FormAccounts()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormAccounts_Load(object sender, EventArgs e)
        {
            SuspendLayout();

            //SqlConnection connection = DB.getConnection();
            //SqlDataAdapter adapter = new SqlDataAdapter("SELECT Item, Cooler, Backstorage FROM Storage WHERE Cooler < 5 OR Backstorage < 10 ORDER BY Cooler ", connection);
            //DB.closeConnection();

            //S
            //adapter.Fill(dataset);//, "Storage");
            
            
            ResumeLayout();
        }

        private void nodeletedToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.userTableAdapter.Nodeleted(this.caffeBarDatabaseDataSet.User);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
            SqlConnection connection = DB.getConnection();

            SqlCommand command3 = new SqlCommand("UPDATE dbo.User SET Deleted = 1 - @times WHERE Id=@id", connection);
            command3.Parameters.AddWithValue("@id", ((DataRow)sender).Field<int>("Id"));

            try
            {              
            }
            catch (Exception ex)
            {
                MessageBox.Show( "ERROR: Database error! " + ex.Message);
            }
            finally
            {
                DB.closeConnection();
            }

        }
    }
}
