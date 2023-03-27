Imports System.Threading
Public Class Game
    Private ReadOnly MultiplayerClient As MultiplayerClient
    Public Sub New(initialPositionPlayer As GamePosition, map As GameMap, fov As Integer, resQuality As Integer, form As Form, MultiplayerClient As MultiplayerClient)
        Me.MultiplayerClient = MultiplayerClient
        Me.MultiplayerClient.Game = Me

        AddHandler form.Paint, AddressOf Render
        AddHandler form.KeyDown, AddressOf KeyboardButtonDown
        AddHandler form.KeyUp, AddressOf KeyboardButtonUp
        AddHandler form.MouseDown, AddressOf MouseButtonDown
        AddHandler form.MouseUp, AddressOf MouseButtonUp
        AddHandler form.GotFocus, AddressOf GainedFocus
        AddHandler form.LostFocus, AddressOf LostFocus


        Me.Form = form
        Dim ClientPlayer = New Player("Cinnabun", New Motion(initialPositionPlayer, New GameVelocity(0, 0, 0), DateTime.UtcNow), Me, True)
        AddEntity(ClientPlayer)
        Me.Map = map

        Dim HUD = New HUD(ClientPlayer.EntityId, Me)

        Dim rays As New List(Of Ray)
        For i = 0 To resQuality - 1
            Dim diff = -fov / 2 + i / (resQuality / fov)
            Dim ray = New Ray(diff)
            rays.Add(ray)
        Next
        Me.Sight = New Renderer(HUD, rays, ClientPlayer, Me, fov)
    End Sub

    Private _entityBuffer As New Dictionary(Of Guid, Entity)
    Public Property entityBuffer As Dictionary(Of Guid, Entity)
        Get
            SyncLock Me
                Return _entityBuffer
            End SyncLock
        End Get
        Set(value As Dictionary(Of Guid, Entity))
            SyncLock Me
                _entityBuffer = value
            End SyncLock
        End Set
    End Property

    Private ReadOnly Form As Form

    Dim _Entities As New Dictionary(Of Guid, Entity)
    Public Property Entities As Dictionary(Of Guid, Entity)
        Get
            SyncLock Me
                Return _Entities
            End SyncLock
        End Get
        Private Set(value As Dictionary(Of Guid, Entity))
            SyncLock Me
                _Entities = value
            End SyncLock
        End Set
    End Property

    Public Property Map As GameMap

    Public ReadOnly Property Sight As Renderer

    Public Property InputState As New InputState()

    Public Sub Start()
        Dim StateCycle As Thread = New Thread(AddressOf funcStateCycle)
        Dim RenderCycle As Thread = New Thread(AddressOf funcRenderCycle)
        StateCycle.Start()
        RenderCycle.Start()
    End Sub

    Public Sub UpdateEntities(entityStates As List(Of MultiplayerClient.RequestParams.EntityState))
        For Each entity In entityStates
            'Console.WriteLine(entity.type_string)
        Next
    End Sub

    Private Async Sub funcStateCycle()
        While True
            Dim copyOfBuffer = New Dictionary(Of Guid, Entity)(entityBuffer)
            For Each KeyValue In copyOfBuffer
                Entities(KeyValue.Key) = KeyValue.Value
            Next

            For i = 0 To Entities.Count - 1
                Entities.Values(i).UpdateState(DateTime.UtcNow)
                Entities.Values(i).UpdatePosition(DateTime.UtcNow)
            Next


            For i = 0 To Entities.Count - 1
                Dim entity = Entities.Values(i)

                Dim dirtyEntities As New List(Of Entity)
                If (entity.isDirty) Then
                    'Console.WriteLine("Dirty")
                    dirtyEntities.Add(entity)
                    Entities.Values(i).isDirty = False
                End If
                If (dirtyEntities.Count <> 0) Then
                    MultiplayerClient.SendDirtyEntities(dirtyEntities)
                End If

            Next

            'For Each entity In copyOfBuffer.Values
            '    Entities(entity.EntityId) = entity
            'Next

            Dim entityList = New List(Of Entity)(Entities.Values)
            For i = 0 To entityList.Count - 1
                If (Not entityList(i).IsAlive) Then
                    Entities.Remove(Entities.Keys(i))
                End If
            Next
        End While
    End Sub

    Private Sub funcRenderCycle()
        While True
            Try
                If (Form.Created) Then
                    Form.Invoke(Sub() Form.Refresh())
                End If
            Catch ex As Exception
                End
            End Try
            Thread.Sleep(5)
        End While
    End Sub

    Private Sub Render(sender As Form, e As PaintEventArgs)
        Sight.Render(e, sender.Size)
    End Sub

    Public Sub AddEntity(entity As Entity)
        RequireNotNull(entity)
        Entities.Add(entity.EntityId, entity)
    End Sub

    Private Sub MouseButtonDown(sender As Object, e As MouseEventArgs)
        InputState.Update(e.Button, True)
    End Sub
    Private Sub MouseButtonUp(sender As Object, e As MouseEventArgs)
        InputState.Update(e.Button, False)
    End Sub

    Private Sub KeyboardButtonDown(sender As Object, e As KeyEventArgs)
        InputState.Update(e.KeyCode, True)
    End Sub
    Private Sub KeyboardButtonUp(sender As Object, e As KeyEventArgs)
        InputState.Update(e.KeyCode, False)
    End Sub

    Public ReadOnly Property formSize As Size
        Get
            Return Form.Size
        End Get
    End Property

    Private _Focused As Boolean
    Public Property Focused As Boolean
        Get
            Return _Focused
        End Get
        Private Set(value As Boolean)
            _Focused = value
        End Set
    End Property
    Private Sub GainedFocus(sender As Object, e As EventArgs)
        Cursor.Hide()
        Focused = True
    End Sub

    Private Sub LostFocus(sender As Object, e As EventArgs)
        Cursor.Show()
        Cursor.Position = New Point(Form.Size.Width / 2, Form.Size.Width / 2)
        Focused = False
    End Sub
End Class
