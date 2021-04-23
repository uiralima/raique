using System;
using System.Reflection;

namespace Raique.Common.Log
{
    public class FileLog
    {
        private static FileLog _log = null;
        private static object _lockLog = new object();
        public static FileLog InternalLog 
        {
            get 
             {
                if (null == _log)
                {
                    lock(_lockLog)
                    {
                        if (null == _log)
                        {
                            _log = new FileLog();
                        }
                    }
                }
                return _log;
            }
        }

        public static void StartLogInPath(string path)
        {
            lock (_lockLog)
            {
                _log = new FileLog(path);
            }
        }   

        private string _path;
        private bool _folderIsOk;

        private FileLog()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            _path = System.IO.Path.GetDirectoryName(path).Replace("\\bin", "");
            _path = System.IO.Path.Combine(_path, "Log");
            _folderIsOk = VerifyFolder();
        }

        private FileLog(string path)
        {
            _path = path;
            _folderIsOk = VerifyFolder();
        }

        private bool VerifyFolder()
        {
            if (!System.IO.Directory.Exists(_path))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(_path);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public bool Log(string stringToLog)
        {
            if (_folderIsOk)
            {
                try
                {
                    string fileName = String.Format("Log_{0:yyyyMMdd}.log", DateTime.Now);
                    string fileFullPath = System.IO.Path.Combine(_path, fileName);
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileFullPath, true))
                    {
                        file.WriteLine(String.Format("{0:dd/MM/yyyy HH:mm:ss} - {1}", DateTime.Now, stringToLog));
                        file.Close();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
