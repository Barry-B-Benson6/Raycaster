Public Class Renderer
    Public Property HUD As HUD
        Get
            Return Nothing
        End Get
        Set(value As HUD)
        End Set
    End Property

    Public Property Rays As System.Collections.Generic.List(Of Ray)
        Get
            Return Nothing
        End Get
        Set(value As System.Collections.Generic.List(Of Ray))
        End Set
    End Property

    Public Property OwnPlayer As OwnPlayer
        Get
            Return Nothing
        End Get
        Set(value As OwnPlayer)
        End Set
    End Property

    Public Sub Render()

    End Sub
End Class
