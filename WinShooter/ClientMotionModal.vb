Public Class ClientMotionModal
    Inherits MotionModal

    Public Property IsCrouching As Boolean
        Get
            Return Nothing
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public Property InputState As InputState
        Get
            Return Nothing
        End Get
        Set(value As InputState)
        End Set
    End Property

    Public Sub Crouch()
    End Sub

    Public Sub UnCrouch()

    End Sub

    Public Sub Jump()

    End Sub

    Public Overrides Sub UpdatePostion()
        Throw New NotImplementedException()
    End Sub
End Class
