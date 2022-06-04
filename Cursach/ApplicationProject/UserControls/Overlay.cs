using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace ApplicationProject.UserControls
{
    public class Overlay
    {
        protected Canvas Layer { get; set; }

        /// <summary>
        /// Occurs when the overlay's background (are not occupied by its elements) is clicked
        /// </summary>
        public event EventHandler BackgroundClick;

        /// <summary>
        /// Gets or sets visibility of the overlay
        /// </summary>
        public bool Visible
        {
            get => Layer.Visibility == Visibility.Visible;
            set => Layer.Visibility = value ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Gets or sets overlay's background's brush
        /// </summary>
        public Brush Background
        {
            get => Layer.Background;
            set => Layer.Background = value;
        }

        /// <summary>
        /// Creates a new overlay
        /// </summary>
        /// <param name="layer">The canvas to use as an overlay</param>
        /// <exception cref="ArgumentNullException" />
        public Overlay(Canvas layer)
        {
            Layer = layer ?? throw new ArgumentNullException(nameof(layer));
            Layer.MouseDown += Layer_MouseDown;
        }

        ~Overlay()
        {
            Layer.MouseDown -= Layer_MouseDown;
        }

        /// <summary>
        /// Moves an element to a different position on the overlay
        /// </summary>
        /// <param name="element">The element to move</param>
        /// <param name="position">The element's new position</param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        public void MoveElement(UIElement element, Point position)
        {
            if(element == null)
                throw new ArgumentNullException(nameof(element));
            else if(!Layer.Children.Contains(element))
                throw new ArgumentException("The element is not a part of the overlay", nameof(element));

            Canvas.SetTop(element, position.Y);
            Canvas.SetLeft(element, position.X);
        }

        /// <summary>
        /// Move an element to a different position on the overlay relative to an element (not necessarily from the overlay)
        /// </summary>
        /// <param name="element">The element to move</param>
        /// <param name="relativeTo">The element the position is relative to</param>
        /// <param name="position">The relative position</param>
        /// <exception cref="ArgumentNullException" />
        /// <exception cref="ArgumentException" />
        public void MoveElement(UIElement element, UIElement relativeTo, Point position)
        {
            if(element == null)
                throw new ArgumentNullException(nameof(element));
            else if(relativeTo == null)
                throw new ArgumentNullException(nameof(relativeTo));
            else if(!Layer.Children.Contains(element))
                throw new ArgumentException("The element is not a part of the overlay", nameof(element));

            Point newPos = relativeTo.TranslatePoint(position, Layer);
            Canvas.SetTop(element, newPos.Y);
            Canvas.SetLeft(element, newPos.X);
        }

        /// <summary>
        /// Adds an element to this overlay
        /// </summary>
        /// <param name="element">The element to add</param>
        /// <exception cref="ArgumentNullException" />
        public void AddElement(UIElement element)
        {
            if(element == null)
                throw new ArgumentNullException(nameof(element));

            Layer.Children.Add(element);
        }

        /// <summary>
        /// Removes an element from this overlay
        /// </summary>
        /// <param name="element">The element to remove</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveElement(UIElement element)
        {
            if(element == null)
                throw new ArgumentNullException(nameof(element));

            Layer.Children.Remove(element);
        }

        private void Layer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.OriginalSource == Layer)
                BackgroundClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
