Imports OpenRiaServices.DomainServices.Client

Namespace Web
    Partial Public Class TaxTypes
        Inherits Entity

        Public ReadOnly Property Desc As String
            Get
                Return $"{Name} ({Rate})"
            End Get
        End Property
    End Class
End Namespace
