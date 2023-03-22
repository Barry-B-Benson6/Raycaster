Imports NUnit.Framework
Imports WinShooter

Namespace UnitTestsWinShooter
    Public Class GameMapTest

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub TestIsWallAt()
            Dim mapValue As Byte(,) = {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
            }
            Dim map = New GameMap(mapValue)
            Assert.AreEqual(True, map.IsWallAt(New GamePosition(0, 0, 0)))
            Assert.AreEqual(True, map.IsWallAt(New GamePosition(0, 0, CellSize_m)))
            Assert.AreEqual(False, map.IsWallAt(New GamePosition(CellSize_m, CellSize_m, 0)))
            Assert.AreEqual(False, map.IsWallAt(New GamePosition(CellSize_m, CellSize_m, CellSize_m)))
        End Sub

        <Test>
        Public Sub TestInvalidMap()
            Assert.Throws(Of ArgumentException)(AddressOf CreateinvalidGame)
        End Sub

        Private Sub CreateinvalidGame()
            Dim mapValue As Byte(,) = {
            {1, 1, 1},
            {1, 0, 0},
            {1, 1, 1}
            }
            Dim map = New GameMap(mapValue)
        End Sub
    End Class
End Namespace
