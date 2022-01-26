using System;

namespace CreatingGameCharacter.Item
{
    class Bow : AbstractItem
    {
        public Bow()
        {
            this.name = "활";
        }
        public override void attack() => Console.WriteLine("휙 휘이익!");

        public override void Introduce() => Console.WriteLine("저는 궁수입니다. 활쏘기 / 민첩함");
    }
}
