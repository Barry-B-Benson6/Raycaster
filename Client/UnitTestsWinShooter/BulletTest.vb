Imports System.Data.Common
Imports System.Drawing.Imaging
Imports Moq
Imports NUnit.Framework
Imports WinShooter

Namespace UnitTestsWinShooter
    Public Class BulletTest
        Private shooter As New Mock(Of Player)
        Private game As New Mock(Of Game)
        Private motion As New Mock(Of Motion)
        Private map As New Mock(Of GameMap)
        <SetUp>
        Public Sub Setup()
            shooter.Setup(Function(s) s.Name).Returns("BOB")
            game.Setup(Function(s) s.Map).Returns(map.Object)
        End Sub

        <Test>
        Public Sub TestHitsWall()
            Dim position = New GamePosition(0, 0, 0)
            map.Setup(Function(s) s.IsWallAt(position)).Returns(True)
            Dim velocity = New GameVelocity(1, 0, 0)
            Dim time = DateTime.UtcNow
            Dim motion = New Motion(position, velocity, time)
            map.Setup(Function(s) s.IsWallAt(It.IsAny(Of GamePosition))).Returns(True)
            Dim out = New Bullet(shooter.Object, motion, game.Object, True)
            Dim time2 = time.AddSeconds(1)
            Assert.AreEqual(True, out.IsAlive)
            out.UpdatePosition(time2)
            out.UpdateState(time2)
            Assert.AreEqual(False, out.IsAlive)

        End Sub

        ''' <summary>
        ''' Tests to see if bullet behaves correctly when not locally owned
        ''' </summary>
        <Test>
        Public Sub TestNotLocallyOwned()
            Dim position = New GamePosition(0, 0, 0)
            map.Setup(Function(s) s.IsWallAt(position)).Returns(True)
            Dim velocity = New GameVelocity(1, 0, 0)
            Dim time = DateTime.UtcNow
            Dim motion = New Motion(position, velocity, time)
            map.Setup(Function(s) s.IsWallAt(It.IsAny(Of GamePosition))).Returns(True)
            Dim out = New Bullet(shooter.Object, motion, game.Object, False) 'sets locally owned to false :)
            Dim time2 = time.AddSeconds(1)
            Assert.AreEqual(True, out.IsAlive)
            out.UpdatePosition(time2)
            out.UpdateState(time2)
            Assert.AreEqual(True, out.IsAlive) 'checks to make sure we didnt notice it is in a wall broski
        End Sub

        <Test>
        Public Sub TestBulletName()
            Dim out = New Bullet(shooter.Object, motion.Object, game.Object, True)
            Assert.AreEqual("BOB's Bullet", out.Name)
        End Sub
    End Class
End Namespace