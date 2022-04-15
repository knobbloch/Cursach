using System;

namespace ApplicationProject.Views.InterPageView
{
    public class CategorySelectedEventArgs : EventArgs
    {
        public enum CategoryType
        {
            Analysis,
            Plan,
            NewEntry
        }

        public CategorySelectedEventArgs(CategoryType category)
        {
            Category = category;
        }

        public CategoryType Category { get; }
    }
}
