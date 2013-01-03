Imports System.Windows.Forms

Public Class MultiColumnComboBoxCell
    Inherits DataGridViewComboBoxCell

    Public Overrides ReadOnly Property EditType As System.Type
        Get
            Return GetType(MultiColumnComboBoxEditingControl)
        End Get
    End Property

    Public Overrides Sub InitializeEditingControl(rowIndex As Integer, initialFormattedValue As Object, dataGridViewCellStyle As System.Windows.Forms.DataGridViewCellStyle)
        Dim ctrl As MultiColumnComboBoxEditingControl = TryCast(DataGridView.EditingControl, MultiColumnComboBoxEditingControl)
        If ctrl IsNot Nothing Then DirectCast(ctrl, MultiColumnComboBoxEditingControl).ownerCell = Me
        MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)
    End Sub

End Class
