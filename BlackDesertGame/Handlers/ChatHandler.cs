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

using BDCommon.Network;
using BlackDesertGame.Network.Protocol;
using BlackDesertGame.Network.Packets.Send;
using BlackDesertGame.Network.Packets;
namespace BlackDesertGame.Handlers
{
    public class ChatHandler
    {
        public delegate bool ChatHandlerDelegate(ChatMessage msg);

        public static event ChatHandlerDelegate OnSendMessage;


        public static void SendMessage(Connection con, string msg, SendMessageType sendType = SendMessageType.BROADCAST)
        {
            SendMessage(new ChatMessage(con, msg, sendType));
        }

        public static void SendMessage(ChatMessage message)
        {
            //if(!AdminEngine.ProccesActionCommand(message.Sender,message.Message))
            if (!OnSendMessage(message))
            switch (message.SendType)
            {
                case SendMessageType.BROADCAST:
                    PacketHandler.BroadcastPacket(new SpChatMessage(message));
                    break;
                case SendMessageType.PRIVATE:
                    PacketHandler.SendPacket((Connection)message.Sender, new SpChatMessage(message));
                    break;
            }
        }
    }

    public class ChatMessage
    {
        public IConnection Sender { get; private set; }
        public string Message { get; private set; }

        public SendMessageType SendType { get; private set; }

        public ChatMessage(IConnection sender, string msg, SendMessageType sendType = SendMessageType.BROADCAST)
        {
            Sender = sender;
            Message = msg;
            SendType = sendType;
        }
    }

    public enum MessageType : byte
    {
        //TODO
        //Need check all!!!
        SYSTEM = 1, 
        SHOUT = 2,
        GENERAL = 3,
        WHISPER = 4,
        SYSTEM2 = 5, 
        GUILD = 6, 
        GROUP = 7 
    }

    public enum SendMessageType
    {
        PRIVATE,
        BROADCAST
    }
}
