﻿Imports OpenRiaServices.DomainServices.Hosting
Imports OpenRiaServices.DomainServices.Server.ApplicationServices

' TODO: Switch to a secure endpoint when deploying the application.
'       The user's name and password should only be passed using https.
'       To do this, set the RequiresSecureEndpoint property on EnableClientAccessAttribute to true.
'
'       [EnableClientAccess(RequiresSecureEndpoint = true)]
'
'       More information on using https with a Domain Service can be found on MSDN.

''' <summary>
''' Domain Service responsible for authenticating users when they log on to the application.
'''
''' Most of the functionality is already provided by the AuthenticationBase class.
''' </summary>
<EnableClientAccess>
Public Class AuthenticationService
    Inherits AuthenticationBase(Of User)
End Class