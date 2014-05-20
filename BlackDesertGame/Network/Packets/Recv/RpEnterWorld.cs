using BlackDesertGame.Network.Packets.Send;

namespace BlackDesertGame.Network.Packets.Recv
{
    class RpEnterWorld : ARecvPacket
    {
        public override void Read()
        {
            
        }

        public override void Process()
        {
            new SpEnterWorldResponse().Send(Connection);
          
            new SpUnk0E90().Send(Connection);
            new SpCharacterInfo().Send(Connection, 1);
            new SpUnk0BF0().Send(Connection, 1);
            new SpUnk0BFD_1().Send(Connection);
            new SpUnk0BFD_2().Send(Connection);
        }
    }
}
