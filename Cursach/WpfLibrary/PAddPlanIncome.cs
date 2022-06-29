using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationProjectViews.AddIncomeCategoryPageView;
using WpfMishaLibrary;
using System.IO;

namespace WpfLibrary
{
    class PAddPlanIncome
    {
        IModel m;
        static IAddIncomeCategoryPageView incomeCategoryy;
        public PAddPlanIncome(IAddIncomeCategoryPageView incomeCategory, IModel model)
        { 
            incomeCategory.AddAction += AddClicked;
            m = model;
            incomeCategoryy = incomeCategory;
        }

        public static void Update()
        {
            ((IAddIncomeCategoryPageView)incomeCategoryy).DispatchUpdate(view => {
                IAddIncomeCategoryPageView addView = (IAddIncomeCategoryPageView)view;
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
            ((IAddIncomeCategoryPageView)source).DispatchUpdate(view => {
                IAddIncomeCategoryPageView addView = (IAddIncomeCategoryPageView)view;
                if ((addView.CategoryName).Trim() == "")
                {
                    addView.CategoryNameError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.EmptyValue, "");
                    return;
                }

                if (!File.Exists(addView.CategoryImagePath))
                {
                    addView.CategoryImagePathError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.InvalidSymbol, "");
                    return;
                }
                else
                    addView.CategoryImagePathError = null;

                if (double.Parse(addView.CurrencyAmount) <0)
                {
                    addView.CurrencyAmountError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.InvalidSymbol, "");
                    return;
                }
                else
                    addView.CurrencyAmountError = null;

                try
                {
                    ModelEditDataResultStates.ReturnPlanIncomeState ret = m.AddPlanIncome(addView.CategoryName, double.Parse(addView.CurrencyAmount), PDate.DateBounds.Start, PDate.DateBounds.End, addView.CategoryImagePath);

                    if (ret == ModelEditDataResultStates.ReturnPlanIncomeState.ErrorTypeNameConstraint)
                        addView.CategoryNameError = new ApplicationProjectViews.ValueInputError(ApplicationProjectViews.ValueInputError.ValueInputErrorType.DuplicateValue, "");
                    else
                    {
                        addView.CategoryNameError = null;
                        Update();
                    }
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
