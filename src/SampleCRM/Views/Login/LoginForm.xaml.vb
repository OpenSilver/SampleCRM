Imports System.ComponentModel.DataAnnotations
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports OpenRiaServices.DomainServices.Client.ApplicationServices
Imports SampleCRM.Views
Imports SampleCRM.Web

Namespace LoginUI

    ''' <summary>
    ''' Form that presents the login fields And handles the login process.
    ''' </summary>
    Partial Public Class LoginForm
        Inherits StackPanel

        Private _parentWindow As LoginRegistrationWindow
        Private _loginInfo As LoginInfo = New LoginInfo()
        Private _userNameTextBox As TextBox

        ''' <summary>
        ''' Creates a New <see cref="LoginForm"/> instance.
        ''' </summary>
        Public Sub New()

            InitializeComponent()

            ' Set the DataContext of this control to the LoginInfo instance to allow for easy binding.
            Me.DataContext = _loginInfo
        End Sub

        ''' <summary>
        ''' Sets the parent window for the current <see cref="LoginForm"/>.
        ''' </summary>
        ''' <param name="window">The window to use as the parent.</param>
        Public Sub SetParentWindow(window As LoginRegistrationWindow)
            _parentWindow = window
        End Sub

        ''' <summary>
        ''' Handles <see cref="DataForm.AutoGeneratingField"/> to provide the PasswordAccessor.
        ''' </summary>
        Private Sub LoginForm_AutoGeneratingField(sender As Object, e As DataFormAutoGeneratingFieldEventArgs)

            If e.PropertyName = "UserName" Then
                _userNameTextBox = CType(e.Field.Content, TextBox)
            ElseIf e.PropertyName = "Password" Then
                Dim myPasswordBox As PasswordBox = New PasswordBox()
                e.Field.ReplaceTextBox(myPasswordBox, PasswordBox.PasswordProperty)
                _loginInfo.PasswordAccessor = Function() myPasswordBox.Password
            End If
        End Sub

        ''' <summary>
        ''' Submits the <see cref="LoginOperation"/> to the server
        ''' </summary>
        Private Sub LoginButton_Click(sender As Object, e As EventArgs)

            ' We need to force validation since we are Not using the standard OK button from the DataForm.
            ' Without ensuring the form Is valid, we get an exception invoking the operation if the entity Is invalid.
            If Me.loginForm.ValidateItem() Then

                _loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(_loginInfo.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                _parentWindow.AddPendingOperation(_loginInfo.CurrentLoginOperation)
            End If
        End Sub

        ''' <summary>
        ''' Completion handler for a <see cref="LoginOperation"/>.
        ''' If operation succeeds, it closes the window.
        ''' If it has an error, it displays an <see cref="ErrorWindow"/> And marks the error as handled.
        ''' If it was Not canceled, but login failed, it must have been because credentials were incorrect so a validation error Is added to notify the user.
        ''' </summary>
        Private Sub LoginOperation_Completed(loginOperation As LoginOperation)

            If loginOperation.LoginSuccess Then
                _parentWindow.DialogResult = True
            ElseIf loginOperation.HasError Then
                ErrorWindow.Show(loginOperation.Error)
                loginOperation.MarkErrorAsHandled()
            ElseIf Not loginOperation.IsCanceled Then
                _loginInfo.ValidationErrors.Add(New ValidationResult("Bad User Name Or Password", New String() {"UserName", "Password"}))
            End If

        End Sub

        ''' <summary>
        ''' Switches to the registration form.
        ''' </summary>
        Private Sub RegisterNow_Click(sender As Object, e As RoutedEventArgs)
            _parentWindow.NavigateToRegistration()
        End Sub

        ''' <summary>
        ''' If a login operation Is in progress And Is cancellable, cancel it.
        ''' Otherwise, close the window.
        ''' </summary>
        Private Sub CancelButton_Click(sender As Object, e As EventArgs)

            If _loginInfo.CurrentLoginOperation IsNot Nothing AndAlso _loginInfo.CurrentLoginOperation.CanCancel Then
                _loginInfo.CurrentLoginOperation.Cancel()
            Else
                _parentWindow.DialogResult = False
            End If

        End Sub

        ''' <summary>
        ''' Maps Esc to the cancel button And Enter to the OK button.
        ''' </summary>
        Private Sub LoginForm_KeyDown(sender As Object, e As KeyEventArgs)

            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter AndAlso Me.loginButton.IsEnabled Then
                Me.LoginButton_Click(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' Sets focus to the user name text box.
        ''' </summary>
        Public Sub SetInitialFocus()
            _userNameTextBox.Focus()
        End Sub

    End Class

End Namespace