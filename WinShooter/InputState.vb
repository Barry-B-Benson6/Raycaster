Public Class InputState
    Sub New()
    End Sub

    Public Property Forward As Boolean

    Public Property Backward As Boolean

    Public Property Left As Boolean

    Public Property Right As Boolean

    Public Property Firing As Boolean

    Public Property Aiming As Boolean

    Public Property Crouching As Boolean

    Public Property Jumping As Boolean

    ''' <summary>
    '''     Updates the input state based off the new keyboard input.
    ''' </summary>
    ''' <param name="Key">The Keyboard key that was used (likely e.KeyCode)</param>
    ''' <param name="isKeyDown">Whether the key is being pressed or released</param>
    Public Sub Update(Key As Keys, isKeyDown As Boolean)
        RequireNotNull(Key)
        Select Case Key
            Case Keys.A
                Left = isKeyDown
            Case Keys.D
                Right = isKeyDown
            Case Keys.W
                Forward = isKeyDown
            Case Keys.S
                Backward = isKeyDown
            Case Keys.LControlKey
                Crouching = isKeyDown
            Case Keys.Space
                Jumping = isKeyDown
        End Select
    End Sub

    ''' <summary>
    '''     Updates the input state based off the new mouse input.
    ''' </summary>
    ''' <param name="Button">The Mouse button that was used (likely e.Button)</param>
    ''' <param name="isKeyDown">Whether the key is being pressed or released</param>
    Public Sub Update(Button As MouseButtons, isKeyDown As Boolean)
        RequireNotNull(Button)
        Select Case Button
            Case MouseButtons.Left
                Firing = isKeyDown
            Case MouseButtons.Right
                Aiming = isKeyDown
        End Select
    End Sub
End Class
