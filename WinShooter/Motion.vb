''' <summary>
''' Provides a position and velocity at a certain time and performs calculations to work out a position at
''' the current time.
''' </summary>
Public Class Motion

    ''' <summary>
    ''' Creates motion object with its initial position,velocity, and time
    ''' </summary>
    ''' <param name="Position">Starting position of the motion</param>
    ''' <param name="Velocity">Starting velocity of the motion</param>
    ''' <param name="Time">Starting time of the motion object</param>
    Public Sub New(Position As GamePosition, Velocity As GameVelocity, Time As DateTime)
        PositionStamp = RequireNotNull(Position)
        VelocityStamp = RequireNotNull(Velocity)
        TimeStamp = RequireNotNull(Time)
    End Sub

    Public ReadOnly Property PositionStamp As WinShooter.GamePosition

    Public ReadOnly Property TimeStamp As DateTime

    Public ReadOnly Property VelocityStamp As WinShooter.GameVelocity

    Public Function CalculatePositionAtTime(currentTime As DateTime) As WinShooter.GamePosition
        Dim timeDiff As TimeSpan = currentTime.Subtract(TimeStamp)
        Dim dT_s = timeDiff.TotalSeconds

        Dim East = PositionStamp.East + (VelocityStamp.East * dT_s)
        Dim North = PositionStamp.North + (VelocityStamp.North * dT_s)
        Dim Up = PositionStamp.Up + (VelocityStamp.Up * dT_s)

        Return New GamePosition(East, North, Up, PositionStamp.Heading_deg, PositionStamp.Tilt_deg)
    End Function
End Class
