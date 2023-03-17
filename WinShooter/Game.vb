Public Class Game
    Public Property Entities As System.Collections.Generic.List(Of WinShooter.Entity)
        Get
            Return Nothing
        End Get
        Set(value As System.Collections.Generic.List(Of WinShooter.Entity))
        End Set
    End Property

    Public Property Map As GameMap
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

    Public Sub Start()

    End Sub
End Class
