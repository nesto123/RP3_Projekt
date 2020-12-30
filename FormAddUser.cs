using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;


namespace CaffeBar
{
    /// <summary>
    /// Display change form, used when payed with cash.
    /// </summary>
    public partial class FormAddUser : Form
    {
        public FormAddUser()
        {
            InitializeComponent();
        }
        private void FormAddUser_Load(object sender, EventArgs e)
        {}

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("e");
            // validate 
            SqlConnection connection = DB.getConnection();
            SqlCommand command = new SqlCommand("INSERT INTO [User](Username, Password)  VALUES(@Username, @Password)", connection);

            command.Parameters.AddWithValue("@Username", textBoxUsername.Text.ToString());
            command.Parameters.AddWithValue("@Password", textBoxPassword.Text.ToString());       

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: Database error! " + ex.Message);
            }
            finally
            {
                DB.closeConnection();
            }
        }

        #region Input validation


        #endregion



    }
}
