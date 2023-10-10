using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

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
            else if (e.PropertyName == nameof(ProductID))
            {
                if (ProductsCombo != null && ProductsCombo.Any())
                {
                    Product = ProductsCombo.FirstOrDefault(x => x.ProductID == ProductID);
                }
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

        private IEnumerable<Models.Products> _productsCombo;
        public IEnumerable<Models.Products> ProductsCombo
        {
            get { return _productsCombo; }
            set
            {
                if (_productsCombo != value)
                {
                    _productsCombo = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ProductsCombo"));
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