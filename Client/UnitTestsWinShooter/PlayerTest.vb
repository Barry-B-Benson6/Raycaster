Imports NUnit.Framework
Imports WinShooter

Namespace UnitTestsWinShooter

    Public Class PlayerTest

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub TestJump()
            Dim startTime = DateTime.UtcNow()

            Dim initialPosition = New GamePosition(0, 0, 0)
            Throw New Exception("Game Class Must be complete to write this")
            'Dim player = New Player("Cinnabun", New Motion(initialPosition, New GameVelocity(1, 1, 1), startTime))
        End Sub

        <Test>
        Public Sub TestCrouchMechanic()
            Throw New Exception("Crouching not Implemented")
        End Sub

        <Test>
        Public Sub TestUpdatePosition()
            Throw New Exception("I'll do it later")
        End Sub

        <Test>
        Public Sub TestUpdateState()
            Throw New Exception("I'll do it later")
        End Sub

    End Class

End Namespace