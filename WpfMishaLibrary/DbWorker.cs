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
        /// <summary>
        /// Holds connection during whole session
        /// </summary>
        private SqliteConnection Connection { get; set; }
        /// <summary>
        /// Creates instance of DbWorker and connects to db
        /// </summary>
        /// <param name="DbPath">Connects to the database specified by path or assigns new</param>
        public DbWorker(string DbPath = "Data Source=UserData.db")
        {
            Connection = new SqliteConnection(DbPath);
            Connection.Open();
        }
        /// <summary>
        /// Excecutes the query, returns count of rows deleted, updated or inserted. 
        /// For SELECT command returns -1.
        /// Returns 0 in case of exception.
        /// </summary>
        /// <param name="commandText">SQL query to excecute</param>
        /// <returns></returns>
        public int ExecuteCommand(string commandText)
        {
            SqliteCommand command = new(commandText, Connection);
            try
            {
                return command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }
        
    }
}
