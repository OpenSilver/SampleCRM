using OpenRiaServices.DomainServices.Client;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SampleCRM.Web.Models
{
    public partial class Products : Entity
    {
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CategoryID))
            {
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(CategoryName)));
            }

            base.OnPropertyChanged(e);
        }

        public bool IsNew => string.IsNullOrEmpty(ProductID);

        private IEnumerable<Categories> _categoriesCombo;
        public IEnumerable<Categories> CategoriesCombo
        {
            get { return _categoriesCombo; }
            set
            {
                if (_categoriesCombo != value)
                {
                    _categoriesCombo = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(CategoriesCombo)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(CategoryName)));
                }
            }
        }

        public string CategoryName => CategoriesCombo.FirstOrDefault(x => x.CategoryID == CategoryID)?.Name;

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