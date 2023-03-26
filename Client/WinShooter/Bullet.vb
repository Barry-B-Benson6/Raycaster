Public Class Bullet
    Inherits Entity
    Public Sub New(shooter As Player, motion As Motion, game As Game, localllyOwned As Boolean)
        MyBase.New(shooter.Name + "'s Bullet", motion, game, localllyOwned, New SizeF(0.1, 0.1))
        Me.Shooter = RequireNotNull(shooter)
    End Sub

    Public Sub New(name As String, initialMovement As Motion, game As Game, entityId As Guid)
        MyBase.New(name, initialMovement, game, False, New SizeF(0.1, 0.1))
        Me.EntityId = entityId
        'Me.Shooter = RequireNotNull(Shooter)
    End Sub

    ''' <summary>
    ''' player who fired the bullet
    ''' </summary>
    Public ReadOnly Property Shooter As Player

    ''' <remarks>Check if should kill itself and if so set isAlive to false</remarks>
    Public Overrides Sub UpdateState(time As DateTime)
        If Not LocallyOwned Then Return

        If Game.Map.IsWallAt(Me.Position) Then
            Motion = Motion.NotMovingAt(Me.Position, time)
            isDirty = True
            IsAlive = False
        End If
    End Sub

    Public Overrides Sub Draw(Distance As Decimal, xCoordOfMiddle As Integer, formSize As Size, PlayerZ As Decimal, e As PaintEventArgs, fov As Integer)
        If (Distance <= 0) Then Exit Sub

        Dim angleOccluded_deg = ToDegrees(2 * Math.Atan(HitBox.Size.Width / (2 * Distance)))
        Dim percentageOfVisionTaken = angleOccluded_deg / fov


        Dim size = New Size((formSize.Height * percentageOfVisionTaken), (formSize.Height * percentageOfVisionTaken))
        Dim Middle = formSize.Height / 2
        Dim yOffset = PlayerZ * (formSize.Height / Distance)
        e.Graphics.FillEllipse(New SolidBrush(Color.Gold), New Rectangle(New Point(xCoordOfMiddle - (size.Width / 2), Middle - (size.Height / 2) + yOffset), size))
    End Sub
End Class
