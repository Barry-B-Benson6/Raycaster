Public Class HUD

    Public Sub New(playerId As Guid, game As Game)
        ClientPlayerId = playerId
        Me.game = game
    End Sub

    Private ReadOnly game As Game
    Private Property ClientPlayerId As Guid

    Public Sub Render(e As PaintEventArgs, screenSize As Size)
        Dim ClientPlayer As Player = game.Entities(ClientPlayerId)
        If (ClientPlayer.IsAiming) Then
            Dim gun = My.Resources.Resources.ADS_gun
            e.Graphics.DrawImage(gun, New Point((screenSize.Width / 2) - (gun.Width / 2), screenSize.Height - gun.Height))
        Else
            e.Graphics.TranslateTransform(screenSize.Width / 2, screenSize.Height / 2)
            If (Not ClientPlayer.Motion.VelocityStamp.isStill) Then e.Graphics.TranslateTransform(0, Rnd() * 5)
            Dim gun = My.Resources.Resources.Hip_Gun
            e.Graphics.DrawImage(gun, New Rectangle(-(screenSize.Width / 32), 0, screenSize.Width / 2, screenSize.Height / 2))
        End If
    End Sub
End Class
