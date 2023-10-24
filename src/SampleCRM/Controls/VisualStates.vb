Namespace Controls

    ''' <summary>
    ''' Names and helpers for visual states in the controls.
    ''' </summary>
    Friend NotInheritable Class VisualStates

        Private Sub New()
        End Sub

#Region "GroupBusyStatus"

        ''' <summary>
        ''' Busy state for BusyIndicator.
        ''' </summary>
        Public Const StateBusy = "Busy"

        ''' <summary>
        ''' Idle state for BusyIndicator.
        ''' </summary>
        Public Const StateIdle = "Idle"

        ''' <summary>
        ''' Busy states group name.
        ''' </summary>
        Public Const GroupBusyStatus = "BusyStatusStates"

#End Region

#Region "GroupVisibility"

        ''' <summary>
        ''' Visible state name for BusyIndicator.
        ''' </summary>
        Public Const StateVisible = "Visible"

        ''' <summary>
        ''' Hidden state name for BusyIndicator.
        ''' </summary>
        Public Const StateHidden = "Hidden"

        ''' <summary>
        ''' BusyDisplay group.
        ''' </summary>
        Public Const GroupVisibility = "VisibilityStates"

#End Region

    End Class

End Namespace