using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CaffeBar
{
    /// <summary>
    /// Database comunication class.
    /// </summary>
    internal static class DB
    {
        //internal static string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\franv\Documents\Faks-noDrive\RP3\projekt\RP3_Projekt\CaffeBarDatabase.mdf;Integrated Security=True";
        private static string part = System.IO.Directory.GetCurrentDirectory().Substring(0, System.IO.Directory.GetCurrentDirectory().LastIndexOf(@"\"));
        internal static string connection_string = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + part.Substring(0, part.LastIndexOf(@"\")) + @"\CaffeBarDatabase.mdf"+ ";Integrated Security=True";
        internal static SqlConnection db = null;

        internal static SqlConnection getConnection()
        {
            if ( DB.db == null )
            {
                String path = System.IO.Directory.GetCurrentDirectory().Substring(0, System.IO.Directory.GetCurrentDirectory().LastIndexOf(@"\"));
                path = path.Substring(0, path.LastIndexOf(@"\")) + @"\CaffeBarDatabase.mdf";
                try
                {
                    DB.db = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + path + ";Integrated Security=True");
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

        internal static void closeConnection()
        {
            if (DB.db.State == ConnectionState.Open)
                DB.db.Close();
        }
    }
}
