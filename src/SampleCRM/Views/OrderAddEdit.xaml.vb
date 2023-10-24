Imports OpenRiaServices.DomainServices.Client
Imports SampleCRM.Web.SampleCRM.Web
Imports System.Windows
Imports System.Windows.Controls

Namespace Views
    Partial Public Class OrderAddEdit
        Inherits BaseUserControl

        Public Event OrderDeleted As EventHandler
        Public Event OrderAdded As EventHandler

        Private _order As Web.Orders = New Web.Orders()
        Public Property Order As Web.Orders
            Get
                Return _order
            End Get
            Set(value As Web.Orders)
                If Not Object.Equals(_order, value) Then
                    _order = value
                    OnPropertyChanged()
#If DEBUG Then
                    Console.WriteLine($"OrderItemAddEdit, Item: {value.OrderID} selected")
#End If
                End If
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
            DataContext = Me
        End Sub

        Public Overrides Sub ArrangeLayout()
            MyBase.ArrangeLayout()
            grdNarrow.Visibility = BoolToVisibilityConverter.Convert(IsMobileUI)
            grdWide.Visibility = BoolToVisibilityConverter.Convert(Not IsMobileUI)
        End Sub

        Public Sub Save(context As OrderContext)
            If context.Orders.CanAdd AndAlso Order.IsNew OrElse context.Orders.CanEdit Then
                If IsMobileUI Then
                    If Not mFormCustomerInfo.CommitEdit() Then
                        ErrorWindow.Show("Invalid Customer Info")
                        Return
                    End If

                    If Not mFormOrderStatus.CommitEdit() Then
                        ErrorWindow.Show("Invalid Order Status")
                        Return
                    End If
                Else
                    If Not formCustomerInfo.CommitEdit() Then
                        ErrorWindow.Show("Invalid Customer Info")
                        Return
                    End If

                    If Not formOrderStatus.CommitEdit() Then
                        ErrorWindow.Show("Invalid Order Status")
                        Return
                    End If
                End If

                If Order.IsNew Then
                    context.Orders.Add(Order)
                End If

                context.SubmitChanges(AddressOf OnAddSubmitCompleted, Nothing)
            Else
                Throw New AccessViolationException("RIA Service Add / Edit Order for Order Context is denied")
            End If
        End Sub

        Private Sub OnDeleteSubmitCompleted(so As SubmitOperation)
            If so.HasError Then
                If so.Error.Message.StartsWith("Submit operation failed. Access to operation") Then
                    ErrorWindow.Show("Access Denied", "Insufficient User Role", so.Error.Message)
                Else
                    ErrorWindow.Show("Access Denied", so.Error.Message, "")
                End If
                so.MarkErrorAsHandled()
            Else
                RaiseEvent OrderDeleted(Me, New EventArgs())
            End If
        End Sub

        Private Sub OnAddSubmitCompleted(so As SubmitOperation)
            If so.HasError Then
                If so.Error.Message.StartsWith("Submit operation failed. Access to operation") Then
                    ErrorWindow.Show("Access Denied", "Insufficient User Role", so.Error.Message)
                Else
                    ErrorWindow.Show("Access Denied", so.Error.Message, "")
                End If
                so.MarkErrorAsHandled()
            Else
                RaiseEvent OrderAdded(Me, New EventArgs())
            End If
        End Sub

        Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs)
            Dim context = New OrderContext()
            If context.Orders.CanRemove Then
                context.Orders.Remove(Order)
                context.SubmitChanges(AddressOf OnDeleteSubmitCompleted, Nothing)
            Else
                Throw New AccessViolationException("RIA Service Delete Entity for Order Context is denied")
            End If
        End Sub

        Private Sub formCustomerInfo_EditEnded(sender As Object, e As DataFormEditEndedEventArgs)

        End Sub

        Private Sub formOrderStatus_EditEnded(sender As Object, e As DataFormEditEndedEventArgs)

        End Sub
    End Class
End Namespace
