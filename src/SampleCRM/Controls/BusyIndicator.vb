Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Shapes
Imports System.Windows.Threading

Namespace Controls

    ''' <summary>
    ''' A control to provide a visual indicator when an application is busy.
    ''' </summary>
    <TemplateVisualState(Name:=VisualStates.StateIdle, GroupName:=VisualStates.GroupBusyStatus)>
    <TemplateVisualState(Name:=VisualStates.StateBusy, GroupName:=VisualStates.GroupBusyStatus)>
    <TemplateVisualState(Name:=VisualStates.StateVisible, GroupName:=VisualStates.GroupVisibility)>
    <TemplateVisualState(Name:=VisualStates.StateHidden, GroupName:=VisualStates.GroupVisibility)>
    <StyleTypedProperty([Property]:="OverlayStyle", StyleTargetType:=GetType(Rectangle))>
    <StyleTypedProperty([Property]:="ProgressBarStyle", StyleTargetType:=GetType(ProgressBar))>
    Public Class BusyIndicator
        Inherits ContentControl

        ''' <summary>
        ''' Gets or sets a value indicating whether the BusyContent is visible.
        ''' </summary>
        Protected Property IsContentVisible As Boolean

        ''' <summary>
        ''' Timer used to delay the initial display and avoid flickering.
        ''' </summary>
        Private _displayAfterTimer As DispatcherTimer

        ''' <summary>
        ''' Instantiates a new instance of the BusyIndicator control.
        ''' </summary>
        Public Sub New()
            DefaultStyleKey = GetType(BusyIndicator)
            _displayAfterTimer = New DispatcherTimer()
            AddHandler _displayAfterTimer.Tick, AddressOf DisplayAfterTimerElapsed
        End Sub

        ''' <summary>
        ''' Overrides the OnApplyTemplate method.
        ''' </summary>
        Public Overrides Sub OnApplyTemplate()
            MyBase.OnApplyTemplate()
            ChangeVisualState(False)
        End Sub

        ''' <summary>
        ''' Handler for the DisplayAfterTimer.
        ''' </summary>
        ''' <param name="sender">Event sender.</param>
        ''' <param name="e">Event arguments.</param>
        Private Sub DisplayAfterTimerElapsed(sender As Object, e As EventArgs)
            _displayAfterTimer.Stop()
            IsContentVisible = True
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' Changes the control's visual state(s).
        ''' </summary>
        ''' <param name="useTransitions">True if state transitions should be used.</param>
        Protected Overridable Sub ChangeVisualState(useTransitions As Boolean)
            CType(Content, FrameworkElement).IsEnabled = Not IsBusy
            ContentVisibility = If(IsContentVisible, Visibility.Collapsed, Visibility.Visible)
        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating the ContentVisibility
        ''' </summary>
        Property ContentVisibility As Visibility
            Get
                Return CType(GetValue(ContentVisibilityProperty), Visibility)
            End Get
            Set(value As Visibility)
                SetValue(ContentVisibilityProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifies the ContentVisibility dependency property.
        ''' </summary>
        Public Shared ReadOnly ContentVisibilityProperty As DependencyProperty = DependencyProperty.Register(
            "ContentVisibility",
            GetType(Visibility),
            GetType(BusyIndicator),
            New PropertyMetadata(Visibility.Visible))

        ''' <summary>
        ''' Gets or sets a value indicating whether the busy indicator should show.
        ''' </summary>
        Property IsBusy As Boolean
            Get
                Return GetValue(IsBusyProperty)
            End Get
            Set(value As Boolean)
                SetValue(IsBusyProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifies the IsBusy dependency property.
        ''' </summary>
        Public Shared ReadOnly IsBusyProperty As DependencyProperty = DependencyProperty.Register(
            "IsBusy",
            GetType(Boolean),
            GetType(BusyIndicator),
            New PropertyMetadata(False, New PropertyChangedCallback(AddressOf OnIsBusyChanged)))

        ''' <summary>
        ''' IsBusyProperty property changed handler.
        ''' </summary>
        ''' <param name="d">BusyIndicator that changed its IsBusy.</param>
        ''' <param name="e">Event arguments.</param>
        Private Shared Sub OnIsBusyChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            CType(d, BusyIndicator).OnIsBusyChanged(e)
        End Sub

        ''' <summary>
        ''' IsBusyProperty property changed handler.
        ''' </summary>
        ''' <param name="e">Event arguments.</param>
        Protected Overridable Sub OnIsBusyChanged(e As DependencyPropertyChangedEventArgs)

            If IsBusy Then
                If DisplayAfter.Equals(TimeSpan.Zero) Then
                    ' Go visible now
                    IsContentVisible = True
                Else
                    ' Set a timer to go visible
                    _displayAfterTimer.Interval = DisplayAfter
                    _displayAfterTimer.Start()
                End If
            Else
                ' No longer visible
                _displayAfterTimer.Stop()
                IsContentVisible = False
            End If
            ChangeVisualState(True)

        End Sub

        ''' <summary>
        ''' Gets or sets a value indicating the busy content to display to the user.
        ''' </summary>
        Property BusyContent As Object
            Get
                Return GetValue(BusyContentProperty)
            End Get
            Set(value As Object)
                SetValue(BusyContentProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifies the BusyContent dependency property.
        ''' </summary>
        Public Shared ReadOnly BusyContentProperty As DependencyProperty = DependencyProperty.Register(
            "BusyContent",
            GetType(Object),
            GetType(BusyIndicator),
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Gets or sets a value indicating the template to use for displaying the busy content to the user.
        ''' </summary>
        Property BusyContentTemplate As DataTemplate
            Get
                Return CType(GetValue(BusyContentTemplateProperty), DataTemplate)
            End Get
            Set(value As DataTemplate)
                SetValue(BusyContentTemplateProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifies the BusyTemplate dependency property.
        ''' </summary>
        Public Shared ReadOnly BusyContentTemplateProperty As DependencyProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            GetType(DataTemplate),
            GetType(BusyIndicator),
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Gets or sets a value indicating how long to delay before displaying the busy content.
        ''' </summary>
        Property DisplayAfter As TimeSpan
            Get
                Return CType(GetValue(DisplayAfterProperty), TimeSpan)
            End Get
            Set(value As TimeSpan)
                SetValue(DisplayAfterProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifies the DisplayAfter dependency property.
        ''' </summary>
        Public Shared ReadOnly DisplayAfterProperty As DependencyProperty = DependencyProperty.Register(
            "DisplayAfter",
            GetType(TimeSpan),
            GetType(BusyIndicator),
            New PropertyMetadata(TimeSpan.FromSeconds(0.1)))

        ''' <summary>
        ''' Gets or sets a value indicating the style to use for the overlay.
        ''' </summary>
        Property OverlayStyle As Style
            Get
                Return CType(GetValue(OverlayStyleProperty), Style)
            End Get
            Set(value As Style)
                SetValue(OverlayStyleProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifies the OverlayStyle dependency property.
        ''' </summary>
        Public Shared ReadOnly OverlayStyleProperty As DependencyProperty = DependencyProperty.Register(
            "OverlayStyle",
            GetType(Style),
            GetType(BusyIndicator),
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Gets or sets a value indicating the style to use for the progress bar.
        ''' </summary>
        Property ProgressBarStyle As Style
            Get
                Return CType(GetValue(ProgressBarStyleProperty), Style)
            End Get
            Set(value As Style)
                SetValue(ProgressBarStyleProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifies the ProgressBarStyle dependency property.
        ''' </summary>
        Public Shared ReadOnly ProgressBarStyleProperty As DependencyProperty = DependencyProperty.Register(
            "ProgressBarStyle",
            GetType(Style),
            GetType(BusyIndicator),
            New PropertyMetadata(Nothing))

    End Class

End Namespace