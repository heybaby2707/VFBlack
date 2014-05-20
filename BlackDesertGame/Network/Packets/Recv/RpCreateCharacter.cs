using BDCommon.Structures.Creature.Player;

namespace BlackDesertGame.Network.Packets.Recv
{
    class RpCreateCharacter : ARecvPacket
    {
        protected Zodiac Zodiac;
        public override void Read()
        {
            Race race = (Race) ReadC(); //0x4
            ReadC(); //0x2

            string name = ReadS(); //Character name

            int zeroOrnot = 58 - name.Length * 2;

            byte[] zerosOrUnk = ReadB(zeroOrnot);

            ReadH();//0
            Zodiac = (Zodiac) ReadC();//11
            ReadH();//0
            ReadD();//1
        }

        public override void Process()
        {
           
        }
    }
}
