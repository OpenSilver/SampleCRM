using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SampleCRM.Web.Models
{
    public partial class Customers : Entity
    {
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "FirstName" 
                || e.PropertyName == "LastName")
            {
                RaisePropertyChanged("FullName");
                RaisePropertyChanged("Initials");
            } 
            else if (e.PropertyName == "AddressLine1" 
                || e.PropertyName == "AddressLine2"
                || e.PropertyName == "City"
                || e.PropertyName == "Region"
                || e.PropertyName == "PostalCode"
                || e.PropertyName == "CountryName")
            {
                RaisePropertyChanged("FullAddress");
            }

            base.OnPropertyChanged(e);
        }

        private IEnumerable<Models.CountryCodes> _countryCodes;
        public IEnumerable<Models.CountryCodes> CountryCodes
        {
            get { return _countryCodes; }
            set
            {
                if (_countryCodes != value)
                {

                    _countryCodes = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CountryCodes"));
                }
            }
        }

        public bool IsNew => CustomerID <= 0;

        private bool _isEditMode;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsEditMode"));
                }
            }
        }

        private string _countryName;
        public string CountryName
        {
            get { return _countryName; }
            set
            {
                if (_countryName != value)
                {

                    _countryName = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CountryName"));
                }
            }
        }

        public string FullName => $"{FirstName} {LastName}";

        public string Initials => String.Format("{0}{1}", $"{FirstName} "[0], $"{LastName} "[0]).Trim().ToUpper();

        public string FullAddress => $"{AddressLine1} {AddressLine2}\n{City}, {Region} {PostalCode}\n{CountryName}";
    }
}