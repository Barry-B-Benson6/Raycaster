Public MustInherit Class Entity
    Public pntLocation As PointF
    Public Bounds As RectangleF
    Public Sub New(initialLocation As PointF, size As SizeF)
        pntLocation = initialLocation
        Bounds = New RectangleF(New PointF(initialLocation.X - (size.Width / 2), initialLocation.Y - (size.Height / 2)), size)
    End Sub

    Public MustOverride Sub Draw(Distance As Decimal, xLocationOfMiddle As Decimal, e As PaintEventArgs)
End Class
