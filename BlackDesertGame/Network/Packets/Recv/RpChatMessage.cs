using BlackDesertGame.Handlers;

namespace BlackDesertGame.Network.Packets.Recv
{
    /* *
     * @author Maxes727
     */

    class RpChatMessage : ARecvPacket
    {
        protected string Msg;
        public override void Read()
        {
            ReadC(); //unk
            ReadC(); //unk
            ReadH(); //Length
            Msg = ReadS(); //Message
        }

        public override void Process()
        {
            ChatHandler.SendMessage(Connection, Msg);
        }
    }
}