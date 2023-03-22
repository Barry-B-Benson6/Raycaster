Public Module MathHelper
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

    Public Function ConstrainToOneRotation(angle_deg As Double)
        While angle_deg < 0
            angle_deg += 360
        End While
        While angle_deg > 360
            angle_deg -= 360
        End While
        Return angle_deg
    End Function

    ''' <summary>
    ''' Checks if an angle is between two other angles
    ''' </summary>
    ''' <param name="angleStart_deg"></param>
    ''' <param name="angleEnd_deg"></param>
    ''' <param name="checkedAngle_deg"></param>
    ''' <returns></returns>
    Public Function IsBetween(angleStart_deg As Double, angleEnd_deg As Double, checkedAngle_deg As Double) As Boolean

        angleStart_deg = ConstrainToOneRotation(angleStart_deg)
        angleEnd_deg = ConstrainToOneRotation(angleEnd_deg)
        checkedAngle_deg = ConstrainToOneRotation(checkedAngle_deg)

        If (angleEnd_deg < angleStart_deg) Then
            angleEnd_deg += 360
        End If
        If (checkedAngle_deg < angleStart_deg) Then
            checkedAngle_deg += 360
        End If

        If (checkedAngle_deg < angleStart_deg) Then
            Return False
        ElseIf (checkedAngle_deg > angleEnd_deg) Then
            Return False
        Else
            Return True
        End If
    End Function

    ''' <summary>
    '''  returns The angular difference between angle1 and angle2 ranging from 180 to -180
    ''' </summary>
    ''' <param name="angle1_deg"></param>
    ''' <param name="angle2_deg"></param>
    ''' <returns>a value between 180 and -180</returns>
    Public Function DifferenceBetweenAngles(angle1_deg As Double, angle2_deg As Double)
        angle1_deg = ConstrainToOneRotation(angle1_deg)
        angle2_deg = ConstrainToOneRotation(angle2_deg)

        If (angle1_deg - angle2_deg > 180) Then
            Return angle1_deg - angle2_deg - 360
        ElseIf (angle1_deg - angle2_deg <= -180) Then
            Return angle1_deg - angle2_deg + 360
        Else
            Return angle1_deg - angle2_deg
        End If
    End Function
End Module
