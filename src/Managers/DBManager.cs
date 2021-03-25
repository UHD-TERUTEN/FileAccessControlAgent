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

#if DEBUG
        public static int Execute(this string sql)
        {
            return Execute(sql, connString);
        }

        public static List<T> Read<T>(this string sql)
        {
            return Read<T>(sql);
        }

        public static int Execute(this string sql, string connString)
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

        public static List<T> Read<T>(this string sql, string connString)
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
#else
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
#endif

        private static readonly string dbName = "test.sqlite";
        
        private static readonly string connString = $"Data Source={dbName};Version=3";
    }
}
