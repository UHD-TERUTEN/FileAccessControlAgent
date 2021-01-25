using Dapper;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace FileAccessControlAgent.Helpers
{
    public static class DBManager
    {
        public static void Init()
        {
            if (!File.Exists(dbName))
                SQLiteConnection.CreateFile(dbName);
        }

        public static int Execute(this string sql)
        {
            using (var conn = new SQLiteConnection(connString))
            {
                conn.Open();
                var command = new SQLiteCommand(sql, conn);
                var result = command.ExecuteNonQuery();

                command.Dispose();
                conn.Close();
                return result;
            }
        }

        public static List<T> Read<T>(this string sql)
        {
            var result = new List<T>();

            using (var conn = new SQLiteConnection(connString))
            {
                conn.Open();
                var command = new SQLiteCommand(sql, conn);
                var reader = command.ExecuteReader();
                var parser = reader.GetRowParser<T>(typeof(T));

                while (reader.Read())
                    result.Add(parser(reader));

                reader.Close();
                command.Dispose();
                conn.Close();
            }
            return result;
        }

        private static readonly string dbName = "test.sqlite";
        
        private static readonly string connString = $"Data Source={dbName};Version=3";
    }
}
