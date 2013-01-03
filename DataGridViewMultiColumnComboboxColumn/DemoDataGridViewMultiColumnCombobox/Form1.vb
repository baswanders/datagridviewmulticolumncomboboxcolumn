Imports DataGridViewMultiColumnComboboxColumn
Public Class Form1

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

        DataGridView1.DataSource = GetGridDataTable()

        'De laatste column weghalen, want die vervangen we door de multicolumn column
        'Als je de kolommen in de designer aanmaakt, dan hoef je ze uiteraard niet weg te halen
        'De MultiColumnComboxColumn is beschikbaar in de designer van het DataGridView als Column Type
        DataGridView1.Columns.RemoveAt(2)

        Dim MultiColumn As New MultiColumnComboBoxColumn
        With MultiColumn
            .DataSource = GetComboboxDataTable()
            .DisplayMember = "Name"
            .ValueMember = "EmployeeID"
            .DataPropertyName = "Employee"
            .DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
        End With

        MultiColumn.Columns(0).Visible = False
        'MultiColumn.Columns(1).Width = 120
        MultiColumn.AutoResize = True

        DataGridView1.Columns.Add(MultiColumn)

    End Sub


    Private Function GetGridDataTable() As DataTable

        Dim dt As New DataTable("Companies")
        dt.Columns.Add("CompanyID", GetType(Integer))
        dt.Columns.Add("Name", GetType(String))
        dt.Columns.Add("Employee", GetType(String))

        dt.Rows.Add(New String() {1.ToString, "PB & Sons", "D4"})
        dt.Rows.Add(New String() {2.ToString, "Cook ltd.", "D12"})
        dt.Rows.Add(New String() {3.ToString, "Wiley & sons", "PM1"})
        dt.Rows.Add(New String() {4.ToString, "JB Algor", "T1"})
        dt.Rows.Add(New String() {5.ToString, "Barn Co.", "D3"})
        dt.Rows.Add(New String() {6.ToString, "Ddd", "D13"})

        Return dt

    End Function

    Private Function GetComboboxDataTable() As DataTable

        Dim dataTable As New DataTable("Employees")

        dataTable.Columns.Add("EmployeeID", GetType([String]))
        dataTable.Columns.Add("Name", GetType([String]))
        dataTable.Columns.Add("Designation", GetType([String]))

        dataTable.Rows.Add(New [String]() {"D1", "Natalia", "Developer"})
        dataTable.Rows.Add(New [String]() {"D2", "Jonathan", "Developer"})
        dataTable.Rows.Add(New [String]() {"D3", "Jake", "Developer"})
        dataTable.Rows.Add(New [String]() {"D4", "Abraham", "Developer"})
        dataTable.Rows.Add(New [String]() {"T1", "Mary", "Team Lead"})
        dataTable.Rows.Add(New [String]() {"PM1", "Calvin", "Project Manager"})
        dataTable.Rows.Add(New [String]() {"T2", "Sarah", "Team Lead"})
        dataTable.Rows.Add(New [String]() {"D12", "Monica", "Developer"})
        dataTable.Rows.Add(New [String]() {"D13", "Donna", "Developer"})

        Return dataTable

    End Function


End Class
