﻿Module Functions
    Public Function DistanceBetweenTwoPoints(point1 As PointF, point2 As PointF)
        Dim SightVector As PointF = New PointF(point2.X - point1.X, point2.Y - point1.Y)
        ''Distance found with pythag
        Return Math.Sqrt(Math.Pow(SightVector.X, 2) + Math.Pow(SightVector.Y, 2))
    End Function
End Module