Imports System.Windows
Imports SampleCRM.Web.SampleCRM.Web

Namespace Views
    Partial Public Class OrderAddEditWindow
        Inherits BaseChildWindow

        Private _context As OrderContext

        Public Overloads Shared Async Function Show(order As Web.Orders, context As OrderContext) As Task(Of Boolean)
            Dim window = New OrderAddEditWindow(order, context)
            Await window.ShowAndWait()
            Return window.DialogResult.GetValueOrDefault(False)
        End Function

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(orderItem As Web.Orders, context As OrderContext)
            Me.New()
            _context = context
            orderAddEditView.Order = orderItem
            Title = If(orderItem.IsNew, "Add Order", "Edit Order")
        End Sub

        Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs)
            orderAddEditView.Save(_context)
        End Sub

        Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)
            DialogResult = False
        End Sub

        Private Sub orderAddEditView_OrderAdded(sender As Object, e As EventArgs)
            DialogResult = True
        End Sub

        Private Sub orderAddEditView_OrderDeleted(sender As Object, e As EventArgs)
            DialogResult = True
        End Sub
    End Class
End Namespace
