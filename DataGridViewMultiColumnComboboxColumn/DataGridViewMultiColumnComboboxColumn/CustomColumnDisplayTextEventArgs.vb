
Public Class CustomColumnDisplayTextEventArgs
    Inherits EventArgs

    Public Sub New(ByVal col As Column, ByVal row As Integer)
        Column = col
        RowIndex = row
    End Sub

    Public Property Column As Column
    Public Property RowIndex As Integer
    Public Property DisplayText() As String

End Class
