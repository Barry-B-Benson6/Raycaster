Imports System.Reflection.Metadata

Public Class GameMap
    Public Sub New(map As Byte(,))
        RequireNotNull(map)
        If Not IsSurroundedByWalls(map) Then Throw New ArgumentException("all maps must have walls corvering every edge")
        Me.map = map
    End Sub

    ''' <summary>
    ''' This exists to allow us to create mocks of this class, it should not be used any other time
    ''' </summary>
    Protected Sub New()
        MyBase.New()
    End Sub
    Public ReadOnly Property map As Byte(,)

    Private Function IsSurroundedByWalls(map As Byte(,)) As Boolean
        Dim RightIndex = map.GetLength(0) - 1
        Dim BottomIndex = map.GetLength(1) - 1
        Dim topIndex = 0
        Dim leftIndex = 0
        For i = 0 To RightIndex - 1
            ''Checking Top wall has no holes
            If map(i, topIndex) = 0 Then Return False
            ''Checking Bottom wall has no holes
            If map(i, BottomIndex) = 0 Then Return False
        Next

        For i = 0 To BottomIndex - 1
            ''Checking Left wall has no holes
            If map(leftIndex, i) = 0 Then Return False
            ''Checking Right wall has no holes
            If map(RightIndex, i) = 0 Then Return False
        Next

        Return True
    End Function

    Public Overridable Function IsWallAt(position As GamePosition)
        Return (map(Math.Floor(position.East_m / Constants.CellSize_m), Math.Floor(position.North_m / Constants.CellSize_m)) <> 0)
    End Function
End Class
