Imports NUnit.Framework
Imports WinShooter

Namespace UnitTestsWinShooter
    Public Class GameVelocityTest

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub TestIsStill()
            Dim velocity = New GameVelocity(1, 0, 1)
            Assert.AreEqual(False, velocity.isStill)
            velocity = New GameVelocity(0, 0, 0)
            Assert.AreEqual(True, velocity.isStill)
            velocity = New GameVelocity(1, 1, 1)
            Assert.AreEqual(False, velocity.isStill)
        End Sub
    End Class
End Namespace
