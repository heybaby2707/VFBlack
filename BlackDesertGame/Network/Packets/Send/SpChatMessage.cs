using System.IO;
using BlackDesertGame.Handlers;

namespace BlackDesertGame.Network.Packets.Send
{
    /* *
     * @author Maxes727
     */

    class SpChatMessage : ASendPacket
    {
        protected ChatMessage Msg;

        public SpChatMessage(ChatMessage msg)
        {
            Msg = msg;
        }

        public override void Write(BinaryWriter writer)
        {
            string data = "0304001C9200780054007A"
            + "00000000000404005C1C004D00650072"
            + "006300790000000CD580B70000000000"
            + "0000000000DD0200000000A7557CC1FD"
            + "7F0000100000000000";

            
            /*
            WriteB(writer, "0304001C9200"); //unk
            WriteS(writer, "MyName"); //Character name
            WriteB(writer, "00000404005C1C00");
            WriteS(writer, "Mercy"); //unk
            WriteB(writer, "0CD580B700000000000000000000DD0200000000A7557CC1FD7F0000100000000000");
            */

            WriteB(writer, data);
            WriteSS(writer, Msg.Message);
        }
    }
}
