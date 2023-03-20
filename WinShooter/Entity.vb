Imports System.Numerics

Public MustInherit Class Entity
    Public Sub New(name As String, motion As Motion, game As Game, locallyOwned As Boolean)
        Me.Name = RequireNotNull(name)
        Me.Motion = RequireNotNull(motion)
        Me.EntityId = Guid.NewGuid()
        Me.Game = RequireNotNull(game)
        Me.IsAlive = True
        Me.isDirty = True
        Me.LocallyOwned = locallyOwned
        Position = motion.CalculatePositionAtTime(DateTime.UtcNow)
        HitBox = New RectangleF(New PointF(Position.East_m, Position.North_m), New SizeF(0.8, 0.8))
    End Sub

    ''' <summary>
    ''' This exists to allow us to create mocks of this class, it should not be used any other time
    ''' </summary>
    Protected Sub New()
        MyBase.New()
    End Sub

    Private _HitBox As RectangleF
    Public Property HitBox As RectangleF
        Get
            Return _HitBox
        End Get
        Set(value As RectangleF)
            _HitBox = value
        End Set
    End Property

    Private _IsAlive As Boolean

    Public Property IsAlive As Boolean
        Get
            Return _IsAlive
        End Get
        Protected Set(value As Boolean)
            _IsAlive = value
        End Set
    End Property

    Private _Position As GamePosition
    Public Property Position As GamePosition
        Get
            SyncLock Me
                Return _Position
            End SyncLock
        End Get
        Protected Set(value As GamePosition)
            SyncLock Me
                _Position = value
            End SyncLock
        End Set
    End Property

    Private _Game As WinShooter.Game
    Public Property Game As WinShooter.Game
        Get
            Return _Game
        End Get
        Protected Set(value As WinShooter.Game)
            _Game = value
        End Set
    End Property

    Private _EntityId As Guid
    Public Property EntityId As Guid
        Get
            Return _EntityId
        End Get
        Protected Set(value As Guid)
            _EntityId = value
        End Set
    End Property

    Private _Name As String
    Public Overridable Property Name As String
        Get
            Return _Name
        End Get
        Protected Set(value As String)
            _Name = value
        End Set
    End Property

    Private _LocallyOwned As Boolean
    Public Property LocallyOwned As Boolean
        Get
            Return _LocallyOwned
        End Get
        Protected Set(value As Boolean)
            _LocallyOwned = value
        End Set
    End Property

    Private _Motion As WinShooter.Motion
    Public Property Motion As WinShooter.Motion
        Get
            SyncLock Me
                Return _Motion
            End SyncLock
        End Get
        Set(value As WinShooter.Motion)
            SyncLock Me
                _Motion = value
            End SyncLock
        End Set
    End Property

    Private _isDirty As Boolean
    Public Property isDirty As Boolean
        Get
            SyncLock Me
                Return _isDirty
            End SyncLock
        End Get
        Protected Set(value As Boolean)
            SyncLock Me
                _isDirty = value
            End SyncLock
        End Set
    End Property

    Public ReadOnly Property Middle As PointF
        Get
            Return New PointF(HitBox.Right / 2, HitBox.Bottom / 2)
        End Get
    End Property

    Public MustOverride Sub Draw(Distance As Decimal, xCoordOfMiddle As Integer, formSize As Size, PlayerZ As Decimal, e As PaintEventArgs)
    Public MustOverride Sub UpdateState(time As DateTime)

    Public Overridable Sub UpdatePosition(time As DateTime)
        Position = Motion.CalculatePositionAtTime(time)
    End Sub
End Class
