Public Class Bullet : Inherits Entity
    Public decAngle As Decimal
    Private Movements As UInt32
    Public Sub New(Angle As Decimal, point As PointF)
        MyBase.New(point, New SizeF(0.4, 0.4))
        decAngle = Angle
        Movements = 0
    End Sub

    Public Sub Move(Map As Byte(,))
        pntLocation.X -= Math.Cos(decAngle) * 1
        pntLocation.Y -= Math.Sin(decAngle) * 1
        Bounds.Location = New PointF(pntLocation.X - (Bounds.Size.Width / 2), pntLocation.Y - (Bounds.Size.Height / 2))
        Movements += 1

        If (Map(Math.Floor(pntLocation.X), Math.Floor(pntLocation.Y)) <> 0) Then
            Movements = 501
        End If
    End Sub

    Public Overrides Sub Draw(Distance As Decimal, xLocationOfMiddle As Decimal, e As PaintEventArgs, formSize As Size)
        Dim size = New Size((formSize.Height / 5 / Distance), (formSize.Height / 5 / Distance))
        Dim Middle = formSize.Height / 2
        e.Graphics.FillEllipse(New SolidBrush(Color.Gold), New Rectangle(New Point(xLocationOfMiddle - (size.Width / 2), Middle - size.Height / 2), size))
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
