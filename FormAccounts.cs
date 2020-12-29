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
using System.Data.SqlServerCe;

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

            SqlConnection connection = DB.getConnection();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT Item, Cooler, Backstorage FROM Storage WHERE Cooler < 5 OR Backstorage < 10 ORDER BY Cooler ", connection);
            DB.closeConnection();

            //S
            //adapter.Fill(dataset);//, "Storage");
            
            
            ResumeLayout();
        }
    }
}
