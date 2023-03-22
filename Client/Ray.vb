Public Class Ray
    ''' <summary>
    ''' In Radians from up clockwise
    ''' </summary>
    Public decDirection As Decimal
    Public decDiff As Decimal
    Public lastCollision As Collision

    Public Sub Main()

    End Sub

    Public Sub New(angle As Decimal, diff As Decimal)
        decDirection = angle
        decDiff = diff
    End Sub

    Public Function CheckCollision(Map As Byte(,), player As Player)
        Dim Point = player.pntLocation
        Dim oldPoint = Point
        Dim Cos = Math.Cos(player.decAngle + decDirection)
        Dim Sin = Math.Sin(player.decAngle + decDirection)

        '' go foward in small increments unill a wall is occured then calculate approx collision point
        For i = 0 To 3000
            If (Point.X >= Map.GetLength(0) Or Point.Y >= Map.GetLength(1)) Then
                oldPoint = Point
                Point.X += Cos * 0.01
                Point.Y += Sin * 0.01
                Continue For
            End If
            If (Point.X < 0) Then
                'Return New PointF(Point.X + 1, Point.Y)
                oldPoint = Point
                Point.X += Cos * 0.01
                Point.Y += Sin * 0.01
                Continue For
            ElseIf (Point.Y < 0) Then
                'Return New PointF(Point.X, Point.Y + 1)
                oldPoint = Point
                Point.X += Cos * 0.01
                Point.Y += Sin * 0.01
                Continue For
            End If
            If (Map(Math.Floor(Point.X), Math.Floor(Point.Y)) <> 0) Then
                Dim value = Map(Math.Floor(Point.X), Math.Floor(Point.Y))
                ''Now we know it has hit the wall we need to Figure out if it was a vertical collision or a horizontal collision
                If (Math.Floor(oldPoint.X) <> Math.Floor(Point.X) And Math.Floor(oldPoint.Y) = Math.Floor(Point.Y)) Then
                    ''Vertical Hit
                    If Math.Floor(oldPoint.X) > Math.Floor(Point.X) Then
                        lastCollision = New Collision(New PointF(Math.Floor(Point.X) + 1, Point.Y), value)
                        Return lastCollision
                    Else
                        lastCollision = New Collision(New PointF(Math.Floor(Point.X), Point.Y), value)
                        Return lastCollision
                    End If
                ElseIf (Math.Floor(oldPoint.Y) <> Math.Floor(Point.Y) And Math.Floor(oldPoint.X) = Math.Floor(Point.X)) Then
                    ''Horizontal Hit
                    If (Math.Floor(oldPoint.Y) > Math.Floor(Point.Y)) Then
                        ''Coming from Bottom
                        lastCollision = New Collision(New PointF(Point.X, Math.Floor(Point.Y) + 1), value)
                        Return lastCollision
                    Else
                        ''Coming From top
                        lastCollision = New Collision(New PointF(Point.X, Math.Floor(Point.Y)), value)
                        Return lastCollision
                    End If
                ElseIf (Math.Floor(oldPoint.Y) <> Math.Floor(Point.Y) And Math.Floor(oldPoint.X) <> Math.Floor(Point.X)) Then
                    lastCollision = New Collision(Point, value)
                    Return lastCollision
                End If

            End If
            oldPoint = Point
            Point.X += Cos * 0.01
            Point.Y += Sin * 0.01
        Next
        lastCollision = New Collision()
        Return lastCollision
    End Function

    Public Class Collision
        Public Point As PointF
        Public value As Byte
        Public Sub New(PointF, bytValue)
            Point = PointF
            value = bytValue
        End Sub
        Public Sub New()
        End Sub
    End Class
End Class
