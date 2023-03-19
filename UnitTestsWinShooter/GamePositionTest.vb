Imports System.Numerics
Imports NUnit.Framework
Imports WinShooter

Namespace UnitTestsWinShooter
    Public Class GamePositionTest

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub TestToVector3()
            Dim position = New GamePosition(-2, 45, 23)
            Assert.AreEqual(New Vector3(-2, 45, 23), position.ToVector3())
            position = New GamePosition(0, 0, 0)
            Assert.AreEqual(New Vector3(0, 0, 0), position.ToVector3())
            position = New GamePosition(1, 0, 0, 23, 0)
            Assert.AreEqual(New Vector3(1, 0, 0), position.ToVector3())
        End Sub
    End Class
End Namespace
