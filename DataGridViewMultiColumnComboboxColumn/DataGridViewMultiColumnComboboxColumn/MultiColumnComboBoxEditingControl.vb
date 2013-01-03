Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing

Public Class MultiColumnComboBoxEditingControl
    Inherits DataGridViewComboBoxEditingControl

    Friend ownerCell As MultiColumnComboBoxCell

    Public Sub New()
        MyBase.New()
        MyBase.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
    End Sub

    Public Event CustomColumnDisplayText(ByVal sender As Object, ByVal e As CustomColumnDisplayTextEventArgs)

    <EditorBrowsable(EditorBrowsableState.Never), Browsable(False)> _
    Public Shadows Property DrawMode As DrawMode
        Get
            Return MyBase.DrawMode
        End Get
        Set(value As DrawMode)
            If value <> Windows.Forms.DrawMode.OwnerDrawFixed Then
                Throw New ArgumentOutOfRangeException("value", "Needs to be OwnerDrawFixed")
            Else
                MyBase.DrawMode = value
            End If
        End Set
    End Property

    Public Shadows Property DropDownStyle As ComboBoxStyle
        Get
            Return MyBase.DropDownStyle
        End Get
        Set(value As ComboBoxStyle)
            If value = ComboBoxStyle.Simple Then
                Throw New ArgumentOutOfRangeException("value", "ComboBoxStyle.Simple not supported")
            Else
                MyBase.DropDownStyle = value
            End If
        End Set
    End Property

    Public ReadOnly Property Columns As MultiColumnComboBoxColumn.ColumnCollection
        Get
            Return DirectCast(ownerCell.OwningColumn, MultiColumnComboBoxColumn).Columns
        End Get
    End Property

    Public ReadOnly Property ColumnPadding As Integer
        Get
            Return DirectCast(ownerCell.OwningColumn, MultiColumnComboBoxColumn).ColumnPadding
        End Get
    End Property

    Public ReadOnly Property AutoResize() As Boolean
        Get
            Return DirectCast(ownerCell.OwningColumn, MultiColumnComboBoxColumn).AutoResize
        End Get
    End Property

    Public Sub AutoResizeColumns()
        If Columns.Count = 0 Then Return
        Using g As Graphics = Me.CreateGraphics()
            Dim x As Integer = 0
            For Each c As Column In Columns
                If Not c.Visible Then Continue For
                x += 1
                Dim width As Integer = 0
                For i As Integer = 0 To Items.Count - 1
                    Dim e As New CustomColumnDisplayTextEventArgs(c, i) With {.DisplayText = Convert.ToString(FilterItemOnProperty(Items(i), c.FieldName))}
                    RaiseEvent CustomColumnDisplayText(Me, e)
                    width = Math.Max(width, g.MeasureString(e.DisplayText, Me.Font, 0).ToSize().Width)
                Next
                c.Width = width
            Next
        End Using
    End Sub

    Private Function CalculateTotalWidth() As Integer
        Dim w As Integer = 0
        For Each c As Column In Columns
            If c.Visible Then w += ColumnPadding + c.Width
        Next
        Return If(w > 0, w + If(Items.Count > MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0), 1)
    End Function


    Protected Overrides Sub OnDataSourceChanged(e As System.EventArgs)
        MyBase.OnDataSourceChanged(e)
        If AutoResize Then AutoResizeColumns()
    End Sub

    Protected Overrides Sub OnDropDown(e As System.EventArgs)
        MyBase.OnDropDown(e)
        Me.DropDownWidth = CalculateTotalWidth()
        If ownerCell IsNot Nothing Then
            ownerCell.DropDownWidth = Me.DropDownWidth
            If TryCast(ownerCell.OwningColumn, MultiColumnComboBoxColumn) IsNot Nothing Then DirectCast(ownerCell.OwningColumn, MultiColumnComboBoxColumn).DropDownWidth = Me.DropDownWidth
        End If
    End Sub

    Protected Overrides Sub OnDrawItem(e As System.Windows.Forms.DrawItemEventArgs)
        'MyBase.OnDrawItem(e)
        If DesignMode OrElse e.Index = -1 Then Return
        e.DrawBackground()
        Dim item As Object = Items(e.Index)
        Dim itemText As String = GetItemText(item)
        If DroppedDown AndAlso Margin.Top <> e.Bounds.Top Then
            Dim orderedColumns = Columns.OrderBy(Function(c As Column) c.DisplayIndex)
            Dim x As Integer = 0
            Dim offset As Integer = e.Bounds.Left
            For Each c As Column In orderedColumns
                If Not c.Visible Then Continue For
                x += 1
                Dim ccdtea As New CustomColumnDisplayTextEventArgs(c, e.Index) With {.DisplayText = Convert.ToString(FilterItemOnProperty(item, c.FieldName))}
                RaiseEvent CustomColumnDisplayText(Me, ccdtea) ' Geef de gebruikers een kans om handmatig andere tekst op te geven
                ' Tekst tekenen
                Dim fgBrush As New SolidBrush(If((e.State And DrawItemState.Selected) = DrawItemState.Selected, SystemColors.HighlightText, ForeColor))
                e.Graphics.DrawString(ccdtea.DisplayText, e.Font, fgBrush, New Rectangle(offset, e.Bounds.Y, c.Width + ColumnPadding, e.Bounds.Height))
                ' Scheidingslijn tekenen
                offset += c.Width + ColumnPadding
                If x < Columns.VisibleCount Then
                    e.Graphics.DrawLine(SystemPens.ActiveBorder, offset - ColumnPadding \ 2, e.Bounds.Y, offset - ColumnPadding \ 2, e.Bounds.Bottom)
                End If
            Next
        Else
            e.Graphics.DrawString(itemText, e.Font, New SolidBrush(If((e.State And DrawItemState.Selected) = DrawItemState.Selected, SystemColors.HighlightText, ForeColor)), e.Bounds)
        End If
    End Sub



End Class
