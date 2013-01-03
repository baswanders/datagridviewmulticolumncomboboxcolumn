Imports System.ComponentModel
Imports System.Windows.Forms

Public Class MultiColumnComboBoxColumn
    Inherits DataGridViewComboBoxColumn

    Private populatingColumns As Boolean
    Private _columns As ColumnCollection
    Private _columnPadding As Integer

    Public Sub New()
        MyBase.New()
        Me.CellTemplate = New MultiColumnComboBoxCell()
        _columns = New ColumnCollection(Me)
        ColumnPadding = 5
        AutoResize = True
    End Sub

    Public ReadOnly Property Columns As ColumnCollection
        Get
            Return _columns
        End Get
    End Property

    <DefaultValue(5)> _
    Public Property ColumnPadding() As Integer
        Get
            Return _columnPadding
        End Get
        Set(ByVal value As Integer)
            If value < 0 Then Throw New ArgumentOutOfRangeException("value", "Must be greater than or equal to zero")
            _columnPadding = value
        End Set
    End Property

    <DefaultValue(True)>
    Public Property AutoResize As Boolean

    Public Shadows Property DataSource As Object
        Get
            Return MyBase.DataSource
        End Get
        Set(value As Object)
            MyBase.DataSource = value
            PopulateColumns()
        End Set
    End Property

    Public Sub PopulateColumns()
        If Me.DataSource Is Nothing Then
            Me.Columns.Clear()
            Return
        End If
        Try
            Me.populatingColumns = True
            Me.Columns.Clear()
            Dim pdc As PropertyDescriptorCollection = ListBindingHelper.GetListItemProperties(DataSource, Nothing)
            For i As Integer = 0 To pdc.Count - 1
                If pdc(i).IsBrowsable Then
                    Dim column As New Column() With {.FieldName = pdc(i).DisplayName, .DisplayIndex = i, .Width = 60}
                    Me.Columns.Add(column)
                End If
            Next i
        Finally
            Me.populatingColumns = False
        End Try
    End Sub

    Public Class ColumnCollection
        Inherits Collections.ObjectModel.Collection(Of Column)

        Dim owner As MultiColumnComboBoxColumn

        Private Sub New()
            MyBase.New()
        End Sub

        Private Sub New(ByVal list As IList(Of Column))
            MyBase.New(list)
        End Sub

        Public Sub New(ByVal owner As MultiColumnComboBoxColumn)
            MyBase.New()
            Me.owner = owner
        End Sub

        Public ReadOnly Property VisibleCount As Integer
            Get
                Dim i As Integer = 0
                For Each c As Column In Me
                    If c.Visible Then i += 1
                Next
                Return i
            End Get
        End Property

    End Class

End Class
