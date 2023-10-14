using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Web.Views
{
    //[ContentProperty("ChildElement")]
    public partial class ResponsivePane : UserControl
    {
        //This class contains all that we use to make the menu on the left disappear when the screen is too small.

        const double MinimumResolutionForMenu = 900d;

        CurrentState _currentState;

        enum CurrentState
        {
            Unset, // Initial value
            LargeResolution_SeeBothMenuAndPage, // This corresponds to tablets and other devices with high resolution. In this case we see both the menu and the page.
            SmallResolution_ShowMenu, // This corresponds to smartphones and other devices with low resolution. In this case we see the menu.
            SmallResolution_HideMenu // This corresponds to smartphones and other devices with low resolution. In this case we do not see the menu.
        }

        public ResponsivePane()
        {
            InitializeComponent();

            Loaded += ResponsivePane_Loaded;
            Unloaded += ResponsivePane_Unloaded;
        }

        private void ResponsivePane_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Window_SizeChanged;
            Window.Current.SizeChanged += Window_SizeChanged;

            UpdateMenuDispositionBasedOnDisplaySize();
        }

        private void ResponsivePane_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }


        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            UpdateMenuDispositionBasedOnDisplaySize();
        }

        void GoToState(CurrentState newState)
        {
            if (newState != _currentState)
            {
                if (newState == CurrentState.LargeResolution_SeeBothMenuAndPage)
                {
                    // Show the menu:
                    MenuBorder.Visibility = Visibility.Visible;

                    // Hide the button to hide/show the menu:
                    ButtonToHideOrShowMenu.Visibility = Visibility.Collapsed;
                }
                else
                {
                    // Revert the changes that are specific to the CurrentState.LargeResolution_SeeBothMenuAndPage state.

                    // Show the button to hide/show the menu:
                    ButtonToHideOrShowMenu.Visibility = Visibility.Visible;
                    MenuBorder.Visibility = Visibility.Collapsed;

                    if (newState == CurrentState.SmallResolution_ShowMenu)
                    {
                        // Show the menu:
                        SmallMenuBorder.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        // Hide the menu:
                        SmallMenuBorder.Visibility = Visibility.Collapsed;
                    }
                }
                _currentState = newState;
            }
        }

        private void UpdateMenuDispositionBasedOnDisplaySize()
        {
            Rect windowBounds = Window.Current.Bounds;
            double displayWidth = windowBounds.Width;

            if (!double.IsNaN(displayWidth) && displayWidth > MinimumResolutionForMenu)
            {
                GoToState(CurrentState.LargeResolution_SeeBothMenuAndPage);
            }
            else if (_currentState == CurrentState.LargeResolution_SeeBothMenuAndPage
                || _currentState == CurrentState.Unset)
            {
                GoToState(CurrentState.SmallResolution_HideMenu);
            }
        }

        void ButtonToHideOrShowMenu_Click(object sender, RoutedEventArgs e)
        {
            if (_currentState == CurrentState.SmallResolution_ShowMenu)
            {
                GoToState(CurrentState.SmallResolution_HideMenu);
            }
            else if (_currentState == CurrentState.SmallResolution_HideMenu)
            {
                GoToState(CurrentState.SmallResolution_ShowMenu);
            }
            else
            {
                // Not supposed to happen because the button is not visible when in large resolution mode.
            }
        }

        public void CollapseIfMobile()
        {
            if (_currentState == CurrentState.SmallResolution_ShowMenu)
                GoToState(CurrentState.SmallResolution_HideMenu);
        }

        public object BigChildElement
        {
            get { return (object)GetValue(BigChildElementProperty); }
            set { SetValue(BigChildElementProperty, value); }
        }

        public static readonly DependencyProperty BigChildElementProperty =
            DependencyProperty.Register("BigChildElement", typeof(object), typeof(ResponsivePane), new PropertyMetadata(null));

        public object SmallChildElement
        {
            get { return (object)GetValue(SmallChildElementProperty); }
            set { SetValue(SmallChildElementProperty, value); }
        }

        public static readonly DependencyProperty SmallChildElementProperty =
            DependencyProperty.Register("SmallChildElementProperty", typeof(object), typeof(ResponsivePane), new PropertyMetadata(null));
    }
}