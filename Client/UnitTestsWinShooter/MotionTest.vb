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
            Assert.AreEqual(1, resultPosition.East_m)
            Assert.AreEqual(1, resultPosition.North_m)
            Assert.AreEqual(0, resultPosition.Up_m)
            Assert.AreEqual(motion.PositionStamp.Heading_deg, resultPosition.Heading_deg)
            Assert.AreEqual(motion.PositionStamp.Tilt_deg, resultPosition.Tilt_deg)
        End Sub

        <Test>
        Public Sub TestGravity()
            Dim currentTime = DateTime.UtcNow
            Dim up_s As Double = 2
            Dim motion = New Motion(New GamePosition(0, 0, 0, 180, 3), New GameVelocity(1, 1, up_s), currentTime)
            Assert.AreEqual(0.0877, motion.CalculatePositionAtTime(currentTime.AddSeconds(0.05)).Up_m, 0.0001)
            Assert.AreEqual(0.151, motion.CalculatePositionAtTime(currentTime.AddSeconds(0.1)).Up_m, 0.0001)
            Assert.AreEqual(0.1896, motion.CalculatePositionAtTime(currentTime.AddSeconds(0.15)).Up_m, 0.0001)
            Assert.AreEqual(0.2038, motion.CalculatePositionAtTime(currentTime.AddSeconds(0.2)).Up_m, 0.0001)
            Assert.AreEqual(0.1934, motion.CalculatePositionAtTime(currentTime.AddSeconds(0.25)).Up_m, 0.0001)
            Assert.AreEqual(0, motion.CalculatePositionAtTime(currentTime.AddSeconds(0.5)).Up_m, 0.0001) ' should be zero as jump should have ended already
        End Sub

    End Class

End Namespace