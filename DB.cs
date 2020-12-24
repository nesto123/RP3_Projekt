﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CaffeBar
{

    static class DB
    {
        //public static string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\franv\Documents\Faks-noDrive\RP3\projekt\CaffeBar\CaffeBarDatabase.mdf;Integrated Security=True";
        public static SqlConnection db = null;

        public static SqlConnection getConnection()
        {
            //MessageBox.Show(DB.db == null);
            if ( DB.db == null )
            {
                try
                {
                    DB.db = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\franv\Documents\Faks-noDrive\RP3\projekt\CaffeBar\CaffeBarDatabase.mdf;Integrated Security=True");
                    DB.db.Open();
                }
                catch (Exception ex) { throw ex; }
            }
            else if( DB.db.State == ConnectionState.Closed )
            {
                try
                {
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