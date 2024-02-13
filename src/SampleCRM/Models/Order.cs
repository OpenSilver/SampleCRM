using OpenRiaServices.DomainServices.Client;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SampleCRM.Web.Models
{
    public partial class Order : Entity
    {
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
                    RaisePropertyChanged(nameof(PaymentTypesVisible));
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
                    RaisePropertyChanged(nameof(ShippedDateVisible));
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
                    RaisePropertyChanged(nameof(ShippedViaVisible));
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
                    RaisePropertyChanged(nameof(DeliveredDateVisible));
                }
            }
        }

        private IEnumerable<Shipper> _shippers;
        public IEnumerable<Shipper> Shippers
        {
            get { return _shippers; }
            set
            {
                if (_shippers != value)
                {
                    _shippers = value;
                    RaisePropertyChanged(nameof(Shippers));
                }
            }
        }

        private IEnumerable<PaymentType> _paymentTypes;
        public IEnumerable<PaymentType> PaymentTypes
        {
            get { return _paymentTypes; }
            set
            {
                if (_paymentTypes != value)
                {
                    _paymentTypes = value;
                    RaisePropertyChanged(nameof(PaymentTypes));
                }
            }
        }

        private IEnumerable<CountryCode> _countryCodes;
        public IEnumerable<CountryCode> CountryCodes
        {
            get { return _countryCodes; }
            set
            {
                if (_countryCodes != value)
                {
                    _countryCodes = value;
                    RaisePropertyChanged(nameof(CountryCodes));
                }
                if (_countryCodes != null)
                    ShipCountryName = _countryCodes.FirstOrDefault(x => x.CountryCodeID == ShipCountryCode)?.Name;
            }
        }

        private IEnumerable<OrderStatu> _statuses;
        public IEnumerable<OrderStatu> Statuses
        {
            get { return _statuses; }
            set
            {
                if (_statuses != value)
                {
                    _statuses = value;
                }
                RaisePropertyChanged(nameof(Statuses));
                RaisePropertyChanged(nameof(Status));
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
                RaisePropertyChanged(nameof(ShipCountryName));
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
                    RaisePropertyChanged(nameof(StatusDesc));
                }
            }
        }

        private Customer _customer = new Customer();
        public Customer Customer
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

                    RaisePropertyChanged(nameof(Customer));
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
                    RaisePropertyChanged(nameof(IsEditMode));
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
                    RaisePropertyChanged(nameof(CustomerSearchText));
                }
            }
        }
    }
}