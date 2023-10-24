Imports System.ComponentModel.DataAnnotations

Namespace Attributes
    Public Class RestrictAccessDeveloperMode
        Inherits AuthorizationAttribute

        Protected Overrides Function IsAuthorized(ByVal principal As System.Security.Principal.IPrincipal, ByVal authorizationContext As AuthorizationContext) As AuthorizationResult
            If OpenSilverBusinessApp.DeveloperMode Then
                Return AuthorizationResult.Allowed
            Else
                Return New AuthorizationResult("DB is readonly for production mode")
            End If
        End Function
    End Class
End Namespace
