using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Linq;

namespace SampleCRM.Web.Views
{
    public class ComboBoxKeyboardBehavior : Behavior<ComboBox>
    {
        protected override void OnAttached()
        {
#if DEBUG
            Console.WriteLine("OnAttached");
#endif
            base.OnAttached();
            AssociatedObject.KeyUp += OnComboBoxKeyUp;
            AssociatedObject.MouseEnter += OnComboBoxMouseEnter;
            AssociatedObject.KeyDown += OnComboBoxKeyDown;
            AssociatedObject.GotFocus += OnComboBoxGotFocus;
        }

        protected override void OnDetaching()
        {
#if DEBUG
            Console.WriteLine("OnDetaching");
#endif
            AssociatedObject.KeyDown -= OnComboBoxKeyDown;
            AssociatedObject.KeyUp -= OnComboBoxKeyUp;
            AssociatedObject.MouseEnter -= OnComboBoxMouseEnter;
            AssociatedObject.GotFocus -= OnComboBoxGotFocus;
            base.OnDetaching();
        }

        private void OnComboBoxMouseEnter(object sender, MouseEventArgs e)
        {
#if DEBUG
            Console.WriteLine("OnComboBoxMouseEnter");
#endif
        }

        private void OnComboBoxGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("OnComboBoxGotFocus");
#endif
        }

        private void OnComboBoxKeyDown(object sender, KeyEventArgs e)
        {
#if DEBUG
            Console.WriteLine("OnComboBoxKeyDown {0}", e.Key.ToString());
#endif
        }

        private void OnComboBoxKeyUp(object sender, KeyEventArgs e)
        {
#if DEBUG
            Console.WriteLine("OnComboBoxKeyUp {0}", e.Key.ToString());
#endif

            //ComboBox c = sender as ComboBox;
            //c.Focus();
            //if (e.Key >= Key.A && e.Key <= Key.Z)
            //{
            //    var keyChar = (char)('A' + (e.Key - Key.A));
            //    var keyString = keyChar.ToString();
            //    var selectedItem = (from item in AssociatedObject.Items
            //                        let itemString = item.ToString()
            //                        where itemString.StartsWith(keyString, StringComparison.OrdinalIgnoreCase)
            //                        select item).FirstOrDefault();
            //    if (selectedItem != null)
            //        AssociatedObject.SelectedItem = selectedItem;
            //}
        }
    }
}