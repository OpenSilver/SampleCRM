using OpenRiaServices.DomainServices.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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
                    OnPropertyChanged();
                    OnPropertyChanged("FilteredCustomersCollection");
                }
            }
        }

        public IEnumerable<Models.Customers> FilteredCustomersCollection
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_searchText))
                {
                    return _customersCollection;
                }
                else
                {
                    return _customersCollection.Where(x => x.FullName.ToLowerInvariant().Contains(_searchText.ToLowerInvariant()));
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
                    OnPropertyChanged();
                    OnPropertyChanged("AnySelectedCustomer");
#if DEBUG
                    Console.WriteLine($"Customers, Customer: {value.FirstName} selected");
#endif
                }
            }
        }

        public bool AnySelectedCustomer
        {
            get
            {
                return _selectedCustomer != null;
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged("FilteredCustomersCollection");
                    OnPropertyChanged();
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
#if DEBUG
            Console.WriteLine("Customers Collection:" + CustomersCollection.Count());
            foreach (var item in CustomersCollection)
            {
                Console.WriteLine("Customer Name:" + item.FirstName);
                Console.WriteLine("Customer Picture Bytes:" + item.Picture.Length);
            }
#endif
        }

        private void btnSearchCancel_Click(object sender, RoutedEventArgs e)
        {
            SearchText = string.Empty;
        }

        private void grdCustomers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("grdCustomers_SelectionChanged, {0} Items Added", e.AddedItems.Count);
#endif
        }
    }
}