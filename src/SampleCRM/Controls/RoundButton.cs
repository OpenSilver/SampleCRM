using System.Windows;
using System.Windows.Controls;

namespace SampleCRM.Controls
{
    public sealed class RoundButton : Button
    {
        public RoundButton()
        {
            DefaultStyleKey = typeof(RoundButton);
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set
            {
                SetValue(CornerRadiusProperty, new CornerRadius(value));
                SetValue(RadiusProperty, value);

            }
        }
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(nameof(Radius), typeof(double), typeof(RoundButton), new PropertyMetadata(0));

        #region CornerRadius
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(RoundButton), new PropertyMetadata(new CornerRadius(0)));
        #endregion
    }
}
