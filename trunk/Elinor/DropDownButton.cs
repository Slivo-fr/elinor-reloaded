using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Elinor
{
    internal class DropDownButton : ToggleButton
    {
        public static readonly DependencyProperty DropDownProperty =
            DependencyProperty.Register("DropDown", typeof (ContextMenu), 
                                        typeof (DropDownButton),
                                        new UIPropertyMetadata(null));

        public ContextMenu DropDown
        {
            get { return (ContextMenu) GetValue(DropDownProperty); }
            set { SetValue(DropDownProperty, value); }
        }

        private void DropDownOnClose(object sender, RoutedEventArgs routedEventArgs)
        {
            IsChecked = false;
        }

        protected override void OnChecked(RoutedEventArgs e)
        {
            DropDown.Closed += DropDownOnClose;
            DropDown.PlacementTarget = this;
            DropDown.Placement = PlacementMode.Top;
            DropDown.IsOpen = true;
        }
    }
}