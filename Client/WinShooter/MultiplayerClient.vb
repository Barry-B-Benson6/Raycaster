Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Text
Imports System.Text.Json

Public Class MultiplayerClient

    Private ReadOnly ClientId As Guid = Guid.NewGuid()
    Public Sub New()

    End Sub

    Public Function RequestServers() As List(Of ResponseObjects.Server)
        Dim uri = New Uri("http://localhost:3000/servers")
        Dim data = Encoding.UTF8.GetBytes("")
        Dim result_post = SendRequest(uri, data, "application/json", "POST")
        If result_post = "[]" Then Return Nothing
        Dim resultObject = JsonSerializer.Deserialize(Of List(Of ResponseObjects.Server))(result_post)
        Return resultObject
    End Function

    ''' <summary>
    ''' This will send a request to the server to make a new game, the server will respond with a responseObjects.CreateGame
    ''' </summary>
    ''' <returns>Server guid if successful or nothing if not</returns>
    Public Function CreateGame() As Guid
        Dim uri = New Uri("http://localhost:3000/createGame")
        Dim dataObject = New RequestParams.CreateGame(ClientId, "Guest1")
        Dim dataString = JsonSerializer.Serialize(Of RequestParams.CreateGame)(dataObject)
        Console.WriteLine(dataObject.hostName)
        Dim data = Encoding.UTF8.GetBytes(dataString)
        Dim result_post = SendRequest(uri, data, "application/json", "POST")
        Dim responseObject = JsonSerializer.Deserialize(Of ResponseObjects.CreateGame)(result_post)
        If (responseObject.success) Then
            Return responseObject.serverId
        Else
            Return Nothing
        End If
    End Function

    Private Function SendRequest(uri As Uri, jsonDataBytes As Byte(), contentType As String, method As String) As String
        Dim response As String
        Dim request As WebRequest

        request = WebRequest.Create(uri)
        request.ContentLength = jsonDataBytes.Length
        request.ContentType = contentType
        request.Method = method

        Using requestStream = request.GetRequestStream
            requestStream.Write(jsonDataBytes, 0, jsonDataBytes.Length)
            requestStream.Close()

            Using responseStream = request.GetResponse.GetResponseStream
                Using reader As New StreamReader(responseStream)
                    response = reader.ReadToEnd()
                End Using
            End Using
        End Using

        Return response
    End Function

    Private Class RequestParams
        Public Class CreateGame
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
    End Class
End Class
