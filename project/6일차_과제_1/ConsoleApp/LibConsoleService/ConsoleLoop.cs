using System;
using System.Collections.Generic;

namespace ConsoleApp
{
    class ConsoleLoop
    {
        const string exitCode = "exit";
        public delegate void ProcesHandler(object sender, string menu);

        public void Run(object sender = null)
        {
            var handlers = new Dictionary<string, ProcesHandler>();
            RegisterSubMenu(handlers);

            var selected = string.Empty;
            do
            {
                if (handlers.ContainsKey(selected))
                {
                    handlers[selected](sender, selected);
                }

                selected = ShowMenu(MenuString);


            } while (!IsExit(selected));
        }

        string ShowMenu(string msg)
        {
            Console.Clear();
            Console.Write(msg);
            return Console.ReadLine();
        }

        protected virtual bool IsExit(string selected)
        {
            return selected == exitCode;
        }
        protected virtual string MenuString
        {
            get { return $"{exitCode}를 누르면 종료합니다."; }
        }

        protected virtual void RegisterSubMenu(Dictionary<string, ProcesHandler> handlers)
        {
        }
    }
}
