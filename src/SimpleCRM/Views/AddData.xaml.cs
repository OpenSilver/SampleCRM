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

namespace SimpleCRM.Views
{
    public partial class AddData : Page
    {
        StudentDomainContext context = new StudentDomainContext();

        public AddData()
        {
            InitializeComponent();
        }
 
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnsubmit_Click(object sender, RoutedEventArgs e)
        {
            student st = new student();
            st.StudentName = txtstudentName.Text;
            st.StudentAge = int.Parse(txtstudentage.Text);

            context.students.Add(st);
            try
            {
                context.SubmitChanges();
                MessageBox.Show("Data added Successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Adding Data failed due to " + ex.Message);
            }

        }

    }
}
