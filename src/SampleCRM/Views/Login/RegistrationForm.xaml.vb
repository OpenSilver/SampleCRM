Imports System.ComponentModel.DataAnnotations
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports OpenRiaServices.DomainServices.Client
Imports OpenRiaServices.DomainServices.Client.ApplicationServices
Imports SampleCRM.Views
Imports SampleCRM.Web

Namespace LoginUI

    ''' <summary>
    ''' Form that presents the <see cref="RegistrationData"/> and performs the registration process.
    ''' </summary>
    Partial Public Class RegistrationForm
        Inherits StackPanel

        Private parentWindow As LoginRegistrationWindow
        Private registrationData As RegistrationData = New RegistrationData()
        Private userRegistrationContext As UserRegistrationContext = New UserRegistrationContext()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Creates a new <see cref="RegistrationForm"/> instance.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Set the DataContext of this control to the Registration instance to allow for easy binding.
            Me.DataContext = Me.registrationData
        End Sub

        ''' <summary>
        ''' Sets the parent window for the current <see cref="RegistrationForm"/>.
        ''' </summary>
        ''' <param name="window">The window to use as the parent.</param>
        Public Sub SetParentWindow(window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Wire up the Password and PasswordConfirmation accessors as the fields get generated.
        ''' Also bind the Question field to a ComboBox full of security questions, and handle the LostFocus event for the UserName TextBox.
        ''' </summary>
        Private Sub RegisterForm_AutoGeneratingField(dataForm As Object, e As DataFormAutoGeneratingFieldEventArgs)
            ' Put all the fields in adding mode
            e.Field.Mode = DataFieldMode.AddNew

            Select Case e.PropertyName
                Case "UserName"
                    Me.userNameTextBox = CType(e.Field.Content, TextBox)
                    AddHandler Me.userNameTextBox.LostFocus, AddressOf Me.UserNameLostFocus
                Case "Password"
                    Dim passwordBox As PasswordBox = New PasswordBox()
                    e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty)
                    Me.registrationData.PasswordAccessor = Function() passwordBox.Password
                Case "PasswordConfirmation"
                    Dim passwordConfirmationBox As PasswordBox = New PasswordBox()
                    e.Field.ReplaceTextBox(passwordConfirmationBox, PasswordBox.PasswordProperty)
                    Me.registrationData.PasswordConfirmationAccessor = Function() passwordConfirmationBox.Password
                Case "Question"
                    Dim questionComboBox As ComboBox = New ComboBox()
                    questionComboBox.ItemsSource = RegistrationForm.GetSecurityQuestions()
                    e.Field.ReplaceTextBox(questionComboBox, ComboBox.SelectedItemProperty, Sub(binding) binding.Converter = New TargetNullValueConverter())
            End Select

        End Sub

        ''' <summary>
        ''' The callback for when the UserName TextBox loses focus.
        ''' Call into the registration data to allow logic to be processed, possibly setting the FriendlyName field.
        ''' </summary>
        ''' <param name="sender">The event sender.</param>
        ''' <param name="e">The event arguments.</param>
        Private Sub UserNameLostFocus(sender As Object, e As RoutedEventArgs)
            Me.registrationData.UserNameEntered((CType(sender, TextBox)).Text)
        End Sub

        ''' <summary>
        ''' Returns a list of the resource strings defined in <see cref="SecurityQuestions" />.
        ''' </summary>
        Private Shared Function GetSecurityQuestions() As IEnumerable(Of String)

            Return New String() {
                "What is the name of your favorite childhood friend?",
                "What was your childhood nickname?",
                "What was the color of your first car?",
                "What was the make and model of your first car?",
                "In what city or town was your first job?",
                "Where did you vacation last year?",
                "What is your maternal grandmother's maiden name?",
                "What is your mother's maiden name?",
                "What is your pet's name?",
                "What school did you attend for sixth grade?",
                "In what year was your father born?"
            }

        End Function

        ''' <summary>
        ''' Submit the new registration.
        ''' </summary>
        Private Sub RegisterButton_Click(sender As Object, e As RoutedEventArgs)

            ' We need to force validation since we are not using the standard OK button from the DataForm.
            ' Without ensuring the form is valid, we would get an exception invoking the operation if the entity is invalid.
            If Me.registerForm.ValidateItem() Then

                Me.registrationData.CurrentOperation = Me.userRegistrationContext.CreateUser(
                    Me.registrationData,
                    Me.registrationData.Password,
                    AddressOf Me.RegistrationOperation_Completed,
                    Nothing)

                Me.parentWindow.AddPendingOperation(Me.registrationData.CurrentOperation)
            End If

        End Sub

        ''' <summary>
        ''' Completion handler for the registration operation.
        ''' If there was an error, an <see cref="ErrorWindow"/> is displayed to the user.
        ''' Otherwise, this triggers a login operation that will automatically log in the just registered user.
        ''' </summary>
        Private Sub RegistrationOperation_Completed(operation As InvokeOperation(Of CreateUserStatus))

            If Not operation.IsCanceled Then

                If operation.HasError Then
                    ErrorWindow.Show(operation.Error)
                    operation.MarkErrorAsHandled()
                    Return
                End If

                Select Case operation.Value
                    Case CreateUserStatus.Success
                        Me.registrationData.CurrentOperation = WebContext.Current.Authentication.Login(Me.registrationData.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                        Me.parentWindow.AddPendingOperation(Me.registrationData.CurrentOperation)
                    Case CreateUserStatus.DuplicateUserName
                        Me.registrationData.ValidationErrors.Add(
                            New ValidationResult("User name already exists. Please enter a different user name.",
                            New String() {"UserName"}))
                    Case CreateUserStatus.DuplicateEmail
                        Me.registrationData.ValidationErrors.Add(
                            New ValidationResult("A user name for that e-mail address already exists. Please enter a different e-mail address.",
                            New String() {"Email"}))
                    Case Else
                        ErrorWindow.Show("An unknown error has occurred. Please contact your administrator for help.")
                End Select

            End If

        End Sub

        ''' <summary>
        ''' Completion handler for the login operation that occurs after a successful registration and login attempt.
        ''' This will close the window. If the operation fails, an <see cref="ErrorWindow"/> will display the error message.
        ''' </summary>
        ''' <param name="loginOperation">The <see cref="LoginOperation"/> that has completed.</param>
        Private Sub LoginOperation_Completed(loginOperation As LoginOperation)

            If Not loginOperation.IsCanceled Then

                Me.parentWindow.DialogResult = True

                If loginOperation.HasError Then
                    ErrorWindow.Show(String.Format(System.Globalization.CultureInfo.CurrentUICulture,
                        "Registration was successful but there was a problem while trying to log in with your credentials: {0}",
                        loginOperation.Error.Message))
                    loginOperation.MarkErrorAsHandled()
                ElseIf Not loginOperation.LoginSuccess Then
                    ' The operation was successful, but the actual login was not
                    ErrorWindow.Show(String.Format(System.Globalization.CultureInfo.CurrentUICulture,
                        "Registration was successful but there was a problem while trying to log in with your credentials: {0}",
                        "The user name or password is incorrect"))
                End If

            End If

        End Sub

        ''' <summary>
        ''' Switches to the login window.
        ''' </summary>
        Private Sub BackToLogin_Click(sender As Object, e As RoutedEventArgs)
            Me.parentWindow.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' If a registration or login operation is in progress and is cancellable, cancel it.
        ''' Otherwise, close the window.
        ''' </summary>
        Private Sub CancelButton_Click(sender As Object, e As EventArgs)

            If Me.registrationData.CurrentOperation IsNot Nothing AndAlso Me.registrationData.CurrentOperation.CanCancel Then
                Me.registrationData.CurrentOperation.Cancel()
            Else
                Me.parentWindow.DialogResult = False
            End If

        End Sub

        ''' <summary>
        ''' Maps Esc to the cancel button and Enter to the OK button.
        ''' </summary>
        Private Sub RegistrationForm_KeyDown(sender As Object, e As KeyEventArgs)

            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter AndAlso Me.registerButton.IsEnabled Then
                Me.RegisterButton_Click(sender, e)
            End If

        End Sub

        ''' <summary>
        ''' Sets focus to the user name text box.
        ''' </summary>
        Public Sub SetInitialFocus()
            Me.userNameTextBox.Focus()
        End Sub

    End Class

End Namespace