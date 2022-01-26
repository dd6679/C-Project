using System;


namespace CreatingGameCharacter.Item
{
    class Sword : AbstractItem
    {
        public Sword()
        {
            this.name = "도검";
        }
        public override void attack() => Console.WriteLine("칭 칭 칭");

        public override void Introduce() => Console.WriteLine("저는 검사입니다. 검술 / 힘");

    }
}
