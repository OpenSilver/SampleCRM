Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel

Namespace Web
    Partial Public Class Products
        Inherits Entity

        Protected Overrides Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
            If e.PropertyName = NameOf(CategoryID) Then
                OnPropertyChanged(New PropertyChangedEventArgs("CategoryName"))
            End If
        End Sub

        Public ReadOnly Property IsNew As Boolean
            Get
                Return String.IsNullOrEmpty(ProductID)
            End Get
        End Property

        Private _categoriesCombo As IEnumerable(Of Categories)

        Public Property CategoriesCombo As IEnumerable(Of Categories)
            Get
                Return _categoriesCombo
            End Get
            Set(ByVal value As IEnumerable(Of Categories))

                If Not _categoriesCombo.Equals(value) Then
                    _categoriesCombo = value
                    OnPropertyChanged(New PropertyChangedEventArgs("CategoriesCombo"))
                    OnPropertyChanged(New PropertyChangedEventArgs("CategoryName"))
                End If
            End Set
        End Property

        Public ReadOnly Property CategoryName As String
            Get
                Return CategoriesCombo.FirstOrDefault(Function(x) x.CategoryID = CategoryID)?.Name
            End Get
        End Property
    End Class
End Namespace
