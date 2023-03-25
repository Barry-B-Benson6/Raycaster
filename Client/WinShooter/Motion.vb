Imports System.Text.Json.Serialization

''' <summary>
''' Provides a position and velocity at a certain time and performs calculations to work out a position at
''' the current time.
''' </summary>
Public Class Motion

    ''' <summary>
    ''' Creates motion object with its initial position,velocity, and time
    ''' </summary>
    ''' <param name="positionStamp">Starting position of the motion</param>
    ''' <param name="velocityStamp">Starting velocity of the motion</param>
    ''' <param name="timeStamp">Starting time of the motion object</param>
    <JsonConstructor>
    Public Sub New(positionStamp As GamePosition, velocityStamp As GameVelocity, timeStamp As DateTime)
        Me.PositionStamp = RequireNotNull(positionStamp)
        Me.VelocityStamp = RequireNotNull(velocityStamp)
        Me.TimeStamp = RequireNotNull(timeStamp)
    End Sub

    ''' <summary>
    ''' This exists to allow us to create mocks of this class, it should not be used any other time
    ''' </summary>
    Protected Sub New()
        MyBase.New()
        PositionStamp = New GamePosition(0, 0, 0)
        VelocityStamp = New GameVelocity(0, 0, 0)
        TimeStamp = DateTime.UtcNow
    End Sub

    Public ReadOnly Property PositionStamp As WinShooter.GamePosition

    Public ReadOnly Property TimeStamp As DateTime

    Public ReadOnly Property VelocityStamp As WinShooter.GameVelocity

    Public Function CalculatePositionAtTime(currentTime As DateTime) As WinShooter.GamePosition
        Dim timeDiff As TimeSpan = currentTime.Subtract(TimeStamp)
        Dim dT_s = timeDiff.TotalSeconds
        Dim gravity_ss = -9.81

        ' calculate the distances travelled
        Dim eastTravelled_m = VelocityStamp.East_ms * dT_s
        Dim northTravelled_m = VelocityStamp.North_ms * dT_s
        Dim upTravelled_m = VelocityStamp.Up_ms * dT_s + 0.5 * gravity_ss * dT_s * dT_s  's = ut+ 0.5 *at^2 

        ' calculate new position
        Dim east_m = PositionStamp.East_m + eastTravelled_m
        Dim north_m = PositionStamp.North_m + northTravelled_m
        Dim up_m = PositionStamp.Up_m + upTravelled_m

        ' check if in ground
        If up_m <= 0 Then
            up_m = 0
        End If

        Return New GamePosition(east_m, north_m, up_m, PositionStamp.Heading_deg, PositionStamp.Tilt_deg)
    End Function
    ''' <summary>
    ''' Creates a new motion at given position without any movement
    ''' </summary>
    ''' <param name="position">Position of entity</param>
    ''' <param name="time">Time of validity</param>
    ''' <returns>A new motion</returns>
    Public Shared Function NotMovingAt(position As GamePosition, time As DateTime) As Motion
        Return New Motion(position, New GameVelocity(0, 0, 0), time)
    End Function
End Class
