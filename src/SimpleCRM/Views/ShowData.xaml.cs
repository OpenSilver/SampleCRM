using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleCRM.Web;
#if OPENSILVER
using OpenRiaServices.DomainServices.Client;
#else
using System.ServiceModel.DomainServices.Client;
#endif

namespace SimpleCRM
{
    public partial class ShowData : Page
    {
        StudentDomainContext context = new StudentDomainContext();
        public ShowData()
        {
            InitializeComponent();
            EntityQuery<student> query = context.GetStudentsQuery();
            LoadOperation<student> loadOp = this.context.Load(query);
            studentgrid.ItemsSource = loadOp.Entities;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}