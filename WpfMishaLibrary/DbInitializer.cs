using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace WpfMishaLibrary
{
    /// <summary>
    /// Checks if .db file is created. Checks databes' tables
    /// </summary>
    internal class DbInitializer
    {
        // Dictionary for initializing tables of the database
        public Dictionary<string, string> tableCreateCommands = new()
        {
            { "Card", "CREATE TABLE 'Card' ( 'Id'	INTEGER NOT NULL UNIQUE, 'CardName'	TEXT NOT NULL DEFAULT 'No name' UNIQUE, 'Balance'	REAL DEFAULT 0, PRIMARY KEY('Id' AUTOINCREMENT) )" },
            { "FactExpenditure", "CREATE TABLE 'FactExpenditure' ( 'FactExpenditureId'	INTEGER NOT NULL UNIQUE, 'ExpenditureName'	TEXT, 'FactExpenditureCategory'	TEXT NOT NULL DEFAULT 'No category', 'Sum'	REAL DEFAULT 0 CHECK('Sum' >= 0), 'Date'	INTEGER NOT NULL, 'CardName'	TEXT NOT NULL DEFAULT 'No card', PRIMARY KEY('FactExpenditureId' AUTOINCREMENT), FOREIGN KEY('FactExpenditureCategory') REFERENCES 'PlanExpenditure'('ExpenditureCategory') ON DELETE SET DEFAULT, FOREIGN KEY('CardName') REFERENCES 'Card'('CardName') ON DELETE SET DEFAULT )" },
            { "FactIncome", "CREATE TABLE 'FactIncome' ( 'FactIncomeId'	INTEGER NOT NULL UNIQUE, 'FactIncomeName'	TEXT, 'FactIncomeCategory'	TEXT NOT NULL DEFAULT 'No category', 'Sum'	REAL DEFAULT 0 CHECK('Sum' >= 0), 'Date'	INTEGER NOT NULL, 'CardName'	TEXT NOT NULL DEFAULT 'No card', FOREIGN KEY('CardName') REFERENCES 'Card'('CardName') ON DELETE SET DEFAULT, FOREIGN KEY('FactIncomeCategory') REFERENCES 'PlanIncome'('IncomeCategory') ON DELETE SET DEFAULT, PRIMARY KEY('FactIncomeId' AUTOINCREMENT) )" },
            { "PlanExpenditure", "CREATE TABLE 'PlanExpenditure' ( 'PlanExId'	INTEGER NOT NULL UNIQUE, 'ExpenditureCategory'	TEXT NOT NULL DEFAULT 'No category' UNIQUE, 'Sum'	REAL DEFAULT 0 CHECK('Sum' >= 0), 'BeginDate'	INTEGER NOT NULL, 'EndDate'	INTEGER NOT NULL, 'PlanExpenditureImagePath'	TEXT, PRIMARY KEY('PlanExId' AUTOINCREMENT) )" },
            { "PlanIncome", "CREATE TABLE 'PlanIncome' ( 'PlanInId'	INTEGER NOT NULL UNIQUE, 'IncomeCategory'	TEXT NOT NULL DEFAULT 'No category' UNIQUE, 'Sum'	REAL DEFAULT 0 CHECK('Sum' >= 0), 'BeginDate'	INTEGER NOT NULL, 'EndDate'	INTEGER NOT NULL, 'PlanIncomeImagePath'	TEXT, PRIMARY KEY('PlanInId' AUTOINCREMENT) )" }
        };
        public string DbPath { get; private set; }
        public string DbConnectionString { get; private set; }
        public DbInitializer(string dbPath, string dbConnectionString)
        {
            DbConnectionString = dbConnectionString;
            DbPath = dbPath;
        }
        public void Initialize(DbWorker dbWorker) 
        {
            try
            {
                // Gets the directoy of the caller class, not directory of the library!
                string path = Directory.GetCurrentDirectory();
                if (!File.Exists(dbWorker.DbPath))
                {
                    var fileStream = File.Create(dbWorker.DbPath);
                    // Close stream after creating file so we can open it right after creating.
                    fileStream.Close();
                } 
                // Checking tables or creating new
                CheckIfTablesExist();
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        private void CheckIfTablesExist()
        {
            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();
                // As much iterations as number of tables
                foreach (string tableName in tableCreateCommands.Keys)
                {
                    // Check expression. Returns table name if exists
                    string sqlExpression = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";
                    SqliteCommand command = new SqliteCommand(sqlExpression, connection);
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        // If table already exists
                        if (reader.HasRows) 
                        {
                            // Read data by line
                            while (reader.Read())
                            {
                                var log = reader.GetValue(0);
                            }
                        }
                        // If not - creates table
                        else
                        {
                            string sqlCreateTableExpression = tableCreateCommands[tableName];
                            SqliteCommand createTableComand = new(sqlCreateTableExpression, connection);
                            // For checking the success of the operation
                            var result = createTableComand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}
