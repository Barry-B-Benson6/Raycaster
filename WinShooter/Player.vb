Public Class Player
    Inherits Entity

    Public Property Crouching As Integer
        Get
            SyncLock Me
                Return Nothing
            End SyncLock
        End Get
        Set(value As Integer)
            SyncLock Me

            End SyncLock
        End Set
    End Property
End Class
