using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SampleCRM.Web.Models
{
    public partial class OrderItems : Entity
    {
        public bool IsNew => OrderLine <= 0;

        private decimal _taxRate;
        public decimal TaxRate
        {
            get { return _taxRate; }
            set
            {
                if (_taxRate != value)
                {
                    _taxRate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("TaxRate"));
                    OnPropertyChanged(new PropertyChangedEventArgs("Total"));
                }
            }
        }

        private Products _product;
        public Products Product
        {
            get { return _product; }
            set
            {
                if (_product != value)
                {
                    _product = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Product"));
                }
            }
        }

        public decimal Subtotal => Quantity * Convert.ToDecimal(UnitPrice);

        public decimal Total => (Subtotal - Convert.ToDecimal(Discount)) * (1 + TaxRate / 100m);
    }

    public partial class Orders : Entity
    {
        public bool IsNew => OrderID <= 0;

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

        private IEnumerable<Models.OrderStatus> _statuses;
        public IEnumerable<Models.OrderStatus> Statuses
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

        private Customers _customer;
        public Customers Customer
        {
            get { return _customer; }
            set
            {
                if (_customer != value)
                {
                    _customer = value;
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