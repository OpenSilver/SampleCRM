Imports OpenRiaServices.DomainServices.Server

Namespace SampleCRM.Web
    Public MustInherit Class SampleCRMService
        Inherits DomainService

        Protected _context As SampleCRMEntities = New SampleCRMEntities()
    End Class
End Namespace
