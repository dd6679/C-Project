using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
	class Base
	{
		public virtual void Trace(string s)
		{
		}
	}

	class Log : Base
	{
		public override void Trace(string s)
		{
			Console.WriteLine(DateTime.Now + " " + s);
		}
	}

	class File : Base
	{
		public override void Trace(string s)
		{
			Console.Write("파일 경로 : ");
			string path = Console.ReadLine();
			Console.Write("파일 이름(txt) : ");
			string name = Console.ReadLine();
			StreamWriter sw = new StreamWriter(path + "\\" + name);
			sw.Write(s);
			sw.Close();
			sw.Dispose();
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			string ver = "", m = "";

			while (true)
            {
				Console.Clear();

				Console.WriteLine("exit. 종료");

				Console.Write("버전을 선택하세요(콘솔/파일) : ");
				ver = Console.ReadLine();

				if (ver == "exit")
					break;

				Console.Write("메시지를 입력하세요 : ");
				m = Console.ReadLine();

				if (m == "exit")
					break;

				if (ver == "콘솔")
				{
					Base c = new Log();
					c.Trace(m);
				}
				else if (ver == "파일")
				{
					Base f = new File();
					f.Trace(m);
				}

				Console.ReadLine();
            }
        }
    }
}
