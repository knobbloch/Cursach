using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMishaLibrary.DbEntities;
using WpfMishaLibrary.VisibleEntities;

namespace WpfMishaLibrary
{
    public static class DbQueries
    {
        #region Card
        
        public static readonly string checkCardQuery = 
            $"SELECT * FROM Card" +
            $" WHERE CardName = @{nameof(Card.CardName)};";

        public static readonly string insertCardQuery = 
            $"INSERT INTO Card " +
            $"(CardName, Balance) " +
            $"VALUES (@{nameof(Card.CardName)}, @{nameof(Card.Balance)});";
        
        public static readonly string getCardsQuery = 
            $"SELECT * FROM Card;";

        public static readonly string updateCardBalance =
            $"UPDATE Card SET Balance = @Balance WHERE Id = @Id;";
        #endregion

        #region FactExpenditure

        public static readonly string insertFactExpenditureQuery = 
            $"INSERT INTO FactExpenditure " +
            $"(ExpenditureName, FactExpenditureCategoryId, Sum, Date, CardId) " +
            $"VALUES " +
            $"(@{nameof(FactExpenditure.ExpenditureName)}, @{nameof(FactExpenditure.FactExpenditureCategoryId)}, @{nameof(FactExpenditure.Sum)}, @{nameof(FactExpenditure.Date)}, @{nameof(FactExpenditure.CardId)});";
        
        public static readonly string getFactExpenditureQuery =
            $"SELECT * FROM FactExpenditure;";

        #endregion

        #region FactIncome

        public static readonly string insertFactIncomeQuery =
            $"INSERT INTO FactIncome " +
            $"(FactIncomeName, FactIncomeCategoryId, Sum, Date, CardId) " +
            $"VALUES " +
            $"(@{nameof(FactIncome.FactIncomeName)}, @{nameof(FactIncome.FactIncomeCategoryId)}, @{nameof(FactIncome.Sum)}, @{nameof(FactIncome.Date)}, @{nameof(FactIncome.CardId)});";
        
        public static readonly string getFactIncomeQuery =
            $"SELECT * FROM FactIncome;";
        
        #endregion

        #region PlanExpenditure
        public static readonly string checkPlanExpenditureQuery =
            $"SELECT * FROM PlanExpenditure" +
            $" WHERE ExpenditureCategory = @{nameof(PlanExpenditure.ExpenditureCategory)} AND BeginDate = @{nameof(PlanExpenditure.BeginDate)} AND EndDate = @{nameof(PlanExpenditure.EndDate)};";

        public static readonly string insertPlanExpenditureQuery =
            $"INSERT INTO PlanExpenditure " +
            $"(ExpenditureCategory, Sum, BeginDate, EndDate, PlanExpenditureImagePath) " +
            $"VALUES " +
            $"(@{nameof(PlanExpenditure.ExpenditureCategory)}, @{nameof(PlanExpenditure.Sum)}, @{nameof(PlanExpenditure.BeginDate)}, @{nameof(PlanExpenditure.EndDate)}, @{nameof(PlanExpenditure.PlanExpenditureImagePath)});";

        public static readonly string getPlanExpenditureQuery =
            $"SELECT * FROM PlanExpenditure;";

        #endregion

        #region PlanIncome

        public static readonly string checkPlanIncomeQuery =
            $"SELECT * FROM PlanIncome" +
            $" WHERE IncomeCategory = @{nameof(PlanIncome.IncomeCategory)} AND BeginDate = @{nameof(PlanIncome.BeginDate)} AND EndDate = @{nameof(PlanIncome.EndDate)};";

        public static readonly string insertPlanIncomeQuery =
            $"INSERT INTO PlanIncome " +
            $"(IncomeCategory, Sum, BeginDate, EndDate, PlanIncomeImagePath) " +
            $"VALUES " +
            $"(@{nameof(PlanIncome.IncomeCategory)}, @{nameof(PlanIncome.Sum)}, @{nameof(PlanIncome.BeginDate)}, @{nameof(PlanIncome.EndDate)}, @{nameof(PlanIncome.PlanIncomeImagePath)});";

        public static readonly string getPlanIncomeQuery =
            $"SELECT * FROM PlanIncome;";

        #endregion

        #region WHERE queries

        // Date comes from method params
        public static readonly string getPlanExpendituresSelectedDay =
            $"SELECT * " +
            $"FROM PlanExpenditure " +
            $"WHERE BeginDate <= @date AND EndDate >= @date;";

        // Date comes from method params
        public static readonly string getPlanIncomesSelectedDay =
            $"SELECT * " +
            $"FROM PlanIncome " +
            $"WHERE BeginDate <= @date AND EndDate >= @date;";

        // beginDate and endDate come from method params
        public static readonly string getPlanExpendituresDiapason =
            $"SELECT * " +
            $"FROM PlanExpenditure " +
            $"WHERE BeginDate = @beginDate AND EndDate = @endDate;";

        // beginDate and endDate come from method params
        public static readonly string getPlanIncomesDiapason =
            $"SELECT * " +
            $"FROM PlanIncome " +
            $"WHERE BeginDate = @beginDate AND EndDate = @endDate;";

        // beginDate and endDate come from method params
        public static readonly string getFactExpendituresDiapason =
            $"SELECT * " +
            $"FROM FactExpenditure " +
            $"WHERE Date >= @beginDate AND Date <= @endDate;";

        // beginDate and endDate come from method params
        public static readonly string getFactIncomesDiapason =
            $"SELECT * " +
            $"FROM FactIncome " +
            $"WHERE Date >= @beginDate AND Date <= @endDate;";

        #endregion

        #region Special queries

        public static readonly string clearTableQuery = "DELETE FROM @TableNaeme";

        public static readonly string getPlanIncomeById =
            $"SELECT * " +
            $"FROM PlanIncome " +
            $"WHERE PlanIncomeId = @Id;";

        public static readonly string getPlanExpenditureById =
            $"SELECT * " +
            $"FROM PlanExpenditure " +
            $"WHERE PlanExpenditureId = @Id;";

        #endregion
    }
}
