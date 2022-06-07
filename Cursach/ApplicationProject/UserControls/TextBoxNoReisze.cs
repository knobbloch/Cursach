using System.Windows;
using System.Windows.Controls;

namespace ApplicationProject.UserControls
{
    internal class TextBoxNoResize : TextBox
    {
        protected override Size MeasureOverride(Size constraint)
        {
            Size size = base.MeasureOverride(constraint);

            return new Size(0, size.Height);
        }
    }
}
