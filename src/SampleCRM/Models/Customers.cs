using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SampleCRM.Web.Models
{
    public partial class Customers : Entity
    {
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
        public bool IsEditMode { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Initials => String.Format("{0}{1}", $"{FirstName} "[0], $"{LastName} "[0]).Trim().ToUpper();
        public string CountryName { get; set; }
        public string FullAddress => $"{AddressLine1} {AddressLine2}\n{City}, {Region} {PostalCode}\n{CountryName}";
    }
}