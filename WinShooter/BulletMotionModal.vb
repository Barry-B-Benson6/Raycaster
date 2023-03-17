Public Class BulletMotionModal
    Inherits MotionModal

    Public Property Motion As Integer
        Get
            Return Nothing
        End Get
        Set(value As Integer)
        End Set
    End Property

    Public Overrides Sub UpdatePostion()
        Throw New NotImplementedException()
    End Sub
End Class
