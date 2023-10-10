using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace SampleCRM.Web.Controls
{
    public partial class RequiredTextBlock : UserControl
    {
        public RequiredTextBlock()
        {
            this.InitializeComponent();
            DataContext = this;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(RequiredTextBlock),
            new PropertyMetadata(""));
    }
}
