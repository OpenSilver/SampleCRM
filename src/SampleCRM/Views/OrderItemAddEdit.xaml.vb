Imports System.Windows
Imports OpenRiaServices.DomainServices.Client
Imports SampleCRM.Web.SampleCRM.Web

Namespace Views
    Partial Public Class OrderItemAddEdit
        Inherits BaseUserControl

        Public Event ItemDeleted As EventHandler
        Public Event ItemAdded As EventHandler

        Private _item As Web.OrderItems = New Web.OrderItems()
        Public Property Item() As Web.OrderItems
            Get
                Return _item
            End Get
            Set(ByVal value As Web.OrderItems)
                If _item IsNot value Then
                    _item = value
                    OnPropertyChanged()
#If DEBUG Then
                    Console.WriteLine($"OrderItemAddEdit, Item: {value.ProductID} selected")
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

        Public Sub Save(context As OrderItemsContext)
            If context.OrderItems.CanAdd AndAlso Item.IsNew OrElse context.OrderItems.CanEdit Then
                If IsMobileUI Then
                    If Not mForm.CommitEdit() Then
                        ErrorWindow.Show("Invalid Order Item")
                        Return
                    End If
                Else
                    If Not form.CommitEdit() Then
                        ErrorWindow.Show("Invalid Order Item")
                        Return
                    End If
                End If

                If Item.IsNew Then
                    context.OrderItems.Add(Item)
                End If

                context.SubmitChanges(AddressOf OnAddSubmitCompleted, Nothing)
            Else
                Throw New AccessViolationException("RIA Service Add / Edit Entity for Order Item Context is denied")
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
                RaiseEvent ItemDeleted(Me, New EventArgs())
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
                RaiseEvent ItemAdded(Me, New EventArgs())
            End If
        End Sub

        Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs)
            Dim context = New OrderItemsContext()
            If context.OrderItems.CanRemove Then
                context.OrderItems.Remove(Item)
                context.SubmitChanges(AddressOf OnDeleteSubmitCompleted, Nothing)
            Else
                Throw New AccessViolationException("RIA Service Delete Entity for Order Item Context is denied")
            End If
        End Sub

        Private Sub form_EditEnded(sender As Object, e As System.Windows.Controls.DataFormEditEndedEventArgs)
        End Sub
    End Class
End Namespace
