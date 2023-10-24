Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls

Namespace Views
    Public Class BaseUserControl
        Inherits UserControl
        Implements INotifyPropertyChanged

        Protected Overridable ReadOnly Property MaxMobileWidth As Double
            Get
                Return 700.0R
            End Get
        End Property

        Public Sub New()
            AddHandler Loaded, AddressOf BaseUserControl_Loaded
            AddHandler Application.Current.MainWindow.SizeChanged, AddressOf MainWindow_SizeChanged
        End Sub

        Private Sub MainWindow_SizeChanged(sender As Object, e As WindowSizeChangedEventArgs)
            ArrangeLayout()
            OnPropertyChanged(NameOf(IsMobileUI))
        End Sub

        Private Sub BaseUserControl_Loaded(sender As Object, e As RoutedEventArgs)
            ArrangeLayout()
        End Sub

        Public Overridable Sub ArrangeLayout()
        End Sub

        Public ReadOnly Property IsMobileUI As Boolean
            Get
                Return Application.Current.MainWindow.ActualWidth <= MaxMobileWidth
            End Get
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = "")
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
End Namespace