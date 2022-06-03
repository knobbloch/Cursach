using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace WpfMishaLibrary
{
    ///<summary>
    /// Creates connection with the local SQLite database and excecutes given query
    ///</summary>
    public class DbWorker
    {
        private string DbPath { get; set; }
        /// <summary>
        /// Creates instance of DbWorker.
        /// Connects to exsisting DB or creates new one at the location of the calling project.
        /// </summary>
        /// <param name="dbPath">Path to Db</param>
        public DbWorker(string dbPath = @"Data Source= UserData.db;")
        {
            DbPath = dbPath;
            // Checks if database and its' tables exist
            DbInitializer initializer = new (dbPath);
            initializer.Initialize(this);
            //TODO: check if path is correct, maybe test connection
        }
        /// <summary>
        /// Excecutes the query, returns count of rows deleted, updated or inserted. 
        /// For SELECT command returns -1.
        /// Returns 0 in case of exception.
        /// </summary>
        /// <param name="commandText">SQL query to excecute</param>
        /// <returns></returns>
        public string ExecuteCommand(string commandText)
        {
            using (var connection = new SqliteConnection(DbPath))
            {
                connection.Open();
                SqliteCommand command = new(commandText, connection);
                try
                {
                    var a = command.ExecuteReader();
                    return a.GetChar(0).ToString();
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine(ex.Message);
                    return "0";
                }
            }
        }
    }
}
