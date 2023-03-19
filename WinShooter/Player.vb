Public Class Player
    Inherits Entity

    Public Sub New(name As String, motion As Motion, game As Game, locallyOwned As Boolean)
        MyBase.New(name, motion, game, locallyOwned)
    End Sub

    ''' <summary>
    ''' This exists to allow us to create mocks of this class, it should not be used any other time
    ''' </summary>
    Protected Sub New()
        MyBase.New()
    End Sub

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



    Private Function CreateBullet() As WinShooter.Bullet
        Throw New NotImplementedException()
    End Function

    '''  <remarks>Fire Bullets and update aiming status is crouching</remarks>
    Public Overrides Sub UpdateState(time As DateTime)

    End Sub

    Private Sub Crouch()
    End Sub

    Private Sub Jump()

    End Sub

    Private Sub UnCrouch()

    End Sub
End Class
