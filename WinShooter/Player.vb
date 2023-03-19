Public Class Player
    Inherits Entity

    Public Property IsCrouching As Integer
        Get
            SyncLock Me
                Return Game.InputState.Crouching
            End SyncLock
        End Get
        Set(value As Integer)
            SyncLock Me

            End SyncLock
        End Set
    End Property

    Public Property IsAiming As Integer
        Get
            SyncLock Me
                Return Game.InputState.Aiming
            End SyncLock
        End Get
        Set(value As Integer)
            SyncLock Me

            End SyncLock
        End Set
    End Property

    Public Overrides ReadOnly Property LocallyOwned As Boolean
        Get
            Return False
        End Get
    End Property

    Private Function CreateBullet() As WinShooter.Bullet
        Throw New NotImplementedException()
    End Function

    '''  <remarks>Fire Bullets and update aiming status is crouching</remarks>
    Public Overrides Sub UpdateState()

    End Sub

    Private Sub Crouch()
    End Sub

    Private Sub Jump()

    End Sub

    Private Sub UnCrouch()

    End Sub
End Class
