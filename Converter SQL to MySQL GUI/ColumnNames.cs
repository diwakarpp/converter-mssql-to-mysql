using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter_SQL_to_MySQL_GUI
{
    class ColumnNames
    {
        public static List<string> getColumnName(string tableName)
        {
            List<string> names = new List<string>();
            //  string xQuery = string.Format("SELECT [name] AS [Column Name] FROM syscolumns WHERE id = (SELECT id FROM sysobjects WHERE  [Name] = '{0}') ", tableName);
            string xQuery = string.Format("SELECT column_name as 'Column Name' FROM information_schema.columns WHERE table_name = '{0}' ", tableName);
            //   string xQuery = string.Format("SELECT data_type, character_maximum_length FROM information_schema.columns WHERE table_name = '{0}' AND column_name = 'name'", tableName);

            using (SqlConnection xConn = new SqlConnection(Connection.PropConnString))
            {
                using (SqlCommand xComm = new SqlCommand(xQuery, xConn))
                {
                    try
                    {
                        xConn.Open();
                        SqlDataReader xRead = xComm.ExecuteReader();
                        while (xRead.Read())
                        {
                            // Console.WriteLine(xRead["Column Name"].ToString());
                            //  names.Add(xRead["Column Name"].ToString());
                            // names.Add(xRead["data_typee"].ToString());
                            names.Add(xRead["Column Name"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message.ToString());
                        throw;
                    }
                    finally
                    {
                        xConn.Close();
                    }
                }
            }
            return names;
        }
    }
}
