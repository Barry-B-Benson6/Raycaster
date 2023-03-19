Public Class GameVelocity
    Public Sub New(east As Decimal, north As Decimal, up As Decimal)
        Me.East = east
        Me.North = north
        Me.Up = up
    End Sub
    Public ReadOnly Property East As Decimal

    Public ReadOnly Property North As Decimal

    Public ReadOnly Property Up As Decimal

    Public ReadOnly Property isStill As Boolean
        Get
            Return (East = 0 And North = 0 And Up = 0)
        End Get
    End Property

End Class
