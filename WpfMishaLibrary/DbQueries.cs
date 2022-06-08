using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMishaLibrary.DbEntities;

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
            $"(ExpenditureName, FactExpenditureCategory, Sum, Date, CardName) " +
            $"VALUES " +
            $"(@{nameof(FactExpenditure.ExpenditureName)}, @{nameof(FactExpenditure.FactExpenditureCategory)}, @{nameof(FactExpenditure.Sum)}, @{nameof(FactExpenditure.Date)}, @{nameof(FactExpenditure.CardName)});";
        public static readonly string getFactExpenditureQuery =
            $"SELECT * FROM FactExpenditure;";
        #endregion
    }
}
