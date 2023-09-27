using SimpleCRM.Web;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SimpleCRM.Views
{
    public partial class Home : Page
    {
        private CategoryContext _categoryContext = new CategoryContext();

        public Home()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var categoriesQuery = _categoryContext.GetCategoriesQuery();
            var categoriesOp = _categoryContext.Load(categoriesQuery);
            gridCategories.ItemsSource = categoriesOp.Entities;
        }
    }
}