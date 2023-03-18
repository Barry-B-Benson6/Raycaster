Public MustInherit Class Entity

    Public Property IsAlive As Integer
        Get
            Return Nothing
        End Get
        Set(value As Integer)
        End Set
    End Property

    Public Property Position As GamePosition
        Get
            SyncLock Me
                Return Nothing
            End SyncLock
        End Get
        Set(value As GamePosition)
            SyncLock Me

            End SyncLock
        End Set
    End Property

    Public Property Game As WinShooter.Game
        Get
            Return Nothing
        End Get
        Set(value As WinShooter.Game)
        End Set
    End Property

    Public Property EntityId As Guid
        Get
            Return Nothing
        End Get
        Set(value As Guid)
        End Set
    End Property

    Public Property Name As Integer
        Get
            Return Nothing
        End Get
        Set(value As Integer)
        End Set
    End Property

    Public Overridable ReadOnly Property LocallyOwned As Boolean
        Get
            Return Nothing
        End Get
    End Property

    Public Property Motion As WinShooter.Motion
        Get
            SyncLock Me
                Return Nothing
            End SyncLock
        End Get
        Set(value As WinShooter.Motion)
            SyncLock Me

            End SyncLock
        End Set
    End Property

    ''' <remarks></remarks>
    Public Overridable Sub UpdateState()

    End Sub

    Public Sub UpdatePosition()

    End Sub
End Class
