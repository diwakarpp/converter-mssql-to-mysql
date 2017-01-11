using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Converter_SQL_to_MySQL_GUI {
    class AllData {
        private const int LIMITROW = 20000;
      //  private static string fileName;
        public static int getAllDAta(
            BackgroundWorker bwParameter,
            string ddirStringParameter,
            List<string> columnsListParameter,
            string tablenameStringParamater,
            string filenameStringParameter,
            List<string> ColumnTypeListParameter) {

            int row = 0;
         //   fileName = filenameStringParameter;
            int size = columnsListParameter.Count;
            string xQuery = string.Format("Select * From {0}", tablenameStringParamater);

            using (SqlConnection xConn = new SqlConnection(Connection.PropConnString)) {
                using (SqlCommand xComm = new SqlCommand(xQuery, xConn)) {
                    try {
                        xConn.Open();
                        SqlDataReader xRead = xComm.ExecuteReader();
                        Boolean tmpBoolToFalseFirst = false;
                        // string wait = "please wait";
                        //double inc_ = 0;
                        int increment = 1;
                        int fileExtension = 1;
                        while (xRead.Read()) {
                            if (increment % LIMITROW == 0) {
                                fileExtension++;
                            }
                            row++;
                            //if (inc_ % 2 == 0) {
                            //  wait += ".";
                            //}
                            // if (inc_ > 10) {
                            //  wait = "please wait";
                            //  inc_ = 0;
                            //}
                            // Console.WriteLine(wait);
                            if (tmpBoolToFalseFirst)
                                write(ddirStringParameter, filenameStringParameter + fileExtension, ",");
                            string RowData = "\t(";
                            int inc = 0;
                            var nameType = columnsListParameter.Zip(ColumnTypeListParameter, (cc, tt) => new { CC = cc, TT = tt });
                            foreach (var CCTT in nameType) {
                                inc++;
                                RowData +=
                                    //====================
                                    //concatination111
                                    (
                                    /*
                                     
                                     FIRST QUOTAION (IF NEEDED)
                                     
                                     */
                                    //condition
                                        (CCTT.TT == "varchar" || CCTT.TT == "datetime" || CCTT.TT == "date")
                                                ?
                                    //then
                                                "'"
                                                :
                                    //else
                                                ""
                                    )
                                    //====================
                                    /*
                                     
                                    VALUE (adding space in no data)
                                     
                                     */
                                    //concatination222

                                    + (
                                    // escapeQiatation(xRead[CCTT.CC].ToString())
                                       escapeQiatation(
                                            (xRead[CCTT.CC].ToString() != "") ? xRead[CCTT.CC].ToString() : " "
                                       )

                                    ) +

                                    /*
                                     
                                     
                                     SECOND QUOTAION (IF NEEDED)
                                     
                                     */
                                    //====================
                                    //concatination333
                                    (
                                    //condition1111
                                        (inc < size)
                                            ?
                                    //then1111
                                            (
                                    //condition222
                                                (CCTT.TT == "varchar" || CCTT.TT == "datetime" || CCTT.TT == "date")
                                                    ?
                                    //then2222
                                                    "'"
                                                    :
                                    //else222
                                                    ""
                                            )
                                            + ","
                                            :
                                    //else11111
                                            (
                                    //condition
                                                (CCTT.TT == "varchar" || CCTT.TT == "datetime" || CCTT.TT == "date")
                                                    ?
                                    //then
                                                    "'"
                                                    :
                                    //else
                                                    ""
                                            )
                                    );
                            }
                            RowData += ")";
                            write(ddirStringParameter, filenameStringParameter + fileExtension, RowData);
                            tmpBoolToFalseFirst = true;
                            //inc_ += 0.5;
                            //Console.Clear();
                            //report progress
                            // int xx = (increment / totalrow.getv()F) * 100;
                            bwParameter.ReportProgress(increment);
                            increment++;
                            //endWhile
                        }

                        write(ddirStringParameter, filenameStringParameter + fileExtension, ";");
                    } catch (Exception ex) {
                        // Console.WriteLine("Error in get the data: " + ex.Message.ToString());
                        MessageBox.Show("Error in get the data: " + ex.Message.ToString());
                        // throw;
                    } finally {
                        xConn.Close();
                    }
                }
            }
            return row;
        }

        private static void write(string dir,string fi, string towrite) {
            FileManagerTask.writeFile(dir, fi, towrite);
        }
        private static string escapeQiatation(string ini) {
            StringBuilder strb = new StringBuilder(ini);
            strb.Replace("\\", "\\\\");
            strb.Replace("'", "");
            strb.Replace("\"", "\\\"");
            strb.Replace(",", "");
            return strb.ToString();
        }
    }
}
