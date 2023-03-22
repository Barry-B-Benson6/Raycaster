Imports NUnit.Framework
Imports WinShooter

Namespace UnitTestsWinShooter

    Public Class MathHelperTest

        <SetUp>
        Public Sub Setup()
        End Sub

        <Test>
        Public Sub TestIsBetween()

            TestIsBetween_implementation(0)
            TestIsBetween_implementation(-1)
            TestIsBetween_implementation(1)


        End Sub

        Private Sub TestIsBetween_implementation(rotationOffset As Integer)
            Dim start_deg = 80
            Dim end_deg = 100
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 10 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 30 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 50 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 70 + (rotationOffset * 360)))
            Assert.AreEqual(True, IsBetween(start_deg, end_deg, 85 + (rotationOffset * 360)))
            Assert.AreEqual(True, IsBetween(start_deg, end_deg, 100 + (rotationOffset * 360)))

            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 110 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 130 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 150 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 160 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 170 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 180 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 190 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 200 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 210 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 220 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 230 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 240 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 250 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 260 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 270 + (rotationOffset * 360)))

            start_deg = 270
            end_deg = 90

            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 190 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 200 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 210 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 220 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 230 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 240 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 250 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 260 + (rotationOffset * 360)))

            Assert.AreEqual(True, IsBetween(start_deg, end_deg, 270 + (rotationOffset * 360)))
            Assert.AreEqual(True, IsBetween(start_deg, end_deg, 300 + (rotationOffset * 360)))
            Assert.AreEqual(True, IsBetween(start_deg, end_deg, 0 + (rotationOffset * 360)))
            Assert.AreEqual(True, IsBetween(start_deg, end_deg, 40 + (rotationOffset * 360)))
            Assert.AreEqual(True, IsBetween(start_deg, end_deg, 90 + (rotationOffset * 360)))

            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 100 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 110 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 120 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 130 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 140 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 150 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 160 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 170 + (rotationOffset * 360)))
            Assert.AreEqual(False, IsBetween(start_deg, end_deg, 180 + (rotationOffset * 360)))
        End Sub

        <Test>
        Public Sub TestDifferenceBetweenAngles()
            For i = -360 To 360 Step 10
                For j = -179 To 180 Step 10
                    Dim angle1 = i
                    Dim angle2 = i + j
                    Assert.AreEqual(-j, DifferenceBetweenAngles(angle1, angle2))
                Next
            Next
        End Sub

    End Class

End Namespace