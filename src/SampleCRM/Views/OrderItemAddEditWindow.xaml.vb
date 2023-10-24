Imports System.Windows
Imports SampleCRM.Web.SampleCRM.Web

Namespace Views
    Partial Public Class OrderItemAddEditWindow
        Inherits BaseChildWindow

        Private _context As OrderItemsContext

        Public Overloads Shared Async Function Show(orderItem As Web.OrderItems, context As OrderItemsContext) As Task(Of Boolean)
            Dim window = New OrderItemAddEditWindow(orderItem, context)
            Await window.ShowAndWait()
            Return window.DialogResult.GetValueOrDefault(False)
        End Function

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(orderItem As Web.OrderItems, context As OrderItemsContext)
            Me.New()
            _context = context
            orderItemAddEditView.Item = orderItem
            Title = If(orderItem.IsNew, "Add Order Item", "Edit Order Item")
        End Sub

        Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs)
            orderItemAddEditView.Save(_context)
        End Sub

        Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)
            DialogResult = False
        End Sub

        Private Sub orderItemAddEdit_ItemAdded(sender As Object, e As System.EventArgs)
            DialogResult = True
        End Sub

        Private Sub orderItemAddEdit_ItemDeleted(sender As Object, e As System.EventArgs)
            DialogResult = True
        End Sub
    End Class
End Namespace
