using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    public class ErrorLogger
    {
        private static readonly object LOCK;

        private static string logPath;

        static ErrorLogger()
        {
            LOCK = new object();
            logPath = $"{AppDomain.CurrentDomain.BaseDirectory}/Logs";
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
        }

        public static void Log(string message, string logType = "Error")
        {
            lock (LOCK)
            {
                using StreamWriter streamWriter = new StreamWriter(string.Format(@"{0}/{1}Log-" + DateTime.Now.ToString("dd-MM-yyyy") + ".log", logPath, logType), append: true);
                StreamWriter streamWriter2 = streamWriter;
                string value = "\r" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                streamWriter2.WriteLine(value);
                streamWriter.WriteLine(message);
                //streamWriter.WriteLine("\n\r");
                streamWriter.Close();
            }
        }

        public static bool SaveToJson(string message, int reportid, int documentid)
        {
            logPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Logs";
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            lock (LOCK)
            {
                try
                {
                    string filepath = string.Format(@"{0}\{1}{2}.txt", logPath, reportid.ToString(), documentid.ToString());
                    using StreamWriter streamWriter = new StreamWriter(filepath);


                    streamWriter.WriteLine(message);

                    streamWriter.Close();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
                
            }
        }
    }
}
