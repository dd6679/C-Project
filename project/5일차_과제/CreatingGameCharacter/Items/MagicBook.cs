using System;

namespace CreatingGameCharacter.Item
{
    class MagicBook : AbstractItem
    {
        public MagicBook()
        {
            this.name = "마법책";
        }
        public override void attack() => Console.WriteLine("~주문 외는 소리~");

        public override void Introduce() => Console.WriteLine("저는 마법사입니다. 마법술 / 소환술");

    }
}
