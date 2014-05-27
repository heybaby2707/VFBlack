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

using System;
using BDCommon.EngineInterfaces;
using BDCommon.Network;
using BDCommon.Scripts.Chat;
using BlackDesertGame.Handlers;
using BlackDesertGame.Network.Packets.Send;
using NLog;


namespace BlackDesertGame.Scripts.Chat
{
    /* *
    * @author karyzir
    */
    [AdminCommand("test", 10)]
    class CmdTest : IAdminEngine
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        //Example Command
        public void ProcessAction(IConnection connection, string message)
        {
            try
            {
                string[] splited = message.Split(':');
                byte type = Convert.ToByte(splited[0]);
                short subType = Convert.ToInt16(splited[1]);
                string name = splited[2];
                string msg = splited[3];

                new SpChatMessage(new ChatMessage(connection, msg), type, subType, name).Send(connection);
            }
            catch (Exception)
            {
            }
        }
    }
}
