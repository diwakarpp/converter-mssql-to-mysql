using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter_SQL_to_MySQL_GUI {
    class Connection {
        static string ConnectionString;//= "Server=LLORIC\\SQLEXPRESS;Database=ProjectXDB;User ID=sa;Password=sa";
        public static string PropConnString { set { ConnectionString = value; } get { return ConnectionString; } }

        public static bool TestConnection() {
            using (SqlConnection iCon = new SqlConnection(PropConnString)) {
                try {
                    iCon.Open();
                    return true;
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message.ToString());
                    return false;
                } finally {
                    iCon.Close();
                }
            }
        }
    }
}
