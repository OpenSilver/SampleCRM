Imports System.Windows
Imports OpenRiaServices.DomainServices.Client
Imports OpenRiaServices.DomainServices.Client.ApplicationServices
Imports SampleCRM.Views
Imports SampleCRM.Web

Partial Public Class App
    Inherits Application

    Public Sub New()

        AddHandler Startup, AddressOf Application_Startup
        AddHandler UnhandledException, AddressOf Application_UnhandledException

        InitializeComponent()

        Dim webContext As WebContext = New WebContext()
        webContext.Authentication = New FormsAuthentication()
        'webContext.Authentication = New WindowsAuthentication()
        ApplicationLifetimeObjects.Add(webContext)

        CType(DomainContext.DomainClientFactory, DomainClientFactory).ServerBaseUri = New Uri("http://localhost:54837/")
    End Sub

    Private Sub Application_Startup(sender As Object, e As StartupEventArgs)
        ' This will enable you to bind controls in XAML to WebContext.Current properties.
        Resources.Add("WebContext", WebContext.Current)

        ' This will automatically authenticate a user when using Windows authentication or when the user chose "Keep me signed in" on a previous login attempt.
        WebContext.Current.Authentication.LoadUser(AddressOf Application_UserLoaded, Nothing)

        RootVisual = New MainPage()
    End Sub

    ''' <summary>
    ''' Invoked when the <see cref="LoadUserOperation"/> completes.
    ''' Use this event handler to switch from the "loading UI" you created in <see cref="InitializeRootVisual"/> to the "application UI".
    ''' </summary>
    Private Sub Application_UserLoaded(operation As LoadUserOperation)

        If operation.HasError Then
            ErrorWindow.Show(operation.Error)
            operation.MarkErrorAsHandled()
        End If

    End Sub

    Private Sub Application_UnhandledException(sender As Object, e As ApplicationUnhandledExceptionEventArgs)

        If Not System.Diagnostics.Debugger.IsAttached Then
            e.Handled = True
            ErrorWindow.Show(e.ExceptionObject)
        End If

    End Sub

End Class