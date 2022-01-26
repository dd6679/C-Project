using System;
using System.Threading;

namespace ElevatorAction
{

    class Program
    {
        public string move_floor, call_floor;
        static Elevator elevator;

        // 입출력
        static string ShowMenu(string msg)
        {
            Console.Write(msg);
            return Console.ReadLine();
        }

        static void Main(string[] args)
        {
            // 작업 시간 : 8시간
            elevator = new Elevator();

            while (true)
                new Program().DoAction();
        }

        // 
        void DoAction()
        {
            Input();

            try
            {
                int floor = int.Parse(move_floor);
                Thread televator = new Thread(() => elevator.Run(floor, call_floor));
                televator.Start();
                // 중간에 호출 들어오면 입력을 받아 메소드 호출
                elevator.CallFloor(Console.ReadLine());
            }
            catch (Exception)
            {
            }

            Console.ReadLine();
            Console.Clear();
        }
        // 호출층, 이동층 입력
        void Input()
        {
            call_floor = ShowMenu("엘리베이터 호출을 입력하세요 : ");

            if (call_floor != "")
            {
                move_floor = ShowMenu("이동할 층을 입력하세요 : ");
                elevator.CloseDoor();
            }
        }
    }
}
