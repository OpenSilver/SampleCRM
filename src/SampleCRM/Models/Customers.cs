using System;
using System.Collections.Generic;
using System.Text;

namespace SampleCRM.Web.Models
{
    public partial class Customers
    {
        public string FullName { get { return $"{FirstName} {LastName}"; } }
        public string CountryName { get; set; }
    }
}