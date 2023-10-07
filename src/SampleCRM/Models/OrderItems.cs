using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SampleCRM.Web.Models
{
    public partial class OrderItems : Entity
    {
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UnitPrice"
                || e.PropertyName == "Quantity")
            {
                RaisePropertyChanged("Subtotal");
            }
            else if (e.PropertyName == "Subtotal" 
                || e.PropertyName == "Discount")
            {
                RaisePropertyChanged("Total");
            }
            else if (e.PropertyName == "OrderLine")
            {
                RaisePropertyChanged("IsNew");
            }

            base.OnPropertyChanged(e);
        }

        public bool IsNew => OrderLine <= 0;

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

        private IEnumerable<Models.TaxTypes> _taxTypes;
        public IEnumerable<Models.TaxTypes> TaxTypes
        {
            get { return _taxTypes; }
            set
            {
                if (_taxTypes != value)
                {
                    _taxTypes = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("TaxTypes"));
                }
            }
        }

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

        private IEnumerable<Models.Products> _products;
        public IEnumerable<Models.Products> Products
        {
            get { return _products; }
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Products"));
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
}