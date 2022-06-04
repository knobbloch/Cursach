using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace WpfMishaLibrary
{
    class DbInitializer
    {
        // 1. Check file if exists
        // 2. Check tables if created
        // Dictionary for initializing tables of the database
        public Dictionary<string, string> tableCreateCommands = new()
        {
            { "Card", "CREATE TABLE 'Card' ( 'Id' INTEGER NOT NULL UNIQUE, 'Balance'	REAL DEFAULT 0, 'Name' TEXT NOT NULL, PRIMARY KEY('Id' AUTOINCREMENT) )" },
            { "ExpenditureCategory", "CREATE TABLE IF NOT EXISTS 'ExpenditureCategory' ( 'Id' INTEGER NOT NULL UNIQUE, 'Name' VARCHAR NOT NULL, PRIMARY KEY('Id' AUTOINCREMENT) )" },
            { "FactExpenditure", "CREATE TABLE 'FactExpenditure' ( 'FactExpenditureId'	INTEGER NOT NULL UNIQUE, 'Date'	TEXT NOT NULL, 'Sum'	REAL DEFAULT 0 CHECK('Sum' >= 0), 'ExpenditureCategoryCategoryId'	INTEGER NOT NULL DEFAULT 1, 'CardId'	INTEGER DEFAULT 1, PRIMARY KEY('FactExpenditureId' AUTOINCREMENT), FOREIGN KEY('CardId') REFERENCES 'Card'('Id') ON DELETE SET DEFAULT )" },
            { "FactIncome", "CREATE TABLE 'FactIncome' ( 'FactIncomeId'	INTEGER NOT NULL UNIQUE, 'Date'	TEXT NOT NULL, 'Sum'	REAL DEFAULT 0 CHECK('Sum' >= 0), 'IncomeCategoryId'	INTEGER NOT NULL DEFAULT 1, 'CardId'	INTEGER DEFAULT 1, PRIMARY KEY('FactIncomeId' AUTOINCREMENT), FOREIGN KEY('CardId') REFERENCES 'Card'('Id') ON DELETE SET DEFAULT )" },
            { "IncomeCategory", "CREATE TABLE 'IncomeCategory' ( 'Id'	INTEGER NOT NULL UNIQUE, 'Name'	VARCHAR NOT NULL, PRIMARY KEY('Id' AUTOINCREMENT) )" },
            { "PlanExpenditure", "CREATE TABLE 'PlanExpenditure' ( 'PlanExId'	INTEGER NOT NULL UNIQUE, 'BeginDate'	TEXT NOT NULL, 'EndDate'	TEXT NOT NULL, 'Sum'	REAL DEFAULT 0 CHECK('Sum'>=0), 'CategoryId'	INTEGER NOT NULL DEFAULT 1, PRIMARY KEY('PlanExId' AUTOINCREMENT), FOREIGN KEY('CategoryId') REFERENCES 'ExpenditureCategory'('Id') ON DELETE SET DEFAULT )" },
            { "PlanIncome", "CREATE TABLE 'PlanIncome' ( 'PlanInId'	INTEGER NOT NULL UNIQUE, 'BeginDate'	TEXT NOT NULL, 'EndDate'	TEXT NOT NULL, 'Sum'	REAL CHECK('Sum'>=0), 'CategoryId'	INTEGER NOT NULL DEFAULT 1, PRIMARY KEY('PlanInId' AUTOINCREMENT), FOREIGN KEY('CategoryId') REFERENCES 'IncomeCategory'('Id') ON DELETE SET DEFAULT )" }
        };
        public string DbPath { get; private set; }
        public DbInitializer(string dbPath) => DbPath = dbPath;
        public void Initialize(DbWorker dbWorker) 
        {
            // SELECT name FROM sqlite_master WHERE type='table' AND name='table_name';
            // tableCreateCommands.Keys
            // Check db file
            try
            {
                string path = Directory.GetCurrentDirectory();
                if (!File.Exists("UserData.db")) 
                    File.Create("UserData.db");
                CheckIfTablesExist();
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
        
        private void CheckIfTablesExist()
        {
            using (var connection = new SqliteConnection(DbPath))
            {
                connection.Open();
                // As much iterations as number of tables
                foreach (string tableName in tableCreateCommands.Keys)
                {
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
                                var res = reader.GetValue(0);
                                Console.WriteLine(res);
                            }
                        }
                        // If not - creates table
                        else
                        {
                            string sqlCreateTableExpression = tableCreateCommands[tableName];
                            SqliteCommand createTableComand = new(sqlCreateTableExpression, connection);
                            var result = createTableComand.ExecuteNonQuery();
                            Console.WriteLine(result);
                        }
                    }
                }
            }
        }
    }
}
