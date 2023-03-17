Public Class Ray

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>Details for Collision or nothing if no collision</returns>
    Public Function CheckCollision() As Collision
    End Function

    Class Collision
        Public Property CollisionPoint As Integer
            Get
                Return Nothing
            End Get
            Set(value As Integer)
            End Set
        End Property

        Public Property Color As Integer
            Get
                Return Nothing
            End Get
            Set(value As Integer)
            End Set
        End Property
    End Class
End Class
