using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barista
{
    public class App
    {
        public string ShowMenu(string s)
        {
            Console.Clear();
            Console.WriteLine(s);
            return Console.ReadLine();
        }
        public string ShowAddtionalMenu(string msg)
        {
            Console.Write(msg);
            return Console.ReadLine();
        }
    }
}
