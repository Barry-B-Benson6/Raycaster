Public Class Bullet
    Public decAngle As Decimal
    Public pntLocation As PointF
    Public Bounds As RectangleF
    Private Movements As UInt32
    Public Sub New(Angle As Decimal, point As PointF)
        decAngle = Angle
        pntLocation = point
        Bounds = New RectangleF(New PointF(point.X - 0.2, point.Y - 0.2), New SizeF(0.4, 0.4))
        Movements = 0
    End Sub

    Public Sub Move(Map As Byte(,))
        pntLocation.X -= Math.Cos(decAngle) * 1
        pntLocation.Y -= Math.Sin(decAngle) * 1
        Bounds = New RectangleF(New PointF(pntLocation.X - 0.2, pntLocation.Y - 0.2), New SizeF(0.4, 0.4))
        Movements += 1

        If (Map(Math.Floor(pntLocation.X), Math.Floor(pntLocation.Y)) <> 0) Then
            Movements = 501
        End If


    End Sub

    Public ReadOnly Property expired As Boolean
        Get
            Return Movements > 500
        End Get
    End Property

    Public ReadOnly Property TopLeft As PointF
        Get
            Return New PointF(Bounds.Left, Bounds.Top)
        End Get
    End Property

    Public ReadOnly Property BottomRight As PointF
        Get
            Return New PointF(Bounds.Right, Bounds.Bottom)
        End Get
    End Property

    Public ReadOnly Property BottomLeft As PointF
        Get
            Return New PointF(Bounds.Left, Bounds.Bottom)
        End Get
    End Property

    Public ReadOnly Property TopRight As PointF
        Get
            Return New PointF(Bounds.Right, Bounds.Top)
        End Get
    End Property

    Public ReadOnly Property Middle As PointF
        Get
            Return New PointF(Bounds.Left + (Bounds.Width / 2), Bounds.Top + (Bounds.Height / 2))
        End Get
    End Property
End Class
