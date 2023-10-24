Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

''' <summary>
'''     Provides extension methods for performing operations on a <see cref="DataField"/>.
''' </summary>
Module DataFieldExtensions

    ''' <summary>
    ''' Replaces a <see cref="DataField" />'s <see cref="TextBox" /> control with another control and updates the bindings.
    ''' </summary>
    ''' <param name="field">The <see cref="DataField"/> whose <see cref="TextBox"/> will be replaced.</param>
    ''' <param name="newControl">The new control you're going to set as <see cref="DataField.Content" />.</param>
    ''' <param name="dataBindingProperty">The control's property that will be used for data binding.</param>
    <Extension()>
    Public Sub ReplaceTextBox(field As DataField, newControl As FrameworkElement, dataBindingProperty As DependencyProperty)
        field.ReplaceTextBox(newControl, dataBindingProperty, Nothing)
    End Sub

    ''' <summary>
    ''' Replaces a <see cref="DataField" />'s <see cref="TextBox" /> control with another control and updates the bindings.
    ''' </summary>
    ''' <param name="field">The <see cref="DataField"/> whose <see cref="TextBox"/> will be replaced.</param>
    ''' <param name="newControl">The new control you're going to set as <see cref="DataField.Content" />.</param>
    ''' <param name="dataBindingProperty">The control's property that will be used for data binding.</param>
    ''' <param name="bindingSetupFunction">
    '''  An optional <see cref="Action"/> you can use to change parameters on the newly generated binding before it is applied to <paramref name="newControl"/>
    ''' </param>
    <Extension()>
    Public Sub ReplaceTextBox(field As DataField, newControl As FrameworkElement, dataBindingProperty As DependencyProperty, bindingSetupFunction As Action(Of Binding))

        If field Is Nothing Then
            Throw New ArgumentNullException(NameOf(field))
        End If

        If newControl Is Nothing Then
            Throw New ArgumentNullException(NameOf(newControl))
        End If

        ' Construct new binding by copying existing one, and sending it to bindingSetupFunction for any changes the caller wants to perform.
        Dim newBinding As Binding = field.Content.GetBindingExpression(TextBox.TextProperty).ParentBinding.CreateCopy()

        bindingSetupFunction?(newBinding)

        ' Replace field
        newControl.SetBinding(dataBindingProperty, newBinding)
        field.Content = newControl

    End Sub

    ''' <summary>
    ''' Creates a new <see cref="Binding"/> object by copying all properties from another <see cref="Binding"/> object.
    ''' </summary>
    ''' <param name="binding"><see cref="Binding"/> from which property values will be copied</param>
    ''' <returns>A new <see cref="Binding"/> object.</returns>
    <Extension()>
    Private Function CreateCopy(binding As Binding) As Binding

        If binding Is Nothing Then
            Throw New ArgumentNullException(NameOf(binding))
        End If

        Dim newBinding As New Binding With {.BindsDirectlyToSource = binding.BindsDirectlyToSource,
            .Converter = binding.Converter,
            .ConverterParameter = binding.ConverterParameter,
            .ConverterCulture = binding.ConverterCulture,
            .Mode = binding.Mode,
            .NotifyOnValidationError = binding.NotifyOnValidationError,
            .Path = binding.Path,
            .UpdateSourceTrigger = binding.UpdateSourceTrigger,
            .ValidatesOnExceptions = binding.ValidatesOnExceptions}

        If binding.ElementName IsNot Nothing Then
            newBinding.ElementName = binding.ElementName
        ElseIf binding.RelativeSource IsNot Nothing Then
            newBinding.RelativeSource = binding.RelativeSource
        Else
            newBinding.Source = binding.Source
        End If

        Return newBinding

    End Function

End Module