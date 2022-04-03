using System;

namespace ApplicationProject.Views.InterPageView
{
    public class BankAccountSelectedEventArgs : EventArgs
    {
        public BankAccountSelectedEventArgs(BankAccountInfo account, int index)
        {
            BankAccount = account;
            IndexInCollection = index;
        }

        public BankAccountInfo BankAccount { get; }
        public int IndexInCollection { get; }
    }
}
