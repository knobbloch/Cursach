using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.AddExpensePageView;
using WpfMishaLibrary;

namespace WpfLibrary
{
    public class PAddExpense
    {
        static IModel m;
        static IAddExpensePageView addExpense;
        static Dictionary<ApplicationProjectViews.CategoryDescriptor, WpfMishaLibrary.VisibleEntities.PlanExpenditureVisible> categoryDict = new Dictionary<ApplicationProjectViews.CategoryDescriptor, WpfMishaLibrary.VisibleEntities.PlanExpenditureVisible>();
        static Dictionary<ApplicationProjectViews.BankAccountInfo, WpfMishaLibrary.VisibleEntities.CardVisible> cardDict = new Dictionary<ApplicationProjectViews.BankAccountInfo, WpfMishaLibrary.VisibleEntities.CardVisible>();
        public PAddExpense(IAddExpensePageView addExpensePage, IModel model)
        {
            addExpensePage.AddAction += AddButton;
            addExpensePage.SelectedDateChanged += Update;
            addExpensePage.ShowPreview += Update;
            m = model;
            addExpense = addExpensePage;
            addExpensePage.SelectedDate = new DateTime(2022, 6, 20);
        }

        static public void Update()
        {
            ((IAddExpensePageView)addExpense).DispatchUpdate(view =>
            {
                IAddExpensePageView addExpensee = (IAddExpensePageView)view;
                addExpensee.BankAccounts.Clear();
                addExpensee.ExpenseCategories.Clear();
                addExpensee.ExpenseName = "";
                addExpensee.SelectedBankAccount = null;
                addExpensee.SelectedExpenseCategory = null;
                addExpensee.CurrencyAmount = "0";
                addExpensee.CurrencyAmountError = null;
                addExpensee.ExpenseNameError = null;
                addExpensee.SelectedExpenseCategoryError = null;
                addExpensee.SelectedBankAccountError = null;
            });
            FillCards();
            FillCategories();
        }

        public static void FillCards()
        {
            ((IAddExpensePageView)addExpense).DispatchUpdate(view =>
            {
                IAddExpensePageView addExpensePage = (IAddExpensePageView)view;
                List<WpfMishaLibrary.VisibleEntities.CardVisible> list = m.GetCards();
                cardDict.Clear();
                addExpense.BankAccounts.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    ApplicationProjectViews.BankAccountInfo tmp = new ApplicationProjectViews.BankAccountInfo() { AccountBalance = Convert.ToDecimal(list[i].Balance), AccountName = list[i].CardName };
                    addExpense.BankAccounts.Add(tmp);
                    cardDict.Add(tmp, list[i]);
                }
            });
        }

        public static void FillCategories()
        {
            ((IAddExpensePageView)addExpense).DispatchUpdate(view =>
            {
                IAddExpensePageView addExpensePage = (IAddExpensePageView)view;
                List<WpfMishaLibrary.VisibleEntities.PlanExpenditureVisible> list = m.GetPlanExpendituresSelectedDay(addExpensePage.SelectedDate);
                categoryDict.Clear();
                addExpense.ExpenseCategories.Clear();
                for (int i = 0; i < list.Count; i++)
                {
                    ApplicationProjectViews.CategoryDescriptor tmp = new ApplicationProjectViews.CategoryDescriptor() { DisplayName = list[i].ExpenditureCategory, ImagePath = list[i].PlanExpenditureImagePath };
                    addExpense.ExpenseCategories.Add(tmp);
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
            ((IAddExpensePageView)source).DispatchUpdate(view => {
                IAddExpensePageView local = (IAddExpensePageView)view;

                WpfMishaLibrary.VisibleEntities.PlanExpenditureVisible category = null;
                WpfMishaLibrary.VisibleEntities.CardVisible card = null;
                try 
                { 
                    categoryDict.TryGetValue(local.SelectedExpenseCategory, out category);
                    local.SelectedExpenseCategoryError = null;
                }
                catch { local.SelectedExpenseCategoryError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.EmptyValue, ""); return;}

                try 
                { 
                    cardDict.TryGetValue(local.SelectedBankAccount, out card);
                    local.SelectedBankAccountError = null;
                }
                catch { local.SelectedBankAccountError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.EmptyValue, ""); return;}

                if ((local.ExpenseName).Trim() == "")
                {
                    local.ExpenseNameError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.EmptyValue, "");
                    return;
                }
                else
                    local.ExpenseNameError = null;

                try
                {
                    ModelEditDataResultStates.ReturnFactExpenditureState ret = m.AddFactExpenditure(local.ExpenseName, double.Parse(local.CurrencyAmount), local.SelectedDate, category, card);
                    if (ret == ModelEditDataResultStates.ReturnFactExpenditureState.ErrorTypeSumConstraint)
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
