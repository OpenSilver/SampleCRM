using OpenRiaServices.DomainServices.Client;
using System.ComponentModel;
using System.Windows;

namespace SampleCRM.Web.Models
{
    public partial class Customers : Entity
    {
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(FirstName):
                case nameof(LastName):
                    RaisePropertyChanged(nameof(FullName));
                    RaisePropertyChanged(nameof(Initials));
                    break;
                case nameof(AddressLine1):
                case nameof(AddressLine2):
                case nameof(City):
                case nameof(Region):
                case nameof(PostalCode):
                case nameof(CountryName):
                    RaisePropertyChanged(nameof(FullAddress));
                    break;
            }

            base.OnPropertyChanged(e);
        }

        public bool IsNew => CustomerID <= 0;

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsEditMode)));
                }
            }
        }

        private string _countryName;
        public string CountryName
        {
            get => _countryName;
            set
            {
                if (_countryName != value)
                {
                    _countryName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(CountryName)));
                }
            }
        }

        public string FullName => $"{FirstName} {LastName}";

        public string Initials => string.Format("{0}{1}", $"{FirstName} "[0], $"{LastName} "[0]).Trim().ToUpper();

        public string FullAddress => $"{AddressLine1} {AddressLine2}\n{City}, {Region} {PostalCode}\n{CountryName}";

        public string PictureUrl => $"{(Application.Current as App).ImageUrl}?customerid={CustomerID}";
    }
}