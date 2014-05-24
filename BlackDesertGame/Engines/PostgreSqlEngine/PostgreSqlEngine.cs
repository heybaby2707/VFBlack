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
/* *
 * @author karyzir aka Tanatos
 */
using System;
using System.Collections.Generic;
using System.Data;
using NLog;
using Npgsql;

namespace BlackDesertGame.Engines.PostgreSqlEngine
{
    class PostgreSqlEngine : IDisposable
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
        protected object Lock = new object();
        public NpgsqlConnection ActiveConnection;

        public void Initilize(string server, string user, string password, int port = 5432, string database = "template1")
        {          
            try
            {
                ActiveConnection = new NpgsqlConnection(new NpgsqlConnectionStringBuilder(string.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};",
                    server,port,user,password,database)));
                Log.Info("[{0}] Successfully installed!", GetType().Name);
            }
            catch (NpgsqlException ex)
            {
                Log.WarnException("PostgreSql", ex);        
            }
        }
        private enum SchemaTableRows
        {
            ColumnName = 0,
            ColumnOrdinal = 1,
            ColumnSize = 2,
            NumericPrecision = 3,
            NumericScale = 4,
            IsUnique = 5,
            IsKey = 6,
            BaseCatalogName = 7,
            BaseColumnName = 8,
            BaseSchemaName = 9,
            BaseTableName = 10,
            DataType = 11,
            AllowDbNull = 12,
            ProviderType = 13,
            IsAliased = 14,
            IsExpression = 15,
            IsIdentity = 16,
            IsAutoIncrement = 17,
            IsRowVersion = 18,
            IsHidden = 19,
            IsLong = 20,
            IsReadOnly = 21,
        }

        public void Dispose()
        {
            lock (Lock)
            {
                Log.Info("PostgreSqlConnector: Close connection...");
                if (ActiveConnection.State != ConnectionState.Closed)
                    ActiveConnection.Close();
            }
        }
        public int Execute(string sql)
        {
            int result = 0;
            lock (Lock)
            {
                try
                {
                    result = new NpgsqlCommand(sql, ActiveConnection).ExecuteNonQuery();
                }
                catch (NpgsqlException ex)
                {
                    Log.WarnException("PostgreSql", ex);
                }
            }
            return result;
        }
        public Object Single(string sql)
        {
            Object result = null;
            lock (Lock)
            {
                try
                {
                    result = new NpgsqlCommand(sql, ActiveConnection).ExecuteScalar();
                }
                catch (NpgsqlException ex)
                {
                    Log.WarnException("PostgreSql", ex);
                }
            }
            return result;
        }
        public Dictionary<string, object> Select(string sql)
        {
            Dictionary<string, object> values = null;
            lock (Lock)
            {
                try
                {
                    NpgsqlDataReader dataReader = new NpgsqlCommand(sql, ActiveConnection).ExecuteReader();
                    if (dataReader.Read())
                        values = GetValues(dataReader);
                    if (dataReader.Read())
                        Log.Info("PostgreSql select \"{1}\" not single!", sql);
                    dataReader.Close();
                }
                catch (NpgsqlException ex)
                {
                    Log.WarnException("PostgreSql", ex);
                }
            }
            return values;
        }
        public List<Dictionary<string, object>> SelectAll(string sql)
        {
            var allValues = new List<Dictionary<string, object>>();
            lock (Lock)
            {
                try
                {
                    NpgsqlDataReader dataReader = new NpgsqlCommand(sql, ActiveConnection).ExecuteReader();
                    DataTable schemaTable = null;
                    while (dataReader.Read())
                        allValues.Add(GetValues(dataReader, schemaTable));
                    dataReader.Close();
                }
                catch (NpgsqlException ex)
                {
                    Log.WarnException("PostgreSql", ex);
                }
            }

            return allValues;
        }
        private static Dictionary<string, object> GetValues(NpgsqlDataReader dataReader, DataTable schemaTable = null)
        {
            var values = new Dictionary<string, object>();
            if (schemaTable == null)
                schemaTable = dataReader.GetSchemaTable();
            for (int i = 0; i < schemaTable.Rows.Count; i++)
            {
                DataRow row = schemaTable.Rows[i];
                if ((Type)row[(int)SchemaTableRows.DataType] == typeof(Boolean))
                    values.Add((string)row[(int)SchemaTableRows.ColumnName], dataReader.GetByte(i));
                else
                    values.Add((string)row[(int)SchemaTableRows.ColumnName],
                               (bool)row[(int)SchemaTableRows.AllowDbNull] && dataReader.IsDBNull(i) ? null : Convert.ChangeType(dataReader.GetValue(i), (Type)row[(int)SchemaTableRows.DataType]));
            }
            return values;
        }
    }
  
}
