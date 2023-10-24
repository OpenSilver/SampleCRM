Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows.Controls

Namespace Views
    Public Class BasePage
        Inherits Page
        Implements INotifyPropertyChanged

        Protected Overridable ReadOnly Property MaxMobileWidth As Double
            Get
                Return 640.0R
            End Get
        End Property

        Public Sub New()
            AddHandler SizeChanged, AddressOf OnSizeChanged
        End Sub

        Protected Overridable Sub OnSizeChanged(ByVal sender As Object, ByVal e As System.Windows.SizeChangedEventArgs)
            OnPropertyChanged(NameOf(IsMobileUI))
        End Sub

        Public ReadOnly Property IsMobileUI As Boolean
            Get
                Return ActualWidth <= MaxMobileWidth
            End Get
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = "")
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

    End Class
End Namespace

