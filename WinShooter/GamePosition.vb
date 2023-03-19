Imports System.Numerics

Public Class GamePosition

    Public Sub New(east As Decimal, north As Decimal, up As Decimal, heading_deg As Decimal, tilt_deg As Decimal)
        Me.North = north
        Me.East = east
        Me.Up = up
        Me.Heading_deg = heading_deg
        Me.Tilt_deg = tilt_deg
    End Sub

    Public Sub New(east As Decimal, north As Decimal, up As Decimal)
        Me.North = north
        Me.East = east
        Me.Up = up
        Me.Heading_deg = 0
        Me.Tilt_deg = 0
    End Sub
    Public ReadOnly Property North As Decimal

    Public ReadOnly Property East As Decimal

    Public ReadOnly Property Up As Decimal

    Public ReadOnly Property Heading_deg As Decimal

    Public ReadOnly Property Tilt_deg As Decimal

    ''' <summary>
    ''' Returns a vector3 representing the currentPosition
    ''' </summary>
    Public Function ToVector3() As Vector3
        Return New Vector3(East, North, Up)
    End Function

End Class
