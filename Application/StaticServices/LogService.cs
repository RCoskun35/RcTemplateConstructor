using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.StaticServices
{
    public static class LogService
    {
        public enum LogType
        {
            Error,
            Warning,
            Information,
            Success,
            Debug
        }
        private static string logBasePath;

        private static string logErrorPath;

        private static string logSuccessPath;

        private static string logInfoPath;

        private static string logWarningPath;

        private static string logDebugPath;

        private static string logPath;

        public static object _locked = new object();

        public static void Init(string path = "")
        {
            logBasePath = (string.IsNullOrEmpty(path) ? Environment.CurrentDirectory : path) + "\\LOG\\";
            logErrorPath = logBasePath + "Error\\";
            logSuccessPath = logBasePath + "Success\\";
            logInfoPath = logBasePath + "Info\\";
            logWarningPath = logBasePath + "Warning\\";
            logDebugPath = logBasePath + "Audit\\";
            if (!Directory.Exists(logBasePath))
            {
                Directory.CreateDirectory(logBasePath);
            }

            if (!Directory.Exists(logErrorPath))
            {
                Directory.CreateDirectory(logErrorPath);
            }

            if (!Directory.Exists(logSuccessPath))
            {
                Directory.CreateDirectory(logSuccessPath);
            }
        }

        public static void IsLogged(this Exception ex, string title = "", LogType type = LogType.Error)
        {
            try
            {
                string text = "";
                text += "**********************\r\n";
                text = text + "Datetime : " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " " + Environment.NewLine;
                text = text + "Type (Exception) : " + type.ToString() + " " + Environment.NewLine;
                text = (!string.IsNullOrEmpty(title) ? text + "Title : " + title + " " : "") + Environment.NewLine;
                text = text + "StackTrace : " + ex.StackTrace + " " + Environment.NewLine;
                text = text + "Source : " + ex.Source + " " + Environment.NewLine;
                text = text + "TargetSite : " + ex.TargetSite?.ToString() + " " + Environment.NewLine;
                text = text + "Message : " + ex.Message + " " + Environment.NewLine;
                text = text + "InnerException : " + ex.InnerException?.ToString() + " " + Environment.NewLine;
                text += "**********************\r\n";
                AddText(text, type);
            }
            catch (Exception ex2)
            {
                AddText("IsLogged::Exception (1) Error : " + ex2.Message);
            }
        }


        public static void IsLogged(this object ex, string title = "", LogType type = LogType.Error)
        {
            try
            {

                string text = "";
                Exception ex2 = (Exception)ex;
                text += "**********************\r\n";
                text = text + "Datetime : " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " " + Environment.NewLine;
                text = text + "Type : (object) " + type.ToString() + " " + Environment.NewLine;
                text = (!string.IsNullOrEmpty(title) ? text + "Title : " + title + " " : "") + Environment.NewLine;
                text = text + "StackTrace : " + ex2.StackTrace + " " + Environment.NewLine;
                text = text + "Source : " + ex2.Source + " " + Environment.NewLine;
                text = text + "TargetSite : " + ex2.TargetSite?.ToString() + " " + Environment.NewLine;
                text = text + "Message : " + ex2.Message + " " + Environment.NewLine;
                text = text + "InnerException : " + ((ex2.InnerException != null) ? ex2.InnerException!.ToString() : "") + " " + Environment.NewLine;
                text += "**********************\r\n";
                AddText(text, type);
            }
            catch (Exception ex3)
            {
                AddText("IsLogged::object (1) Error : " + ex3.Message);
            }
        }

        public static void IsLogged(this object ex, object entity = null, LogType type = LogType.Error)
        {
            try
            {

                string text = "";
                Exception ex2 = (Exception)ex;
                text += "**********************\r\n";
                text = text + "Datetime : " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " " + Environment.NewLine;
                text = text + "Type : (object) " + type.ToString() + " " + Environment.NewLine;
                text = text + (entity != null ? "Parameters : " + JsonConvert.SerializeObject(entity, Formatting.Indented)
                + " " : "") + Environment.NewLine;
                text = text + "StackTrace : " + ex2.StackTrace + " " + Environment.NewLine;
                text = text + "Source : " + ex2.Source + " " + Environment.NewLine;
                text = text + "TargetSite : " + ex2.TargetSite?.ToString() + " " + Environment.NewLine;
                text = text + "Message : " + ex2.Message + " " + Environment.NewLine;
                text = text + "InnerException : " + ((ex2.InnerException != null) ? ex2.InnerException!.ToString() : "") + " " + Environment.NewLine;
                text += "**********************\r\n";
                AddText(text, type);
            }
            catch (Exception ex3)
            {
                AddText("IsLogged::object (1) Error : " + ex3.Message);
            }
        }

        public static void IsLogged(string msg, LogType type = LogType.Error)
        {
            try
            {
                string text = "";
                text += "**********************\r\n";
                text = text + "Datetime : " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " " + Environment.NewLine;
                text = text + "Type (str) : " + type.ToString() + " " + Environment.NewLine;
                text = text + "Message : " + msg + " " + Environment.NewLine;
                text += "**********************\r\n";
                AddText(text, type);
            }
            catch (Exception ex)
            {
                AddText("IsLogged::Exception (1) Error : " + ex.Message);
            }
        }

        private static void AddText(string message, LogType type = LogType.Error)
        {
            try
            {
                var path = "";

                switch (type)
                {
                    case LogType.Error:
                        path = logErrorPath;
                        break;
                    case LogType.Warning:
                        path = logWarningPath;
                        break;
                    case LogType.Information:
                        path = logInfoPath;
                        break;
                    case LogType.Success:
                        path = logSuccessPath;
                        break;
                    default:
                        path = logErrorPath;
                        break;
                }

                logPath = path + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
                lock (_locked)
                {
                    if (!File.Exists(logPath))
                    {
                        using StreamWriter streamWriter = File.CreateText(logPath);
                        streamWriter.WriteLine(message + Environment.NewLine);
                    }
                    else
                    {
                        using StreamWriter streamWriter2 = File.AppendText(logPath);
                        streamWriter2.WriteLine(message + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }


    }
}
