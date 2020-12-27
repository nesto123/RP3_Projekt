using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CaffeBar
{
    internal class Service
    {
        //  Get items on menu
        internal static List<Tuple<int, String>> getMenuItems(out String errorMessage)
        {
            var list = new List<Tuple<int, String>>();
            var connection = DB.getConnection();
            var command = new SqlCommand("SELECT Id, Item FROM dbo.Storage", connection);
            //SqlDataReader dataReader;

            errorMessage = "";

            try
            {
                var dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    list.Add(Tuple.Create(dataReader.GetInt32(0), dataReader.GetString(1)));
            }
            catch (Exception ex)
            {
                errorMessage = "ERROR: Database error! " + ex.Message;
            }
            finally
            {
                DB.closeConnection();
            }
            return list;
        }

        //  Get item details for new receipt from STORAGE
        internal static decimal getPrice( in int id, out String errorMessage)
        {
            SqlConnection connection = DB.getConnection();
            SqlCommand command = new SqlCommand("SELECT Price FROM dbo.Storage WHERE Id=@id;", connection);
            decimal price = -1;
            errorMessage = "";

            command.Parameters.Add("@id", SqlDbType.Int);
            command.Parameters["@id"].Value = id;
           
            
            try
            {
                SqlDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                price = dataReader.GetDecimal(0);
            }
            catch (Exception ex)
            {
                errorMessage="ERROR: Database error! " + ex.Message;
            }
            finally
            {
                DB.closeConnection();
            }
            return price;
        }

        //  Insert new receipt
        internal static String newReceipt(  DataTable dataTableReceipt,  String PaymentMethod, out Double total, out Int32 receiptId)
        {
            String errorMessage = "";
            SqlTransaction transaction = null;
            total = 0.0;
            Double amount, price;
            receiptId = 0 ;

            foreach (DataRow item in dataTableReceipt.Rows)
            {
                 Double.TryParse(item["Amount"].ToString(), out amount); Double.TryParse(item["Price per unit"].ToString(), out price);
                total += amount * price;
            }

            SqlConnection connection = DB.getConnection();

            SqlCommand command = new SqlCommand("INSERT INTO dbo.Receipts(Total,Payment_method) output INSERTED.ID VALUES(@total,@payment_method)", connection);
            SqlCommand command2 = new SqlCommand("INSERT INTO dbo.Receipts_item(Id_ItemFK,Id_receiptFK,Amount,Price) VALUES(@id_ItemFK,@id_receiptFK,@amount,@price)", connection);

            command.Parameters.AddWithValue("@total", total);//.ToString().Replace(",","."));
            command.Parameters.AddWithValue("@payment_method", PaymentMethod);
            
            try
            {
                SqlDataReader reader =  command.ExecuteReader();
                reader.Read();
                receiptId = reader.GetInt32(0);
                reader.Close();
                foreach (DataRow item in dataTableReceipt.Rows)
                {
                    command2.Parameters.AddWithValue("@id_ItemFK", item["Id"]);
                    command2.Parameters.AddWithValue("@id_receiptFK", receiptId);
                    command2.Parameters.AddWithValue("@amount", item["Amount"]);
                    command2.Parameters.AddWithValue("@price", item["Price per unit"]);
                    command2.ExecuteNonQuery();
                    command2.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                errorMessage = "ERROR: Database error! " + ex.Message;
            }
            finally
            {
                DB.closeConnection();
            }
            return errorMessage;
        }

        //get receipt atributes
        internal static List<String> getReceiptDetails( int receiptID, out String errorMessage)
        {
            SqlConnection connection = DB.getConnection();
            SqlCommand command = new SqlCommand("SELECT * FROM dbo.Receipts WHERE Id=@id;", connection);
            errorMessage = "";
            var list = new List<String>();

            command.Parameters.Add("@id", SqlDbType.Int);
            command.Parameters["@id"].Value = receiptID;
            
            try
            {
                SqlDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                
                for (var i = 0; i < 5; ++i)
                    list.Add(dataReader.GetValue(i).ToString());
            }
            catch (Exception ex)
            {
                errorMessage = "ERROR: Database error! " + ex.Message;
            }
            finally
            {
                DB.closeConnection();
            }
            //errorMessage = receiptID.ToString();
            return list;
        }

        public static List<Tuple<String,String>> getAllEmployeData(out String errorMessage)
        {
            SqlConnection connection = DB.getConnection();
            SqlCommand command = new SqlCommand("SELECT Id, Username FROM [User] WHERE Deleted=@deleted;", connection);
            errorMessage = "";
            var list = new List<Tuple<String, String>>();

            command.Parameters.Add("@deleted", SqlDbType.Int);
            command.Parameters["@deleted"].Value = 0;

            try
            {
                SqlDataReader dataReader = command.ExecuteReader();
                
                while(dataReader.Read())
                    list.Add(Tuple.Create(dataReader.GetValue(0).ToString(), dataReader.GetValue(1).ToString()));
            }
            catch (Exception ex)
            {
                errorMessage = "ERROR: Database error! " + ex.Message;
            }
            finally
            {
                DB.closeConnection();
            }
            return list;
        }

        //  HappyHour -- not done
        internal static bool onHappyHour(in int itemId ,out int price, out String errorMessage) //AKO NIJE I NALAZI SE TU POZAVAT OD TU DELETE
        {

            //SqlConnection connection = DB.getConnection();
            //SqlCommand command = new SqlCommand("SELECT Id, Username FROM [User] WHERE Deleted=@deleted;", connection);
            //errorMessage = "";
            //var list = new List<Tuple<String, String>>();

            //command.Parameters.Add("@deleted", SqlDbType.Int);
            //command.Parameters["@deleted"].Value = 0;

            //try
            //{
            //    SqlDataReader dataReader = command.ExecuteReader();

            //    while (dataReader.Read())
            //        list.Add(Tuple.Create(dataReader.GetValue(0).ToString(), dataReader.GetValue(1).ToString()));
            //}
            //catch (Exception ex)
            //{
            //    errorMessage = "ERROR: Database error! " + ex.Message;
            //}
            //finally
            //{
            //    DB.closeConnection();
            //}
            errorMessage = "";
            price = 0;
            return false;

            //      Upiti
            //INSERT INTO dbo.Happyhour(IdItem_FK, From_, Untill, Pastprice)  VALUES(3, '2018-06-23 23:30:20', '2018-06-23 07:30:20', 2)

        }

        internal static String  removeExpiredFromHappyHour(in int itemId, out String errorMessage)
        {
            errorMessage = "";
            
            return "";
        }
        internal static String/*errorMessage*/ addToHappyHour(in int id, out String errorMessage)
        {
            errorMessage = "";

            return "";
        }

    }
}
