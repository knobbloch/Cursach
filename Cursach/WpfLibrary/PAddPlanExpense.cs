using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.AddExpenseCategoryPageView;
using WpfMishaLibrary;
using System.IO;

namespace WpfLibrary
{
    class PAddPlanExpense
    {
        IModel m;
        static IAddExpenseCategoryPageView expenseCategoryy;
        public PAddPlanExpense(IAddExpenseCategoryPageView expenseCategory, IModel model)
        {
            expenseCategory.AddAction += AddClicked;
            m = model;
            expenseCategoryy = expenseCategory;
            Update();
        }

        public static void Update()
        {
            ((IAddExpenseCategoryPageView)expenseCategoryy).DispatchUpdate(view => {
                IAddExpenseCategoryPageView addView = (IAddExpenseCategoryPageView)view;
                addView.CurrencyAmount = "0";
                addView.CategoryName = "";
                addView.CategoryImagePath = "";
                addView.CategoryImagePathError = null;
                addView.CategoryNameError = null;
                addView.CurrencyAmountError = null;
                PPlan.Update();
            });
        }

        public void AddClicked(object source, EventArgs a)
        {
            ((IAddExpenseCategoryPageView)source).DispatchUpdate(view => {
                IAddExpenseCategoryPageView addView = (IAddExpenseCategoryPageView)view;
                if (addView.CategoryName == "")
                {
                    addView.CategoryNameError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.EmptyValue, "");
                    return;
                }

                if (!File.Exists(addView.CategoryImagePath))
                {
                    addView.CategoryImagePathError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.InvalidSymbol, "");
                    return;
                }

                try
                {
                    if (double.Parse(addView.CurrencyAmount) < 0)
                    {
                        addView.CurrencyAmountError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.OutOfBoundsValue, "");
                        return;
                    }
                    ModelEditDataResultStates.ReturnPlanExpenditureState ret = m.AddPlanExpenditure(addView.CategoryName, double.Parse(addView.CurrencyAmount), PDate.DateBounds.Start, PDate.DateBounds.End, addView.CategoryImagePath);

                    if (ret == ModelEditDataResultStates.ReturnPlanExpenditureState.ErrorTypeNameConstraint)
                        addView.CategoryNameError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.DuplicateValue, "");
                    else
                        PPlan.Update();
                }
                catch
                {
                    try 
                        { double number = double.Parse(addView.CurrencyAmount); }
                    catch (FormatException) 
                        { addView.CurrencyAmountError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.OutOfBoundsValue, ""); }
                }
            });

        }
    }
}
