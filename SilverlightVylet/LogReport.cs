using System;
using System.IO;
using System.Text;
using System.Reflection;

namespace Log
{
    class LogReport
    {
        private static string m_path = null;

        public enum LogType { LogNone=0, LogError=1, LogDebug=2, LogInfo=3, LogReflection=4 };
        private static string[] types = { "", "Error", "Debug", "Info", "Reflection" };

        private static bool m_write_info = true;
        private static bool m_write_debug = true;
        private static bool m_write_error = true;

        public static void initLog(string fname)
        {
            m_path = fname;
        }

        public static void setWhatWrite(bool info, bool debug, bool err)
        {
            m_write_info = info;
            m_write_debug = debug;
            m_write_error = err;
        }


        public static bool Write(string line)
        {
            return WriteLine(line, LogType.LogNone);
        }

        public static bool Info(string line)
        {
            return WriteLine(line, LogType.LogInfo);
        }

        /// <summary>
        /// Zapis Debug hlasky do logu
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool Debug(string line)
        {
            return WriteLine(line, LogType.LogDebug);
        }

        /// <summary>
        /// Zapis Error hlasky do logu
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool Error(string line)
        {
            return WriteLine(line, LogType.LogError);
        }

        public static bool Method(MethodBase method)
        {
            bool ret = false;
            string message = types[(int)LogType.LogReflection] + ">";
            message += method.Name + " " + method.ToString();
            ret = WriteLine(message);

            return ret;
        }

        public static bool WriteLine(string line)
        {
            return WriteLine(line, LogType.LogNone);
        }
        public static bool WriteLine(string line, LogType type)
        {

            if (type == LogType.LogInfo && m_write_info == false)
                return true;
            if (type == LogType.LogError && m_write_error == false)
                return true;
            if (type == LogType.LogDebug && m_write_debug == false)
                return true;

            string headLine = "[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "]"+types[(int)type]+">";

            try
            {
                FileStream fs = new FileStream(m_path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                fs.Seek(0, SeekOrigin.End);
                StreamWriter swFromFileStream = new StreamWriter(fs);                
                swFromFileStream.WriteLine(headLine + line);
                swFromFileStream.Flush();
                swFromFileStream.Close();
                fs.Close();
                fs.Dispose();
                swFromFileStream.Dispose();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
