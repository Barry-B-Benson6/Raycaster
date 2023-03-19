Public Class Game

    Dim _Entities As List(Of Entity)
    Public Property Entities As System.Collections.Generic.List(Of WinShooter.Entity)
        Get
            Return _Entities
        End Get
        Private Set(value As System.Collections.Generic.List(Of WinShooter.Entity))
            _Entities = value
        End Set
    End Property

    Public Overridable Property Map As GameMap
        Get
            Return Nothing
        End Get
        Set(value As GameMap)
        End Set
    End Property

    Public Property Sight As Renderer
        Get
            Return Nothing
        End Get
        Set(value As Renderer)
        End Set
    End Property

    Public Property InputState As InputState
        Get
            Return Nothing
        End Get
        Set(value As InputState)
        End Set
    End Property

    Public Sub Start()

    End Sub

    Public Sub AddEntity(entity As Entity)

    End Sub
End Class
