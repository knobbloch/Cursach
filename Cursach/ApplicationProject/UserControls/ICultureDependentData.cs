using System.Globalization;

namespace ApplicationProjectViews
{
    public interface ICultureDependentData
    {
        /// <summary>
        /// Should be called when current culture (localization) changes
        /// </summary>
        /// <param name="newCulture">The new UI culture</param>
        void OnCultureChanged(CultureInfo newCulture);
    }
}
