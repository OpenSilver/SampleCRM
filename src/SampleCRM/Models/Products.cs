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
                OnPropertyChanged(new PropertyChangedEventArgs("CategoryName"));
            }
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
                    OnPropertyChanged(new PropertyChangedEventArgs("CategoriesCombo"));
                    OnPropertyChanged(new PropertyChangedEventArgs("CategoryName"));
                }
            }
        }

        public string CategoryName => CategoriesCombo.FirstOrDefault(x => x.CategoryID == CategoryID)?.Name;
    }
}