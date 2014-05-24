using BlackDesertGame.Services;

namespace BlackDesertGame.Network.Packets.Recv
{
    class RpEnterWorld : ARecvPacket
    {
        protected int CharacterId;
        protected int AccountId;

        public override void Read()
        {
            CharacterId = ReadD();
            AccountId = ReadD();
        }

        public override void Process()
        {
            GameService.EnterInWorld(Connection, CharacterId);
        }
    }
}
