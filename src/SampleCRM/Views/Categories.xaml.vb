Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports System.Collections.ObjectModel
Imports OpenRiaServices.DomainServices.Client
Imports SampleCRM.Web.SampleCRM.Web

Namespace Views

    Partial Public Class Categories
        Inherits BasePage
        Private _categoryContext As New CategoryContext()

        Private _categoryCollection As IEnumerable(Of Web.Categories) = New ObservableCollection(Of Web.Categories)()
        Public Property CategoryCollection As IEnumerable(Of Web.Categories)
            Get
                Return _categoryCollection
            End Get
            Set(value As IEnumerable(Of Web.Categories))
                If _categoryCollection IsNot value Then
                    _categoryCollection = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Private _selectedCategory As Categories
        Public Property SelectedCategory As Categories
            Get
                Return _selectedCategory
            End Get
            Set(value As Categories)
                If _selectedCategory IsNot value Then
                    _selectedCategory = value
                    OnPropertyChanged()
                End If
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
            DataContext = Me
        End Sub

        Protected Overrides Sub OnSizeChanged(sender As Object, e As SizeChangedEventArgs)
            MyBase.OnSizeChanged(sender, e)
            gridCategories.Columns.Last().Visibility = BoolToVisibilityConverter.Convert(Not IsMobileUI)
        End Sub

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            LoadElements()
        End Sub

        Private Async Sub LoadElements()
            Dim categoriesQuery = _categoryContext.GetCategoriesQuery()
            Dim categoriesOp = Await _categoryContext.LoadAsync(categoriesQuery)
            CategoryCollection = categoriesOp.Entities
#If DEBUG Then
            Console.WriteLine("Categories Collection:" & CategoryCollection.Count())
            For Each item In CategoryCollection
                Console.WriteLine("Category Name:" & item.Name)
                Console.WriteLine("Category Picture Bytes:" & item.Picture.Length)
            Next
#End If
        End Sub

        Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs)
            _categoryContext.SubmitChanges(AddressOf OnSubmitCompleted, Nothing)
        End Sub

        Private Sub RejectButton_Click(sender As Object, e As RoutedEventArgs)
            _categoryContext.RejectChanges()
            CheckChanges()
        End Sub

        Private Sub gridCategories_RowEditEnded(sender As Object, e As DataGridRowEditEndedEventArgs)
            CheckChanges()
        End Sub

        Private Sub formCategories_EditEnded(sender As Object, e As DataFormEditEndedEventArgs)
            CheckChanges()
        End Sub

        Private Sub CheckChanges()
            Dim changeSet As EntityChangeSet = _categoryContext.EntityContainer.GetChanges()
            ChangeText.Text = changeSet.ToString()

            Dim hasChanges As Boolean = _categoryContext.HasChanges
            SaveButton.IsEnabled = hasChanges
            RejectButton.IsEnabled = hasChanges
        End Sub

        Private Sub OnSubmitCompleted(so As SubmitOperation)
            If so.HasError Then
                If so.Error.Message.StartsWith("Submit operation failed. Access to operation") Then
                    ErrorWindow.Show("Access Denied", "Insufficient User Role", so.Error.Message)
                Else
                    ErrorWindow.Show("Access Denied", so.Error.Message, "")
                End If
                so.MarkErrorAsHandled()
            End If
            CheckChanges()
        End Sub
    End Class

End Namespace