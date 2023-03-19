Module MathHelper
    Public Function ToRadians(angle_deg As Decimal)
        Return angle_deg * (180 / Math.PI)
    End Function

    Public Function ToDegrees(angle_rad As Decimal)
        Return angle_rad * (Math.PI / 180)
    End Function
End Module
