Imports System.ComponentModel.DataAnnotations
Imports OpenRiaServices.DomainServices.Client
Imports OpenRiaServices.DomainServices.Client.ApplicationServices

Namespace Web

    ''' <summary>
    ''' Extensions to provide client side custom validation and data binding to <see cref="RegistrationData"/>.
    ''' </summary>
    Partial Public Class RegistrationData

        Private _currentOperation As OperationBase

        ''' <summary>
        ''' Gets or sets a function that returns the password.
        ''' </summary>
        Friend Property PasswordAccessor As Func(Of String)

        ''' <summary>
        ''' Gets and sets the password.
        ''' </summary>
        <Required(ErrorMessage:="This field is required")>
        <Display(Order:=3, Name:="Password", Description:="The password must be 7 characters long and must contain at least one special character e.g. @ or #")>
        <RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessage:="A password needs to contain at least one special character e.g. @ or #")>
        <StringLength(50, MinimumLength:=7, ErrorMessage:="Password must be at least 7 and at most 50 characters long")>
        Public Property Password As String
            Get
                If Me.PasswordAccessor Is Nothing Then
                    Return String.Empty
                Else
                    Return Me.PasswordAccessor()()
                End If
            End Get
            Set(ByVal value As String)
                Me.ValidateProperty("Password", value)
                Me.CheckPasswordConfirmation()

                ' Do not store the password in a private field as it should not be stored in memory in plain-text.
                ' Instead, the supplied PasswordAccessor serves as the backing store for the value.
                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a function that returns the password confirmation.
        ''' </summary>
        Friend Property PasswordConfirmationAccessor As Func(Of String)

        ''' <summary>
        ''' Gets and sets the password confirmation string.
        ''' </summary>
        <Required(ErrorMessage:="This field is required")>
        <Display(Order:=4, Name:="Confirm password")>
        Public Property PasswordConfirmation As String
            Get
                If Me.PasswordConfirmationAccessor Is Nothing Then
                    Return String.Empty
                Else
                    Return Me.PasswordConfirmationAccessor()()
                End If
            End Get
            Set(ByVal value As String)
                Me.ValidateProperty("PasswordConfirmation", value)
                Me.CheckPasswordConfirmation()

                ' Do not store the password in a private field as it should not be stored in memory in plain-text.
                ' Instead, the supplied PasswordAccessor serves as the backing store for the value.
                Me.RaisePropertyChanged("PasswordConfirmation")
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the current registration or login operation.
        ''' </summary>
        Friend Property CurrentOperation As OperationBase
            Get
                Return _currentOperation
            End Get
            Set(value As OperationBase)

                If _currentOperation IsNot Nothing Then
                    RemoveHandler _currentOperation.Completed, AddressOf CurrentOperationChanged
                End If

                _currentOperation = value

                If _currentOperation IsNot Nothing Then
                    AddHandler _currentOperation.Completed, AddressOf CurrentOperationChanged
                End If

                CurrentOperationChanged(Me, EventArgs.Empty)

            End Set
        End Property

        ''' <summary>
        ''' Gets a value indicating whether the user is presently being registered or logged in.
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property IsRegistering As Boolean
            Get
                Return _currentOperation IsNot Nothing AndAlso Not _currentOperation.IsComplete
            End Get
        End Property

        ''' <summary>
        ''' Helper method for when the current operation changes.
        ''' Used to raise appropriate property change notifications.
        ''' </summary>
        Private Sub CurrentOperationChanged(sender As Object, e As EventArgs)
            Me.RaisePropertyChanged("IsRegistering")
        End Sub

        ''' <summary>
        ''' Checks to ensure the password and confirmation match.
        ''' If they don't match, a validation error is added.
        ''' </summary>
        Private Sub CheckPasswordConfirmation()

            ' If either of the passwords has not yet been entered, then don't test for equality between the fields.
            ' The Required attribute will ensure a value has been entered for both fields.
            If String.IsNullOrWhiteSpace(Me.Password) OrElse String.IsNullOrWhiteSpace(Me.PasswordConfirmation) Then
                Return
            End If

            ' If the values are different, then add a validation error with both members specified
            If Me.Password <> Me.PasswordConfirmation Then
                Me.ValidationErrors.Add(
                    New ValidationResult("Passwords do not match",
                    New String() {"PasswordConfirmation", "Password"}))
            End If

        End Sub

        ''' <summary>
        ''' Perform logic after the UserName value has been entered.
        ''' </summary>
        ''' <param name="userName">The user name value that was entered.</param>
        ''' <remarks>
        ''' Allow the form to indicate when the value has been completely entered.
        ''' Using the OnUserNameChanged method can lead to a premature call before the user has finished entering the value in the form.
        ''' </remarks>
        Friend Sub UserNameEntered(userName As String)

            ' Auto-Fill FriendlyName to match UserName for new entities when there is not a friendly name specified
            If String.IsNullOrWhiteSpace(Me.FriendlyName) Then
                Me.FriendlyName = userName
            End If

        End Sub

        ''' <summary>
        ''' Creates a new <see cref="LoginParameters"/> initialized with this entity's data (IsPersistent will default to false).
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, False, Nothing)
        End Function

    End Class

End Namespace