using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using System.Data.SQLite;
using WpfMishaLibrary.DbEntities;
using WpfMishaLibrary.VisibleEntities;

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
        public ModelEditDataResultStates.ReturnFactExpenditureState AddFactExpenditure(string expenditureName, string factExpenditureCategory, double sum, DateTime date, string cardName, PlanExpenditureVisible planExpenditureVisible)
        {
            // Creating FactExpenditure object.
            // CONVERTING DATETIME DATE TO TICKS, SO TYPEOF(DATE) = LONG
            var factExpenditureObject = new FactExpenditure
            {
                ExpenditureName = expenditureName,
                FactExpenditureCategory = factExpenditureCategory,
                FactExpenditureCategoryId = ((PlanExpenditure)planExpenditureVisible).PlanExpenditureId,
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
        public ModelEditDataResultStates.ReturnFactIncomeState AddFactIncome(string incomeName, string factIncomeCategory, double sum, DateTime date, string cardName, PlanIncomeVisible planIncomeVisible)
        {
            var factIncomeObject = new FactIncome
            {
                FactIncomeName = incomeName,
                FactIncomeCategory = factIncomeCategory,
                // Cast object to unpack Id
                FactIncomeCategoryId = ((PlanIncome)planIncomeVisible).PlanIncomeId,
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
        /// <summary>
        /// Adds PlanExpenditure to the database
        /// </summary>
        /// <param name="expenditureCategory"> Category </param>
        /// <param name="sum"> Has to be more than 0 or equal 0 </param>
        /// <param name="beginDate"> Value of begin date is less than value of endDate</param>
        /// <param name="endDate"> Value of endDate is more than value of beginDate </param>
        /// <param name="planExpenditureImagePath"> Src to image's local path </param>
        /// <returns>
        /// Success : success!;
        /// ErrorTypeDateConstraint : DateOfBegin > DateOfEnd;
        /// ErrorTypeNameConstraint : PlanExpenditure name(category) is already in the table;
        /// ErrorTypeUnrecognized : call the tech guy....
        /// </returns>
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
            if (!CheckPlanExpenditureIfExists(planExpenditureObject))
            {
                if (!CheckDateBorder(beginDate, endDate))
                    return ModelEditDataResultStates.ReturnPlanExpenditureState.ErrorTypeDateConstraint;
                // Num of rows that were affected
                int numOfRows;
                using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
                {
                    numOfRows = dbConnection.Execute(DbQueries.insertPlanExpenditureQuery, planExpenditureObject);
                }
                return numOfRows > 0 ? ModelEditDataResultStates.ReturnPlanExpenditureState.Success : ModelEditDataResultStates.ReturnPlanExpenditureState.ErrorTypeUnrecognized;
            }
            else
                return ModelEditDataResultStates.ReturnPlanExpenditureState.ErrorTypeNameConstraint;
        }
        /// <summary>
        /// Adds PlanIncome to the database
        /// </summary>
        /// <param name="incomeCategory"> Category </param>
        /// <param name="sum"> Has to be more than 0 or equal 0 </param>
        /// <param name="beginDate"> Value of begin date is less than value of endDate</param>
        /// <param name="endDate"> Value of endDate is more than value of beginDate </param>
        /// <param name="planIncomeImagePath"> Src to image's local path </param>
        /// <returns>
        /// Success : success!;
        /// ErrorTypeDateConstraint : DateOfBegin > DateOfEnd;
        /// ErrorTypeNameConstraint : PlanIncome name(category) is already in the table;
        /// ErrorTypeUnrecognized : call the tech guy....
        /// </returns>
        public ModelEditDataResultStates.ReturnPlanIncomeState AddPlanIncome(string incomeCategory, double sum, DateTime beginDate, DateTime endDate, string planIncomeImagePath)
        {
            // Creating PlanIncome object
            var planIncomeObject = new PlanIncome
            {
                IncomeCategory = incomeCategory,
                Sum = sum,
                BeginDate = beginDate.Ticks,
                EndDate = endDate.Ticks,
                PlanIncomeImagePath = planIncomeImagePath
            };
            if (!CheckPlanIncomeIfExists(planIncomeObject))
            {
                if (!CheckDateBorder(beginDate, endDate))
                    return ModelEditDataResultStates.ReturnPlanIncomeState.ErrorTypeDateConstraint;
                // Num of rows that were affected
                int numOfRows;
                using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
                {
                    numOfRows = dbConnection.Execute(DbQueries.insertPlanIncomeQuery, planIncomeObject);
                }
                return numOfRows > 0 ? ModelEditDataResultStates.ReturnPlanIncomeState.Success : ModelEditDataResultStates.ReturnPlanIncomeState.ErrorTypeUnrecognized;
            }
            else
                return ModelEditDataResultStates.ReturnPlanIncomeState.ErrorTypeNameConstraint;
        }
        #endregion
        #region ReturnMethods
        /// <summary>
        /// Returns list of card objects from database
        /// </summary>
        /// <returns>List typeof(Card) if Card table isn't empty. Else returns empty list.</returns>
        public List<CardVisible> GetCards()
        {
            List<CardVisible> cards;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                cards = dbConnection.Query<Card>(DbQueries.getCardQuery).Select(x => x as CardVisible).ToList();
            }
            return cards;
        }
        /// <summary>
        /// Returns list of factExpenditures objects from database
        /// </summary>
        /// <returns>List typeof(FactExpenditure) if factExpenditure table isn't empty. Else returns empty list.</returns>
        public List<FactExpenditureVisible> GetFactExpenditures()
        {
            List<FactExpenditureVisible> factExpenditures;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                // Gets IEnumerable<FactExpenditure>
                // Transforms ticks to DateTime
                // UpCast to FactExpenditureVisible
                // Converting to List<FactExpenditureVisible>
                factExpenditures = dbConnection.Query<FactExpenditure>(DbQueries.getFactExpenditureQuery)
                    .Select(x => 
                    { 
                        x.DateDateTime =  new DateTime(x.Date);
                        return x as FactExpenditureVisible; 
                    }).ToList();
            }
            // Converting from ticks to DateTime format
            return factExpenditures;
        }
        /// <summary>
        /// Returns list of FactIncomes objects from database
        /// </summary>
        /// 
        /// <returns> 
        /// List typeof(FactIncome) if FactIncome table isn't empty. Else returns empty list.
        /// </returns>
        public List<FactIncomeVisible> GetFactIncomes()
        {
            List<FactIncomeVisible> factIncomes;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                // Gets IEnumerable<FactIncome>
                // Transforms ticks to DateTime
                // UpCast to FactIncomeVisible
                // Converting to List<FactIncomeVisible>
                factIncomes = dbConnection.Query<FactIncome>(DbQueries.getFactIncomeQuery)
                    .Select(x => 
                    { 
                        x.DateDateTime = new DateTime(x.Date);
                        return x as FactIncomeVisible;
                    }).ToList();
            }
            return factIncomes;
        }
        /// <summary>
        /// Returns list of PlanExpenditures objects from database
        /// </summary>
        /// 
        /// <returns> 
        /// List typeof(PlanExpenditures) if PlanExpenditure table isn't empty. Else returns empty list.
        /// </returns>
        public List<PlanExpenditureVisible> GetPlanExpenditures()
        {
            List<PlanExpenditureVisible> planExpenditures;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                // Gets IEnumerable<PlanExpenditure>
                // Transforms ticks to DateTime
                // UpCast to PlanExpenditureVisible
                // Converting to List<PlanExpenditureVisible>
                planExpenditures = dbConnection.Query<PlanExpenditure>(DbQueries.getPlanExpenditureQuery)
                    .Select(x => 
                    {
                        x.BeginDateDateTime = new DateTime(x.BeginDate);
                        x.EndDateDateTime = new DateTime(x.EndDate);
                        return x as PlanExpenditureVisible; 
                    }).ToList();
            }
            return planExpenditures;
        }
        /// <summary>
        /// Returns list of PlanIncomes objects from database
        /// </summary>
        /// 
        /// <returns> 
        /// List typeof(PlanIncomes) if PlanIncome table isn't empty. Else returns empty list.
        /// </returns>
        public List<PlanIncomeVisible> GetPlanIncomes()
        {
            List<PlanIncomeVisible> planIncomes;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                // Gets IEnumerable<PlanIncome>
                // Transforms ticks to DateTime
                // UpCast to PlanIncomeVisible
                // Converting to List<PlanIncomeVisible>
                planIncomes = dbConnection.Query<PlanIncome>(DbQueries.getPlanIncomeQuery)
                    .Select(x =>
                    {
                        x.BeginDateDateTime = new DateTime(x.BeginDate);
                        x.EndDateDateTime = new DateTime(x.EndDate);
                        return x as PlanIncomeVisible;
                    }).ToList();
            }
            return planIncomes;
        }
        public List<PlanExpenditureVisible> GetPlanExpendituresSelectedDay(DateTime day)
        {
            List<PlanExpenditureVisible> planExpenditures;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                // Gets IEnumerable<PlanExpenditure> specified by !date.Ticks! param
                // Transforms ticks to DateTime
                // UpCast to PlanExpenditureVisible
                // Converting to List<PlanExpenditureVisible>
                long date = day.Ticks;
                planExpenditures = dbConnection.Query<PlanExpenditure>(DbQueries.getPlanExpendituresSelectedDay,
                    param: new { date })
                    .Select(x =>
                    {
                        x.BeginDateDateTime = new DateTime(x.BeginDate);
                        x.EndDateDateTime = new DateTime(x.EndDate);
                        return x as PlanExpenditureVisible;
                    }).ToList();
            }
            return planExpenditures;
        }

        public List<PlanIncomeVisible> GetPlanIncomesSelectedDay(DateTime day)
        {
            List<PlanIncomeVisible> planIncomes;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                // Gets IEnumerable<PlanIncome> specified by !date.Ticks! param
                // Transforms ticks to DateTime
                // UpCast to PlanIncomeVisible
                // Converting to List<PlanIncomeVisible>
                long date = day.Ticks;
                planIncomes = dbConnection.Query<PlanIncome>(DbQueries.getPlanIncomesSelectedDay,
                    param: new { date })
                    .Select(x =>
                    {
                        x.BeginDateDateTime = new DateTime(x.BeginDate);
                        x.EndDateDateTime = new DateTime(x.EndDate);
                        return x as PlanIncomeVisible;
                    }).ToList();
            }
            return planIncomes;
        }
        public List<PlanExpenditureVisible> GetPlanExpendituresDiapason(DateTime start, DateTime end)
        {
            if (start > end)
                throw new Exception("Error: 'start' parameter can't be > than 'end' parameter");
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                // Gets IEnumerable<PlanExpenditure> specified by !date.Ticks! param
                // Transforms ticks to DateTime
                // UpCast to PlanExpenditureVisible
                // Converting to List<PlanExpenditureVisible>
                List<PlanExpenditureVisible> planExpenditures;
                long beginDate = start.Ticks;
                long endDate = end.Ticks;
                planExpenditures = dbConnection.Query<PlanExpenditure>(DbQueries.getPlanExpendituresDiapason,
                    param: new { beginDate, endDate })
                    .Select(x =>
                    {
                        x.BeginDateDateTime = new DateTime(x.BeginDate);
                        x.EndDateDateTime = new DateTime(x.EndDate);
                        return x as PlanExpenditureVisible;
                    }).ToList();
                return planExpenditures;
            }
        }

        public List<PlanIncomeVisible> GetPlanIncomesDiapason(DateTime start, DateTime end)
        {
            if (start > end)
                throw new Exception("Error: 'start' parameter can't be > than 'end' parameter");
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                // Gets IEnumerable<PlanExpenditure> specified by !date.Ticks! param
                // Transforms ticks to DateTime
                // UpCast to PlanExpenditureVisible
                // Converting to List<PlanExpenditureVisible>
                List<PlanIncomeVisible> planIncomes;
                long beginDate = start.Ticks;
                long endDate = end.Ticks;
                planIncomes = dbConnection.Query<PlanIncome>(DbQueries.getPlanIncomesDiapason,
                    param: new { beginDate, endDate })
                    .Select(x =>
                    {
                        x.BeginDateDateTime = new DateTime(x.BeginDate);
                        x.EndDateDateTime = new DateTime(x.EndDate);
                        return x as PlanIncomeVisible;
                    }).ToList();
                return planIncomes;
            }
        }
        public List<FactExpenditureVisible> GetFactExpendituresDiapason(DateTime start, DateTime end)
        {
            if (start > end)
                throw new Exception("Error: 'start' parameter can't be > than 'end' parameter");
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                // Gets IEnumerable<PlanExpenditure> specified by !date.Ticks! param
                // Transforms ticks to DateTime
                // UpCast to PlanExpenditureVisible
                // Converting to List<PlanExpenditureVisible>
                List<FactExpenditureVisible> factExpenditures;
                long beginDate = start.Ticks;
                long endDate = end.Ticks;
                factExpenditures = dbConnection.Query<FactExpenditure>(DbQueries.getFactExpendituresDiapason,
                    param: new { beginDate, endDate })
                    .Select(x =>
                    {
                        x.DateDateTime = new DateTime(x.Date);
                        return x as FactExpenditureVisible;
                    }).ToList();
                return factExpenditures;
            }
        }

        public List<FactIncomeVisible> GetFactIncomesDiapason(DateTime start, DateTime end)
        {
            if (start > end)
                throw new Exception("Error: 'start' parameter can't be > than 'end' parameter");
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                // Gets IEnumerable<PlanExpenditure> specified by !date.Ticks! param
                // Transforms ticks to DateTime
                // UpCast to PlanExpenditureVisible
                // Converting to List<PlanExpenditureVisible>
                List<FactIncomeVisible> factIncomes;
                long beginDate = start.Ticks;
                long endDate = end.Ticks;
                factIncomes = dbConnection.Query<FactIncome>(DbQueries.getFactIncomesDiapason,
                    param: new { beginDate, endDate })
                    .Select(x =>
                    {
                        x.DateDateTime = new DateTime(x.Date);
                        return x as FactIncomeVisible;
                    }).ToList();
                return factIncomes;
            }
        }

        #endregion
        #region PrivateMethods
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
        /// 
        /// <param name="planExpenditureObject">
        /// PlanExpenditure to find.
        /// </param>
        /// 
        /// <returns>
        /// True, if PlanExpenditure is in the database. Else false.
        /// </returns>
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
        /// <summary>
        /// Checks whether PlanIncome(income categoty) exists in database.
        /// </summary>
        /// 
        /// <param name="planIncomeObject">
        /// PlanIncome to find.
        /// </param>
        /// 
        /// <returns>
        /// True, if PlanIncome is in the database. Else false.
        /// </returns>
        private bool CheckPlanIncomeIfExists(PlanIncome planIncomeObject)
        {
            int numOfRows;
            using (IDbConnection dbConnection = new SQLiteConnection(DbConnectionString))
            {
                var planExpenditures = dbConnection.Query<PlanIncome>(DbQueries.checkPlanIncomeQuery, planIncomeObject);
                numOfRows = planExpenditures.Count();
            }
            return (numOfRows > 0);
        }
        /// <summary>
        /// t1 must be more than t2
        /// </summary>
        /// 
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// 
        /// <returns>
        /// True, if t1 less than t2. Else false.
        /// </returns>
        private bool CheckDateBorder(DateTime beginDate, DateTime endDate) => beginDate <= endDate;

        #endregion
    }
}
