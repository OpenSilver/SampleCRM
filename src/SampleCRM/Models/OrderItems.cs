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
            switch (e.PropertyName)
            {
                case nameof(UnitPrice):
                case nameof(Quantity):
                    RaisePropertyChanged(nameof(Subtotal));
                    break;
                case nameof(Subtotal):
                case nameof(Discount):
                    RaisePropertyChanged(nameof(Total));
                    break;
                case nameof(OrderLine):
                    RaisePropertyChanged(nameof(IsNew));
                    break;
                case nameof(ProductID):
                    {
                        if (ProductsCombo != null)
                            Product = ProductsCombo.FirstOrDefault(x => x.ProductID == ProductID);

                        break;
                    }
                case nameof(TaxType):
                    {
                        if (TaxTypes != null)
                        {
                            decimal.TryParse(TaxTypes.FirstOrDefault(x => x.TaxTypeID == TaxType).Rate, out var taxRate);
                            TaxRate = taxRate;
                        }

                        break;
                    }
            }

            base.OnPropertyChanged(e);
        }

        public bool IsNew => OrderLine < 1;

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

        private IEnumerable<TaxTypes> _taxTypes;
        public IEnumerable<TaxTypes> TaxTypes
        {
            get { return _taxTypes; }
            set
            {
                if (_taxTypes != value)
                {
                    _taxTypes = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(TaxTypes)));
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
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(TaxRate)));
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Total)));
                }
            }
        }

        private IEnumerable<Products> _productsCombo;
        public IEnumerable<Products> ProductsCombo
        {
            get { return _productsCombo; }
            set
            {
                if (_productsCombo != value)
                {
                    _productsCombo = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(ProductsCombo)));
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
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(Product)));
                    if (_product != null)
                    {
                        Quantity = 1;
                        UnitPrice = _product.ListPrice;
                        Discount = _product.Discount;
                        TaxType = _product.TaxType;
                    }
                }
            }
        }

        public decimal Subtotal => Quantity * Convert.ToDecimal(UnitPrice);

        public decimal Total => (Subtotal - Convert.ToDecimal(Discount)) * (1 + TaxRate / 100m);
    }
}