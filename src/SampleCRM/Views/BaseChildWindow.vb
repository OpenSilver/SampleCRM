Imports System.Windows
Imports System.Windows.Controls

Namespace Views
    Public Class BaseChildWindow
        Inherits ChildWindow

        Protected Overridable ReadOnly Property windowMobileSizeMult As Double
            Get
                Return 1.0R
            End Get
        End Property

        Protected Overridable ReadOnly Property windowSizeMult As Double
            Get
                Return 0.85R
            End Get
        End Property

        Protected Overridable ReadOnly Property MaxMobileWidth As Double
            Get
                Return 700.0R
            End Get
        End Property

        Public Property InnerControl As BaseUserControl

        Public ReadOnly Property IsMobileUI As Boolean
            Get
                Return Application.Current.MainWindow.ActualWidth <= MaxMobileWidth
            End Get
        End Property

        Public Sub New()
            arrangeSize()
            AddHandler Application.Current.MainWindow.SizeChanged, AddressOf MainWindow_SizeChanged
        End Sub

        Private Sub MainWindow_SizeChanged(ByVal sender As Object, ByVal e As WindowSizeChangedEventArgs)
            arrangeSize()
        End Sub

        Private Sub arrangeSize()
            If IsMobileUI Then
                Width = Application.Current.MainWindow.ActualWidth * windowMobileSizeMult
                Height = Application.Current.MainWindow.ActualHeight * windowMobileSizeMult
            Else
                Width = Application.Current.MainWindow.ActualWidth * windowSizeMult
                Height = Application.Current.MainWindow.ActualHeight * windowSizeMult
            End If
        End Sub
    End Class
End Namespace


