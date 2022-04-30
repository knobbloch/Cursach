using System;
using Microsoft.Data.Sqlite;

namespace WpfMishaLibrary
{
    public class DbHandler
    {
        public SqliteConnection Connection { get; private set; }
        public DbHandler(string DbPath)
        {
            Connection = new SqliteConnection(DbPath);
        }
    }
}
