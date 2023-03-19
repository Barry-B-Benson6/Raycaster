Public Class Bullet
    Inherits Entity
    Public Sub New(shooter As Player, motion As Motion, game As Game, localllyOwned As Boolean)
        MyBase.New(shooter.Name + " Bullet", motion, game, localllyOwned)
        Me.Shooter = RequireNotNull(shooter)
    End Sub
    ''' <summary>
    ''' player who fired the bullet
    ''' </summary>
    Public ReadOnly Property Shooter As Player
    ''' <remarks>Check if should kill itself and if so set isAlive to false</remarks>
    Public Overrides Sub UpdateState()

    End Sub
End Class
