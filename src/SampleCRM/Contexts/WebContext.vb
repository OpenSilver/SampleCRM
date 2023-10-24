Imports OpenRiaServices.DomainServices.Client.ApplicationServices

Namespace Web

    ''' <summary>
    ''' Context for the RIA application.
    ''' </summary>
    ''' <remarks>
    ''' This context extends the base to make application services and types available
    ''' for consumption from code and xaml.
    ''' </remarks>
    Partial Public NotInheritable Class WebContext
        Inherits WebContextBase

#Region "Extensibility Method Definitions"

        ''' <summary>
        ''' This method is invoked from the constructor once initialization is complete and
        ''' can be used for further object setup.
        ''' </summary>
        Partial Private Sub OnCreated()
        End Sub

#End Region

        ''' <summary>
        ''' Initializes a new instance of the WebContext class.
        ''' </summary>
        Public Sub New()
            Me.OnCreated()
        End Sub

        ''' <summary>
        ''' Gets the context that is registered as a lifetime object with the current application.
        ''' </summary>
        ''' <exception cref="InvalidOperationException"> is thrown if there is no current application,
        ''' no contexts have been added, or more than one context has been added.
        ''' </exception>
        ''' <seealso cref="System.Windows.Application.ApplicationLifetimeObjects"/>
        ' TODO: Check if we can use Shadows
        Public Shared Shadows ReadOnly Property Current As WebContext
            Get
                Return CType(WebContextBase.Current, WebContext)
            End Get
        End Property

        ''' <summary>
        ''' Gets a user representing the authenticated identity.
        ''' </summary>
        ' TODO: Check if we can use Shadows
        Public Shadows ReadOnly Property User As User
            Get
                Return CType(MyBase.User, User)
            End Get
        End Property

    End Class

End Namespace