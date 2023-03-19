Public Class Bullet
    Inherits Entity
    Public Sub New(shooter As Player, motion As Motion, game As Game, localllyOwned As Boolean)
        MyBase.New(shooter.Name + "'s Bullet", motion, game, localllyOwned)
        Me.Shooter = RequireNotNull(shooter)
    End Sub
    ''' <summary>
    ''' player who fired the bullet
    ''' </summary>
    Public ReadOnly Property Shooter As Player
    ''' <remarks>Check if should kill itself and if so set isAlive to false</remarks>
    Public Overrides Sub UpdateState(time As DateTime)
        If Not LocallyOwned Then
            Return
        End If
        If Game.Map.IsWallAt(Me.Position) Then
            Motion = Motion.NotMovingAt(Me.Position, time)
            IsAlive = False
        End If
    End Sub
End Class
