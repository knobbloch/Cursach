using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.AddBankAccountPageView;
using WpfMishaLibrary;

namespace WpfLibrary
{
    class PCard
    {
        static IModel m;
        static IAddBankAccountPageView cardd;
        public PCard(IAddBankAccountPageView card, WpfMishaLibrary.IModel model)
        {
            card.ExitAction += ExitCard;
            card.AddAction += AddCard;
            m = model;
            cardd = card;
            Update();
        }

        public static void Update()
        {
            ((IAddBankAccountPageView)cardd).DispatchUpdate(view => {
                IAddBankAccountPageView cardView = (IAddBankAccountPageView)view;
                cardView.AccountName = "";
                cardView.CurrencyAmount = "0";
                cardView.CurrencyAmountError = null;
                cardView.AccountNameError = null;
            });
        }

        public void ExitCard(object source, EventArgs a)
        {
            Update();
        }

        public void AddCard(object source, EventArgs a)
        {
            ((IAddBankAccountPageView)source).DispatchUpdate(view => {
                IAddBankAccountPageView cardView = (IAddBankAccountPageView)view;
                if (cardView.AccountName == "")
                {
                    cardView.AccountNameError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.EmptyValue, "");
                    return;
                }
                try
                {
                    ModelEditDataResultStates.ReturnCardState ret = m.AddCard(cardView.AccountName, double.Parse(cardView.CurrencyAmount));
                    if (ret == ModelEditDataResultStates.ReturnCardState.ErrorTypeNameConstraint)
                        cardView.AccountNameError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.DuplicateValue, "");
                    else
                        Update();
                }
                catch
                {
                    cardView.CurrencyAmountError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.OutOfBoundsValue, "");
                }
            });    
        }
    }
}
