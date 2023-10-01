using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SampleCRM.Web.Views
{
   
    public partial class Customers : BasePage
    {
        private CustomersContext _customersContext = new CustomersContext();
        private CountryCodesContext _countryCodesContext = new CountryCodesContext();

        private IEnumerable<Models.Customers> _customersCollection = new ObservableCollection<Models.Customers>();
        public IEnumerable<Models.Customers> CustomersCollection
        {
            get { return _customersCollection; }
            set
            {
                if (_customersCollection != value)
                {
                    _customersCollection = value;
                    base.OnPropertyChanged();
                }
            }
        }

        private Models.Customers _selectedCustomer;
        public Models.Customers SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                if (_selectedCustomer != value)
                {
                    _selectedCustomer = value;
                    base.OnPropertyChanged();
                }
            }
        }

        public Customers()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadElements();
        }

        private async void LoadElements()
        {
            var customersQuery = _customersContext.GetCustomersQuery();
            var categoriesOp = await _customersContext.LoadAsync(customersQuery);
            CustomersCollection = categoriesOp.Entities;

            var countryCodesquery = _countryCodesContext.GetCountriesQuery();
            var countriesOp = await _countryCodesContext.LoadAsync(countryCodesquery);
            var countries = countriesOp.Entities;

            foreach (var c in CustomersCollection)
            {
                c.CountryName = countries.SingleOrDefault(x => x.CountryCodeID == c.CountryCode).Name;
            }
//#if DEBUG
//            Console.WriteLine("Customers Collection:" + CustomersCollection.Count());
//            foreach (var item in CustomersCollection)
//            {
//                Console.WriteLine("Customer Name:" + item.FirstName);
//                Console.WriteLine("Customer Picture Bytes:" + item.Picture.Length);
//            }
//#endif
        }

        private void grdCustomers_RowEditEnded(object sender, DataGridRowEditEndedEventArgs e)
        {

        }
    }
}