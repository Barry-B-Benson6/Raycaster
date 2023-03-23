Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.WebSockets
Imports System.Text
Imports System.Text.Json
Imports System.Threading

Public Class MultiplayerClient

    Private ReadOnly ClientId As Guid = Guid.NewGuid()
    Private ServerId As Guid = Guid.NewGuid()

    Private ws As ClientWebSocket = New ClientWebSocket
    Public Sub New()

    End Sub

    Public ReadOnly Property IsConnected As Boolean
        Get
            Return ws.State = WebSocketState.Open
        End Get
    End Property

    Public Async Sub InitializeConnection()
        Try
            Await ws.ConnectAsync(New Uri("ws://localhost:3001"), Nothing)
            If (ws.State = WebSocketState.Open) Then
                Console.WriteLine("Connection opened")
                While True
                    Dim buffer(256) As Byte
                    Dim result = Await ws.ReceiveAsync(buffer, CancellationToken.None)
                    If (result.MessageType = WebSocketMessageType.Close) Then
                        Await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, Nothing, CancellationToken.None)
                    Else
                        'Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, result.Count))
                    End If
                End While
            End If
        Catch ex As Exception
            Console.WriteLine("Connection err: {0}", ex)
        End Try
    End Sub

    Public Async Function SendDirtyEntities(entites As List(Of Entity)) As Task(Of Boolean)
        Dim uri = New Uri(String.Format("http://localhost:3000/sendDirtyEntities?serverId={0}&clientId={1}", ServerId.ToString(), ClientId.ToString()))

        Dim ListOfStates As New List(Of RequestParams.EntityState)
        For Each entity In entites
            ListOfStates.Add(New RequestParams.EntityState(entity))
        Next

        Dim data_string = JsonSerializer.Serialize(Of RequestParams.SendDirtyEntities)(New RequestParams.SendDirtyEntities(ListOfStates, ServerId, ClientId))
        Dim data = Encoding.UTF8.GetBytes(data_string)


        Dim result_string = Await SendMessage(data)
        Return True
    End Function


    ''' <summary>
    ''' Gives the server a serverId and requests to join it. Currently allways allowed in the future may require password
    ''' </summary>
    ''' <param name="serverId">The Guid of the server to join</param>
    ''' <returns>TorF representing whether it was allowed to join</returns>
    Public Async Function JoinGame(serverId As Guid) As Task(Of Boolean)
        Dim data_string = JsonSerializer.Serialize(Of RequestParams.JoinGame)(New RequestParams.JoinGame(serverId, ClientId))
        Dim data = Encoding.UTF8.GetBytes(data_string)

        Dim result_string = Await SendMessage(data)
        Dim resultObject = JsonSerializer.Deserialize(Of ResponseObjects.JoinGame)(result_string)
        If (resultObject.success) Then Me.ServerId = serverId
        Return resultObject.success
    End Function

    ''' <summary>
    ''' Asks the server for a list of all game servers
    ''' </summary>
    ''' <returns>List(of Server)</returns>
    Public Async Function RequestServers() As Task(Of List(Of ResponseObjects.Server))
        Dim data_string = JsonSerializer.Serialize(Of RequestParams.GetServers)(New RequestParams.GetServers())
        Dim data = Encoding.UTF8.GetBytes(data_string)

        Dim result_string As String = Await SendMessage(data)
        If result_string = "[]" Then Return New List(Of ResponseObjects.Server)
        Dim resultObject = JsonSerializer.Deserialize(Of List(Of ResponseObjects.Server))(result_string)
        Return resultObject
    End Function

    ''' <summary>
    ''' This will send a request to the server to make a new game, the server will respond with a responseObjects.CreateGame
    ''' </summary>
    ''' <returns>Server guid if successful or nothing if not</returns>
    Public Async Function CreateGame() As Task(Of Guid)
        Dim dataObject = New RequestParams.CreateGame(ClientId, "Guest1")
        Dim dataString = JsonSerializer.Serialize(Of RequestParams.CreateGame)(dataObject)
        Dim data = Encoding.UTF8.GetBytes(dataString)

        Dim result_string = Await SendMessage(data)
        Dim resultObject = JsonSerializer.Deserialize(Of ResponseObjects.CreateGame)(result_string)
        If (resultObject.success) Then
            Return resultObject.serverId
        Else
            Return Nothing
        End If
    End Function

    Private Async Function SendMessage(data As Byte()) As Task(Of String)
        Await ws.SendAsync(data, WebSocketMessageType.Text, True, Nothing)

        Dim bytes(4096) As Byte
        Dim result = Await ws.ReceiveAsync(bytes, Nothing)
        Return Encoding.ASCII.GetString(bytes, 0, result.Count)
    End Function

    Private Class RequestParams

        Public Class GetServers
            Public Property endpoint As String = "GetServers"
        End Class

        Public Class EntityState

            Public Property motion As Motion
            Public Property isAlive As Boolean
            Public Property entityId As Guid
            Public Sub New(entity As Entity)
                Me.motion = entity.Motion
                Me.isAlive = entity.IsAlive
                Me.entityId = entity.EntityId
            End Sub
        End Class

        Public Class CreateGame
            Public Property endpoint As String = "CreateGame"
            Public Property serverName As String
            Public Property clientId As Guid = clientId
            Public Property hostName As String
            Public Property serverId As Guid = Guid.NewGuid()

            ''' <summary>
            ''' 
            ''' </summary>
            ''' <param name="id"></param>
            ''' <param name="username"></param>
            Public Sub New(id, username)
                clientId = id
                hostName = username
                serverName = String.Format("{0}'s Server", username)
            End Sub
        End Class

        Public Class JoinGame
            Public Property endpoint = "JoinGame"
            Public Property serverId As Guid
            Public Property clientId As Guid

            Public Sub New(serverId As Guid, clientId As Guid)
                Me.serverId = serverId
                Me.clientId = clientId
            End Sub
        End Class

        Public Class SendDirtyEntities
            Public Property endpoint As String = "SendDirtyEntities"
            Public Property entities As List(Of EntityState)
            Public Property serverId As Guid
            Public Property clientId As Guid

            Public Sub New(entities As List(Of EntityState), serverId As Guid, clientId As Guid)
                Me.entities = entities
                Me.serverId = serverId
                Me.clientId = clientId
            End Sub

        End Class
    End Class

    Public Class ResponseObjects
        Public Class Server
            Public Property serverName As String
            Public Property hostName As String
            Public Property serverId As Guid
        End Class
        Public Class CreateGame
            Public Property success As Boolean
            Public Property serverId As Guid
        End Class

        Public Class JoinGame
            Public Property success As Boolean
        End Class

        Public Class SendDirtyEntites
            Public Property success As Boolean
        End Class

    End Class
End Class
