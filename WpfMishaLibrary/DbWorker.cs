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
            // Creating Card object.
            var cardObject = new Card
            {
                CardName = cardName,
                Balance = balance
            };
            // If card exists in table => returns ConstraintError type, else tries to add
            if (!CheckCardIfExists(cardObject))
            {
                // Num of rows that were affected
                int numOfRows;
                using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
                {
                    numOfRows = dbConnection.Execute(DbQueries.insertCardQuery, cardObject);
                }
                // If inserted a row
                return numOfRows > 0 ? ModelEditDataResultStates.ReturnCardState.Success : ModelEditDataResultStates.ReturnCardState.ErrorTypeNameConstraint;
            }
            // If the same name is in the database
            else
                return ModelEditDataResultStates.ReturnCardState.ErrorTypeNameConstraint;
        }
        /// <summary>
        /// Checks whether card exists in database.
        /// </summary>
        /// <param name="cardObject">Card to find.</param>
        /// <returns>True, if card is in the database. Else false.</returns>
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
        /// <summary>
        /// Returns list of card objects from database
        /// </summary>
        /// <returns>List<Card> if Card table isn't empty. Else returns null. </card></returns>
        public List<Card> GetCards()
        {
            List<Card> cards;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                cards = dbConnection.Query<Card>(DbQueries.getCardQuery).ToList();
            }
            return cards;
        }
        /// <summary>
        /// Adds FactExpenditure record to the database
        /// </summary>
        /// <param name="expenditureName">User's label of the record</param>
        /// <param name="factExpenditureCategory">Category from category list</param>
        /// <param name="sum"></param>
        /// <param name="date">Date of the expenditure</param>
        /// <param name="cardName">Card from card list</param>
        /// <returns>
        /// Success: success!;
        /// ErrorTypeNameConstraint: some constraint in database raised an error.
        /// </returns>
        public ModelEditDataResultStates.ReturnFactExpenditureState AddFactExpenditure(string expenditureName, string factExpenditureCategory, double sum, DateTime date, string cardName)
        {
            // Creating FactExpenditure object.
            // CONVERTING DATETIME DATE TO TICKS, SO TYPEOF(DATE) = LONG
            var factExpenditureObject = new FactExpenditure
            {
                ExpenditureName = expenditureName,
                FactExpenditureCategory = factExpenditureCategory,
                Sum = sum,
                // DateTime to long.
                Date = date.Ticks,
                CardName = cardName
            };
            // Num of rows that were affected
            int numOfRows;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                numOfRows = dbConnection.Execute(DbQueries.insertFactExpenditureQuery, factExpenditureObject);
            }
            return numOfRows > 0 ? ModelEditDataResultStates.ReturnFactExpenditureState.Success : ModelEditDataResultStates.ReturnFactExpenditureState.ErrorTypeNameConstraint;
        }
        /// <summary>
        /// Returns list of factExpenditures objects from database
        /// </summary>
        /// <returns>List<FactExpenditure> if factExpenditure table isn't empty. Else returns null. </card></returns>
        public List<FactExpenditure> GetFactExpenditures()
        {
            List<FactExpenditure> factExpenditures;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                factExpenditures = dbConnection.Query<FactExpenditure>(DbQueries.getFactExpenditureQuery).ToList();
            }
            return factExpenditures;
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
