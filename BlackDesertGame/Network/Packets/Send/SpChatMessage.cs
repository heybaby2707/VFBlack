using System.IO;
using BDCommon.Structures.Player;
using BlackDesertGame.Handlers;
using BlackDesertGame.Network.Protocol;
using NLog.Config;

namespace BlackDesertGame.Network.Packets.Send
{
    /* *
     * @author Maxes727
     */

    public class SpChatMessage : ASendPacket
    {
        protected ChatMessage Msg;
        protected byte Type = 3;
        protected short SubType = 4;
        protected string Name = "xTz";

        public SpChatMessage(ChatMessage msg, byte type = 3, short subtype = 4, string name = "xTz")
        {
            Msg = msg;
            Type = type;
            SubType = subtype;
            Name = name;
        }

        public override void Write(BinaryWriter writer)
        {
            string data = "0101001C9200780054007A"
            + "00000000000404005C1C004D00650072"
            + "006300790000000CD580B70000000000"
            + "0000000000DD0200000000A7557CC1FD"
            + "7F0000100000000000";

            Player player = ((Connection)Msg.Sender).CurrentPlayer;

            WriteC(writer, Type); //0x03
            WriteH(writer, SubType); //0x04

            WriteC(writer, 0x1C);
            WriteH(writer, 0x92);

            //WriteS(writer, player.CharacterData.Name);
            WriteS(writer, Name);

            WriteH(writer, 0x00);
            WriteC(writer, 0x04);

            WriteH(writer, 0x04);
            WriteC(writer, 0x5C);
            WriteH(writer, 0x1C);

            WriteS(writer, "Mercy");

            WriteB(writer, "0CD580B700000000000000000000DD0200000000A7557CC1FD7F0000100000000000");
            
            //WriteB(writer, data);
            WriteSs(writer, Msg.Message);
        }
    }
}
