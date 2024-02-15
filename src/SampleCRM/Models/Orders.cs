using CommunityToolkit.Mvvm.Input;
using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SampleCRM.Web.Models
{
    public partial class Orders : Entity
    {
        public event EventHandler OrderShown;
        public event EventHandler Deleted;

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Status):
                    {
                        PaymentTypesVisible = _status > 0;
                        ShippedDateVisible = ShippedViaVisible = _status > 1;
                        DeliveredDateVisible = _status > 2;
                        StatusDesc = _statuses?.FirstOrDefault(x => x.Status == _status)?.Name;
                        break;
                    }
                case nameof(ShipCountryCode):
                    ShipCountryName = _countryCodes?.FirstOrDefault(x => x.CountryCodeID == _shipCountryCode)?.Name;
                    break;

            }

            base.OnPropertyChanged(e);
        }

        public bool IsNew => OrderID <= 0;

        private bool _paymentTypesVisible;
        public bool PaymentTypesVisible
        {
            get { return _paymentTypesVisible; }
            set
            {
                if (_paymentTypesVisible != value)
                {
                    _paymentTypesVisible = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(PaymentTypesVisible)));
                }
            }
        }

        private bool _shippedDateVisible;
        public bool ShippedDateVisible
        {
            get { return _shippedDateVisible; }
            set
            {
                if (_shippedDateVisible != value)
                {
                    _shippedDateVisible = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(ShippedDateVisible)));
                }
            }
        }

        private bool _shippedViaVisible;
        public bool ShippedViaVisible
        {
            get { return _shippedViaVisible; }
            set
            {
                if (_shippedViaVisible != value)
                {
                    _shippedViaVisible = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(ShippedViaVisible)));
                }
            }
        }

        private bool _deliveredDateVisible;
        public bool DeliveredDateVisible
        {
            get { return _deliveredDateVisible; }
            set
            {
                if (_deliveredDateVisible != value)
                {
                    _deliveredDateVisible = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(DeliveredDateVisible)));
                }
            }
        }

        private IEnumerable<Shippers> _shippers;
        public IEnumerable<Shippers> Shippers
        {
            get { return _shippers; }
            set
            {
                if (_shippers != value)
                {
                    _shippers = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Shippers)));
                }
            }
        }

        private IEnumerable<PaymentTypes> _paymentTypes;
        public IEnumerable<PaymentTypes> PaymentTypes
        {
            get { return _paymentTypes; }
            set
            {
                if (_paymentTypes != value)
                {
                    _paymentTypes = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(PaymentTypes)));
                }
            }
        }

        private IEnumerable<CountryCodes> _countryCodes;
        public IEnumerable<CountryCodes> CountryCodes
        {
            get { return _countryCodes; }
            set
            {
                if (_countryCodes != value)
                {
                    _countryCodes = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(CountryCodes)));
                }
                if (_countryCodes != null)
                    ShipCountryName = _countryCodes.FirstOrDefault(x => x.CountryCodeID == ShipCountryCode)?.Name;
            }
        }

        private IEnumerable<OrderStatus> _statuses;
        public IEnumerable<OrderStatus> Statuses
        {
            get { return _statuses; }
            set
            {
                if (_statuses != value)
                {
                    _statuses = value;
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Statuses)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Status)));
            }
        }

        private string _shipCountryName;
        public string ShipCountryName
        {
            get { return _shipCountryName; }
            set
            {

                if (_shipCountryName != value)
                {
                    _shipCountryName = value;
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ShipCountryName)));
            }
        }

        private string _statusDesc;
        public string StatusDesc
        {
            get { return _statusDesc; }
            set
            {
                if (_statusDesc != value)
                {
                    _statusDesc = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(StatusDesc)));
                }
            }
        }

        private Customers _customer = new Customers();
        public Customers Customer
        {
            get { return _customer; }
            set
            {
                if (_customer != value)
                {
                    _customer = value;

                    if (_customer != null && _isEditMode)
                    {
                        if (CustomerID != _customer.CustomerID)
                            CustomerID = _customer.CustomerID;

                        ShipAddress = _customer.AddressLine1;
                        ShipCity = _customer.City;
                        ShipRegion = _customer.Region;
                        ShipCountryCode = _customer.CountryCode;
                        ShipPostalCode = _customer.PostalCode;
                    }

                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Customer)));
                }
            }
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get { return _isEditMode; }
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsEditMode)));
                }
            }
        }

        private string _customerSearchText;
        public string CustomerSearchText
        {
            get { return _customerSearchText; }
            set
            {
                if (_customerSearchText != value)
                {
                    _customerSearchText = value.Trim();
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(CustomerSearchText)));
                }
            }
        }

        [RelayCommand]
        public void ShowOrder()
        {
            if (OrderShown != null)
                OrderShown(this, new EventArgs());
        }

        [RelayCommand]
        public void Delete()
        {
            if (Deleted != null)
                Deleted(this, new EventArgs());
        }
    }
}