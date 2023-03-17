Public Class Motion

    Public Property PositionStamp As WinShooter.GamePosition
        Get
            Return Nothing
        End Get
        Set(value As WinShooter.GamePosition)
        End Set
    End Property

    Public Property TimeStamp As Date
        Get
            Return Nothing
        End Get
        Set(value As Date)
        End Set
    End Property

    Public Property VelocityStamp As WinShooter.GameVelocity
        Get
            Return Nothing
        End Get
        Set(value As WinShooter.GameVelocity)
        End Set
    End Property

    Public Function CalculateCurrentPosition() As WinShooter.GamePosition
    End Function
End Class
