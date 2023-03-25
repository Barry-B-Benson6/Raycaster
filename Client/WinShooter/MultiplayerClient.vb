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
    Private ServerId As Guid

    Private ws As ClientWebSocket = New ClientWebSocket
    Private entityReceiver As ClientWebSocket = New ClientWebSocket

    Public Sub New()

    End Sub

    Public ReadOnly Property IsConnected As Boolean
        Get
            Return (ws.State = WebSocketState.Open)
        End Get
    End Property

    Private _HasGame
    Private _Game
    Public Property Game As Game
        Get
            Return _Game
        End Get
        Set(value As Game)
            _Game = value
            _HasGame = True
        End Set
    End Property

    Private _IsLinked As Boolean
    Public Property IsLinked As Boolean
        Get
            Return _IsLinked
        End Get
        Private Set(value As Boolean)
            _IsLinked = value
        End Set
    End Property

    Public Async Sub InitializeConnection()
        Try
            Await ws.ConnectAsync(New Uri("ws://localhost:3001"), Nothing)
            Console.WriteLine(ws.State)
            If (ws.State = WebSocketState.Open) Then
                Console.WriteLine("Connection opened")
            End If

            Await entityReceiver.ConnectAsync(New Uri("ws://localhost:3001"), Nothing)

            If (entityReceiver.State = WebSocketState.Open) Then
                Console.WriteLine("Reciever Connection Established")

                While True
                    Console.WriteLine("Looping")
                    Dim buffer(4096) As Byte
                    Dim result = Await entityReceiver.ReceiveAsync(buffer, CancellationToken.None)
                    Console.WriteLine("Reciever recieved")
                    If (result.MessageType = WebSocketMessageType.Close) Then
                        Console.WriteLine("Receiver connection closing")
                        Await entityReceiver.CloseAsync(WebSocketCloseStatus.NormalClosure, Nothing, CancellationToken.None)
                    Else
                        Console.WriteLine("Entity receiver has recieved a message:")
                        Dim result_string = Encoding.ASCII.GetString(buffer, 0, result.Count)

                        Dim resultObject = JsonSerializer.Deserialize(Of ResponseObjects.LinkReciever)(result_string)
                        Console.WriteLine(resultObject.action + " " + result_string)
                        If (resultObject.action = "FinishLinking") Then
                            IsLinked = resultObject.success
                        Else
                            Dim data = JsonSerializer.Deserialize(Of ResponseObjects.UpdateEntities)(result_string)

                            Dim entitys = New List(Of Entity)

                            'For Each entityState In data.entityStates
                            '    Dim type_String = entityState.type_string
                            '    Dim typeObj As Type = Type.GetType(type_String, True)
                            '    Dim myObject As Object = Activator.CreateInstance(typeObj, "HI")
                            '    Dim myInstance = Convert.ChangeType(myObject, typeObj)
                            'Next
                        End If


                    End If
                End While
            End If

        Catch ex As Exception
            Console.WriteLine("Connection err: {0}", ex)
        End Try
    End Sub

    Public Async Function SendDirtyEntities(entites As List(Of Entity)) As Task(Of Boolean)

        Dim ListOfStates As New List(Of RequestParams.EntityState)
        For Each entity In entites
            ListOfStates.Add(New RequestParams.EntityState(entity))
        Next

        Dim data_string = JsonSerializer.Serialize(Of RequestParams.SendDirtyEntities)(New RequestParams.SendDirtyEntities(ListOfStates, ServerId, ClientId))
        Dim data = Encoding.UTF8.GetBytes(data_string)

        Console.WriteLine("awaiting Message")
        Await SendMessage(data)
        Console.WriteLine("returning true")
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
        Await LinkReciever(serverId)
        If (resultObject.success And IsLinked) Then Me.ServerId = serverId
        Return (resultObject.success And IsLinked)
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
        Await LinkReciever(dataObject.serverId)
        If (resultObject.success And IsLinked) Then
            ServerId = dataObject.serverId
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

    Private Async Function LinkReciever(serverId) As Task(Of Task())
        Dim dataObject = New RequestParams.LinkReciever(serverId, ClientId)
        Dim dataString = JsonSerializer.Serialize(Of RequestParams.LinkReciever)(dataObject)
        Dim data = Encoding.UTF8.GetBytes(dataString)

        Await entityReceiver.SendAsync(data, WebSocketMessageType.Text, True, Nothing)
    End Function

    Public Class RequestParams

        Public Class GetServers
            Public Property endpoint As String = "GetServers"
        End Class

        Public Class EntityState
            Public Property type_string As String
            Public Property motion As Motion
            Public Property isAlive As Boolean
            Public Property entityId As Guid
            Public Sub New(entity As Entity)
                Me.motion = entity.Motion
                Me.isAlive = entity.IsAlive
                Me.entityId = entity.EntityId
                Me.type_string = entity.GetType().ToString()
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

        Public Class LinkReciever
            Public Property endpoint As String = "LinkSocketToServer"
            Public Property serverId As Guid
            Public Property clientId As Guid
            Public Sub New(serverId As Guid, clientId As Guid)
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

        Public Class LinkReciever
            Public Property action As String
            Public Property success As Boolean
        End Class

        Public Class UpdateEntities
            Public Property action As String
            Public Property entityStates As List(Of ResponseObjects.EntityState)
        End Class

        Public Class EntityState
            Public Property type_string As String
            Public Property motion As Motion
            Public Property isAlive As Boolean
            Public Property entityId As Guid
        End Class
    End Class
End Class
