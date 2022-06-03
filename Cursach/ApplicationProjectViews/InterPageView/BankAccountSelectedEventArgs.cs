using System;

namespace ApplicationProjectViews.InterPageView
{
    public delegate void BankAccountSelectedSelectedEventHandler(object source, BankAccountSelectedEventArgs e);

    public class BankAccountSelectedEventArgs : EventArgs
    {
        public BankAccountSelectedEventArgs(BankAccountInfo account, int index) : base()
        {
            BankAccount = account;
            IndexInCollection = index;
        }

        public BankAccountInfo BankAccount { get; }
        public int IndexInCollection { get; }
    }
}
