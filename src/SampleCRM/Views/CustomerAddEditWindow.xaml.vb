Imports System.Windows
Imports SampleCRM.Web.SampleCRM.Web

Namespace Views
    Partial Public Class CustomerAddEditWindow
        Inherits BaseChildWindow

        Private _customerContext As CustomersContext

        Public Overloads Shared Async Function Show(customer As Web.Customers, customersContext As CustomersContext) As Task(Of Boolean)
            Dim window = New CustomerAddEditWindow(customer, customersContext)
            Await window.ShowAndWait()
            Return window.DialogResult.GetValueOrDefault(False)
        End Function

        Public Sub New()
            InitializeComponent()
            InnerControl = customerAddEditView
        End Sub

        Public Sub New(customer As Web.Customers, customersContext As CustomersContext)
            Me.New()
            _customerContext = customersContext
            customerAddEditView.CustomerViewModel = customer
            Title = If(customer.IsNew, "Add Customer", "Edit Customer")
        End Sub

        Private Sub OKButton_Click(sender As Object, e As RoutedEventArgs)
            customerAddEditView.Save(_customerContext)
            'DialogResult = True
        End Sub

        Private Sub CancelButton_Click(sender As Object, e As RoutedEventArgs)
            DialogResult = False
        End Sub

        Private Sub customerAddEditView_CustomerDeleted(sender As Object, e As EventArgs)
            DialogResult = True
        End Sub

        Private Sub customerAddEditView_CustomerAdded(sender As Object, e As EventArgs)
            DialogResult = True
        End Sub
    End Class
End Namespace
