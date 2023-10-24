Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Controls
Imports OpenRiaServices.DomainServices.Client

Namespace LoginUI

    ''' <summary>
    ''' <see cref="ChildWindow"/> class that controls the registration process.
    ''' </summary>
    Partial Public Class LoginRegistrationWindow
        Inherits ChildWindow

        Private possiblyPendingOperations As IList(Of OperationBase) = New List(Of OperationBase)()

        ''' <summary>
        ''' Creates a New <see cref="LoginRegistrationWindow"/> instance.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            Me.registrationForm.SetParentWindow(Me)
            Me.loginForm.SetParentWindow(Me)

            NavigateToLogin()
        End Sub

        ''' <summary>
        ''' Ensures the visual state And focus are correct when the window Is opened.
        ''' </summary>
        Protected Overrides Sub OnOpened()
            MyBase.OnOpened()
            Me.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' Updates the window title according to which panel (registration / login) Is currently being displayed.
        ''' </summary>
        Private Sub UpdateTitle(sender As Object, e As EventArgs)
            If Me.registrationForm.Visibility = Visibility.Visible Then
                Me.Title = "Register"
            Else
                Me.Title = "Login"
            End If
        End Sub

        ''' <summary>
        ''' Notifies the <see cref="LoginRegistrationWindow"/> window that it can only close if <paramref name="operation"/> Is finished Or can be cancelled.
        ''' </summary>
        ''' <param name="operation">The pending operation to monitor</param>
        Public Sub AddPendingOperation(operation As OperationBase)
            Me.possiblyPendingOperations.Add(operation)
        End Sub

        ''' <summary>
        ''' Causes the <see cref="VisualStateManager"/> to change to the "AtLogin" state.
        ''' </summary>
        Public Overridable Sub NavigateToLogin()
            Me.loginForm.Visibility = Visibility.Visible
            Me.registrationForm.Visibility = Visibility.Collapsed
            UpdateTitle(Me, EventArgs.Empty)
        End Sub

        ''' <summary>
        ''' Causes the <see cref="VisualStateManager"/> to change to the "AtRegistration" state.
        ''' </summary>
        Public Overridable Sub NavigateToRegistration()
            Me.loginForm.Visibility = Visibility.Collapsed
            Me.registrationForm.Visibility = Visibility.Visible
            UpdateTitle(Me, EventArgs.Empty)
        End Sub

        ''' <summary>
        ''' Prevents the window from closing while there are operations in progress
        ''' </summary>
        Private Sub LoginWindow_Closing(sender As Object, e As CancelEventArgs)

            For Each operation As OperationBase In Me.possiblyPendingOperations

                If Not operation.IsComplete Then

                    If operation.CanCancel Then
                        operation.Cancel()
                    Else
                        e.Cancel = True
                    End If

                End If
            Next

        End Sub

    End Class

End Namespace