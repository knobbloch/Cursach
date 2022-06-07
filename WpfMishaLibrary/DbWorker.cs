using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Data;
using System.Data.SQLite;
using WpfMishaLibrary.DbEntities;

namespace WpfMishaLibrary
{
    ///<summary>
    /// Creates connection with the local SQLite database and excecutes given query
    ///</summary>
    public class DbWorker: IModel
    {
        public string DbPath { get; private set; }
        public string DbConnectionString { get; private set; }

        /// <summary>
        /// Creates instance of DbWorker.
        /// Connects to exsisting DB or creates new one at the location of the calling project.
        /// </summary>
        /// <param name="dbPath">Relative path to Db "UserData.db"</param>
        public DbWorker(string dbPath = "UserData.db")
        {
            DbPath = dbPath;
            DbConnectionString = String.Format($"Data Source= {dbPath};");
            DbInitializer initializer = new (dbPath, DbConnectionString);
            // Checks if database and its' tables exist
            initializer.Initialize(this);
        }
        /// <summary>
        /// Adds Card to the database
        /// </summary>
        /// <param name="cardName">Name of card</param>
        /// <param name="balance">Balance of the card</param>
        /// <returns>
        /// Success: success!;
        /// ErrorTypeNameConstraint: card with the same name is already in database; 
        /// ErrorTypeUnrecognized: uncaught error;
        /// </returns>
        public ModelEditDataResultStates.ReturnCardState AddCard(string cardName, double balance)
        {
            // Creating card object.
            var cardObject = new Card
            {
                CardName = cardName
            };
            // If card exists in table => returns ConstraintError type, else tries to add
            if (!CheckCardIfExists(cardObject))
            {
                // Num of rows that where affected
                int numOfRows;
                using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
                {
                    numOfRows = dbConnection.Execute(DbQueries.insertCardQuery, cardObject);
                }
                // If inserted a row
                if (numOfRows > 0)
                    return ModelEditDataResultStates.ReturnCardState.Success;
                // If not
                else
                    return ModelEditDataResultStates.ReturnCardState.ErrorTypeUnrecognized;
            }
            // If the same name is in the database
            else
                return ModelEditDataResultStates.ReturnCardState.ErrorTypeNameConstraint;
        }
        private bool CheckCardIfExists(Card cardObject)
        {
            int numOfRows;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                var cards = dbConnection.Query<Card>(DbQueries.checkCardQuery, cardObject);
                numOfRows = cards.Count();
            }
            return (numOfRows > 0);
        }

        public bool AddFactExpenditure(string expenditureName, string factExpenditureCategory, double sum, DateTime date, string cardName)
        {
            throw new NotImplementedException();
        }

        public bool AddFactIncome(string incomeName, string factIncomeCategory, double sum, DateTime date, string cardName)
        {
            throw new NotImplementedException();
        }

        public bool AddPlanExpenditure(string expenditureCategory, double sum, DateTime beginDate, DateTime endDate, string planExpenditureImagePath)
        {
            throw new NotImplementedException();
        }

        public bool AddPlanIncome(string incomeCategory, double sum, DateTime beginDate, DateTime endDate, string planIncomeImagePath)
        {
            throw new NotImplementedException();
        }
    }
}
