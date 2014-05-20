/*
 * This file is part of black desert-emu <http://necroz-project.net/>.
 *  
 * black desert-emu is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * black desert-emu is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *  
 * You should have received a copy of the GNU General Public License
 * along with black desert-emu. If not, see <http://www.gnu.org/licenses/>.
 */

using BlackDesertGame.Engines;
using BlackDesertGame.Network.Protocol;
using BlackDesertGame.Network.Packets.Send;
using BlackDesertGame.Network.Packets;
namespace BlackDesertGame.Handlers
{
    class ChatHandler
    {
        public static void SendMessage(Connection con, string msg, SendMessageType sendType = SendMessageType.BROADCAST)
        {
            SendMessage(new ChatMessage(con, msg, sendType));
        }

        public static void SendMessage(ChatMessage message)
        {
            if(!AdminEngine.ProccesActionCommand(message.Sender,message.Message))
            switch (message.SendType)
            {
                case SendMessageType.BROADCAST:
                    PacketHandler.BroadcastPacket(new SpChatMessage(message));
                    break;
                case SendMessageType.PRIVATE:
                    PacketHandler.SendPacket(message.Sender, new SpChatMessage(message));
                    break;
            }
            
        }
    }

    class ChatMessage
    {
        public Connection Sender { get; private set; }
        public string Message { get; private set; }

        public SendMessageType SendType { get; private set; }

        public ChatMessage(Connection sender, string msg, SendMessageType sendType = SendMessageType.BROADCAST)
        {
            Sender = sender;
            Message = msg;
            SendType = sendType;
        }
    }

    enum SendMessageType
    {
        PRIVATE,
        BROADCAST
    }
}
