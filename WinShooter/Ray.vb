Imports System.Net.Http.Headers
Imports System.Numerics

Public Class Ray

    Public Sub New(HeadingDiff_deg As Decimal)
        HeadingDiffFromCenterPov_deg = HeadingDiff_deg
    End Sub
    Private ReadOnly Property HeadingDiffFromCenterPov_deg As Decimal

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>Details for Collision or nothing if no collision</returns>
    Public Function CheckCollision(Map As Byte(,), player As Player) As Collision
        Dim collision
        Dim Point As PointF = player.Position.ToCellSpacePointF()
        Dim oldPoint = Point
        Dim Cos = Math.Cos(ToRadians(player.Position.Heading_deg + HeadingDiffFromCenterPov_deg))
        Dim Sin = Math.Sin(ToRadians(player.Position.Heading_deg + HeadingDiffFromCenterPov_deg))

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
                        collision = New Collision(New PointF(Math.Floor(Point.X) + 1, Point.Y), value)
                        Return collision
                    Else
                        collision = New Collision(New PointF(Math.Floor(Point.X), Point.Y), value)
                        Return collision
                    End If
                ElseIf (Math.Floor(oldPoint.Y) <> Math.Floor(Point.Y) And Math.Floor(oldPoint.X) = Math.Floor(Point.X)) Then
                    ''Horizontal Hit
                    If (Math.Floor(oldPoint.Y) > Math.Floor(Point.Y)) Then
                        ''Coming from Bottom
                        collision = New Collision(New PointF(Point.X, Math.Floor(Point.Y) + 1), value)
                        Return collision
                    Else
                        ''Coming From top
                        collision = New Collision(New PointF(Point.X, Math.Floor(Point.Y)), value)
                        Return collision
                    End If
                ElseIf (Math.Floor(oldPoint.Y) <> Math.Floor(Point.Y) And Math.Floor(oldPoint.X) <> Math.Floor(Point.X)) Then
                    collision = New Collision(New PointF(Point.X, Point.Y), value)
                    Return collision
                End If

            End If
            oldPoint = Point
            Point.X += Cos * 0.01
            Point.Y += Sin * 0.01
        Next
        Return Nothing
    End Function

    ''' <summary>
    ''' Represents where a ray has collided with a wall and the color of the wall it collided with in 2d gamespace
    ''' </summary>
    Class Collision

        ''' <summary>
        '''  Creates new collision
        ''' </summary>
        ''' <param name="Color">The value on the map at the point of collision</param>
        ''' <param name="pointOfCollision">the point of collision in gamespace</param>
        Public Sub New(pointOfCollision As PointF, Color As Byte)
            CollisionPoint = RequireNotNull(pointOfCollision)
            Me.Color = Color
        End Sub

        Public ReadOnly Property CollisionPoint As PointF

        ''' <summary>
        ''' This is the value on the map at the point of collision
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Color As Byte
    End Class
End Class
