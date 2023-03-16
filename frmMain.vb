Imports System.Drawing.Text
Imports System.Numerics
Imports System.Reflection.Emit
Imports System.Windows.Input

Public Class frmMain
    Private Map As Byte(,)
    Private sizCellSize As Size
    Private player As Player
    Private rays As List(Of Ray)
    Private Focused As Boolean
    Private Bullets As List(Of Bullet)

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Randomize()
        DoubleBuffered = True
        Map = {
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 1, 0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 1, 0, 0, 0, 0, 2, 2, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 2, 2, 0, 0, 0, 1},
            {1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
        }
        sizCellSize = New Size(50, 50)
        player = New Player()
        rays = New List(Of Ray)
        Bullets = New List(Of Bullet)
        For i = 0 To 360
            Dim diff = (Math.PI / 4) - ((i / 4) * (Math.PI / 180))
            Dim ray = New Ray((player.decAngle - diff), -diff)
            rays.Add(ray)
        Next
        Focused = True
        Timer1.Start()
    End Sub

    Private Sub GotFocuss(sender As Object, e As EventArgs) Handles MyBase.GotFocus
        Cursor.Hide()
        Cursor.Position = New Point(Me.Width / 2, Me.Height / 2)
        tmrMovementHandler.Start()
    End Sub

    Private Sub LostFocuss(sender As Object, e As EventArgs) Handles MyBase.LostFocus
        Cursor.Show()
        tmrMovementHandler.Stop()
    End Sub

    Private Sub frmMainPaint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint

        Dim Middle = Me.Height / 2


        e.Graphics.FillRectangle(New SolidBrush(Color.LightBlue), New Rectangle(New Point(0, 0), New Size(Me.Width, Me.Height / 2)))
        e.Graphics.FillRectangle(New SolidBrush(Color.Green), New Rectangle(New Point(0, Middle), New Size(Me.Width, Me.Height / 2)))

        ''Draw Walls
        For i = 0 To rays.Count - 1
            Dim collision As Ray.Collision = rays(i).CheckCollision(Map, player)
            If (collision.Point <> Nothing) Then
                Dim value = collision.value
                Dim point = collision.Point


                Dim vectorBetween = New PointF(point.X - player.pntLocation.X, point.Y - player.pntLocation.Y)
                Dim distance = Math.Sqrt(Math.Pow(vectorBetween.X, 2) + Math.Pow(vectorBetween.Y, 2))
                Dim sectionWidth = Me.Width / rays.Count
                Dim rectLeft = i * sectionWidth

                Dim Diff = rays(i).decDiff
                distance = Math.Abs(distance * Math.Cos(Diff))

                Dim height = (Me.Height / distance)
                'If (distance < 1) Then height = Me.Height


                Dim percent = (height / (Me.Height)) * 1.6
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

                e.Graphics.FillRectangle(New SolidBrush(colour), New Rectangle(New Point(rectLeft, Middle - (height / 2)), New Size(sectionWidth + 1, height)))

            End If

        Next

        ''Move Bullets
        Dim newBullets = New List(Of Bullet)
        For Each bullet As Bullet In Bullets
            If (Not bullet.expired) Then
                newBullets.Add(bullet)
            End If
        Next
        Bullets = newBullets

        For i = 0 To Bullets.Count - 1
            Bullets(i).Move(Map)
        Next

        e.Graphics.DrawString(player.decVerticalAngle.ToString(), Me.Font, New SolidBrush(Color.Black), New PointF(0, 0))

        ''See Bullets
        For i = 0 To Bullets.Count - 1
            Dim sightDistance = Nothing
            Dim raysOfSight = New List(Of UInt16)
            For j = 0 To rays.Count - 1
                ''SightPoint in gamespace
                Dim Sight As PointF = player.SeeBullet(Bullets(i), rays(j).decDiff)
                If (Sight = Nothing) Then Continue For
                ''SightPoint relative to player (needed for distance calculation)
                Dim SightVector As PointF = New PointF(Sight.X - player.pntLocation.X, Sight.Y - player.pntLocation.Y)
                ''Distance found with pythag
                sightDistance = Math.Sqrt(Math.Pow(SightVector.X, 2) + Math.Pow(SightVector.Y, 2))
                ''add rayIndex to list of rays that can see the bullet
                raysOfSight.Add(j)
            Next
            If (raysOfSight.Count = 0) Then Continue For
            ''If here then the the bullet has been seen

            ''Find the middle of the section of the screen where the bullet is seen
            Dim rayIndex = raysOfSight(Math.Floor(raysOfSight.Count / 2))

            Dim sectionwidth = Me.Width / rays.Count
            ''Find the x coord of where the center of the bullet will be drawn
            Dim rectleft = rayIndex * sectionwidth
            If (sightDistance = 0) Then Continue For
            Dim size = New Size((Me.Height / 5 / sightDistance), (Me.Height / 5 / sightDistance))
            e.Graphics.FillEllipse(New SolidBrush(Color.Gold), New Rectangle(New Point(rectleft - (size.Width / 2), Middle - size.Height / 2), size))
        Next

        ''Draw Gun
        e.Graphics.TranslateTransform(Me.Width / 2, Me.Height / 2)
        e.Graphics.TranslateTransform(0, Rnd() * 5)
        e.Graphics.RotateTransform(45)
        e.Graphics.DrawImage(My.Resources.Resources.SeekPng_com_ak_47_png_169312, New Point(0, 0))

#Region "MiniMapDraw"
        '_______________________________________________________________________________________________________

        'e.Graphics.ResetTransform()

        'For x = 0 To Map.GetLength(0) - 1
        '    For y = 0 To Map.GetLength(1) - 1
        '        If (Map(x, y) = 0) Then Continue For
        '        Dim pos = New Point(x * sizCellSize.Width + 5, y * sizCellSize.Height + 5)
        '        e.Graphics.FillRectangle(New SolidBrush(Color.Black), New Rectangle(pos, sizCellSize))
        '    Next
        'Next
        'Dim player2dPoint = New Point(player.pntLocation.X * sizCellSize.Width, player.pntLocation.Y * sizCellSize.Height)
        'e.Graphics.FillEllipse(New SolidBrush(Color.Blue), New Rectangle(New Point(player2dPoint.X - 10, player2dPoint.Y - 10), New Size(20, 20)))
        'e.Graphics.DrawLine(New Pen(Color.Black, 5), player2dPoint, New Point(player2dPoint.X + (Math.Cos(player.decAngle) * 10), player2dPoint.Y + (Math.Sin(player.decAngle) * 10)))




        'For i = 0 To rays.Count - 1
        '    Dim ray = rays(i)
        '    Dim collision As Ray.Collision = ray.CheckCollision(Map, player)
        '    If (collision.Point <> Nothing) Then
        '        Dim collisionPoint As PointF = collision.Point
        '        e.Graphics.DrawLine(New Pen(Color.Red), player2dPoint, New Point(collisionPoint.X * sizCellSize.Width, collisionPoint.Y * sizCellSize.Height))
        '    End If
        'Next

        'For i = 0 To Bullets.Count - 1
        '    Dim bounds = Bullets(i).Bounds
        '    e.Graphics.FillEllipse(New SolidBrush(Color.Gold), New Rectangle(New Point(bounds.Left * 50, bounds.Top * 50), New Size(bounds.Width * 50, bounds.Height * 50)))
        'Next

#Region "draw sights 2d"
        'For j = 0 To Bullets.Count - 1
        '    Dim sightdistance = Nothing
        '    Dim raysIndexOfSight = New List(Of UInt16)
        '    For i = 0 To rays.Count - 1
        '        If (player.SeeBullet(Bullets(j), rays(i).decDiff) <> Nothing) Then
        '            Dim sightPoint As PointF = player.SeeBullet(Bullets(j), rays(i).decDiff)

        '            e.Graphics.FillRectangle(New SolidBrush(Color.Blue), New Rectangle(New Point(sightPoint.X * 50, sightPoint.Y * 50), New Size(20, 20)))
        '        End If
        '    Next
        'Next
#End Region

        'e.Graphics.DrawString(player.decAngle * 180 / Math.PI, Font, New SolidBrush(Color.White), New PointF(0, 0))
#End Region

    End Sub

    Private Sub frmMain_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        player.Move(e, Map)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Refresh()
    End Sub

    Private Sub tmrMovementHandler_Tick(sender As Object, e As EventArgs) Handles tmrMovementHandler.Tick
        player.decAngle += (Cursor.Position.X - (Me.Width / 2)) * (Math.PI / 180) / 4
        If (player.decAngle > Math.PI * 2) Then
            player.decAngle -= Math.PI * 2
        End If
        If (player.decAngle < 0) Then
            player.decAngle += Math.PI * 2
        End If

        player.decVerticalAngle += (Cursor.Position.Y - (Me.Height / 2)) * (Math.PI / 180) / 4
        If (player.decVerticalAngle > Math.PI / 2) Then player.decVerticalAngle = Math.PI / 2
        If (player.decVerticalAngle < -Math.PI / 2) Then player.decVerticalAngle = -Math.PI / 2
        Cursor.Position = New Point(Me.Width / 2, Me.Height / 2)
    End Sub

    Private Sub frmMain_MouseDown(sender As Object, e As MouseEventArgs) Handles MyBase.MouseDown
        Bullets.Add(New Bullet(player.decAngle, player.pntLocation))
    End Sub
End Class
