using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
    public partial class Categories : BasePage
    {
        private CategoryContext _categoryContext = new CategoryContext();


        public IEnumerable<Models.Categories> CategoryCollection
        {
            get { return (IEnumerable<Models.Categories>)GetValue(CategoryCollectionProperty); }
            set { SetValue(CategoryCollectionProperty, value); }
        }
        public static readonly DependencyProperty CategoryCollectionProperty =
            DependencyProperty.Register("CategoryCollection", typeof(IEnumerable<Models.Categories>), typeof(Categories), new PropertyMetadata(null));

        public Models.Categories SelectedCategory
        {
            get { return (Models.Categories)GetValue(SelectedCategoryProperty); }
            set { SetValue(SelectedCategoryProperty, value); }
        }
        public static readonly DependencyProperty SelectedCategoryProperty =
            DependencyProperty.Register("SelectedCategory", typeof(Models.Categories), typeof(Categories), new PropertyMetadata(null));


        public Categories()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.OnSizeChanged(sender, e);
            gridCategories.Columns.Last().Visibility = BoolToVisibilityConverter.Convert(!IsMobileUI);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await AsyncHelper.RunAsync(LoadElements);
        }

        private async Task LoadElements()
        {
            CategoryCollection = (await _categoryContext.LoadAsync(_categoryContext.GetCategoriesQuery())).Entities;
#if DEBUG
            Console.WriteLine("Categories Collection:" + CategoryCollection.Count());
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
            var hasChanges = _categoryContext.HasChanges;
            SaveButton.IsEnabled = hasChanges;
            RejectButton.IsEnabled = hasChanges;
        }

        private void OnSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                if (so.Error.Message.StartsWith("Submit operation failed. Access to operation"))
                {
                    ErrorWindow.Show("Access Denied", "Insuficient User Role", so.Error.Message);
                }
                else
                {
                    ErrorWindow.Show("Access Denied", so.Error.Message, "");
                }
                so.MarkErrorAsHandled();
            }
            CheckChanges();
        }
    }
}