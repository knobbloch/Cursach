using System;

namespace ApplicationProject.Views.InterPageView
{
    public delegate void CategorySelectedEventHandler(object source, CategorySelectedEventArgs e);

    public class CategorySelectedEventArgs : EventArgs
    {
        public enum CategoryType
        {
            Analysis,
            Plan,
            NewEntry
        }

        public CategorySelectedEventArgs(CategoryType category) : base()
        {
            Category = category;
        }

        public CategoryType Category { get; }
    }
}
