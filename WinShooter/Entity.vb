﻿Public MustInherit Class Entity
    Public Property MotionModal As MotionModal
        Get
            Return Nothing
        End Get
        Set(value As MotionModal)
        End Set
    End Property

    Public Property IsAlive As Integer
        Get
            Return Nothing
        End Get
        Set(value As Integer)
        End Set
    End Property

    Public Property Position As Integer
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

    Public Property Game As WinShooter.Game
        Get
            Return Nothing
        End Get
        Set(value As WinShooter.Game)
        End Set
    End Property

    Public Property EntityId As Integer
        Get
            Return Nothing
        End Get
        Set(value As Integer)
        End Set
    End Property

    Public Property Name As Integer
        Get
            Return Nothing
        End Get
        Set(value As Integer)
        End Set
    End Property

    Public Overridable ReadOnly Property Updatable As Boolean
        Get
            Return Nothing
        End Get
    End Property

    ''' <remarks></remarks>
    Public Overridable Sub UpdateState()

    End Sub

    Public Sub UpdatePosition()

    End Sub
End Class
