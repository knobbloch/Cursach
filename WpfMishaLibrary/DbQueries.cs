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
        public static readonly string checkCardQuery = $"SELECT * FROM Card WHERE CardName = @{nameof(Card.CardName)};";
        public static readonly string insertCardQuery = $"INSERT INTO Card (CardName) VALUES (@{nameof(Card.CardName)});";
        //public static readonly string insertIncomeCategoryQuery = $"INSERT INTO PlanIncome (IncomeName) VALUES (@{nameof(FactIncome.IncomeName)});";
    }
}
