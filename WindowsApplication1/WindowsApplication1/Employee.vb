Imports System.Data.SqlClient

Public Class Employee
    Dim objcon As New Connect
    Dim objcon1 As New Connect
    Dim command As SqlCommand
    Dim reader As SqlDataReader

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim radioValue, checkValue As String

        If RadioButton1.Checked = True Then
            radioValue = "Male"
        ElseIf RadioButton2.Checked = True Then
            radioValue = "Female"
        Else
            radioValue = ""
        End If

        If CheckBox1.Checked = True Then
            checkValue = "Football"
        ElseIf CheckBox2.Checked = True Then
            checkValue = "Basket"
        ElseIf CheckBox3.Checked = True Then
            checkValue = "Badminton"
        Else
            checkValue = ""
        End If

        'for edit
        If Button1.Text = "Update" Then
            objcon.cnn.Open()
            command = New SqlClient.SqlCommand("UPDATE EMPLOYEE SET NAME='" & TextBox2.Text & "',BIRTH_DATE='" & DateTimePicker1.Value & "' " +
                                                " ,SEX='" & radioValue & "',MARITAL='" & ComboBox1.Text & "',HOBY='" & checkValue & "' " +
                                                " WHERE EMP_ID='" & TextBox1.Text & "'", objcon.cnn)
            command.ExecuteReader()
            objcon.cnn.Close()
            MsgBox("Update Successfully")
            Me.BindGrid()
        Else
            'for insert
            If TextBox1.Text = "" Or TextBox2.Text = "" Or DateTimePicker1.Text = "" Or radioValue = "" Or ComboBox1.Text = "" Or checkValue = "" Then
                MsgBox("Please insert data")
            Else
                objcon.cnn.Open()
                command = New SqlClient.SqlCommand("SELECT * FROM EMPLOYEE WHERE EMP_ID='" & TextBox1.Text & "'", objcon.cnn)
                reader = command.ExecuteReader()
                If (reader.Read()) Then
                    MsgBox("Emp ID Already Exist")
                Else
                    objcon1.cnn.Open()
                    command = New SqlClient.SqlCommand("INSERT INTO EMPLOYEE (EMP_ID,NAME,BIRTH_DATE,SEX,MARITAL,HOBY) " +
                                            " VALUES('" & TextBox1.Text & "','" & TextBox2.Text & "','" & DateTimePicker1.Value & "' " +
                                            " ,'" & radioValue & "','" & ComboBox1.Text & "','" & checkValue & "')", objcon1.cnn)
                    command.ExecuteReader()
                    objcon1.cnn.Close()
                    MsgBox("Insert Successfully")
                End If
                objcon.cnn.Close()
                Me.BindGrid()
            End If
        End If

    End Sub

    Private Sub Employee_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.BindGrid()
    End Sub

    Private Sub BindGrid()
        'Hide the last blank line.
        DataGridView1.AllowUserToAddRows = False

        'Clear Columns.
        DataGridView1.Columns.Clear()

        TextBox1.Text = ""
        TextBox2.Text = ""
        DateTimePicker1.Value = Now()
        RadioButton1.Checked = False
        RadioButton2.Checked = False
        ComboBox1.Text = ""
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        CheckBox3.Checked = False
        Button1.Text = "Save"
        TextBox1.Enabled = True

        'Add Columns.
        Dim customerId As DataGridViewColumn = New DataGridViewTextBoxColumn()
        customerId.Name = "EMP_ID"
        customerId.HeaderText = "EMP ID"
        customerId.DataPropertyName = "EMP_ID"
        customerId.Width = 100
        DataGridView1.Columns.Insert(0, customerId)

        Dim name As DataGridViewColumn = New DataGridViewTextBoxColumn()
        name.HeaderText = "NAME"
        name.Name = "NAME"
        name.DataPropertyName = "NAME"
        name.Width = 100
        DataGridView1.Columns.Insert(1, name)

        Dim bDate As DataGridViewColumn = New DataGridViewTextBoxColumn()
        bDate.Name = "BIRTH_DATE"
        bDate.HeaderText = "BIRTH_DATE"
        bDate.DataPropertyName = "BIRTH_DATE"
        bDate.Width = 100
        DataGridView1.Columns.Insert(2, bDate)

        Dim sex As DataGridViewColumn = New DataGridViewTextBoxColumn()
        sex.Name = "SEX"
        sex.HeaderText = "SEX"
        sex.DataPropertyName = "SEX"
        sex.Width = 100
        DataGridView1.Columns.Insert(3, sex)

        Dim marital As DataGridViewColumn = New DataGridViewTextBoxColumn()
        marital.Name = "MARITAL"
        marital.HeaderText = "MARITAL"
        marital.DataPropertyName = "MARITAL"
        marital.Width = 100
        DataGridView1.Columns.Insert(4, marital)

        Dim hoby As DataGridViewColumn = New DataGridViewTextBoxColumn()
        hoby.Name = "HOBY"
        hoby.HeaderText = "HOBY"
        hoby.DataPropertyName = "HOBY"
        hoby.Width = 100
        DataGridView1.Columns.Insert(5, hoby)

        'Bind the DataGridView.
        DataGridView1.DataSource = Nothing
        Using cmd As SqlCommand = New SqlCommand("SELECT EMP_ID, NAME, BIRTH_DATE, SEX, MARITAL, HOBY FROM EMPLOYEE", objcon.cnn)
            cmd.CommandType = CommandType.Text
            Using sda As SqlDataAdapter = New SqlDataAdapter(cmd)
                Using dt As DataTable = New DataTable()
                    sda.Fill(dt)
                    DataGridView1.DataSource = dt
                End Using
            End Using
        End Using

        'Add the Button Column.
        Dim buttonColumn As DataGridViewButtonColumn = New DataGridViewButtonColumn()
        buttonColumn.HeaderText = ""
        buttonColumn.Width = 60
        buttonColumn.Name = "buttonColumn"
        buttonColumn.Text = "Delete"
        buttonColumn.UseColumnTextForButtonValue = True
        DataGridView1.Columns.Insert(6, buttonColumn)

        'Add the Button Column.
        Dim buttonUpdate As DataGridViewButtonColumn = New DataGridViewButtonColumn()
        buttonUpdate.HeaderText = ""
        buttonUpdate.Width = 60
        buttonUpdate.Name = "buttonColumn"
        buttonUpdate.Text = "Update"
        buttonUpdate.UseColumnTextForButtonValue = True
        DataGridView1.Columns.Insert(7, buttonUpdate)
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.ColumnIndex = 6 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            If MessageBox.Show(String.Format("Do you want to delete EMP ID: {0}", row.Cells("EMP_ID").Value), "Confirmation", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                Using cmd As New SqlCommand("DELETE FROM EMPLOYEE WHERE EMP_ID = @EMPID", objcon.cnn)
                    cmd.CommandType = CommandType.Text
                    cmd.Parameters.AddWithValue("@EMPID", row.Cells("EMP_ID").Value)
                    objcon.cnn.Open()
                    cmd.ExecuteNonQuery()
                    objcon.cnn.Close()
                End Using
                Me.BindGrid()
            End If
        End If

        If e.ColumnIndex = 7 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)
            TextBox1.Enabled = False
            TextBox1.Text = row.Cells("EMP_ID").Value
            TextBox2.Text = row.Cells("NAME").Value
            DateTimePicker1.Text = row.Cells("BIRTH_DATE").Value

            If row.Cells("SEX").Value = "Male" Then
                RadioButton1.Checked = True
            ElseIf row.Cells("SEX").Value = "Female" Then
                RadioButton2.Checked = True
            End If

            ComboBox1.Text = row.Cells("MARITAL").Value

            If row.Cells("HOBY").Value = "Football" Then
                CheckBox1.Checked = True
                CheckBox2.Checked = False
                CheckBox3.Checked = False
            ElseIf row.Cells("HOBY").Value = "Basket" Then
                CheckBox1.Checked = False
                CheckBox2.Checked = True
                CheckBox3.Checked = False
            ElseIf row.Cells("HOBY").Value = "Badminton" Then
                CheckBox1.Checked = False
                CheckBox2.Checked = False
                CheckBox3.Checked = True
            End If

            Button1.Text = "Update"
        End If
    End Sub

End Class