using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter_SQL_to_MySQL_GUI {
    class FileManagerTask {
        public static void writeFile(string dir, string filename, string toWrite) {


            string path = string.Format(@"{0}\{1}.sql", dir, filename);
            try {
                // This text is added only once to the file.
                if (!File.Exists(path)) {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path)) {
                        sw.WriteLine(toWrite);
                    }
                }

                // This text is always added, making the file longer over time
                // if it is not deleted.
                using (StreamWriter sw = File.AppendText(path)) {
                    sw.WriteLine(toWrite);
                }

                // Open the file to read from.
                //using (StreamReader sr = File.OpenText(path))
                //{
                //    string s = "";
                //    while ((s = sr.ReadLine()) != null)
                //    {
                //        Console.WriteLine(s);
                //    }
                //}



            } catch (Exception e) {
                Console.WriteLine(e.Message.ToString());
            }


        }
    }
}
