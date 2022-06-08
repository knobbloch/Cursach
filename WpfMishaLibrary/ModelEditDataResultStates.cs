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
            ErrorTypeNameConstraint,
            ErrorTypeUnrecognized
        }
        public enum ReturnFactExpenditureState
        {
            Success,
            ErrorTypeNameConstraint
        }
    }
}
