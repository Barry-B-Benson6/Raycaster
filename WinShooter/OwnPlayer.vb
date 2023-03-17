Public Class OwnPlayer
    Inherits Player

    Public Property InputState As InputState
        Get
            Return Nothing
        End Get
        Set(value As InputState)
        End Set
    End Property

    Public Property Aiming As Integer
        Get
            SyncLock Me
                Return Nothing
            End SyncLock
        End Get
        Set(value As Integer)
            SyncLock Me

            End SyncLock
        End Set
    End Property

    Public Overrides ReadOnly Property Updatable As Boolean
        Get
            Return False
        End Get
    End Property

    ''' <remarks>Fire Bullets and update aiming status is crouching</remarks>
    Public Overrides Sub UpdateState()

    End Sub

    Private Function CreateBullet() As WinShooter.Bullet
    End Function
End Class
