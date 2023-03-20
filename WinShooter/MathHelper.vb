Module MathHelper
    Public Function ToRadians(angle_deg As Decimal)
        Return angle_deg * (Math.PI / 180)
    End Function

    Public Function ToDegrees(angle_rad As Decimal)
        Return angle_rad * (180 / Math.PI)
    End Function

    Public Function DistanceBetweenTwoPoints(point1 As PointF, point2 As PointF)
        Dim SightVector As PointF = New PointF(point2.X - point1.X, point2.Y - point1.Y)
        ''Distance found with pythag
        Return LengthOfVector(SightVector)
    End Function

    Public Function LengthOfVector(vector As PointF)
        Return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2))
    End Function
End Module
