using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.AddIncomePageView;
using WpfMishaLibrary;

namespace WpfLibrary
{
    public class PAddIncome
    {
        static IModel m;
        static IAddIncomePageView addIncome;
        static Dictionary<ApplicationProjectViews.CategoryDescriptor, WpfMishaLibrary.VisibleEntities.PlanIncomeVisible> categoryDict = new Dictionary<ApplicationProjectViews.CategoryDescriptor, WpfMishaLibrary.VisibleEntities.PlanIncomeVisible>();
        static Dictionary<ApplicationProjectViews.BankAccountInfo, WpfMishaLibrary.VisibleEntities.CardVisible> cardDict = new Dictionary<ApplicationProjectViews.BankAccountInfo, WpfMishaLibrary.VisibleEntities.CardVisible>();

        public PAddIncome(IAddIncomePageView addIncomePage, IModel model)
        {
            addIncomePage.AddAction += AddButton;
            addIncomePage.SelectedDateChanged += Update;
            addIncomePage.ShowPreview += Update;
            m = model;
            addIncome = addIncomePage;
            addIncomePage.SelectedDate = new DateTime(2022, 6, 20);
            Update();
        }

        static public void Update()
        {
            ((IAddIncomePageView)addIncome).DispatchUpdate(view =>
            {
                IAddIncomePageView addIncomee = (IAddIncomePageView)view;
                addIncomee.BankAccounts.Clear();
                addIncomee.IncomeCategories.Clear();
                addIncomee.IncomeName = "";
                addIncomee.SelectedBankAccount = null;
                addIncomee.SelectedIncomeCategory = null;
                addIncomee.CurrencyAmount = "0";
                addIncomee.CurrencyAmountError = null;
                addIncomee.IncomeNameError = null;
                addIncomee.SelectedIncomeCategoryError = null;
                addIncomee.SelectedBankAccountError = null;
            });
            FillCards();
            FillCategories();
        }

        public static void FillCards()
        {
            ((IAddIncomePageView)addIncome).DispatchUpdate(view =>
            {
                IAddIncomePageView addIncomePage = (IAddIncomePageView)view;
                List<WpfMishaLibrary.VisibleEntities.CardVisible> list = m.GetCards();
                cardDict.Clear();
                addIncome.BankAccounts.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    ApplicationProjectViews.BankAccountInfo tmp = new ApplicationProjectViews.BankAccountInfo() { AccountBalance = Convert.ToDecimal(list[i].Balance), AccountName = list[i].CardName };
                    addIncome.BankAccounts.Add(tmp);
                    cardDict.Add(tmp, list[i]);
                }
            });
        }

        public static void FillCategories()
        {
            ((IAddIncomePageView)addIncome).DispatchUpdate(view =>
            {
                IAddIncomePageView addIncomePage = (IAddIncomePageView)view;
                List<WpfMishaLibrary.VisibleEntities.PlanIncomeVisible> list = m.GetPlanIncomesSelectedDay (addIncomePage.SelectedDate);
                categoryDict.Clear();
                addIncome.IncomeCategories.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    ApplicationProjectViews.CategoryDescriptor tmp = new ApplicationProjectViews.CategoryDescriptor() { DisplayName = list[i].IncomeCategory, ImagePath = list[i].PlanIncomeImagePath };
                    addIncome.IncomeCategories.Add(tmp);
                    categoryDict.Add(tmp, list[i]);
                }
            });
        }

        public void Update(object source, EventArgs a)
        {
            Update();
        }

        public void AddButton(object source, EventArgs args)
        {
            ((IAddIncomePageView)source).DispatchUpdate(view => {
                IAddIncomePageView local = (IAddIncomePageView)view;

                WpfMishaLibrary.VisibleEntities.PlanIncomeVisible category = null;
                WpfMishaLibrary.VisibleEntities.CardVisible card = null;
                try
                {
                    categoryDict.TryGetValue(local.SelectedIncomeCategory, out category);
                    local.SelectedIncomeCategoryError = null;
                }
                catch { local.SelectedIncomeCategoryError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.EmptyValue, ""); return; }

                try
                {
                    cardDict.TryGetValue(local.SelectedBankAccount, out card);
                    local.SelectedBankAccountError = null;
                }
                catch { local.SelectedBankAccountError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.EmptyValue, ""); return; }

                if ((local.IncomeName).Trim() == "")
                {
                    local.IncomeNameError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.EmptyValue, "");
                    return;
                }
                else
                    local.IncomeNameError = null;

                try
                {
                    ModelEditDataResultStates.ReturnFactIncomeState ret = m.AddFactIncome (local.IncomeName, double.Parse(local.CurrencyAmount), local.SelectedDate, category, card);
                    if (ret == ModelEditDataResultStates.ReturnFactIncomeState.ErrorTypeSumConstraint)
                        local.CurrencyAmountError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.OutOfBoundsValue, ">=0");
                    else
                    {
                        local.CurrencyAmountError = null;
                        PAnalysis.Update();
                        PPlan.Update();
                        PInterPage.Update();
                    }
                }
                catch
                {
                    local.CurrencyAmountError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.InvalidSymbol, "");
                }
            });
        }
    }
}

