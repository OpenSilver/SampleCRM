Imports System.ComponentModel

Namespace Web

    ''' <summary>
    ''' Extensions to the <see cref="User"/> class.
    ''' </summary>
    Partial Public Class User

#Region "Make DisplayName Bindable"

        ''' <summary>
        ''' Override of the <c>OnPropertyChanged</c> method that generates property change notifications when <see cref="User.DisplayName"/> changes.
        ''' </summary>
        ''' <param name="e">The property change event args.</param>
        Protected Overrides Sub OnPropertyChanged(e As PropertyChangedEventArgs)

            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" OrElse e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If

        End Sub

#End Region

    End Class

End Namespace