Public Class Game

    Public Sub New(initialPositionPLayer As GamePosition, map As GameMap, fov As Integer, resQuality As Integer)

        Dim ClientPlayer = New Player("Cinnabun", New Motion(initialPositionPLayer, New GameVelocity(0, 0, 0), DateTime.UtcNow), Me, True)
        AddEntity(ClientPlayer)
        Me.Map = map

        Dim HUD = New HUD(ClientPlayer.EntityId, Me)

        Dim rays As New List(Of Ray)
        For i = 0 To resQuality - 1
            Dim diff = -fov / 2 + i / (resQuality / fov)
            Dim ray = New Ray(diff)
            rays.Add(ray)
        Next
        Me.Sight = New Renderer(HUD, rays, ClientPlayer, Me)
    End Sub

    Dim _Entities As Dictionary(Of Guid, Entity)
    Public Property Entities As Dictionary(Of Guid, Entity)
        Get
            Return _Entities
        End Get
        Private Set(value As Dictionary(Of Guid, Entity))
            _Entities = value
        End Set
    End Property

    Public Property Map As GameMap

    Public ReadOnly Property Sight As Renderer

    Public Property InputState As New InputState()

    Public Sub Start()

    End Sub

    Public Sub AddEntity(entity As Entity)
        Entities.Add(entity.EntityId, entity)
    End Sub
End Class
