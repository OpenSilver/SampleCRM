using OpenRiaServices.DomainServices.Client;
using SampleCRM.Web.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SampleCRM.Web.Models
{
    public partial class Product : Entity
    {
        partial void OnCategoryIDChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(CategoryName)));
        }

        public bool IsNew => string.IsNullOrEmpty(ProductID);

        private static IEnumerable<Category> _categoriesCombo;
        public IEnumerable<Category> CategoriesCombo
        {
            get
            {
                if (_categoriesCombo == null)
                {
                    var context = new CategoryContext();
                    context.Load(context.GetCategoriesQuery(), op =>
                    {
                        CategoriesCombo = op.Entities;
                    });
                }
                return _categoriesCombo;
            }
            private set
            {
                if (_categoriesCombo != value)
                {
                    _categoriesCombo = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(CategoriesCombo)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(CategoryName)));
                }
            }
        }

        public string CategoryName => CategoriesCombo?.FirstOrDefault(x => x.CategoryID == CategoryID)?.Name;

        public string PictureUrl
        {
            get
            {
                var app = (System.Windows.Application.Current as App);
                var imageUrl = app.ImageUrl;
                return $"{imageUrl}?productid={ProductID}";
            }
        }
    }
}