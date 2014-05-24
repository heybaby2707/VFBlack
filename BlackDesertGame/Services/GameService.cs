using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BDCommon.Structures.Player;
using BDCommon.Utils;
using BlackDesertGame.Database;
using BlackDesertGame.Network.Packets.Send;
using BlackDesertGame.Network.Protocol;
using NLog;

namespace BlackDesertGame.Services
{
    /*
     * @author Karyzir
     * 
     */
    public class GameService
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public static Dictionary<int, List<Player>> Players = new Dictionary<int, List<Player>>();
        private static readonly object _playersLock = new object();

        public static Thread DbSaveThread;
        public static int SaveInterval = 600;
        private static int _lastSaveUtc = Funcs.GetRoundedUtc();

        static GameService()
        {
            DbSaveThread = new Thread(() =>
            {
                while (GameServer.IsWorked)
                {
                    if (Funcs.GetRoundedUtc() - _lastSaveUtc > SaveInterval)
                    {
                        SaveCharacters();
                        _lastSaveUtc = Funcs.GetRoundedUtc();
                    }
                    Thread.Sleep(1000);
                }
            });
            DbSaveThread.Start();
        }
        /// <summary>
        /// Initilize service
        /// </summary>
        public static void Init()
        {
            Players = CharacterDataBase.LoadAll();
            Log.Info("Loaded characters: {0}", Players.Count);
        }
        /// <summary>
        /// Save all accounts from server cache
        /// </summary>
        private static void SaveCharacters()
        {
            Log.Info("Periodic save. Characters count: {0}", Players.Count);
            lock (_playersLock)
                CharacterDataBase.SaveAll(Players);
        }

        public static void CreateNewCharacter(Connection connection, CharacterData playerData)
        {
            //TODO
            var cached = new Player
            {
                AccountId = connection.AccountInfo.Id,
                CharacterData = playerData
            };
            lock (_playersLock)
            {
                if (!Players.ContainsKey(connection.AccountInfo.Id))
                {
                    Players.Add(connection.AccountInfo.Id, new List<Player>());
                }

                cached.PlayerId = GUIDGenerator.NextGUID();
                cached.Level = 0;
                Players[connection.AccountInfo.Id].Add(cached);
                connection.Players.Add(cached);
            }
            new SpCreateCharacter(cached,connection).Send(connection, 1);
            Log.Info("New character created!");
        }

        public static void SendCharacterList(Connection connection)
        {
            new SpCharacterList(connection.Players).Send(connection, 1);
        }

        public static void EnterInWorld(Connection connection, int characterId)
        {
            //Jesus... TODO
            foreach (var player in connection.Players.Where(s => s.PlayerId == characterId))
            {
                new SpEnterWorldResponse().Send(connection);
                new SpUnk0E90().Send(connection);
                new SpCharacterInfo(player).Send(connection, 1);
                new SpUnk0Bf0().Send(connection, 1);
                new SpUnk0BFD_1().Send(connection);
                new SpUnk0BFD_2().Send(connection);

                connection.CurrentPlayer = player;
            }
        }
    }
}
