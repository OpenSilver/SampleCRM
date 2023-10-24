Namespace Global.SampleCRM.Web

    ''' <summary>
    ''' Partial class extending the User type that adds shared properties and methods.
    ''' These properties and methods will be available both to the server app and the client app.
    ''' </summary>
    Partial Public Class User

        ''' <summary>
        ''' Returns the user display name, which by default is its FriendlyName.
        ''' If FriendlyName is not set, the User Name is returned.
        ''' </summary>
        Public ReadOnly Property DisplayName As String
            Get
                Return If(Not String.IsNullOrEmpty(Me.FriendlyName), Me.FriendlyName, Me.Name)
            End Get
        End Property

    End Class

End Namespace