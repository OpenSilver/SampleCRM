using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenRiaServices.DomainServices.Client;
using SampleCRM.Web.Views;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SampleCRM.Web.Models
{
    public partial class CategoriesPageVM : ObservableObject
    {
        #region Properties
        private readonly CategoryContext _categoryContext = new();

        [ObservableProperty]
        private IEnumerable<Category> categoryCollection;

        [ObservableProperty]
        private Category selectedCategory;

        [ObservableProperty]
        private bool saveEnabled;

        [ObservableProperty]
        private bool rejectEnabled;
        #endregion

        #region Commands

        [RelayCommand]
        public async Task Initialize() 
            => await AsyncHelper.RunAsync(async () 
                => CategoryCollection = (await _categoryContext.LoadAsync(_categoryContext.GetCategoriesQuery())).Entities);

        [RelayCommand]
        public void Save() => _categoryContext.SubmitChanges(OnSubmitCompleted, null);

        [RelayCommand]
        public void Reject()
        {
            _categoryContext.RejectChanges();
            CheckChanges();
        }

        [RelayCommand]
        public void FormEditEnded(DataGridEditAction e) => CheckChanges();

        [RelayCommand]
        public void FormRowEditEnded(DataGridEditAction e) => CheckChanges();

        private void CheckChanges()
        {
            var hasChanges = _categoryContext.HasChanges;
            SaveEnabled = hasChanges;
            RejectEnabled = hasChanges;
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
        #endregion
    }
}