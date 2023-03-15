Imports System.Numerics

Public Class Player

    Public pntLocation As PointF
    Public decAngle As Decimal
    Public decVerticalAngle As Decimal
    Public Sub New()
        pntLocation = New PointF(1.5, 1.5)
        decAngle = Math.PI
    End Sub

    Public Sub Move(e As KeyEventArgs, Map As Byte(,))
        Dim oldPoint = pntLocation
        Select Case e.KeyCode
            Case Keys.D
                'player.decAngle += (Math.PI * 2) / 180
            Case Keys.A
                'player.decAngle -= (Math.PI * 2) / 180
            Case Keys.W
                pntLocation.X -= Math.Cos(decAngle) * 0.1
                pntLocation.Y -= Math.Sin(decAngle) * 0.1
                Try
                    If (Map(Math.Floor(pntLocation.X), Math.Floor(pntLocation.Y)) > 0) Then
                        Dim x = Math.Floor(pntLocation.X)
                        Dim y = Math.Floor(pntLocation.Y)
                        Dim xOLd = Math.Floor(oldPoint.X)
                        Dim yOLd = Math.Floor(oldPoint.Y)

                        If (x <> xOLd And y <> yOLd) Then
                            pntLocation = oldPoint
                        ElseIf (x <> xOLd) Then
                            pntLocation.X = oldPoint.X
                        Else
                            pntLocation.Y = oldPoint.Y
                        End If
                    End If
                Catch
                End Try
            Case Keys.S
                pntLocation.X += Math.Cos(decAngle) * 0.1
                pntLocation.Y += Math.Sin(decAngle) * 0.1
                Try
                    If (Map(Math.Floor(pntLocation.X), Math.Floor(pntLocation.Y)) > 0) Then
                        Dim x = Math.Floor(pntLocation.X)
                        Dim y = Math.Floor(pntLocation.Y)
                        Dim xOLd = Math.Floor(oldPoint.X)
                        Dim yOLd = Math.Floor(oldPoint.Y)

                        If (x <> xOLd And y <> yOLd) Then
                            pntLocation = oldPoint
                        ElseIf (x <> xOLd) Then
                            pntLocation.X = oldPoint.X
                        Else
                            pntLocation.Y = oldPoint.Y
                        End If
                    End If
                Catch
                End Try
        End Select
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


    Private Function AngleBetweenTwoPoints(Point1 As Vector2, Point2 As Vector2)
        Dim offsetPoint = Vector2.Add(Point2, -Point1)
        Dim interiorAngle = Math.Atan2(-offsetPoint.Y, offsetPoint.X)
        Dim theta = interiorAngle - Math.PI / 2
        Return theta
    End Function
End Class
