﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CaffeBar
{
    class Service
    {
        //  Get items on menu
        public static List<Tuple<int, String>> getMenuItems(out String errorMessage)
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
        public static decimal getPrice( in int id, out String errorMessage)
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
        public static String newReceipt(  DataTable dataTableReceipt,  String PaymentMethod, out Double total, out Int32 receiptId)
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

    }
}