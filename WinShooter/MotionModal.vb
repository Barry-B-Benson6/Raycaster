Public MustInherit Class MotionModal

    Public Property IsObstructedByWall As Integer
        Get
            Return Nothing
        End Get
        Set(value As Integer)
        End Set
    End Property

    Public Property BelievedDead As Boolean
        Get
            Return Nothing
        End Get
        Set(value As Boolean)
        End Set
    End Property

    Public Property Position As WinShooter.Motion
        Get
            Return Nothing
        End Get
        Set(value As WinShooter.Motion)
        End Set
    End Property

    Public MustOverride Sub UpdatePostion()

    Protected Overridable Sub CheckInWall()

    End Sub
End Class
