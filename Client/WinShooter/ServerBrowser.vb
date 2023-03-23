Imports System.Threading

Public Class ServerBrowser

    Private MultiplayerClient As New MultiplayerClient()
    Dim Servers As List(Of MultiplayerClient.ResponseObjects.Server)
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        MultiplayerClient.InitializeConnection()

        While Not MultiplayerClient.IsConnected
            Thread.Sleep(1)
        End While

    End Sub

    Private Async Sub ServerBrowser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Servers = Await MultiplayerClient.RequestServers()
        If Servers IsNot Nothing Then
            For Each server In Servers
                lstServer.Items.Add(server.serverName)
            Next
        End If
    End Sub

    Private Async Sub btnCreateGame_Click(sender As Object, e As EventArgs) Handles btnCreateGame.Click
        Dim serverid = Await MultiplayerClient.CreateGame()
        If (serverid <> Nothing) Then
            Me.Hide()
            Dim frm As New GameForm(MultiplayerClient)
            frm.Show()
            'Dim game = New GameForm(serverid, MultiplayerClient)
        End If
    End Sub

    Private Async Sub btnJoinGame_Click(sender As Object, e As EventArgs) Handles btnJoinGame.Click
        If lstServer.SelectedIndex = -1 Then Exit Sub
        Dim serverid = Servers(lstServer.SelectedIndex).serverId
        If (Await MultiplayerClient.JoinGame(serverid)) Then
            Me.Hide()
            Dim frm As New GameForm(MultiplayerClient)
            frm.Show()
        End If
    End Sub

End Class