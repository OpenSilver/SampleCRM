using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenRiaServices.DomainServices.Client;
using SampleCRM.Web.Views;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private bool hasChanges;
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
        public void CheckChanges()
        {
            HasChanges = _categoryContext.HasChanges;
        }

        private void OnSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                if (so.Error.Message.StartsWith("Submit operation failed. Access to operation"))
                {
                    ErrorWindow.Show("Access Denied", "Insufficient User Role", so.Error.Message);
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