Imports System.Reflection

Public Class Renderer

    Public Sub New(Hud As HUD, rays As List(Of Ray), ClientPlayer As Player, Game As Game)
        Me.Game = Game
        Me.HUD = Hud
        Me.Rays = rays
        Me.OwnPlayer = ClientPlayer
    End Sub

    Private Property Game As Game
    Private Property HUD As HUD

    Private Property Rays As System.Collections.Generic.List(Of Ray)

    Private Property OwnPlayer As WinShooter.Player

    Public Sub Render(e As PaintEventArgs, formSize As Size)
        ''Draw Walls
        For i = 0 To Rays.Count - 1
            Dim collision As Ray.Collision = Rays(i).CheckCollision(Game.Map.map, OwnPlayer)
            If (Not collision Is Nothing) Then
                Dim value = collision.Color
                Dim point = collision.CollisionPoint

                Dim distance = DistanceBetweenTwoPoints(New PointF(OwnPlayer.Position.East_m, OwnPlayer.Position.North_m), point)
                Dim sectionWidth = formSize.Width / Rays.Count
                Dim rectLeft = i * sectionWidth

                Dim Diff = Rays(i).HeadingDiffFromCenterPov_deg
                distance = Math.Abs(distance * Math.Cos(Diff))

                Dim height = (formSize.Height / distance)
                'If (distance < 1) Then height = Me.Height


                Dim percent = (height / (formSize.Height)) * 1.6
                If (percent > 1 Or percent <= 0) Then percent = 1

                Dim colour As Color
                Select Case value
                    Case CByte(1)
                        colour = Color.FromArgb(255 * percent, 255 * percent, 0 * percent)
                    Case CByte(2)
                        colour = Color.FromArgb(255 * percent, 0 * percent, 0 * percent)
                    Case CByte(3)
                        colour = Color.FromArgb(0, 0, 255 * percent)
                End Select

                Dim yOffset = OwnPlayer.Position.Up_m * height
                Dim Middle = formSize.Height / 2
                e.Graphics.FillRectangle(New SolidBrush(colour), New Rectangle(New Point(rectLeft, Middle - (height / 2) + yOffset), New Size(sectionWidth + 1, height)))

            End If

        Next

        ''See Entities
        For i = 0 To Game.Entities.Count - 1
            Dim sightDistance = Nothing
            Dim raysOfSight = New List(Of UInt16)
            For j = 0 To Rays.Count - 1
                ''SightPoint in gamespace
                Dim Sight As PointF = SeeEntity(Game.Entities(i), Rays(j).HeadingDiffFromCenterPov_deg)
                If (Sight = Nothing) Then Continue For

                sightDistance = DistanceBetweenTwoPoints(New PointF(OwnPlayer.Position.East_m, OwnPlayer.Position.North_m), Sight)

                Dim rayCollision As Ray.Collision = Rays(j).CheckCollision(Game.Map.map, OwnPlayer)

                ''TODO: ThIS MAKE THE BULLETS KINDA FUNKY, FIX IT

                '''Check if the current ray can also see a wall
                'If (rayCollision.Point <> Nothing) Then
                '    Dim wallDistance = DistanceBetweenTwoPoints(player.pntLocation, rayCollision.Point)
                '    ''check if the wall is obscuring the bullet
                '    If (wallDistance < sightDistance) Then Continue For
                'End If

                ''add rayIndex to list of rays that can see the bullet
                raysOfSight.Add(j)
            Next
            If (raysOfSight.Count = 0) Then Continue For
            ''If here then the the bullet has been seen

            ''Find the middle of the section of the screen where the bullet is seen
            Dim rayIndex = raysOfSight(Math.Floor(raysOfSight.Count / 2))

            Dim sectionwidth = formSize.Width / Rays.Count
            ''Find the x coord of where the center of the bullet will be drawn
            Dim rectleft = rayIndex * sectionwidth
            If (sightDistance = 0) Then Continue For

            Game.Entities(i).Draw(sightDistance, rectleft, formSize, OwnPlayer.Position.Up_m, e)
        Next

        HUD.Render(e, formSize)
    End Sub

    Private Function SeeEntity(Entity As Entity, RayAngleDiff_deg As Decimal)
        Dim rayAngle = OwnPlayer.Position.Heading_deg + RayAngleDiff_deg
        'Console.WriteLine(rayAngle * 180 / Math.PI)

        If (rayAngle < 0) Then rayAngle += Math.PI * 2
        If (rayAngle > Math.PI * 2) Then rayAngle -= Math.PI * 2

        If (Math.Cos(rayAngle) = 0 Or Math.Sin(rayAngle) = 0) Then Return Nothing
        Dim rayGradient = (Math.Sin(rayAngle) / Math.Cos(rayAngle))
        Dim c = -(OwnPlayer.Position.East_m * rayGradient) + OwnPlayer.Position.North_m
        'Console.Write(pntLocation.ToString())
        'Console.WriteLine("y = {0}x + {1}", rayGradient, c)

        Dim xAtTop = (Entity.HitBox.Top - c) / rayGradient
        Dim xAtBottom = (Entity.HitBox.Bottom - c) / rayGradient
        Dim yAtLeft = (rayGradient * Entity.HitBox.Left) + c
        Dim yAtRight = (rayGradient * Entity.HitBox.Right) + c

        ''Now check if any of the points exist on the rectangles edges
        If (Entity.HitBox.Left <= xAtTop And xAtTop <= Entity.HitBox.Right) Then
            ''Hit Top (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle, Entity)
        End If
        If (Entity.HitBox.Left <= xAtBottom And xAtBottom <= Entity.HitBox.Right) Then
            ''Hit Bottom (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle, Entity)
        End If
        If (Entity.HitBox.Top <= yAtLeft And yAtLeft <= Entity.HitBox.Bottom) Then
            ''Hit Left (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle, Entity)
        End If
        If (Entity.HitBox.Top <= yAtRight And yAtRight <= Entity.HitBox.Bottom) Then
            ''Hit Right (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle, Entity)
        End If

        Return Nothing
    End Function

    Private Function CheckIfForward(Angle As Integer, Entity As Entity)
        If (Math.Cos(Angle) > 0) Then
            If (Entity.HitBox.Right < Entity.Position.East_m) Then
                Return Entity.Middle
            End If
        Else
            If (Entity.HitBox.Left > Entity.Position.East_m) Then
                Return Entity.Middle
            End If
        End If
        Return Nothing
    End Function
End Class
