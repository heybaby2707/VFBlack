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
using System.Collections.Generic;
using BDCommon.Database;
using BDCommon.Structures.Player;

namespace BlackDesertLogin.Database
{
    /*
     * @autor Maxes727
     * */
    class AccountDB : BaseDBReader
    {
        const string FileDB = "Accounts.cache";
        public static Dictionary<int, AccountData> LoadAll()
        {
            Dictionary<int, AccountData> accounts = null;
            accounts = DeserializeFile<Dictionary<int, AccountData>>(FileDB) ?? new Dictionary<int, AccountData>();
            return accounts;
        }

        public static void SaveAll(Dictionary<int, AccountData> accounts)
        {
            SerializeFile(FileDB, accounts);
        }
    }
}
