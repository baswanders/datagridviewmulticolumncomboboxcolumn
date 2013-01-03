Imports System.ComponentModel
Imports System.Windows.Forms

Public Class Column
    Inherits Component

    Public Sub New()
        MyBase.New()
        Name = If(MyBase.Site Is Nothing, "Column", MyBase.Site.Name)
        DisplayIndex = -1
        Width = 20
        Visible = True
    End Sub

    <EditorBrowsable(EditorBrowsableState.Never), Browsable(False)>
    Public Property MultiColumnComboBox As ComboBox
    Public Property Name As String
    Public Property FieldName As String
    <DefaultValue(-1)>
    Public Property DisplayIndex As Integer
    <DefaultValue(20)>
    Public Property Width As Integer
    <DefaultValue(DirectCast(Nothing, Object))>
    Public Property Tag As Object
    <DefaultValue(True)>
    Public Property Visible As Boolean

    Public Overrides Function ToString() As String
        Return String.Format("{0} [{1}]", Name, FieldName)
    End Function

End Class
