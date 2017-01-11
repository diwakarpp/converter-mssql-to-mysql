using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Converter_SQL_to_MySQL_GUI {
    public partial class Form1 : Form {

        public Form1() {
            InitializeComponent();
         //   reset();
        }
        //private void reset() {
        //    l1.Text = "";
        //    l2.Text = "";
        //    l3.Text = "";
        //    l4.Text = "";
        //    l5.Text = "";
        //    l6.Text = "";
        //}
        private void simpleButton1_Click(object sender, EventArgs e) {
            status_lbl.Text = "--";
            if (fileLocation_txt.Text != "" && server_txt.Text != "" && db_txt.Text != "" && user_txt.Text != "" && pass_txt.Text != "") {
                if (!backgroundWorker.IsBusy)
                    backgroundWorker.RunWorkerAsync();
            } else
                MessageBox.Show("Field Required!");
        }

        private void Proccess() {
            if (checktbl_ch1.Checked) {
                SetText(new string[] { "current table: 1" });
               // l1.Text = "please wait";
                convert(tbl_txt1.Text);
               // l1.Text = "done";
            }
            if (checktbl_ch2.Checked) {
                SetText(new string[] { "current table: 2" });
               // l2.Text = "please wait";
                convert(tbl_txt2.Text);
                // l2.Text = "done";
            }
            if (checktbl_ch3.Checked) {
                SetText(new string[] { "current table: 3" });
               // l3.Text = "please wait";
                convert(tbl_txt3.Text);
                // l3.Text = "done";
            }
            if (checktbl_ch4.Checked) {
                SetText(new string[] { "current table: 4" });
               // l4.Text = "please wait";
                convert(tbl_txt4.Text);
                // l4.Text = "done";
            }
            if (checktbl_ch5.Checked) {
                SetText(new string[] { "current table: 5" });
               // l5.Text = "please wait";
                convert(tbl_txt5.Text);
                // l5.Text = "done";
            }
            if (checktbl_ch6.Checked) {
                SetText(new string[] { "current table: 6" });
              //  l6.Text = "please wait";
                convert(tbl_txt6.Text);
                // l6.Text = "done";
            }
            SetText(new string[] { "done" });
            MessageBox.Show("Done!");
        }

        private void convert(String tableName) {
            //get row count
            ///-------------------------------------------------------
           
            using (var sqlCon = new SqlConnection(Connection.PropConnString)) {
                sqlCon.Open();

                var com = sqlCon.CreateCommand();
                com.CommandText = "select * from " + tableName;
                using (var reader = com.ExecuteReader()) {
                    //here you retrieve what you need
                }

                com.CommandText = "select @@ROWCOUNT";
                int totalRowa = Convert.ToInt32(com.ExecuteScalar());
                totalrow.setv(totalRowa);
                sqlCon.Close();
            }
           // MessageBox.Show(">> " + totalRow);
            ///-----------------------------------------------------------------------
            //write(tableName,
            //    "SET SQL_MODE = \"NO_AUTO_VALUE_ON_ZERO\";" +
            //    "\nSET time_zone = \"+00:00\";");
            write(tableName, string.Format("\nCREATE TABLE IF NOT EXISTS `{0}` (", tableName));
            //this will creating the table
            List<string> columnsANDTypes = ColumnNamesANDTypes.getColumnNameANDType(tableName);
            int size = columnsANDTypes.Count;
            int inc = 0;
            foreach (string column in columnsANDTypes) {
                inc++;
                write(tableName, "\t" + column + ((inc < size) ? "," : ""));
            }
            FileManagerTask.writeFile(fileLocation_txt.Text, tableName, ");");


            //this will inerting data into a created table
            List<string> names = ColumnNames.getColumnName(tableName);
            write(tableName, string.Format("INSERT INTO `{0}` (", tableName)); ;
            inc = 0;
            foreach (string name in names) {
                inc++;
                write(tableName, "\t`" + name + "`" + ((inc < size) ? "," : ""));
            }
            write(tableName, ") VALUES");
            int row = AllData.getAllDAta(backgroundWorker, fileLocation_txt.Text, names, tableName, tableName, ColumnTypes.getColumnTypes(tableName));
        }
        private void write(string fileName, string towrite) {
            FileManagerTask.writeFile(fileLocation_txt.Text, fileName, towrite);
        }
        private void Form1_Load(object sender, EventArgs e) {
            this.CenterToScreen();
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e) {
            if (checktbl_ch1.Checked)
                tbl_txt1.Enabled = true;
            else
                tbl_txt1.Enabled = false;
        }

        private void checktbl_ch2_CheckedChanged(object sender, EventArgs e) {
            if (checktbl_ch2.Checked)
                tbl_txt2.Enabled = true;
            else
                tbl_txt2.Enabled = false;
        }

        private void checktbl_ch3_CheckedChanged(object sender, EventArgs e) {
            if (checktbl_ch3.Checked)
                tbl_txt3.Enabled = true;
            else
                tbl_txt3.Enabled = false;
        }

        private void checktbl_ch4_CheckedChanged(object sender, EventArgs e) {
            if (checktbl_ch4.Checked)
                tbl_txt4.Enabled = true;
            else
                tbl_txt4.Enabled = false;
        }

        private void checktbl_ch5_CheckedChanged(object sender, EventArgs e) {
            if (checktbl_ch5.Checked)
                tbl_txt5.Enabled = true;
            else
                tbl_txt5.Enabled = false;
        }

        private void checktbl_ch6_CheckedChanged(object sender, EventArgs e) {
            if (checktbl_ch6.Checked)
                tbl_txt6.Enabled = true;
            else
                tbl_txt6.Enabled = false;
        }

        private void pass_txt_EditValueChanged(object sender, EventArgs e) {

        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            Connection.PropConnString = string.Format("Server={0};Database={1};User ID={2};Password={3}", server_txt.Text, db_txt.Text, user_txt.Text, pass_txt.Text);
            if (Connection.TestConnection()) {
             //   status_lbl.Text = "Connected";
               // MessageBox.Show("conected");
                Proccess();
            } else
               // status_lbl.Text = "Failed to connect";
            MessageBox.Show("fail to connect");
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            float dd = (e.ProgressPercentage/totalrow.getv()) ;
            int v=Convert.ToInt32(dd * 100);
            progressBar.Value = v;
            per.Text = v + "%";
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {

        }
        delegate void SetTextCallback(string[] text);
        private void SetText(string[] text) {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label.InvokeRequired) {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            } else {
                // this.dgv.Rows.Clear();
                    this.label.Text = text[0];
            }
        }
    }
}