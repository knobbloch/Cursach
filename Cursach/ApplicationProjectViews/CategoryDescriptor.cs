using System;

namespace ApplicationProjectViews
{
    public class CategoryDescriptor
    {
        private string m_DisplayName;
        public string DisplayName
        {
            get => m_DisplayName;
            init => m_DisplayName = value ?? throw new ArgumentNullException(nameof(DisplayName));
        }

        private string m_ImagePath;
        public string ImagePath
        {
            get => m_ImagePath;
            init => m_ImagePath = value ?? throw new ArgumentNullException(nameof(ImagePath));
        }
    }
}
