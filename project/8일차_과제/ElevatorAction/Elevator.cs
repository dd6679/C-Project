using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ElevatorAction
{
    class Elevator
    {
        private int floor;
        public bool open;
        List<string> floor_list = new List<string>();

        public Elevator()
        {
            floor = 1;
            open = false;
            Console.WriteLine($"엘리베이터가 {floor}층에 있습니다.");
        }

        // 문 닫힘
        public void CloseDoor()
        {
            Thread.Sleep(2000);
            Console.WriteLine($"문이 닫힙니다.");
            Thread.Sleep(2000);
        }

        // 문 열림
        public void OpenDoor()
        {
            Console.WriteLine($"{floor}층에 도착했습니다.");
            Console.WriteLine($"문이 열립니다.");
        }

        // 매개변수 리스트에 추가하여 호출층 처리
        public void CallFloor(string call_floor)
        {
            try
            {
                floor_list.Add(call_floor);
                floor_list = floor_list.Distinct().ToList();

                foreach (string i in floor_list)
                {
                    if (int.Parse(i) == floor)
                    {
                        OpenDoor();
                        CloseDoor();
                        floor_list.Remove(i);
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public void Run(int move_floor, string call_floor)
        {
            // 6 이상의 이동층 입력 시
            if (move_floor > 5)
            {
                Console.WriteLine("건물은 5층까지 있습니다.");
                return;
            }
            // 1층에서 2초 간격으로 이동층까지 업데이트
            while (floor != move_floor)
            {
                Console.WriteLine(floor);
                CallFloor(call_floor);
                Thread.Sleep(2000);
                if (floor < move_floor)
                    floor++;
            }
            OpenDoor();
            floor = 1;
        }
    }
}
