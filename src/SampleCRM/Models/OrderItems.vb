Imports OpenRiaServices.DomainServices.Client
Imports System.ComponentModel

Namespace Web
    Partial Public Class OrderItems
        Inherits Entity

        Protected Overrides Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
            If e.PropertyName = "UnitPrice" OrElse e.PropertyName = "Quantity" Then
                RaisePropertyChanged("Subtotal")
            ElseIf e.PropertyName = "Subtotal" OrElse e.PropertyName = "Discount" Then
                RaisePropertyChanged("Total")
            ElseIf e.PropertyName = "OrderLine" Then
                RaisePropertyChanged("IsNew")
            ElseIf e.PropertyName = NameOf(ProductID) Then

                If ProductsCombo IsNot Nothing AndAlso ProductsCombo.Any() Then
                    Product = ProductsCombo.FirstOrDefault(Function(x) x.ProductID = ProductID)
                End If
            End If

            MyBase.OnPropertyChanged(e)
        End Sub

        Public ReadOnly Property IsNew As Boolean
            Get
                Return OrderLine <= 0
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

        Private _taxTypes As IEnumerable(Of TaxTypes)

        Public Property TaxTypes As IEnumerable(Of TaxTypes)
            Get
                Return _taxTypes
            End Get
            Set(ByVal value As IEnumerable(Of TaxTypes))

                If Not _taxTypes.Equals(value) Then
                    _taxTypes = value
                    OnPropertyChanged(New PropertyChangedEventArgs("TaxTypes"))
                End If
            End Set
        End Property

        Private _taxRate As Decimal

        Public Property TaxRate As Decimal
            Get
                Return _taxRate
            End Get
            Set(ByVal value As Decimal)

                If _taxRate <> value Then
                    _taxRate = value
                    OnPropertyChanged(New PropertyChangedEventArgs("TaxRate"))
                    OnPropertyChanged(New PropertyChangedEventArgs("Total"))
                End If
            End Set
        End Property

        Private _productsCombo As IEnumerable(Of Products)

        Public Property ProductsCombo As IEnumerable(Of Products)
            Get
                Return _productsCombo
            End Get
            Set(ByVal value As IEnumerable(Of Products))

                If Not _productsCombo.Equals(value) Then
                    _productsCombo = value
                    OnPropertyChanged(New PropertyChangedEventArgs("ProductsCombo"))
                End If
            End Set
        End Property

        Private _product As Products

        Public Property Product As Products
            Get
                Return _product
            End Get
            Set(ByVal value As Products)

                If Not _product.Equals(value) Then
                    _product = value
                    OnPropertyChanged(New PropertyChangedEventArgs("Product"))
                End If
            End Set
        End Property

        Public ReadOnly Property Subtotal As Decimal
            Get
                Return Quantity * Convert.ToDecimal(UnitPrice)
            End Get
        End Property

        Public ReadOnly Property Total As Decimal
            Get
                Return (Subtotal - Convert.ToDecimal(Discount)) * (1 + TaxRate / 100D)
            End Get
        End Property
    End Class
End Namespace
