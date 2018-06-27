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

                while ((bytes_read = stream.Read(buffer, 0, buffer.Length)) > 0) {
                    temp_stream.Write(buffer, 0, bytes_read);
                }

                return temp_stream.ToString();
            }
        }
    }
}
