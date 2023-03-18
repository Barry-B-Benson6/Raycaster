Imports NUnit.Framework
Imports WinShooter

Namespace UnitTestsWinShooter

    Public Class MotionTests

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub TestCalculatePosition()
            Dim currentTime = DateTime.UtcNow
            Dim motion = New Motion(New GamePosition(0, 0, 0, 180, 3), New GameVelocity(1, 1, 0), currentTime.Subtract(New TimeSpan(10000000)))
            Dim resultPosition = motion.CalculatePositionAtTime(currentTime)
            Assert.AreEqual(1, resultPosition.East)
            Assert.AreEqual(1, resultPosition.North)
            Assert.AreEqual(0, resultPosition.Up)
            Assert.AreEqual(motion.PositionStamp.Heading_deg, resultPosition.Heading_deg)
            Assert.AreEqual(motion.PositionStamp.Tilt_deg, resultPosition.Tilt_deg)
        End Sub

    End Class

End Namespace