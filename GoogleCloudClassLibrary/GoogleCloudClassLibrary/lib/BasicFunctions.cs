using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudClassLibrary{
    public class BasicFunctions {
        public static Boolean isEmpty(String str) {
            return ((str == null || str == "") ? true : false);
        }

        public static String processResponseStream(Stream stream) {
            using (var temp_stream = new System.IO.MemoryStream()) {
                byte[] buffer = new byte[2048];
                int bytes_read = 0;
                String str = "";

                while ((bytes_read = stream.Read(buffer, 0, buffer.Length)) > 0) {
                    temp_stream.Write(buffer, 0, bytes_read);
                    str += buffer.ToString();
                    Console.WriteLine(str);
                }

                return temp_stream.ToString();
            }
        }

        public static String getFieldsListString(List<Places.Fields> fields) {
            String output = "";
            int len = fields.Count;

            for (int i = 0; i < len; i++) {
                if (i != len - 1) {
                    output += fields[i].ToString().ToLower() + ",";
                }
                else {
                    output += fields[i].ToString().ToLower();
                }
            }

            return output;
        }

        public static String processTextQuery(String input) {
            String processedInput = "";
            int len = input.Length;

            for (int i = 0; i < len; i++) {
                if (input[i] != ' ') {
                    processedInput += input[i];
                }
                else {
                    processedInput += "%20";
                }
            }

            return processedInput;
        }
    }
}
