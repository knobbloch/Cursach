using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.AddBankAccountPageView;
using WpfMishaLibrary;

namespace WpfLibrary
{
    class Card
    {
        static string nameCard = null;
        static double sumCard = 0;
        IModel m;
        public Card(IAddBankAccountPageView card, WpfMishaLibrary.IModel model)
        {
            card.ExitAction += ExitCard;
            card.AddAction += AddCard;
            m = model;
        }

        public void ExitCard(object source, EventArgs a)
        {
            //((IAddBankAccountPageView)source).DispatchUpdate(view => { IAddBankAccountPageView cardView = (IAddBankAccountPageView)view;  });
        }

        public void AddCard(object source, EventArgs a)
        {
            IAddBankAccountPageView c = ((IAddBankAccountPageView)source);
            m.AddCard(c.AccountName, decimal.ToDouble(c.CurrencyAmount));
        }
    }
}
