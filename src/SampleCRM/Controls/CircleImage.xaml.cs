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
    public partial class CircleImage : UserControl
    {
        public CircleImage()
        {
            this.InitializeComponent();
            DataContext = this;
        }

        public ImageSource ImageSource
        {
            get
            {
                return (ImageSource)GetValue(ImageSourceProperty);
            }
            set
            {
                SetValue(ImageSourceProperty, value);
            }
        }

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageBrush), null);

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius",
            typeof(CornerRadius),
            typeof(CircleImage),
            new PropertyMetadata(new CornerRadius(0)));

        //public SolidColorBrush BorderBrush
        //{
        //    get { return (SolidColorBrush)GetValue(BorderBrushProperty); }
        //    set { SetValue(BorderBrushProperty, value); }
        //}

        //public static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register(
        //    "BorderBrush",
        //    typeof(SolidColorBrush),
        //    typeof(CircleImage),
        //    new PropertyMetadata(new SolidColorBrush(Colors.Transparent)));
    }
}
