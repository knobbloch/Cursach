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
        // Path to the local database: "UserData.db"
        public string DbPath { get; private set; }
        // Path string with specific format: $"Data Source= UserData.db;"
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
        #region AddMethods
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
        /// Adds FactExpenditure record to the database
        /// </summary>
        /// <param name="expenditureName">User's label of the record</param>
        /// <param name="factExpenditureCategory">Category from category list</param>
        /// <param name="sum"></param>
        /// <param name="date">Date of the expenditure</param>
        /// <param name="cardName">Card from card list</param>
        /// <returns>
        /// Success: success!;
        /// ErrorTypeSumConstraint: sum less than 0.
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
            return numOfRows > 0 ? ModelEditDataResultStates.ReturnFactExpenditureState.Success : ModelEditDataResultStates.ReturnFactExpenditureState.ErrorTypeSumConstraint;
        }
        /// <summary>
        /// Adds FactIncome record to the database
        /// </summary>
        /// <param name="incomeName">User's label of the record</param>
        /// <param name="factIncomeCategory">Category from category list</param>
        /// <param name="sum"></param>
        /// <param name="date">Date of the expenditure</param>
        /// <param name="cardName">Card from card list</param>
        /// <returns>
        /// Success: success!;
        /// ErrorTypeSumConstraint: sum less than 0.
        /// </returns>
        public ModelEditDataResultStates.ReturnFactIncomeState AddFactIncome(string incomeName, string factIncomeCategory, double sum, DateTime date, string cardName)
        {
            var factIncomeObject = new FactIncome
            {
                FactIncomeName = incomeName,
                FactIncomeCategory = factIncomeCategory,
                Sum = sum,
                // DateTime to long.
                Date = date.Ticks,
                CardName = cardName
            };
            // Num of rows that were affected
            int numOfRows;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                numOfRows = dbConnection.Execute(DbQueries.insertFactIncomeQuery, factIncomeObject);
            }
            return numOfRows > 0 ? ModelEditDataResultStates.ReturnFactIncomeState.Success : ModelEditDataResultStates.ReturnFactIncomeState.ErrorTypeSumConstraint;
        }
        public ModelEditDataResultStates.ReturnPlanExpenditureState AddPlanExpenditure(string expenditureCategory, double sum, DateTime beginDate, DateTime endDate, string planExpenditureImagePath)
        {
            // Creating PlanExpenditure object
            var planExpenditureObject = new PlanExpenditure
            {
                ExpenditureCategory = expenditureCategory,
                Sum = sum,
                BeginDate = beginDate.Ticks,
                EndDate = endDate.Ticks,
                PlanExpenditureImagePath = planExpenditureImagePath
            };
            throw new NotImplementedException("В разработке");
        }
        #endregion
        #region ReturnMethods
        /// <summary>
        /// Returns list of card objects from database
        /// </summary>
        /// <returns>List typeof(Card) if Card table isn't empty. Else returns empty list.</returns>
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
        /// Returns list of factExpenditures objects from database
        /// </summary>
        /// <returns>List typeof(FactExpenditure) if factExpenditure table isn't empty. Else returns empty list.</returns>
        public List<FactExpenditure> GetFactExpenditures()
        {
            List<FactExpenditure> factExpenditures;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                factExpenditures = dbConnection.Query<FactExpenditure>(DbQueries.getFactExpenditureQuery).ToList();
            }
            // Converting from ticks to DateTime format
            foreach (var elem in factExpenditures)
                elem.DateDateTime = new DateTime(elem.Date);
            return factExpenditures;
        }
        /// <summary>
        /// Returns list of FactIncomes objects from database
        /// </summary>
        /// <returns> 
        /// List typeof(FactIncome) if FactIncome table isn't empty. Else returns empty list.
        /// </returns>
        public List<FactIncome> GetFactIncomes()
        {
            List<FactIncome> factIncomes;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                factIncomes = dbConnection.Query<FactIncome>(DbQueries.getFactIncomeQuery).ToList();
            }
            // Converting from ticks to DateTime format
            foreach (var elem in factIncomes)
                elem.DateDateTime = new DateTime(elem.Date);
            return factIncomes;
        }
        #endregion
        public bool AddPlanIncome(string incomeCategory, double sum, DateTime beginDate, DateTime endDate, string planIncomeImagePath)
        {
            throw new NotImplementedException();
        }
        #region PrivateMethods
        private List<T> ConvertObjectDate<T>(List<T> objList)
            where T : Record, IEnumerable<T>
        {
            foreach (var elem in objList)
                elem.DateDateTime = new DateTime(elem.Date);
            return objList;
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
        /// Checks whether PlanExpenditure(expenditure categoty) exists in database.
        /// </summary>
        /// <param name="cardObject">PlanExpenditure to find.</param>
        /// <returns>True, if PlanExpenditure is in the database. Else false.</returns>
        private bool CheckPlanExpenditureIfExists(PlanExpenditure planExpenditureObject)
        {
            int numOfRows;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                var planExpenditures = dbConnection.Query<PlanExpenditure>(DbQueries.checkPlanExpenditureQuery, planExpenditureObject);
                numOfRows = planExpenditures.Count();
            }
            return (numOfRows > 0);
        }
        #endregion
    }
}
