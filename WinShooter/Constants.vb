''' <summary>
''' Contains game-wide constant values.
''' </summary>
Public Module Constants
    ''' <summary>
    ''' The acceleration due to gravity, in metres per second squared.
    ''' </summary>
    Public Const G_mss As Decimal = 9.81

    ''' <summary>
    ''' The width, in metres, of a single cell of the map.
    ''' </summary>
    Public Const CellSize_m As Decimal = 2.0

    ''' <summary>
    ''' The Fire rate in shots per second
    ''' </summary>
    Public Const FireRate_s As Decimal = 4

    ''' <summary>
    ''' The Speed of a bullet in ms^-1
    ''' </summary>
    Public Const BulletSpeed_ms As Decimal = 2

    ''' <summary>
    ''' The rate at which the state updates in updates per second
    ''' </summary>
    Public Const StateUpdateRate As Integer = 240
End Module
