﻿using System.Linq;
using System.Windows;

namespace SampleCRM.Web.Views
{
    public partial class Categories : BasePage
    {
        public Categories()
        {
            InitializeComponent();
        }

        protected override void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            base.OnSizeChanged(sender, e);
            gridCategories.Columns.Last().Visibility = BoolToVisibilityConverter.Convert(!IsMobileUI);
        }
    }
}