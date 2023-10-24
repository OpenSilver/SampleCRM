Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel

Namespace Web
    Partial Public Class Customers
        Inherits Entity

        Protected Overrides Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
            If e.PropertyName = "FirstName" OrElse e.PropertyName = "LastName" Then
                RaisePropertyChanged("FullName")
                RaisePropertyChanged("Initials")
            ElseIf e.PropertyName = "AddressLine1" OrElse e.PropertyName = "AddressLine2" OrElse e.PropertyName = "City" OrElse e.PropertyName = "Region" OrElse e.PropertyName = "PostalCode" OrElse e.PropertyName = "CountryName" Then
                RaisePropertyChanged("FullAddress")
            End If

            MyBase.OnPropertyChanged(e)
        End Sub

        Private _countryCodes As IEnumerable(Of CountryCodes)

        Public Property CountryCodes As IEnumerable(Of CountryCodes)
            Get
                Return _countryCodes
            End Get
            Set(ByVal value As IEnumerable(Of CountryCodes))

                If Not _countryCodes.Equals(value) Then
                    _countryCodes = value
                    OnPropertyChanged(New PropertyChangedEventArgs("CountryCodes"))
                End If
            End Set
        End Property

        Public ReadOnly Property IsNew As Boolean
            Get
                Return CustomerID <= 0
            End Get
        End Property

        Private _isEditMode As Boolean

        Public Property IsEditMode As Boolean
            Get
                Return _isEditMode
            End Get
            Set(ByVal value As Boolean)

                If _isEditMode <> value Then
                    _isEditMode = value
                    OnPropertyChanged(New PropertyChangedEventArgs("IsEditMode"))
                End If
            End Set
        End Property

        Private _countryName As String

        Public Property CountryName As String
            Get
                Return _countryName
            End Get
            Set(ByVal value As String)

                If _countryName <> value Then
                    _countryName = value
                    OnPropertyChanged(New PropertyChangedEventArgs("CountryName"))
                End If
            End Set
        End Property

        Public ReadOnly Property FullName As String
            Get
                Return $"{FirstName} {LastName}"
            End Get
        End Property

        Public ReadOnly Property Initials As String
            Get
                Return String.Format("{0}{1}", $"{FirstName} "(0), $"{LastName} "(0)).Trim().ToUpper()
            End Get
        End Property

        Public ReadOnly Property FullAddress As String
            Get
                Return $"{AddressLine1} {AddressLine2}\n{City}, {Region} {PostalCode}\n{CountryName}"
            End Get
        End Property
    End Class
End Namespace
