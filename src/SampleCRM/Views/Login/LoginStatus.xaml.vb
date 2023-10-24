Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Controls
Imports OpenRiaServices.DomainServices.Client.ApplicationServices
Imports SampleCRM.Views
Imports SampleCRM.Web

Namespace LoginUI

    ''' <summary>
    ''' <see cref="UserControl"/> class that shows the current login status And allows login And logout.
    ''' </summary>
    Partial Public Class LoginStatus
        Inherits UserControl

        ''' <summary>
        ''' Creates a New <see cref="LoginStatus"/> instance.
        ''' </summary>
        Public Sub New()

            InitializeComponent()

            If DesignerProperties.IsInDesignTool Then
                VisualStateManager.GoToState(Me, "loggedOut", False)
            Else
                Me.DataContext = WebContext.Current
                AddHandler WebContext.Current.Authentication.LoggedIn, AddressOf Authentication_LoggedIn
                AddHandler WebContext.Current.Authentication.LoggedOut, AddressOf Authentication_LoggedOut
                Me.UpdateLoginState()
            End If
        End Sub

        Private Sub LoginButton_Click(sender As Object, e As RoutedEventArgs)
            Dim loginWindow As LoginRegistrationWindow = New LoginRegistrationWindow()
            loginWindow.Show()
        End Sub

        Private Sub LogoutButton_Click(sender As Object, e As RoutedEventArgs)
            WebContext.Current.Authentication.Logout(AddressOf Me.HandleLogoutOperationErrors, Nothing)
        End Sub

        Private Sub HandleLogoutOperationErrors(ByVal logoutOperation As LogoutOperation)
            If logoutOperation.HasError Then
                ErrorWindow.Show(logoutOperation.Error)
                logoutOperation.MarkErrorAsHandled()
            End If
        End Sub

        Private Sub Authentication_LoggedIn(sender As Object, e As AuthenticationEventArgs)
            Me.UpdateLoginState()
        End Sub

        Private Sub Authentication_LoggedOut(sender As Object, e As AuthenticationEventArgs)
            Me.UpdateLoginState()
        End Sub

        Private Sub UpdateLoginState()

            If WebContext.Current.User.IsAuthenticated Then
                Me.welcomeText.Text = String.Format(
                    CultureInfo.CurrentUICulture,
                    "Welcome {0}",
                    WebContext.Current.User.DisplayName)
            Else
                Me.welcomeText.Text = "authenticating..."
            End If

            If TypeOf WebContext.Current.Authentication Is WindowsAuthentication Then
                VisualStateManager.GoToState(Me, "windowsAuth", True)
            Else
                VisualStateManager.GoToState(Me, If(WebContext.Current.User.IsAuthenticated, "loggedIn", "loggedOut"), True)
            End If

        End Sub

    End Class

End Namespace