Public Class ServerBrowser

    Private MultiplayerClient As New MultiplayerClient()
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim Servers As List(Of MultiplayerClient.ResponseObjects.Server) = MultiplayerClient.RequestServers()
        If Servers IsNot Nothing Then
            For Each server In Servers
                lstServer.Items.Add(server.serverName)
            Next
        End If
    End Sub
    Private Sub btnCreateGame_Click(sender As Object, e As EventArgs) Handles btnCreateGame.Click
        Dim serverid = MultiplayerClient.CreateGame()
        If (serverid <> Nothing) Then
            Me.Hide()
            Dim frm As New GameForm(serverid, MultiplayerClient)
            frm.Show()
            'Dim game = New GameForm(serverid, MultiplayerClient)
        End If
    End Sub
End Class