using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.InterPageView;
using ApplicationProjectViews.AddBankAccountPageView;
using WpfMishaLibrary;

namespace WpfLibrary
{
    class PInterPage
    {
        static IModel m;
        static IInterPageView interPage;
        public PInterPage(IInterPageView inter, WpfMishaLibrary.IModel model, IAddBankAccountPageView card)
        {
            card.AddAction += DisplayCards;
            m = model;
            interPage = inter;
            DisplayCards(inter, new EventArgs());
        }

        public void DisplayCards(object source, EventArgs a)
        {
            interPage.DispatchUpdate(view => {
                IInterPageView interView = (IInterPageView)view;
                List<WpfMishaLibrary.VisibleEntities.CardVisible> list = m.GetCards();
                int b = list.Count;
                interView.BankAccounts.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    interView.BankAccounts.Add(new ApplicationProjectViews.BankAccountInfo() { AccountBalance = Convert.ToDecimal(list[i].Balance), AccountName = list[i].CardName });
                }
            });
        }
    }
}
