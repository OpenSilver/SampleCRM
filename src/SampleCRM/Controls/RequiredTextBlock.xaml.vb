Imports System.Windows
Imports System.Windows.Controls

Namespace Controls
    Partial Public Class RequiredTextBlock
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = Me
        End Sub

        Public Property Text As String
            Get
                Return CStr(GetValue(TextProperty))
            End Get
            Set(ByVal value As String)
                SetValue(TextProperty, value)
            End Set
        End Property

        Public Shared ReadOnly TextProperty As DependencyProperty = DependencyProperty.Register("Text", GetType(String), GetType(RequiredTextBlock), New PropertyMetadata(""))
    End Class
End Namespace
