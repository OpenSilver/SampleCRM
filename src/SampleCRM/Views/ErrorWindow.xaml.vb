Imports System.Windows

Namespace Views
    Partial Public Class ErrorWindow
        Inherits BaseChildWindow

        Protected Overrides ReadOnly Property windowSizeMult As Double
            Get
                Return 0.5
            End Get
        End Property

        Public Overloads Shared Sub Show(ex As Exception)
            Dim errorWindow = New ErrorWindow(ex)
            errorWindow.Show()
        End Sub

        Public Overloads Shared Sub Show(uri As Uri)
            Dim errorWindow = New ErrorWindow(uri)
            errorWindow.Show()
        End Sub

        Public Overloads Shared Sub Show(message As String, Optional details As String = "")
            Dim errorWindow = New ErrorWindow(message, details)
            errorWindow.Show()
        End Sub

        Public Overloads Shared Sub Show(title As String, message As String, Optional details As String = "")
            Dim errorWindow = New ErrorWindow(title, message, details)
            errorWindow.Show()
        End Sub

        Private Sub New(e As Exception)
            InitializeComponent()
            If e IsNot Nothing Then
                ErrorTextBox.Text = e.Message + Environment.NewLine + Environment.NewLine + e.StackTrace
            End If
        End Sub

        Private Sub New(uri As Uri)
            InitializeComponent()
            If uri IsNot Nothing Then
                ErrorTextBox.Text = "Page not found: """ + uri.ToString() + """"
            End If
        End Sub

        Private Sub New(message As String, details As String)
            InitializeComponent()
            ErrorTextBox.Text = message + Environment.NewLine + Environment.NewLine + details
        End Sub

        Private Sub New(title As String, message As String, details As String)
            InitializeComponent()
            title = title
            IntroductoryText.Text = message
            ErrorTextBox.Text = details
        End Sub

        Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs)
            DialogResult = True
        End Sub
    End Class
End Namespace

