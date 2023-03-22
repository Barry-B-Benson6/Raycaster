Imports System.Numerics

Public Class GamePosition

    Public Sub New(east_m As Decimal, north_m As Decimal, up_m As Decimal, heading_deg As Decimal, tilt_deg As Decimal)
        Me.North_m = north_m
        Me.East_m = east_m
        Me.Up_m = up_m
        Me.Heading_deg = heading_deg
        Me.Tilt_deg = tilt_deg
    End Sub

    Public Sub New(east_m As Decimal, north_m As Decimal, up_m As Decimal)
        Me.North_m = north_m
        Me.East_m = east_m
        Me.Up_m = up_m
        Me.Heading_deg = 0
        Me.Tilt_deg = 0
    End Sub
    Public ReadOnly Property North_m As Decimal

    Public ReadOnly Property East_m As Decimal

    Public ReadOnly Property Up_m As Decimal

    Public ReadOnly Property Heading_deg As Decimal

    Public ReadOnly Property Tilt_deg As Decimal

    ''' <summary>
    ''' Returns a vector3 representing the currentPosition
    ''' </summary>
    Public Function ToCellSpacePointF() As PointF
        Return New PointF(East_m / Constants.CellSize_m, North_m / Constants.CellSize_m)
    End Function

End Class
