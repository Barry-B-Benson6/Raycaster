﻿Imports System.Threading
Public Class Game

    Public Sub New(initialPositionPLayer As GamePosition, map As GameMap, fov As Integer, resQuality As Integer, form As Form)

        AddHandler form.Paint, AddressOf Render
        AddHandler form.KeyDown, AddressOf KeyboardButtonDown
        AddHandler form.KeyUp, AddressOf KeyboardButtonUp
        AddHandler form.MouseDown, AddressOf MouseButtonDown
        AddHandler form.MouseUp, AddressOf MouseButtonUp



        Me.Form = form
        Dim ClientPlayer = New Player("Cinnabun", New Motion(initialPositionPLayer, New GameVelocity(0, 0, 0), DateTime.UtcNow), Me, True)
        AddEntity(ClientPlayer)
        Me.Map = map

        Dim HUD = New HUD(ClientPlayer.EntityId, Me)

        Dim rays As New List(Of Ray)
        For i = 0 To resQuality - 1
            Dim diff = -fov / 2 + i / (resQuality / fov)
            Dim ray = New Ray(diff)
            rays.Add(ray)
        Next
        Me.Sight = New Renderer(HUD, rays, ClientPlayer, Me)
    End Sub

    Private ReadOnly Form As Form

    Dim _Entities As New Dictionary(Of Guid, Entity)
    Public Property Entities As Dictionary(Of Guid, Entity)
        Get
            SyncLock Me
                Return _Entities
            End SyncLock
        End Get
        Private Set(value As Dictionary(Of Guid, Entity))
            SyncLock Me
                _Entities = value
            End SyncLock
        End Set
    End Property

    Public Property Map As GameMap

    Public ReadOnly Property Sight As Renderer

    Public Property InputState As New InputState()

    Public Sub Start()
        Dim StateCycle As Thread = New Thread(AddressOf funcStateCycle)
        Dim RenderCycle As Thread = New Thread(AddressOf funcRenderCycle)
        StateCycle.Start()
        RenderCycle.Start()
    End Sub

    Private Sub funcStateCycle()
        While True
            For i = 0 To Entities.Count - 1
                Entities.Values(i).UpdateState(DateTime.UtcNow)
                Entities.Values(i).UpdatePosition(DateTime.UtcNow)
            Next
        End While
    End Sub

    Private Sub funcRenderCycle()
        While True
            Form.Invoke(Sub() Form.Refresh())
            Thread.Sleep(10)
        End While
    End Sub

    Private Sub Render(sender As Form, e As PaintEventArgs)
        Sight.Render(e, sender.Size)
    End Sub

    Public Sub AddEntity(entity As Entity)
        RequireNotNull(entity)
        Entities.Add(entity.EntityId, entity)
    End Sub

    Private Sub MouseButtonDown(sender As Object, e As MouseEventArgs)
        InputState.Update(e.Button, True)
    End Sub
    Private Sub MouseButtonUp(sender As Object, e As MouseEventArgs)
        InputState.Update(e.Button, False)
    End Sub

    Private Sub KeyboardButtonDown(sender As Object, e As KeyEventArgs)
        InputState.Update(e.KeyCode, True)
    End Sub
    Private Sub KeyboardButtonUp(sender As Object, e As KeyEventArgs)
        InputState.Update(e.KeyCode, False)
    End Sub
End Class
