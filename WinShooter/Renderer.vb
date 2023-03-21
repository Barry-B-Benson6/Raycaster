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

        Dim middle = formSize.Height / 2

        e.Graphics.FillRectangle(New SolidBrush(Color.LightBlue), New Rectangle(New Point(0, 0), New Size(Game.formSize.Width, Game.formSize.Height / 2)))
        e.Graphics.FillRectangle(New SolidBrush(Color.Green), New Rectangle(New Point(0, middle), New Size(Game.formSize.Width, Game.formSize.Height / 2)))

        ''Draw Walls
        DrawWalls(e, formSize)

        ''See Entities
        DrawEntities(e, formSize)

        ''Draw Map
        ''DrawMap(e, formSize)

        HUD.Render(e, formSize)
        e.Graphics.ResetTransform()
    End Sub

    Private Sub DrawMap(e As PaintEventArgs, formSize As Size)
        Dim sizCellSize = New Size(50, 50)
        Dim Map = Game.Map.map
        e.Graphics.ResetTransform()

        Dim CellPlayerLocation = OwnPlayer.Position.ToCellSpacePointF

        For x = 0 To Map.GetLength(0) - 1
            For y = 0 To Map.GetLength(1) - 1
                If (Map(x, y) = 0) Then Continue For
                Dim pos = New Point(x * sizCellSize.Width + 5, y * sizCellSize.Height + 5)
                e.Graphics.FillRectangle(New SolidBrush(Color.Black), New Rectangle(pos, sizCellSize))
            Next
        Next
        Dim player2dPoint = New Point(CellPlayerLocation.X * sizCellSize.Width, CellPlayerLocation.Y * sizCellSize.Height)
        e.Graphics.FillEllipse(New SolidBrush(Color.Blue), New Rectangle(New Point(player2dPoint.X - 10, player2dPoint.Y - 10), New Size(20, 20)))
        e.Graphics.DrawLine(New Pen(Color.Black, 5), player2dPoint, New Point(player2dPoint.X + (Math.Cos(ToRadians(OwnPlayer.Position.Heading_deg)) * 10), player2dPoint.Y + (Math.Sin(ToRadians(OwnPlayer.Position.Heading_deg)) * 10)))




        For i = 0 To Rays.Count - 1
            Dim ray = Rays(i)
            Dim collision As Ray.Collision = ray.CheckCollision(Map, OwnPlayer)
            If (collision IsNot Nothing) Then
                Dim collisionPoint As PointF = collision.CollisionPoint
                e.Graphics.DrawLine(New Pen(Color.Red), player2dPoint, New Point(collisionPoint.X * sizCellSize.Width, collisionPoint.Y * sizCellSize.Height))
            End If
        Next

        Dim entities = New Dictionary(Of Guid, Entity)(Game.Entities).Values
        For i = 0 To entities.Count - 1
            Dim cellEntityPos = entities(i).Position.ToCellSpacePointF()
            Dim cellEntitySize = entities(i).HitBox.Size / Constants.CellSize_m
            Dim entity2dPoint = New Point((cellEntityPos.X - cellEntitySize.Width) * sizCellSize.Width, (cellEntityPos.Y - cellEntitySize.Height) * sizCellSize.Height)

            e.Graphics.DrawRectangle(New Pen(Color.Blue), New Rectangle(entity2dPoint, New Size(20, 20)))
        Next
    End Sub

    Private Sub DrawEntities(e As PaintEventArgs, formSize As Size)
        Dim Entities = New Dictionary(Of Guid, Entity)(Game.Entities)
        For i = 0 To Entities.Count - 1
            ''Dont Draw Self
            If (Entities.Values(i).LocallyOwned And Entities.Values(i).isPLayer) Then Continue For
            Dim sightDistance = Nothing
            Dim raysOfSight = New List(Of UInt16)
            For j = 0 To Rays.Count - 1
                ''SightPoint in gamespace
                Dim Sight As PointF = SeeEntity(Entities.Values(i), Rays(j).HeadingDiffFromCenterPov_deg)
                If (Sight = Nothing) Then Continue For
                Sight = New PointF(Sight.X / Constants.CellSize_m, Sight.Y / Constants.CellSize_m)
                sightDistance = DistanceBetweenTwoPoints(New PointF(OwnPlayer.Position.East_m / Constants.CellSize_m, OwnPlayer.Position.North_m / Constants.CellSize_m), Sight)

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

            Entities.Values(i).Draw(sightDistance, rectleft, formSize, OwnPlayer.Position.Up_m, e)
        Next
    End Sub

    Private Sub DrawWalls(e As PaintEventArgs, formSize As Size)
        Dim stopwatch = New Stopwatch()
        stopwatch.Start()

        Dim middle = formSize.Height / 2
        For i = 0 To Rays.Count - 1
            Dim collision As Ray.Collision = Rays(i).CheckCollision(Game.Map.map, OwnPlayer)
            If (Not collision Is Nothing) Then
                Dim value = collision.Color
                Dim point = collision.CollisionPoint

                Dim distance = DistanceBetweenTwoPoints(OwnPlayer.Position.ToCellSpacePointF(), point)
                Dim sectionWidth = formSize.Width / Rays.Count
                Dim rectLeft = i * sectionWidth

                Dim Diff = Rays(i).HeadingDiffFromCenterPov_deg
                distance = Math.Abs(distance * Math.Cos(ToRadians(Diff)))

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

                Dim yOffset = OwnPlayer.Position.Up_m / Constants.CellSize_m * height
                e.Graphics.FillRectangle(New SolidBrush(colour), New Rectangle(New Point(rectLeft, middle - (height / 2) + yOffset), New Size(sectionWidth + 1, height)))

            End If

        Next
        Dim font = Form.DefaultFont
        e.Graphics.DrawString((1000 / stopwatch.ElapsedMilliseconds).ToString(), Form.DefaultFont, New SolidBrush(Color.Black), New Point(0, 0))
        'Console.WriteLine(1000 / stopwatch.ElapsedMilliseconds)
    End Sub

    Private Function SeeEntity(Entity As Entity, RayAngleDiff_deg As Decimal)
        Dim rayAngle_deg = OwnPlayer.Position.Heading_deg + RayAngleDiff_deg
        'Console.WriteLine(rayAngle * 180 / Math.PI)

        If (rayAngle_deg < 0) Then rayAngle_deg += 360
        If (rayAngle_deg > 360) Then rayAngle_deg -= 360

        If (Math.Cos(ToRadians(rayAngle_deg)) = 0 Or Math.Sin(ToRadians(rayAngle_deg)) = 0) Then Return Nothing
        Dim rayGradient = (Math.Sin(ToRadians(rayAngle_deg)) / Math.Cos(ToRadians(rayAngle_deg)))
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
            Return CheckIfForward(rayAngle_deg, Entity)
        End If
        If (Entity.HitBox.Left <= xAtBottom And xAtBottom <= Entity.HitBox.Right) Then
            ''Hit Bottom (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle_deg, Entity)
        End If
        If (Entity.HitBox.Top <= yAtLeft And yAtLeft <= Entity.HitBox.Bottom) Then
            ''Hit Left (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle_deg, Entity)
        End If
        If (Entity.HitBox.Top <= yAtRight And yAtRight <= Entity.HitBox.Bottom) Then
            ''Hit Right (doesnt matter whether this is exit or entry)
            Return CheckIfForward(rayAngle_deg, Entity)
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
