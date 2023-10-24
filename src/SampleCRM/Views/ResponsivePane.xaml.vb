Imports System.Windows
Imports System.Windows.Controls

Namespace Views
    ' This class contains all the logic for making the menu on the left disappear when the screen is too small.
    Partial Public Class ResponsivePane
        Inherits UserControl

        ' This corresponds to tablets and other devices with high resolution. In this case, we see both the menu and the page.
        Const MinimumResolutionForMenu As Double = 900.0

        Private _currentState As CurrentState

        Enum CurrentState
            Unset ' Initial value
            LargeResolution_SeeBothMenuAndPage ' This corresponds to tablets and other devices with high resolution. In this case we see both the menu and the page.
            SmallResolution_ShowMenu ' This corresponds to smartphones and other devices with low resolution. In this case we see the menu.
            SmallResolution_HideMenu ' This corresponds to smartphones and other devices with low resolution. In this case we do not see the menu.
        End Enum

        Public Sub New()
            InitializeComponent()

            AddHandler Loaded, AddressOf ResponsivePane_Loaded
            AddHandler Unloaded, AddressOf ResponsivePane_Unloaded
        End Sub

        Private Sub ResponsivePane_Loaded(sender As Object, e As RoutedEventArgs)
            RemoveHandler Window.Current.SizeChanged, AddressOf Window_SizeChanged
            AddHandler Window.Current.SizeChanged, AddressOf Window_SizeChanged

            UpdateMenuDispositionBasedOnDisplaySize()
        End Sub

        Private Sub ResponsivePane_Unloaded(sender As Object, e As RoutedEventArgs)
            RemoveHandler Window.Current.SizeChanged, AddressOf Window_SizeChanged
        End Sub

        Private Sub Window_SizeChanged(sender As Object, e As WindowSizeChangedEventArgs)
            UpdateMenuDispositionBasedOnDisplaySize()
        End Sub

        Private Sub GoToState(newState As CurrentState)
            If newState <> _currentState Then
                If newState = CurrentState.LargeResolution_SeeBothMenuAndPage Then
                    ' Show the menu:
                    MenuBorder.Visibility = Visibility.Visible

                    ' Hide the button to hide/show the menu:
                    ButtonToHideOrShowMenu.Visibility = Visibility.Collapsed
                Else
                    ' Revert the changes that are specific to the CurrentState.LargeResolution_SeeBothMenuAndPage state.

                    ' Show the button to hide/show the menu:
                    ButtonToHideOrShowMenu.Visibility = Visibility.Visible
                    MenuBorder.Visibility = Visibility.Collapsed

                    If newState = CurrentState.SmallResolution_ShowMenu Then
                        ' Show the menu:
                        SmallMenuBorder.Visibility = Visibility.Visible
                    Else
                        ' Hide the menu:
                        SmallMenuBorder.Visibility = Visibility.Collapsed
                    End If
                End If
                _currentState = newState
            End If
        End Sub

        Private Sub UpdateMenuDispositionBasedOnDisplaySize()
            Dim windowBounds As Rect = Window.Current.Bounds
            Dim displayWidth As Double = windowBounds.Width

            If Not Double.IsNaN(displayWidth) AndAlso displayWidth > MinimumResolutionForMenu Then
                GoToState(CurrentState.LargeResolution_SeeBothMenuAndPage)
            ElseIf _currentState = CurrentState.LargeResolution_SeeBothMenuAndPage OrElse _currentState = CurrentState.Unset Then
                GoToState(CurrentState.SmallResolution_HideMenu)
            End If
        End Sub

        Private Sub ButtonToHideOrShowMenu_Click(sender As Object, e As RoutedEventArgs)
            If _currentState = CurrentState.SmallResolution_ShowMenu Then
                GoToState(CurrentState.SmallResolution_HideMenu)
            ElseIf _currentState = CurrentState.SmallResolution_HideMenu Then
                GoToState(CurrentState.SmallResolution_ShowMenu)
            Else
                ' Not supposed to happen because the button is not visible when in large resolution mode.
            End If
        End Sub

        Public Sub CollapseIfMobile()
            If _currentState = CurrentState.SmallResolution_ShowMenu Then
                GoToState(CurrentState.SmallResolution_HideMenu)
            End If
        End Sub

        Public Property BigChildElement As Object
            Get
                Return GetValue(BigChildElementProperty)
            End Get
            Set(value As Object)
                SetValue(BigChildElementProperty, value)
            End Set
        End Property

        Public Shared ReadOnly BigChildElementProperty As DependencyProperty = DependencyProperty.Register("BigChildElement", GetType(Object), GetType(ResponsivePane), New PropertyMetadata(Nothing))

        Public Property SmallChildElement As Object
            Get
                Return GetValue(SmallChildElementProperty)
            End Get
            Set(value As Object)
                SetValue(SmallChildElementProperty, value)
            End Set
        End Property

        Public Shared ReadOnly SmallChildElementProperty As DependencyProperty = DependencyProperty.Register("SmallChildElement", GetType(Object), GetType(ResponsivePane), New PropertyMetadata(Nothing))
    End Class
End Namespace