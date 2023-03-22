Public Module ArgumentHelper
    Public Function RequireNotNull(arg)
        If arg Is Nothing Then
            Throw New ArgumentException("Argument should not be Nothing")
        Else
            Return arg
        End If
    End Function
End Module
