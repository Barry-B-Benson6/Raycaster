Module Functions
    Public Function DistanceBetweenTwoPoints(point1 As PointF, point2 As PointF)
        Dim SightVector As PointF = New PointF(point2.X - point1.X, point2.Y - point1.Y)
        ''Distance found with pythag
        Return LengthOfVector(SightVector)
    End Function

    Public Function Lerp(Initial As Double, Final As Double, speed As Double)
        Return (Final - Initial) * speed + Initial
    End Function

    Public Function LengthOfVector(vector As PointF)
        Return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2))
    End Function
End Module
