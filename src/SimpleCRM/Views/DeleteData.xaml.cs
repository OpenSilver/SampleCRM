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
    public partial class DeleteData : Page
    {
        StudentDomainContext context = new StudentDomainContext();
        public DeleteData()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btngo_Click(object sender, RoutedEventArgs e)
        {
            var studentid = int.Parse(searchstudentName.Text);
            var query = context.GetStudentsByIDQuery(studentid);
            context.Load(query, LoadData, null);//LoadData is a callback function for Load operation
        }

        private void LoadData(LoadOperation lo)
        {
            foreach (student st in lo.Entities)
            {
                txtstudentName.Text = st.StudentName;
                txtstudentage.Text = st.StudentAge.ToString();
            }
        }

        private void btndelete_Click(object sender, RoutedEventArgs e)
        {
            var studentid = int.Parse(searchstudentName.Text);
            var query = context.GetStudentsByIDQuery(studentid);
            context.Load(query, Datadelete, null);
        }

        private void Datadelete(LoadOperation<student> lo)
        {
            student st = lo.Entities.First();
            context.students.Remove(st);
            try
            {
                context.SubmitChanges();
                MessageBox.Show("Data deleted successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data deletion failed due to " + ex.Message);
            }
        }
    }
}
