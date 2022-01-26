using System;
using System.IO;

namespace Logger
{
    class Base
    {
        public virtual void Trace(string s)
        {
        }

        public string Format(string s)
        {
            return (DateTime.Now.ToString("[yyyy/MM/dd HH:mm:ss.fff] ") + s);
        }
    }

    class Log : Base
    {
        public override void Trace(string s)
        {
            Console.WriteLine(Format(s));
        }
    }

    class File : Base
    {
        public string path;

        public File(string path)
        {
            this.path = path;
        }
        public override void Trace(string s)
        {
            FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
            sw.WriteLine(Format(s));
            sw.Flush();
        }
    }

    class Program
    {
        static string ShowMenu(string s)
        {
            Console.WriteLine(s);
            return Console.ReadLine();
        }

        static Base Create(string ver, string option = "")
        {
            if (ver == "파일")
                return new File(option);
            return new Log();
        }
        static void Main(string[] args)
        {
            string ver = "", m = "", option = "";

            Console.Clear();

            Console.WriteLine("exit. 종료");

            ver = ShowMenu("버전을 선택하세요(콘솔/파일) : ");

            if (ver == "exit")
                return;

            if (ver == "파일")
                option = ShowMenu("저장 경로 : ");


            Base b = Create(ver, option);

            while (true)
            {
                m = ShowMenu("메시지를 입력하세요 : ");
                if (m == "exit")
                    return;
                b.Trace(m);
            }

            Console.ReadLine();

        }
    }
}
