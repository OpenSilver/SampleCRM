Imports OpenRiaServices.DomainServices.Client
Imports OpenSilver.Controls
Imports SampleCRM.Web.SampleCRM.Web
Imports System.Windows

Namespace Views
    Partial Public Class CustomerAddEdit
        Inherits BaseUserControl

        Public Event CustomerDeleted As EventHandler
        Public Event CustomerAdded As EventHandler

        Private _customerViewModel As New Web.Customers()
        Public Property CustomerViewModel As Web.Customers
            Get
                Return _customerViewModel
            End Get
            Set(value As Web.Customers)
                If _customerViewModel IsNot value Then
                    _customerViewModel = value
                    OnPropertyChanged()
#If DEBUG Then
                    Console.WriteLine($"CustomerAddEdit, Customer: {value.FirstName} selected")
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

        Private Async Sub btnEditPicture_Click(sender As Object, e As RoutedEventArgs)
            Dim fileDialog = New OpenFileDialog()
            fileDialog.Filter = "All Images Files (*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif)|*.png;*.jpeg;*.gif;*.jpg;*.bmp;*.tiff;*.tif" +
                                "|PNG Portable Network Graphics (*.png)|*.png" +
                                "|JPEG File Interchange Format (*.jpg *.jpeg *jfif)|*.jpg;*.jpeg;*.jfif" +
                                "|BMP Windows Bitmap (*.bmp)|*.bmp" +
                                "|TIF Tagged Imaged File Format (*.tif *.tiff)|*.tif;*.tiff" +
                                "|GIF Graphics Interchange Format (*.gif)|*.gif"

            Dim result = Await fileDialog.ShowDialogAsync()
            If Not result.GetValueOrDefault() Then
                Return
            End If

            Dim imageFile = fileDialog.File
            If imageFile.Length < 1 Then
                Return
            End If

            Using fileStream = imageFile.OpenRead()
                Dim buffer = New Byte(fileStream.Length - 1) {}
                Await fileStream.ReadAsync(buffer, 0, buffer.Length)
                CustomerViewModel.Picture = buffer
            End Using
        End Sub

        Private Sub formPersonalInfo_EditEnded(sender As Object, e As System.Windows.Controls.DataFormEditEndedEventArgs)

        End Sub

        Private Sub formAddress_EditEnded(sender As Object, e As System.Windows.Controls.DataFormEditEndedEventArgs)

        End Sub

        Private Sub formDemographic_EditEnded(sender As Object, e As System.Windows.Controls.DataFormEditEndedEventArgs)

        End Sub

        Public Sub Save(customersContext As CustomersContext)
            If customersContext.Customers.CanAdd Then
                If IsMobileUI Then
                    If Not formPersonalInfo.CommitEdit() Then
                        ErrorWindow.Show("Invalid Personal Info")
                        Return
                    End If

                    If Not formAddress.CommitEdit() Then
                        ErrorWindow.Show("Invalid Address Info")
                        Return
                    End If

                    If Not formDemographic.CommitEdit() Then
                        ErrorWindow.Show("Invalid Demographic")
                        Return
                    End If
                Else
                    If Not mFormPersonalInfo.CommitEdit() Then
                        ErrorWindow.Show("Invalid Personal Info")
                        Return
                    End If

                    If Not mFormAddress.CommitEdit() Then
                        ErrorWindow.Show("Invalid Address Info")
                        Return
                    End If

                    If Not mFormDemographic.CommitEdit() Then
                        ErrorWindow.Show("Invalid Demographic")
                        Return
                    End If
                End If

                customersContext.Customers.Add(CustomerViewModel)
                customersContext.SubmitChanges(AddressOf OnAddSubmitCompleted, Nothing)
            Else
                Throw New AccessViolationException("RIA Service Add Entity for Customer Context is denied")
            End If
        End Sub

        Private Sub btnDelete_Click(sender As Object, e As RoutedEventArgs)
            Dim customersContext = New CustomersContext()
            If customersContext.Customers.CanRemove Then
                customersContext.Customers.Remove(CustomerViewModel)
                customersContext.SubmitChanges(AddressOf OnDeleteSubmitCompleted, Nothing)
            Else
                Throw New AccessViolationException("RIA Service Delete Entity for Customer Context is denied")
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
                RaiseEvent CustomerDeleted(Me, New EventArgs())
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
                RaiseEvent CustomerAdded(Me, New EventArgs())
            End If
        End Sub
    End Class
End Namespace


