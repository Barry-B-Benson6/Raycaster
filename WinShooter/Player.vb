Imports System.Net
Imports System.Security.Policy

Public Class Player
    Inherits Entity

    ''' <summary>
    ''' This exists to allow us to create mocks of this class, it should not be used any other time
    ''' </summary>
    Protected Sub New()
        MyBase.New()
    End Sub

    Private LastBulletFire As DateTime

    Public Sub New(name As String, initialMovement As Motion, game As Game, locallyOwned As Boolean)
        MyBase.New(name, initialMovement, game, locallyOwned)
        LastBulletFire = DateTime.UtcNow().AddSeconds(-(1 / Constants.FireRate_s))
    End Sub

    Private _isCrouching As Boolean
    Public Property IsCrouching As Boolean
        Get
            SyncLock Me
                Return _isCrouching
            End SyncLock
        End Get
        Private Set(value As Boolean)
            SyncLock Me
                _isCrouching = value
            End SyncLock
        End Set
    End Property

    Private _IsAiming As Boolean
    Public Property IsAiming As Boolean
        Get
            SyncLock Me
                Return _IsAiming
            End SyncLock
        End Get
        Set(value As Boolean)
            SyncLock Me
                _IsAiming = value
            End SyncLock
        End Set
    End Property

    Private Function CreateBullet(time As DateTime) As WinShooter.Bullet
        Dim vE_ms = Math.Cos(ToRadians(Position.Heading_deg)) * BulletSpeed_ms
        Dim vN_ms = Math.Sin(ToRadians(Position.Heading_deg)) * BulletSpeed_ms
        Dim vU_ms = 0

        Dim velocity = New GameVelocity(vE_ms, vN_ms, vU_ms)
        Return New Bullet(Me, New Motion(Position, velocity, time), Game, True)
    End Function

    Public Overrides Sub Draw(Distance As Decimal, xCoordOfMiddle As Integer, formSize As Size, PlayerZ As Decimal, e As PaintEventArgs)
        Throw New NotImplementedException()
    End Sub

    '''  <remarks>Fire Bullets and update aiming status is crouching</remarks>
    Public Overrides Sub UpdateState(time As DateTime)

        If (LocallyOwned) Then PerformLocallyOwnedUpdateState(time)
    End Sub

    Public Overrides Sub UpdatePosition(time As DateTime)
        Dim nextPosition = Motion.CalculatePositionAtTime(time)
        Try
            If (Game.Map.IsWallAt(nextPosition)) Then
                Dim x = Math.Floor(nextPosition.East_m / CellSize_m)
                Dim y = Math.Floor(nextPosition.North_m / CellSize_m)
                Dim xOLd = Math.Floor(Position.East_m / CellSize_m)
                Dim yOLd = Math.Floor(Position.North_m / CellSize_m)

                If (x <> xOLd And y <> yOLd) Then
                    Exit Sub
                ElseIf (x <> xOLd) Then
                    Position = New GamePosition(Position.East_m, nextPosition.North_m, nextPosition.Up_m, nextPosition.Heading_deg, nextPosition.Tilt_deg)
                Else
                    Position = New GamePosition(nextPosition.East_m, Position.North_m, nextPosition.Up_m, nextPosition.Heading_deg, nextPosition.Tilt_deg)
                End If
            Else
                Position = nextPosition
            End If
        Catch
        End Try
    End Sub

    Private Sub PerformLocallyOwnedUpdateState(time As DateTime)
        IsAiming = Game.InputState.Aiming

        If Not IsCrouching And Game.InputState.Crouching Then Crouch()
        If IsCrouching And Not Game.InputState.Crouching Then UnCrouch()

        IsCrouching = Game.InputState.Crouching

        If Game.InputState.Jumping Then Jump()

        If (Game.InputState.Firing) Then
            Dim timeBetweenNowAndLast = time.Subtract(LastBulletFire)
            If (timeBetweenNowAndLast.TotalSeconds > (1 / FireRate_s)) Then
                Dim newBullet = CreateBullet(time)
                Game.AddEntity(newBullet)
            End If
        End If

        Dim nE_m, nN_m, nU_m, nH_deg, nT_deg As Decimal
        Dim MouseDiff = Game.InputState.MouseDiff

        If MouseDiff <> New PointF(0, 0) Then
            nH_deg -= MouseDiff.X * 0.1
            nT_deg += MouseDiff.Y * 0.1
        End If

        If (Game.Focused) Then
            Dim Middle = New Point(Game.formSize.Width / 2, Game.formSize.Height / 2)
            Dim diffPnt = New PointF(Middle.X - Cursor.Position.X, Middle.Y - Cursor.Position.Y)
            Cursor.Position = Middle
            Game.InputState.Update(diffPnt)
        End If
        If (Game.InputState.Left) Then
            nE_m -= Math.Cos(ToRadians(Position.Heading_deg + 90)) * 100
            nN_m -= Math.Sin(ToRadians(Position.Heading_deg + 90)) * 100
        End If
        If (Game.InputState.Right) Then
            nE_m -= Math.Cos(ToRadians(Position.Heading_deg - 90)) * 100
            nN_m -= Math.Sin(ToRadians(Position.Heading_deg - 90)) * 100
        End If
        If (Game.InputState.Forward) Then
            nE_m += Math.Cos(ToRadians(Position.Heading_deg)) * 100
            nN_m += Math.Sin(ToRadians(Position.Heading_deg)) * 100
        End If
        If (Game.InputState.Backward) Then
            nE_m -= Math.Cos(ToRadians(Position.Heading_deg)) * 100
            nN_m -= Math.Sin(ToRadians(Position.Heading_deg)) * 100
        End If
        Dim newPosition = New GamePosition(Position.East_m, Position.North_m, Position.Up_m, Position.Heading_deg + nH_deg, Position.Tilt_deg + nT_deg)
        Motion = New Motion(newPosition, New GameVelocity(nE_m, nN_m, nU_m), DateTime.UtcNow)
    End Sub
    Private Sub Jump()
        If Motion.VelocityStamp.Up_ms = 0 Then
            ''5.111 is the initial velocity that causes a jump height of 1.333
            Dim JumpVel_ms = 5.111
            Motion = New Motion(Position, New GameVelocity(Motion.VelocityStamp.East_ms, Motion.VelocityStamp.North_ms, JumpVel_ms), DateTime.Now())
        End If
    End Sub

    Private Sub Crouch()

    End Sub


    Private Sub UnCrouch()

    End Sub
End Class
