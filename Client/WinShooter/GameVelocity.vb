Public Class GameVelocity
    Public Sub New(east_ms As Decimal, north_ms As Decimal, up_ms As Decimal)
        Me.East_ms = east_ms
        Me.North_ms = north_ms
        Me.Up_ms = up_ms
    End Sub
    Public ReadOnly Property East_ms As Decimal

    Public ReadOnly Property North_ms As Decimal

    Public ReadOnly Property Up_ms As Decimal

    Public ReadOnly Property isStill As Boolean
        Get
            Return (East_ms = 0 And North_ms = 0 And Up_ms = 0)
        End Get
    End Property

End Class
