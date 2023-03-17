Public Class NetworkMotionModal
    Inherits MotionModal

    Public Property MostRecentMotion As WinShooter.Motion
        Get
            Return Nothing
        End Get
        Set(value As WinShooter.Motion)
        End Set
    End Property

    Public Overrides Sub UpdatePostion()
        Throw New NotImplementedException()
    End Sub
End Class
