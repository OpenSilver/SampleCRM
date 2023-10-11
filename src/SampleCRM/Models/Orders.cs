using OpenRiaServices.DomainServices.Client;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SampleCRM.Web.Models
{
    public partial class Orders : Entity
    {
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CustomerID))
            {
                if (CustomersCombo != null && CustomersCombo.Any())
                {
                    Customer = CustomersCombo.FirstOrDefault(x => x.CustomerID == CustomerID);
                }
                else
                {
                    Customer = new Customers();
                }
            }
            else if (e.PropertyName == nameof(Status))
            {
                PaymentTypesVisible = Status > 0;
                ShippedDateVisible = ShippedViaVisible = Status > 1;
                DeliveredDateVisible = Status > 2;
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
                    OnPropertyChanged(new PropertyChangedEventArgs("PaymentTypesVisible"));
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
                    OnPropertyChanged(new PropertyChangedEventArgs("ShippedDateVisible"));
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
                    OnPropertyChanged(new PropertyChangedEventArgs("ShippedViaVisible"));
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
                    OnPropertyChanged(new PropertyChangedEventArgs("DeliveredDateVisible"));
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
                    OnPropertyChanged(new PropertyChangedEventArgs("Shippers"));
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
                    OnPropertyChanged(new PropertyChangedEventArgs("PaymentTypes"));
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
                    OnPropertyChanged(new PropertyChangedEventArgs("CountryCodes"));
                }
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
                    OnPropertyChanged(new PropertyChangedEventArgs("Statuses"));
                }
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
                    OnPropertyChanged(new PropertyChangedEventArgs("ShipCountryName"));
                }
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
                    OnPropertyChanged(new PropertyChangedEventArgs("StatusDesc"));
                }
            }
        }

        private IEnumerable<Customers> _customersCombo;
        public IEnumerable<Customers> CustomersCombo
        {
            get { return _customersCombo; }
            set
            {
                if (_customersCombo != value)
                {
                    _customersCombo = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("CustomersCombo"));
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
                    if (_customer != null)
                    {
                        ShipAddress = _customer.AddressLine1;
                        ShipCity = _customer.City;
                        ShipRegion = _customer.Region;
                        ShipCountryCode = _customer.CountryCode;
                        ShipPostalCode = _customer.PostalCode;
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("Customer"));
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
                    OnPropertyChanged(new PropertyChangedEventArgs("IsEditMode"));
                }
            }
        }
    }
}