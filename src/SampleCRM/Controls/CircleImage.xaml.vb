Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media

Namespace Controls
    Partial Public Class CircleImage
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = Me
        End Sub

        Public Property ImageSource As ImageSource
            Get
                Return CType(GetValue(ImageSourceProperty), ImageSource)
            End Get
            Set(ByVal value As ImageSource)
                SetValue(ImageSourceProperty, value)
            End Set
        End Property

        Public Shared ReadOnly ImageSourceProperty As DependencyProperty = DependencyProperty.Register("ImageSource", GetType(ImageSource), GetType(ImageBrush), Nothing)

        Public Property CornerRadius As CornerRadius
            Get
                Return CType(GetValue(CornerRadiusProperty), CornerRadius)
            End Get
            Set(ByVal value As CornerRadius)
                SetValue(CornerRadiusProperty, value)
            End Set
        End Property

        Public Shared ReadOnly CornerRadiusProperty As DependencyProperty = DependencyProperty.Register("CornerRadius", GetType(CornerRadius), GetType(CircleImage), New PropertyMetadata(New CornerRadius(0)))
    End Class
End Namespace
