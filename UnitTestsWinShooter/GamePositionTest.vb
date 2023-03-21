Imports System.Drawing
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
            Assert.AreEqual(New PointF(-2 / CellSize_m, 45 / CellSize_m), position.ToCellSpacePointF())
            position = New GamePosition(0, 0, 0)
            Assert.AreEqual(New PointF(0, 0), position.ToCellSpacePointF())
            position = New GamePosition(1, 0, 0, 23, 0)
            Assert.AreEqual(New PointF(1 / CellSize_m, 0), position.ToCellSpacePointF())
        End Sub
    End Class
End Namespace
