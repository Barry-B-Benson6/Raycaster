Public Class GamePosition
    Public Property North As Decimal
        Get
            Return Nothing
        End Get
        Set(value As Decimal)
        End Set
    End Property

    Public Property East As Decimal
        Get
            Return Nothing
        End Get
        Set(value As Decimal)
        End Set
    End Property

    Public Property Up As Decimal
        Get
            Return Nothing
        End Get
        Set(value As Decimal)
        End Set
    End Property

    Public Property Heading_deg As Decimal
        Get
            Return Nothing
        End Get
        Set(value As Decimal)
        End Set
    End Property

    Public Property Tilt_deg As Decimal
        Get
            Return Nothing
        End Get
        Set(value As Decimal)
        End Set
    End Property

    ''' <summary>
    ''' Returns true if this position corresponds to the edge or inside of a wall.
    ''' </summary>
    ''' <param name="map">The game map for reference.</param>
    Public Sub IsInsideWall(map As GameMap)

    End Sub
End Class
