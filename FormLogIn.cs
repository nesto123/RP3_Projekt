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



namespace CaffeBar
{
    

    public partial class FormLogIn : Form
    {
        
        public FormLogIn()
        {
            InitializeComponent();
        }

        public string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\franv\Documents\Faks-noDrive\RP3\projekt\CaffeBar\CaffeBarDatabase.mdf;Integrated Security=True";

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            /*SqlConnection connection = new SqlConnection(connection_string);
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
            SqlCommand command = new SqlCommand("SELECT Password FROM [User] WHERE Username=@username;", connection);
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
                pass = (String)dataReader.GetValue(0);
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
                MessageBox.Show("Ulogirani ste!");
            else
                MessageBox.Show("Invalid credentials!");

        }
    }
    class DB
    {
        public static SqlConnection db = null;

        public static SqlConnection getConnection()
        {
            //MessageBox.Show(DB.db == null);
            if (DB.db == null || DB.db.State == ConnectionState.Closed)
            {
                try
                {
                    DB.db = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\franv\Documents\Faks-noDrive\RP3\projekt\CaffeBar\CaffeBarDatabase.mdf;Integrated Security=True");
                    DB.db.Open();
                }
                catch (Exception ex) { throw ex; }
            }
            return DB.db;
        }

        public static void closeConnection()
        {
            if (DB.db.State == ConnectionState.Open)
                DB.db.Close();
        }
    }
}
