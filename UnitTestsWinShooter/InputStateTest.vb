Imports System.Windows.Forms
Imports NUnit.Framework
Imports WinShooter

Namespace UnitTestsWinShooter
    Public Class InputStateTest

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub TestKeyboardInput()
            Dim inputState = New InputState()

            inputState.Update(Keys.A, True)
            Assert.AreEqual(True, inputState.Left)

            inputState.Update(Keys.A, False)
            Assert.AreEqual(False, inputState.Left)

            inputState.Update(Keys.D, False)
            Assert.AreEqual(False, inputState.Right)

            inputState.Update(Keys.W, True)
            Assert.AreEqual(False, inputState.Right)
        End Sub

        <Test>
        Public Sub TestMouseInput()
            Dim inputState = New InputState()

            inputState.Update(MouseButtons.Left, True)
            Assert.AreEqual(True, inputState.Firing)

            inputState.Update(MouseButtons.Right, True)
            Assert.AreEqual(True, inputState.Aiming)

            inputState.Update(MouseButtons.Left, False)
            Assert.AreEqual(False, inputState.Firing)

            inputState.Update(MouseButtons.Left, False)
            Assert.AreEqual(False, inputState.Firing)

            ''test input that does nothing
            Dim inputStateBefore = inputState
            inputState.Update(MouseButtons.XButton1, True)
            Assert.AreEqual(inputStateBefore, inputState)
        End Sub
    End Class
End Namespace
