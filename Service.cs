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

        //  Get item details for new receipt from STORAGE --- (coolerAmount,price)
        internal static Tuple<int,decimal> getPriceCooler( in int id, out String errorMessage)
        {
            SqlConnection connection = DB.getConnection();
            SqlCommand command = new SqlCommand("SELECT Cooler, Price FROM dbo.Storage WHERE Id=@id;", connection);
            errorMessage = "";
            Tuple<int, decimal> tuple;
            command.Parameters.Add("@id", SqlDbType.Int);
            command.Parameters["@id"].Value = id;

            try
            {
                SqlDataReader dataReader = command.ExecuteReader();
                dataReader.Read();
                tuple = Tuple.Create<int, decimal>(dataReader.GetInt32(0), dataReader.GetDecimal(1));
            }
            catch (Exception ex)
            {
                tuple = Tuple.Create<int, decimal>(0,(decimal)0.0);
                errorMessage ="ERROR: Database error! " + ex.Message;
            }
            finally
            {
                DB.closeConnection();
            }
            return tuple;
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
            SqlCommand command3 = new SqlCommand("UPDATE dbo.Storage SET Cooler = Cooler - @times WHERE Id=@id", connection);


            command.Parameters.AddWithValue("@total", total);
            command.Parameters.AddWithValue("@payment_method", PaymentMethod);
            command3.Parameters.Add("@id", SqlDbType.Int);
            command3.Parameters.Add("@times", SqlDbType.Int);

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
                    command3.Parameters["@id"].Value = int.Parse(item["id"].ToString());
                    command3.Parameters["@times"].Value= int.Parse(item["Amount"].ToString());
                    command2.ExecuteNonQuery();
                    command3.ExecuteNonQuery();
                    command2.Parameters.Clear();
                    
                    //errorMessage = int.Parse(item["Amount"].ToString()).ToString()+" i="+ item["Id"].ToString();
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

        //  HappyHour -- not done -- testirat
        internal static bool onHappyHour(in int itemId ,out decimal newPrice,  out String errorMessage) //AKO NIJE I NALAZI SE TU POZAVAT OD TU DELETE
        {

            SqlConnection connection = DB.getConnection();
            SqlCommand command = new SqlCommand("select IdItem_FK, From_, Untill, Pastprice from Happyhour Where IdItem_FK = @idItem", connection);
            var onSale = false;

            errorMessage = ""; newPrice = decimal.Parse("-1");
            command.Parameters.AddWithValue("@idItem", itemId);

            try
            {
                SqlDataReader dataReader = command.ExecuteReader();
                if( dataReader.HasRows)
                {
                    dataReader.Read();
                    var untill = dataReader.GetDateTime(2);
                    var from = dataReader.GetDateTime(1);
                    if (DateTime.Compare(from, DateTime.Now) <= 0 && !(DateTime.Compare(untill, DateTime.Now) <= 0))
                    {
                        //akcija je krenula i nije prošla akcija
                        onSale = true;
                        newPrice = dataReader.GetDecimal(3);
                    }
                    else if (DateTime.Compare(untill, DateTime.Now) <= 0)    // prošla akcija
                    {
                        dataReader.Close();
                        removeExpiredFromHappyHour(itemId, out errorMessage);
                    }
                    //else akcija nije krenula pa ništa
                }
            }
            catch (Exception ex)
            {
                errorMessage += "ERROR: Database error! " + ex.Message;
            }
            finally
            {
                DB.closeConnection();
            }

            return onSale;
        }

        internal static void  removeExpiredFromHappyHour(in int itemId, out String errorMessage)
        {
            SqlConnection connection = DB.getConnection();
            SqlCommand command3 = new SqlCommand("Delete from Happyhour WHERE IdItem_FK = @idItem", connection);

            errorMessage = "";
            command3.Parameters.AddWithValue("@idItem", itemId);

            try
            {
                command3.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errorMessage += "ERROR: Database error! " + ex.Message;
            }
            finally
            {
                DB.closeConnection();
            }
            return;          
        }
        // not done -- not tested
        internal static String/*errorMessage*/ addToHappyHour(in int itemId, in String from_, in String untill) // dodat još provjeru je li artikl već na akciji pa onemogućit to
        {
            SqlConnection connection = DB.getConnection();
            SqlCommand command = new SqlCommand("INSERT INTO dbo.Happyhour(IdItem_FK, From_, Untill, Pastprice)  VALUES(@idItem_FK, @from_, @untill, @pastprice)", connection);
            SqlCommand command2 = new SqlCommand("SELECT Price FROM [Storage] WHERE Id=@id;", connection);

            String errorMessage = "";
            var list = new List<Tuple<String, String>>();

            command2.Parameters.Add("@id", SqlDbType.Int);
            command2.Parameters["@id"].Value = itemId;

            command.Parameters.Add("@idItem_FK", SqlDbType.Int);
            command.Parameters["@idItem_FK"].Value = itemId;
            command.Parameters.Add("@from_", SqlDbType.DateTime2);
            command.Parameters["@from_"].Value = from_;
            command.Parameters.Add("@untill", SqlDbType.DateTime2);
            command.Parameters["@untill"].Value = untill;

            command.Parameters.Add("@pastprice", SqlDbType.Decimal);


            try
            {
                SqlDataReader dataReader = command2.ExecuteReader();

                dataReader.Read();
                var past_price = dataReader.GetDecimal(0);
                command.Parameters["@pastprice"].Value = past_price;
                command.ExecuteNonQuery();
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

        ////////////////////////////
        // add amount to 
        internal static void addAmount(int id, int addAmount,string column, out String errorMessage)
        {
            errorMessage = "";
            SqlConnection connection = DB.getConnection();
            SqlCommand sqlcommand2 = new SqlCommand("UPDATE dbo.Storage SET [Backstorage]= [Backstorage] - @amount WHERE Id = @id", connection);
            SqlCommand sqlcommand = new SqlCommand("UPDATE dbo.Storage SET ["+column+"]="+column+" + @amount WHERE Id = @id", connection);
            sqlcommand.Parameters.AddWithValue("@id", id);
            sqlcommand.Parameters.AddWithValue("@amount", addAmount);
            sqlcommand2.Parameters.AddWithValue("@id", id);
            sqlcommand2.Parameters.AddWithValue("@amount", addAmount);

            try
            {
                sqlcommand.ExecuteNonQuery();
                if (column == "Cooler")
                    sqlcommand2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                errorMessage += ("ERROR:" + ex.Message);
            }
            finally
            {
                DB.closeConnection();
            }
        }


    }
}
