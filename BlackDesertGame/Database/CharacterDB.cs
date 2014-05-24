using System;
using System.Collections.Generic;
using System.Linq;
using BDCommon.Database;
using BDCommon.Structures.Player;
using BDCommon.Utils;

namespace BlackDesertGame.Database
{
    class CharacterDataBase : BaseDBReader
    {
        const string FileDb = "Players.cache";

        public static Dictionary<int,List<Player>> LoadAll()
        {
            Dictionary<int, List<Player>> players = DeserializeFile<Dictionary<int, List<Player>>>(FileDb);
            if (players != null)
            {
                int maxID = players.Select(a => a.Value.Max(p => p.PlayerId)).Max();
                GUIDGenerator.SetGUID(maxID);
            }
            return players ?? new Dictionary<int, List<Player>>();
        }

        public static void SaveAll(Dictionary<int, List<Player>> players)
        {
            SerializeFile(FileDb, players);
        }
    }
}
