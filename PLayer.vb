Imports System.Numerics

Public Class Player

    Public pntLocation As PointF
    Public decAngle As Decimal
    Public decVerticalAngle As Decimal
    Public Movement As MovementState
    Private Velocity As Double
    Private maxVelocity As Double
    Public Sub New()
        pntLocation = New PointF(1.5, 1.5)
        maxVelocity = 0.2
        decAngle = Math.PI
        Movement = New MovementState()
        Velocity = 0
    End Sub

    Public ReadOnly Property Height
        Get
            ''g = -1
            ''h = -(1/3)g + (1/2)g(x-1)^2
            Return ((1 / 2) - (0.5 * Math.Pow(Movement.PointInJump - 1, 2))) * (2 / 3)
        End Get
    End Property

    Public Sub Jump()
        If (Movement.PointInJump <> 0) Then Return
        Movement.PointInJump = 2
    End Sub

    Public Sub Move(Map As Byte(,))
        If (Movement.PointInJump > 0) Then
            Movement.PointInJump -= 0.1
            Movement.PointInJump = Math.Clamp(Movement.PointInJump, 0, 2)
        End If
        If (Movement.Moving) Then
            Velocity += 0.01
            Velocity = Math.Clamp(Velocity, 0, maxVelocity)
        Else
            Velocity = 0
        End If
        Dim oldPoint = pntLocation
        Dim movementVector = New PointF(0, 0)
        If Movement.right Then
            movementVector.X -= Math.Cos(decAngle + Math.PI / 2) * 0.1
            movementVector.Y -= Math.Sin(decAngle + Math.PI / 2) * 0.1
        End If
        If Movement.Left Then
            movementVector.X -= Math.Cos(decAngle - Math.PI / 2) * 0.1
            movementVector.Y -= Math.Sin(decAngle - Math.PI / 2) * 0.1
        End If
        If Movement.Forward Then
            movementVector.X -= Math.Cos(decAngle) * 0.1
            movementVector.Y -= Math.Sin(decAngle) * 0.1
        End If
        If Movement.Backward Then
            movementVector.X += Math.Cos(decAngle) * 0.1
            movementVector.Y += Math.Sin(decAngle) * 0.1
        End If
        ''Now normalize movement
        Dim currentLength = LengthOfVector(movementVector)
        movementVector = New PointF(movementVector.X / currentLength * Velocity, movementVector.Y / currentLength * Velocity)
        HandleWallCollisions(Map, oldPoint, movementVector)
    End Sub

    Private Sub HandleWallCollisions(Map As Byte(,), oldPoint As PointF, movementVector As PointF)
        Dim newPoint = New PointF(oldPoint.X + movementVector.X, oldPoint.Y + movementVector.Y)
        Try
            If (Map(Math.Floor(newPoint.X), Math.Floor(newPoint.Y)) > 0) Then
                Dim x = Math.Floor(newPoint.X)
                Dim y = Math.Floor(newPoint.Y)
                Dim xOLd = Math.Floor(oldPoint.X)
                Dim yOLd = Math.Floor(oldPoint.Y)

                If (x <> xOLd And y <> yOLd) Then
                    pntLocation = oldPoint
                ElseIf (x <> xOLd) Then
                    pntLocation.X = oldPoint.X
                Else
                    pntLocation.Y = oldPoint.Y
                End If
            Else
                pntLocation = newPoint
            End If
        Catch
        End Try
    End Sub


    Public Function SeeBullet(bullet As Bullet, rayDiffvPlayerAngle As Decimal)

        Dim rayAngle = decAngle + rayDiffvPlayerAngle
        'Console.WriteLine(rayAngle * 180 / Math.PI)

        If (rayAngle < 0) Then rayAngle += Math.PI * 2
        If (rayAngle > Math.PI * 2) Then rayAngle -= Math.PI * 2

        If (Math.Cos(rayAngle) = 0 Or Math.Sin(rayAngle) = 0) Then Return Nothing
        Dim rayGradient = (Math.Sin(rayAngle) / Math.Cos(rayAngle))
        Dim c = -(pntLocation.X * rayGradient) + pntLocation.Y
        'Console.Write(pntLocation.ToString())
        'Console.WriteLine("y = {0}x + {1}", rayGradient, c)

        Dim xAtTop = (bullet.Bounds.Top - c) / rayGradient
        Dim xAtBottom = (bullet.Bounds.Bottom - c) / rayGradient
        Dim yAtLeft = (rayGradient * bullet.Bounds.Left) + c
        Dim yAtRight = (rayGradient * bullet.Bounds.Right) + c

        ''Now check if any of the points exist on the rectangles edges
        If (bullet.Bounds.Left <= xAtTop And xAtTop <= bullet.Bounds.Right) Then
            ''Hit Top (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle, bullet)
        End If
        If (bullet.Bounds.Left <= xAtBottom And xAtBottom <= bullet.Bounds.Right) Then
            ''Hit Bottom (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle, bullet)
        End If
        If (bullet.Bounds.Top <= yAtLeft And yAtLeft <= bullet.Bounds.Bottom) Then
            ''Hit Left (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle, bullet)
        End If
        If (bullet.Bounds.Top <= yAtRight And yAtRight <= bullet.Bounds.Bottom) Then
            ''Hit Right (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle, bullet)
        End If

        Return Nothing
    End Function

    Private Function CheckIfForward(Angle As Integer, Bullet As Bullet)
        If (Math.Cos(Angle) > 0) Then
            If (Bullet.Bounds.Right < pntLocation.X) Then
                Return Bullet.Middle
            End If
        Else
            If (Bullet.Bounds.Left > pntLocation.X) Then
                Return Bullet.Middle
            End If
        End If
        Return Nothing
    End Function


    Private Function AngleBetweenTwoPoints(Point1 As Vector2, Point2 As Vector2) As Double
        Dim offsetPoint = Vector2.Add(Point2, -Point1)
        Dim interiorAngle = Math.Atan2(-offsetPoint.Y, offsetPoint.X)
        Dim theta = interiorAngle - Math.PI / 2
        Return theta
    End Function

    Class MovementState
        Public Forward As Boolean
        Public Backward As Boolean
        Public Left As Boolean
        Public right As Boolean
        Public PointInJump As Double

        Public Sub New()

        End Sub

        Public ReadOnly Property Moving As Boolean
            Get
                Return Forward Or Backward Or Left Or right
            End Get
        End Property
    End Class
End Class
