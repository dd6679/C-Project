using System;
using System.IO;

namespace Logger
{
    class Logger
    {
        public virtual void Trace(string msg) { }
        protected string Format(string msg)
        {
            return (DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss.fff] ") + msg);
        }
    }

    class FileLogger : Logger
    {
        private string _logPath = string.Empty; 
        public FileLogger(string path)
        {
            _logPath = path;
        }
        public override void Trace(string msg)
        {
            using (var fs = new FileStream(_logPath, FileMode.Append, FileAccess.Write))
            {
                using (var sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(Format(msg));
                    sw.Flush();
                }
            }
        }
    }

    class ConsoleLogger : Logger
    {
        public override void Trace(string msg) => Console.WriteLine(Format(msg));
    }

    class Program
    {
        static string ShowMenu(string msg)
        {
            Console.WriteLine(msg);
            return Console.ReadLine();
        }

        static Logger CreateLogger(string menu, string option = "")
        {
            if (menu == "파일")
            {
                return new FileLogger(option);
            }

            return new ConsoleLogger();
        }

        static void Main(string[] args)
        {
            string option = string.Empty;
            string msg = ShowMenu("버전을 선택하세요(콘솔/파일): ");
            if (msg == "파일")
            {
                option = ShowMenu("저장할 경로를 입력하세요. : ");
            }

            if (msg == "exit")
            {
                return;
            }

            Logger logger = CreateLogger(msg, option);
            while (true)
            {
                msg = ShowMenu("메시지를 입력하세요 : ");
                if (msg == "exit")
                {
                    return;
                }

                logger.Trace(msg);
            }
        }
    }
}
