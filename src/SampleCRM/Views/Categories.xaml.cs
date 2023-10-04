using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Categories: BasePage
    {
        private CategoryContext _categoryContext = new CategoryContext();

        private IEnumerable<Models.Categories> _categoryCollection = new ObservableCollection<Models.Categories>();
        public IEnumerable<Models.Categories> CategoryCollection
        {
            get { return _categoryCollection; }
            set
            {
                if (_categoryCollection != value)
                {
                    _categoryCollection = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private Models.Categories _selectedCategory;
        public Models.Categories SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                }
            }
        }

        public Categories()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadElements();
        }

        private async void LoadElements()
        {
            var categoriesQuery = _categoryContext.GetCategoriesQuery();
            var categoriesOp = await _categoryContext.LoadAsync(categoriesQuery);
            CategoryCollection = categoriesOp.Entities;
#if DEBUG
            Console.WriteLine("Categories Collection:" + CategoryCollection.Count());
            foreach (var item in CategoryCollection)
            {
                Console.WriteLine("Category Name:" + item.Name);
                Console.WriteLine("Category Picture Bytes:" + item.Picture.Length);
            }
#endif
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _categoryContext.SubmitChanges(OnSubmitCompleted, null);
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            _categoryContext.RejectChanges();
            CheckChanges();
        }

        private void gridCategories_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {
            CheckChanges();
        }

        private void formCategories_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            CheckChanges();
        }

        private void CheckChanges()
        {
            EntityChangeSet changeSet = _categoryContext.EntityContainer.GetChanges();
            ChangeText.Text = changeSet.ToString();

            bool hasChanges = _categoryContext.HasChanges;
            SaveButton.IsEnabled = hasChanges;
            RejectButton.IsEnabled = hasChanges;
        }

        private void OnSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message));
                so.MarkErrorAsHandled();
            }
            CheckChanges();
        }
    }
}