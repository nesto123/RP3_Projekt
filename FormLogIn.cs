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
    /// The entry point for the application - login.
    /// </summary>
    public partial class FormLogIn : Form
    {
        public FormLogIn()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            /*string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\franv\Documents\Faks-noDrive\RP3\projekt\CaffeBar\CaffeBarDatabase.mdf;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connection_string);
            SqlCommand command = new SqlCommand("SELECT Password FROM [User] WHERE Username=@username;", connection);
            command.Parameters.Add("@username", SqlDbType.NChar);
            command.Parameters["@username"].Value = textBoxUsername.Text.ToString();

            SqlDataReader dataReader;
            String pass= "nista";
           // CaffeBarDatabaseDataSet;
            try
            {
                connection.Open();
                dataReader = command.ExecuteReader();
                if(dataReader.HasRows)
                {
                    dataReader.Read(); 
                    pass = (String) dataReader.GetValue(0);
                }

            }catch(Exception ex)
            {
                MessageBox.Show("ERROR: Database connection unsuccessful!" + ex.ToString());
                this.Close();
            }

            MessageBox.Show(pass);
            */
            SqlConnection connection = DB.getConnection();
            SqlCommand command = new SqlCommand("SELECT Id, Password, Authorisation FROM [User] WHERE Username=@username;", connection);
            command.Parameters.Add("@username", SqlDbType.NChar);
            command.Parameters["@username"].Value = textBoxUsername.Text.ToString();

            SqlDataReader dataReader;
            String pass = "";

            try
            {
                dataReader = command.ExecuteReader();
                if (!dataReader.HasRows)
                {
                    MessageBox.Show("Invalid credentials!");
                    DB.closeConnection();

                    return;
                }
                dataReader.Read();
                User.id = dataReader.GetInt32(0);
                pass = (String)dataReader.GetValue(1);
                User.authorisation = dataReader.GetString(2);
                User.name = textBoxUsername.Text.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: Database connection unsuccessful!" + ex.ToString());                
            }
            finally
            {
                DB.closeConnection();
            }


            if (pass == textBoxPassword.Text.ToString())
            {
                this.Hide();
                FormApp formApp = new FormApp();
                formApp.ShowDialog();
            }
            else
                MessageBox.Show("Invalid credentials!");
        }




    }
}

/*namespace Connection_Class
{
    public class Connection_Query
    {

        string ConnectionString = "";
        SqlConnection con;

        public void OpenConection()
        {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }


        public void CloseConnection()
        {
            con.Close();
        }


    }
}*/
