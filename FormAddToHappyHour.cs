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
    public partial class FormAddToHappyHour : Form
    {
        public FormAddToHappyHour()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormAddToHappyHour_Load(object sender, EventArgs e)
        {
            dateTimePickerFrom.Format = DateTimePickerFormat.Custom;
            dateTimePickerFrom.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            dateTimePickerTo.Format = DateTimePickerFormat.Custom;
            dateTimePickerTo.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            UpdateStorageView();
            if (User.authorisation.Contains("Konobar"))
            {
                this.buttonAddItem.Dispose();
            }
        }

        private void UpdateStorageView(String filter = "")
        {
            SuspendLayout();
            SqlConnection connection = DB.getConnection();
            DataSet dataset = new DataSet();

            using (SqlDataAdapter adapter = new SqlDataAdapter("  SELECT S.*, CASE WHEN H.IdItem_FK IS NOT NULL THEN 1 ELSE 0  END AS OnHappyHour FROM dbo.Storage S LEFT JOIN dbo.Happyhour H ON S.Id = H.IdItem_FK ", connection))
            {
                adapter.Fill(dataset);//, "Storage");
            }

            var dv = dataset.Tables[0].DefaultView;
            dv.RowFilter = filter;
            dataGridView1.DataSource = dv;

            // postavljamo da se editati može samo cooler i backstorage
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns["Deleted"].Visible = false;

            DB.closeConnection();
            ResumeLayout();
        }

        private void buttonAddItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
            //dateTimePickerFrom.Value;
            //dateTimePickerTo.Value;
            //textBoxPrice.Text
            if (dataGridView1.SelectedRows.Count < 1)
                MessageBox.Show("Please select a row!");
            if (decimal.TryParse(textBoxPrice.Text, out _))
                MessageBox.Show("Price not valid!");
            else
            {
                string error = Service.addToHappyHour((int)dataGridView1.SelectedRows[0].Cells[0].Value, dateTimePickerFrom.Value, dateTimePickerTo.Value, decimal.Parse(textBoxPrice.Text));
                if (error != "")
                    MessageBox.Show(error);
                UpdateStorageView();
            }

        }
    }
}
