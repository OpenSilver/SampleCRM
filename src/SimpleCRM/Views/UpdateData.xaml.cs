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
using System.Windows.Shapes;
using System.Windows.Navigation;
using SimpleCRM.Web;
#if OPENSILVER
using OpenRiaServices.DomainServices.Client;
#else
using System.ServiceModel.DomainServices.Client;
#endif

namespace SimpleCRM.Views
{
    public partial class UpdateData : Page
    {
        StudentDomainContext context = new StudentDomainContext();
        public UpdateData()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btngo_Click(object sender, RoutedEventArgs e)
        {
            var studentid=int.Parse(searchstudentName.Text);
            var query = context.GetStudentsByIDQuery(studentid);
            context.Load(query, LoadData, null);
        }

        private void LoadData(LoadOperation lo)
        {
            foreach (student st in lo.Entities)
            {
                txtstudentName.Text = st.StudentName;
                txtstudentage.Text = st.StudentAge.ToString();
            }
        }

        private void btnupdate_Click(object sender, RoutedEventArgs e)
        {
            var studentid = int.Parse(searchstudentName.Text);
            var query = context.GetStudentsByIDQuery(studentid);
            context.Load(query, EDitData, null);
        }

        private void EDitData(LoadOperation<student> lo)
        {
            student st = lo.Entities.First();
            st.StudentName = txtstudentName.Text;
            st.StudentAge = int.Parse(txtstudentage.Text);
            try
            {
                context.SubmitChanges();
                MessageBox.Show("Data updated successfully!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Data updation failed due to "+ex.Message);
            }
        }
    }
}
