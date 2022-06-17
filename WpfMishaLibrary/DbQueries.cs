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
        
        public static readonly string getCardQuery = 
            $"SELECT * FROM Card;";

        #endregion
        
        #region FactExpenditure
        
        public static readonly string insertFactExpenditureQuery = 
            $"INSERT INTO FactExpenditure " +
            $"(ExpenditureName, FactExpenditureCategory, FactExpenditureCategoryId, Sum, Date, CardName) " +
            $"VALUES " +
            $"(@{nameof(FactExpenditure.ExpenditureName)}, @{nameof(FactExpenditure.FactExpenditureCategory)}, @{nameof(FactExpenditure.FactExpenditureCategoryId)}, @{nameof(FactExpenditure.Sum)}, @{nameof(FactExpenditure.Date)}, @{nameof(FactExpenditure.CardName)});";
        
        public static readonly string getFactExpenditureQuery =
            $"SELECT * FROM FactExpenditure;";

        #endregion

        #region FactIncome

        public static readonly string insertFactIncomeQuery =
            $"INSERT INTO FactIncome " +
            $"(FactIncomeName, FactIncomeCategory, FactIncomeCategoryId, Sum, Date, CardName) " +
            $"VALUES " +
            $"(@{nameof(FactIncome.FactIncomeName)}, @{nameof(FactIncome.FactIncomeCategory)}, @{nameof(FactIncome.FactIncomeCategoryId)}, @{nameof(FactIncome.Sum)}, @{nameof(FactIncome.Date)}, @{nameof(FactIncome.CardName)});";
        
        public static readonly string getFactIncomeQuery =
            $"SELECT * FROM FactIncome;";
        
        #endregion

        #region PlanExpenditure
        public static readonly string checkPlanExpenditureQuery =
            $"SELECT * FROM PlanExpenditure" +
            $" WHERE ExpenditureCategory = @{nameof(PlanExpenditure.ExpenditureCategory)};";

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
            $" WHERE IncomeCategory = @{nameof(PlanIncome.IncomeCategory)};";

        public static readonly string insertPlanIncomeQuery =
            $"INSERT INTO PlanIncome " +
            $"(IncomeCategory, Sum, BeginDate, EndDate, PlanIncomeImagePath) " +
            $"VALUES " +
            $"(@{nameof(PlanIncome.IncomeCategory)}, @{nameof(PlanIncome.Sum)}, @{nameof(PlanIncome.BeginDate)}, @{nameof(PlanIncome.EndDate)}, @{nameof(PlanIncome.PlanIncomeImagePath)});";

        public static readonly string getPlanIncomeQuery =
            $"SELECT * FROM PlanIncome;";

        #endregion
    }
}
