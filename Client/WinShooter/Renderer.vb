Imports System.Reflection

Public Class Renderer

    Private WallTextures As New List(Of Bitmap)
    Private FloorTextures As New List(Of Bitmap)

    Public Sub New(Hud As HUD, rays As List(Of Ray), ClientPlayer As Player, Game As Game, fov As Integer)
        Me.Game = Game
        Me.HUD = Hud
        Me.Rays = rays
        Me.OwnPlayer = ClientPlayer
        Me.Fov = fov

        FloorTextures.Add(New Bitmap(My.Resources.Textures.pixil_frame_0, New Size(Constants.FloorTextureQuality, Constants.FloorTextureQuality)))

        WallTextures.Add(New Bitmap(My.Resources.Textures.PaintedPlaster013_1K_Color, New Size(Constants.WallTextureQuality, Constants.WallTextureQuality)))
    End Sub

    ''' <summary>
    ''' the number of degrees as an integer that the renderers sight spans 
    ''' </summary>
    Private ReadOnly Fov As Integer

    Private Property Game As Game
    Private Property HUD As HUD

    Private Property Rays As System.Collections.Generic.List(Of Ray)

    Private Property OwnPlayer As WinShooter.Player

    Public Sub Render(e As PaintEventArgs, formSize As Size)
        Dim stopwatch = New Stopwatch()
        stopwatch.Start()

        Dim CopyOfPlayer = New Player(OwnPlayer)

        Dim middle = formSize.Height / 2

        Dim yOffsetDuetoLook = Math.Tan(ToRadians(CopyOfPlayer.Position.Tilt_deg)) * middle

        'e.Graphics.FillRectangle(New SolidBrush(Color.LightBlue), New Rectangle(New Point(0, 0), New Size(Game.formSize.Width, (Game.formSize.Height / 2) + yOffsetDuetoLook)))
        'e.Graphics.FillRectangle(New SolidBrush(Color.Green), New Rectangle(New Point(0, middle + yOffsetDuetoLook), New Size(Game.formSize.Width, (Game.formSize.Height / 2) - yOffsetDuetoLook)))

        ''Draw Walls
        DrawWalls(e, formSize, CopyOfPlayer)

        ''See Entities
        DrawEntities(e, formSize, CopyOfPlayer)

        ''Draw Map
        ''DrawMap(e, formSize, CopyOfPlayer)

        HUD.Render(e, formSize)
        e.Graphics.ResetTransform()

        Dim font = Form.DefaultFont
        e.Graphics.DrawString((1000 / stopwatch.ElapsedMilliseconds).ToString(), Form.DefaultFont, New SolidBrush(Color.Black), New Point(0, 0))
    End Sub

    Private Sub DrawMap(e As PaintEventArgs, formSize As Size, Player As Player)
        Dim sizCellSize = New Size(50, 50)
        Dim Map = Game.Map.map
        e.Graphics.ResetTransform()

        Dim CellPlayerLocation = Player.Position.ToCellSpacePointF

        For x = 0 To Map.GetLength(0) - 1
            For y = 0 To Map.GetLength(1) - 1
                If (Map(x, y) = 0) Then Continue For
                Dim pos = New Point(x * sizCellSize.Width + 5, y * sizCellSize.Height + 5)
                e.Graphics.FillRectangle(New SolidBrush(Color.Black), New Rectangle(pos, sizCellSize))
            Next
        Next
        Dim player2dPoint = New Point(CellPlayerLocation.X * sizCellSize.Width, CellPlayerLocation.Y * sizCellSize.Height)
        e.Graphics.FillEllipse(New SolidBrush(Color.Blue), New Rectangle(New Point(player2dPoint.X - 10, player2dPoint.Y - 10), New Size(20, 20)))
        e.Graphics.DrawLine(New Pen(Color.Black, 5), player2dPoint, New Point(player2dPoint.X + (Math.Cos(ToRadians(Player.Position.Heading_deg)) * 10), player2dPoint.Y + (Math.Sin(ToRadians(Player.Position.Heading_deg)) * 10)))




        For i = 0 To Rays.Count - 1
            Dim ray = Rays(i)
            Dim collision As Ray.Collision = ray.CheckCollision(Map, Player)
            If (collision IsNot Nothing) Then
                Dim collisionPoint As PointF = collision.CollisionPoint
                e.Graphics.DrawLine(New Pen(Color.FromArgb(40, Color.Red)), player2dPoint, New Point(collisionPoint.X * sizCellSize.Width, collisionPoint.Y * sizCellSize.Height))
            End If
        Next

        Dim entities = New Dictionary(Of Guid, Entity)(Game.Entities).Values
        For i = 0 To entities.Count - 1
            Dim cellEntityPos = New PointF(entities(i).HitBox.Right / CellSize_m, entities(i).HitBox.Bottom / CellSize_m)
            Dim cellEntitySize = entities(i).HitBox.Size / Constants.CellSize_m
            Dim entity2dPoint = New Point((cellEntityPos.X - cellEntitySize.Width) * sizCellSize.Width, (cellEntityPos.Y - cellEntitySize.Height) * sizCellSize.Height)

            e.Graphics.DrawRectangle(New Pen(Color.Blue), New Rectangle(entity2dPoint, New Size(20, 20)))
            e.Graphics.DrawString(entities(i).Position.ToCellSpacePointF().ToString(), Form.DefaultFont, New SolidBrush(Color.Black), entity2dPoint)
        Next

        e.Graphics.DrawLine(New Pen(Color.HotPink), player2dPoint, New Point(player2dPoint.X + (Math.Cos(0) * 30), player2dPoint.Y - (Math.Sin(0) * 30)))
    End Sub

    Private Sub DrawEntities(e As PaintEventArgs, formSize As Size, Player As Player)
        Dim Entities = New Dictionary(Of Guid, Entity)(Game.Entities)
        Dim playerPos = Player.Position.ToCellSpacePointF()
        For i = 0 To Entities.Count - 1
            ''Dont Draw Self
            If (Entities.Values(i).LocallyOwned And Entities.Values(i).isPLayer) Then Continue For
            Dim entityPos = Entities.Values(i).Position.ToCellSpacePointF()

            Dim VectorBetween = New PointF(entityPos.X - playerPos.X, entityPos.Y - playerPos.Y)

            Dim angleOfVector_deg As Decimal

            If (VectorBetween.Y > 0) Then
                If (VectorBetween.X > 0) Then
                    ''The Vector between is in the top right quadrant
                    angleOfVector_deg = 90 - ToDegrees(Math.Atan(VectorBetween.X / VectorBetween.Y))
                ElseIf (VectorBetween.X < 0) Then
                    ''The Vector between is in the top left quadrant
                    angleOfVector_deg = 180 + ToDegrees(Math.Atan(VectorBetween.Y / VectorBetween.X))
                Else
                    ''the vector between the two is veritcal up
                    angleOfVector_deg = 0
                End If
            ElseIf (VectorBetween.Y < 0) Then
                If (VectorBetween.X > 0) Then
                    ''The vector between is in the bottom right quadrant
                    angleOfVector_deg = 0 + ToDegrees(Math.Atan(VectorBetween.Y / VectorBetween.X))
                ElseIf (VectorBetween.X < 0) Then
                    ''The Vector between is in the bottom left quadrant
                    angleOfVector_deg = 180 + (90 - ToDegrees(Math.Atan(VectorBetween.X / VectorBetween.Y)))
                Else
                    ''The vector between is vertical down
                    angleOfVector_deg = 180
                End If
            Else
                If (VectorBetween.X > 0) Then
                    ''The vector between is horizontal right
                    angleOfVector_deg = 90
                ElseIf (VectorBetween.X < 0) Then
                    ''The vector between is horizontal left
                    angleOfVector_deg = 270
                Else
                    ''The entity is in the same position as the player
                    Continue For
                End If
            End If


            Dim slopeOfVector = VectorBetween.Y / VectorBetween.X

            Dim startFov_deg = Player.Position.Heading_deg + Rays(0).HeadingDiffFromCenterPov_deg
            Dim endFov_deg = Player.Position.Heading_deg + Rays(Rays.Count - 1).HeadingDiffFromCenterPov_deg


            Dim inFov = IsBetween(startFov_deg, endFov_deg, angleOfVector_deg)

            If inFov Then
                ''Now check if obstructed
                Dim diff
                If Player.Position.Heading_deg < angleOfVector_deg Then
                    diff = DifferenceBetweenAngles(Player.Position.Heading_deg, angleOfVector_deg)
                Else
                    diff = DifferenceBetweenAngles(angleOfVector_deg, Player.Position.Heading_deg)
                End If
                Dim tempRay = New Ray(diff)
                Dim collision = tempRay.CheckCollision(Game.Map.map, Player)
                Dim EntityDistance = LengthOfVector(VectorBetween)
                If (collision IsNot Nothing) Then
                    Dim wallDistance = DistanceBetweenTwoPoints(Player.Position.ToCellSpacePointF(), collision.CollisionPoint)
                    If (wallDistance < EntityDistance) Then Continue For
                End If

                Dim percentInFovThatSightIs = DifferenceBetweenAngles(startFov_deg, angleOfVector_deg) / DifferenceBetweenAngles(startFov_deg, endFov_deg)
                Dim xCoordToDrawCenter As Integer = Math.Floor(formSize.Width * percentInFovThatSightIs)
                Entities.Values(i).Draw(EntityDistance, xCoordToDrawCenter, formSize, Player.Position.Up_m, e, Fov, Player.Position.Tilt_deg)

            End If

        Next
    End Sub

    Private Sub DrawWalls(e As PaintEventArgs, formSize As Size, Player As Player)

        Dim middle = formSize.Height / 2
        For i = 0 To Rays.Count - 1
            Dim collision As Ray.Collision = Rays(i).CheckCollision(Game.Map.map, Player)
            If (Not collision Is Nothing) Then
                Dim value = collision.Color
                Dim point = collision.CollisionPoint

                Dim texture = WallTextures(value - 1)


                DrawSlice(texture, collision, formSize, e, Player, i)

            End If

        Next
    End Sub

    Private Function DrawSlice(WallTexture As Bitmap, collision As Ray.Collision, formSize As Size, e As PaintEventArgs, player As Player, i As Integer)
        Dim collisionPoint = collision.CollisionPoint
        Dim distance = DistanceBetweenTwoPoints(player.Position.ToCellSpacePointF(), collisionPoint)
        Dim sectionWidth = formSize.Width / Rays.Count
        Dim rectLeft = i * sectionWidth

        Dim Diff = Rays(i).HeadingDiffFromCenterPov_deg
        distance = Math.Abs(distance * Math.Cos(ToRadians(Diff)))

        Dim height = (formSize.Height / distance)
        If (distance < 1) Then height = formSize.Height


        Dim percent = (height / (formSize.Height)) * 1.6
        If (percent > 1 Or percent <= 0) Then percent = 1


        Dim yOffsetDueToLookANgle As Decimal = Math.Tan(ToRadians(player.Position.Tilt_deg)) * ((height - formSize.Height) / 2)

        Dim yOffset = player.Position.Up_m / Constants.CellSize_m * height
        yOffset -= yOffsetDueToLookANgle

        Dim xIndexInTexture As Integer

        If (collision.isVertical) Then
            ''Collision is vertical
            Dim workingY = collisionPoint.Y - Math.Floor(collisionPoint.Y)
            xIndexInTexture = Math.Floor(workingY * Constants.WallTextureQuality)
        Else
            ''Collision is horizontal
            Dim workingX = collisionPoint.X - Math.Floor(collisionPoint.X)
            xIndexInTexture = Math.Floor(workingX * Constants.WallTextureQuality)
        End If

        Dim sectionHeight As Decimal = (height / Constants.WallTextureQuality)
        Dim brush As SolidBrush
        For i = 0 To Constants.WallTextureQuality - 1
            Dim color = WallTexture.GetPixel(xIndexInTexture, i)
            If (collision.isVertical) Then color = Color.FromArgb(color.R * 0.7, color.G * 0.7, color.B * 0.7)
            brush = New SolidBrush(color)
            Dim topLeft = New Point(rectLeft, (sectionHeight * i) + yOffset)
            e.Graphics.FillRectangle(brush, New Rectangle(topLeft, New Size(sectionWidth + 1, sectionHeight + 1)))
        Next

        ''Now draw floor

        'Dim firstPxToDraw = ((formSize.Height - height) / 2) + height + yOffset
        'Dim numOfPx = formSize.Height - firstPxToDraw
        'If (numOfPx < 0) Then Throw New Exception("neg")
        'Dim middleY = formSize.Height / 2
        'For y = firstPxToDraw To numOfPx + firstPxToDraw Step sectionHeight
        '    Dim corrctedYForMath = y - middleY
        '    Dim percentY = middleY / corrctedYForMath
        '    Dim angle = (Fov / 2) * percentY
        '    Dim dist = (player.Position.Up_m + 1) * Math.Sin(ToRadians(90 - angle))

        '    Dim lineHeading_deg = (Fov - ((rectLeft / formSize.Width) * Fov)) + player.Position.Heading_deg

        '    Dim floorX = player.Position.East_m - (dist * Math.Cos(ToRadians(lineHeading_deg)))
        '    Dim floorY = player.Position.North_m - (dist * Math.Sin(ToRadians(lineHeading_deg)))

        '    Dim floorTexture = FloorTextures(0)

        '    Dim xIndexInFloorTexture As Integer
        '    Dim yIndexInFloorTexture As Integer

        '    Dim workingY = floorY - Math.Floor(floorY)
        '    yIndexInFloorTexture = Math.Floor(workingY * Constants.FloorTextureQuality)

        '    Dim workingX = floorX - Math.Floor(floorX)
        '    xIndexInFloorTexture = Math.Floor(workingX * Constants.FloorTextureQuality)
        '    brush = New SolidBrush(floorTexture.GetPixel(xIndexInFloorTexture, yIndexInFloorTexture))
        '    Dim topLeft = New Point(rectLeft, y)
        '    e.Graphics.FillRectangle(brush, New Rectangle(topLeft, New Size(sectionWidth, sectionHeight)))
        'Next

    End Function

    Private Function SeeEntity(Entity As Entity, RayAngleDiff_deg As Decimal, player As Player)
        Dim rayAngle_deg = player.Position.Heading_deg + RayAngleDiff_deg
        'Console.WriteLine(rayAngle * 180 / Math.PI)

        If (rayAngle_deg < 0) Then rayAngle_deg += 360
        If (rayAngle_deg > 360) Then rayAngle_deg -= 360

        If (Math.Cos(ToRadians(rayAngle_deg)) = 0 Or Math.Sin(ToRadians(rayAngle_deg)) = 0) Then Return Nothing
        Dim rayGradient = (Math.Sin(ToRadians(rayAngle_deg)) / Math.Cos(ToRadians(rayAngle_deg)))
        Dim c = -(player.Position.East_m * rayGradient) + player.Position.North_m
        'Console.Write(pntLocation.ToString())
        'Console.WriteLine("y = {0}x + {1}", rayGradient, c)

        Dim top = Entity.Position.Up_m + (Entity.HitBox.Size.Height / 2)
        Dim bottom = Entity.Position.Up_m - (Entity.HitBox.Size.Height / 2)
        Dim left = Entity.Position.East_m - (Entity.HitBox.Size.Width / 2)
        Dim right = Entity.Position.East_m + (Entity.HitBox.Size.Width / 2)


        Dim xAtTop = (top - c) / rayGradient
        Dim xAtBottom = (bottom - c) / rayGradient
        Dim yAtLeft = (rayGradient * left) + c
        Dim yAtRight = (rayGradient * right) + c

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
