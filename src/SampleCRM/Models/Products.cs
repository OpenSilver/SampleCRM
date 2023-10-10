using OpenRiaServices.DomainServices.Client;
using System.Collections.Generic;
using System.ComponentModel;

namespace SampleCRM.Web.Models
{
    public partial class Products : Entity
    {
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
                }
            }
        }
    }
}