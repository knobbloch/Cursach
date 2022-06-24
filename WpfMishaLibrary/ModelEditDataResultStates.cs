using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMishaLibrary
{
    public class ModelEditDataResultStates
    {
        public enum ReturnCardState
        {
            Success,
            // Card name is already in the table
            ErrorTypeNameConstraint,
            ErrorTypeUnrecognized
        }
        public enum ReturnFactExpenditureState
        {
            Success,
            // Sum < 0
            ErrorTypeSumConstraint,
            ErrorTypeUnrecognized,
            ErrorTypeUpdatingBalance
        }
        public enum ReturnFactIncomeState
        {
            Success,
            // Sum < 0
            ErrorTypeSumConstraint,
            ErrorTypeUnrecognized
        }
        public enum ReturnPlanExpenditureState
        {
            Success,
            // DateOfBegin > DateOfEnd
            ErrorTypeDateConstraint,
            // Plan expenditure name(category) is already in the table
            ErrorTypeNameConstraint,
            ErrorTypeUnrecognized
        }
        public enum ReturnPlanIncomeState
        {
            Success,
            // DateOfBegin > DateOfEnd
            ErrorTypeDateConstraint,
            // Plan expenditure name(category) is already in the table
            ErrorTypeNameConstraint,
            ErrorTypeUnrecognized
        }
    }
}
