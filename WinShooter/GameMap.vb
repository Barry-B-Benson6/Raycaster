Imports System.Reflection.Metadata

Public Class GameMap
    Public Sub New(map As Byte(,))
        RequireNotNull(map)
        If Not IsSurroundedByWalls(map) Then Throw New ArgumentException("all maps must have walls corvering every edge")
        Me.map = map
    End Sub
    Private ReadOnly Property map As Byte(,)

    Private Function IsSurroundedByWalls(map As Byte(,)) As Boolean
        Dim RightIndex = map.GetLength(0) - 1
        Dim BottomIndex = map.GetLength(1) - 1
        Dim topIndex = 0
        Dim leftIndex = 0
        For i = 0 To RightIndex
            ''Checking Top wall has no holes
            If map(i, topIndex) = 0 Then Return False
            ''Checking Bottom wall has no holes
            If map(i, BottomIndex) = 0 Then Return False
        Next

        For i = 0 To BottomIndex
            ''Checking Left wall has no holes
            If map(i, leftIndex) = 0 Then Return False
            ''Checking Right wall has no holes
            If map(i, RightIndex) = 0 Then Return False
        Next

        Return True
    End Function

    Public Function IsWallAt(position As GamePosition)
        Return (map(Math.Floor(position.East_m / Constants.CellSize_m), Math.Floor(position.North_m / Constants.CellSize_m)) <> 0)
    End Function
End Class
