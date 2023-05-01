Imports System.ComponentModel
Imports System.Security.Principal
Imports System.Windows.Forms

Public Class frmcliente

    Private dt As New DataTable
    Private Sub frmcliente_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mostrar()
    End Sub

    Private Sub limpiar()
        btnGuardar.Visible = True
        btnEditar.Visible = False

        txtNombres.Text = ""
        txtApellidos.Text = ""
        txtDireccion.Text = ""
        txtTelefono.Text = ""
        txtDNI.Text = ""
        txtIdCliente.Text = ""

    End Sub

    Private Sub mostrar()

        Try
            Dim func As New fcliente
            dt = func.mostrar
            dataListado.Columns.Item("Eliminar").Visible = False

            If dt.Rows.Count <> 0 Then
                dataListado.DataSource = dt
                txtBuscar.Enabled = True
                dataListado.ColumnHeadersVisible = True
                inexistente.Visible = True

            Else
                dataListado.DataSource = Nothing
                txtBuscar.Enabled = False
                dataListado.ColumnHeadersVisible = False
                inexistente.Visible = False
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


        btnNuevo.Visible = True
        btnEditar.Visible = False
        btnGuardar.Visible = True

        buscar()

    End Sub


    Private Sub buscar()

        Try
            Dim ds As New DataSet
            ds.Tables.Add(dt.Copy)
            Dim dv As New DataView(ds.Tables(0))

            dv.RowFilter = cboCampo.Text & " like '" & txtBuscar.Text & "%'"

            If dv.Count <> 0 Then
                inexistente.Visible = False
                dataListado.DataSource = dv
                ocultar_columnas()
            Else
                inexistente.Visible = True
                dataListado.DataSource = Nothing
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub ocultar_columnas()
        dataListado.Columns(1).Visible = False
    End Sub

    Private Sub txtNombres_Validating(sender As Object, e As CancelEventArgs) Handles txtNombres.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese el nombre del cliente porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub txtApellidos_Validating(sender As Object, e As CancelEventArgs) Handles txtApellidos.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese el Apellidos del cliente porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub txtDireccion_Validating(sender As Object, e As CancelEventArgs) Handles txtDireccion.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese el Direccion del cliente porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub txtTelefono_Validating(sender As Object, e As CancelEventArgs) Handles txtTelefono.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese el Telefono del cliente porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub txtDNI_Validating(sender As Object, e As CancelEventArgs) Handles txtDNI.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese el DNI del cliente porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        limpiar()
        mostrar()

    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Dim result As DialogResult

        result = MessageBox.Show("Realmente quiere editar los datos del cliente",
                                 "Editar registro?",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question)

        If result = DialogResult.OK Then

            If Me.ValidateChildren = True And
               txtIdCliente.Text <> "" And
               txtNombres.Text <> "" And
               txtApellidos.Text <> "" And
               txtTelefono.Text <> "" And
               txtDireccion.Text <> "" And
               txtDNI.Text <> "" Then

                Try
                    Dim dts As New vcliente
                    Dim func As New fcliente

                    dts.gidcliente = txtIdCliente.Text
                    dts.gnombre = txtNombres.Text
                    dts.gapellidos = txtApellidos.Text
                    dts.gdireccion = txtDireccion.Text
                    dts.gtelefono = txtTelefono.Text
                    dts.gdni = txtDNI.Text

                    If func.editar(dts) Then
                        MessageBox.Show("Cliente editado correctamente",
                                        "Editar registro",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Question)
                        mostrar()
                        limpiar()
                    Else
                        MessageBox.Show("Cliente no editado",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error)

                    End If

                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

            Else
                MessageBox.Show("Faltan agregar algunos campos",
                                "Editar registro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning)
            End If

        End If


    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click


        If Me.ValidateChildren = True And
           txtNombres.Text <> "" And
           txtApellidos.Text <> "" And
           txtTelefono.Text <> "" And
           txtDireccion.Text <> "" And
           txtDNI.Text <> "" Then

            Try
                Dim dts As New vcliente
                Dim func As New fcliente

                dts.gnombre = txtNombres.Text
                dts.gapellidos = txtApellidos.Text
                dts.gdireccion = txtDireccion.Text
                dts.gtelefono = txtTelefono.Text
                dts.gdni = txtDNI.Text

                If func.insertar(dts) Then
                    MsgBox("Cliente registrado correctamente")
                    mostrar()
                    limpiar()
                Else
                    MsgBox("Error: Cliente no registrado")

                End If

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        Else
            MsgBox("Faltan ingresar algunos datos")

        End If

    End Sub

    Private Sub dataListado_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellClick
        txtIdCliente.Text = dataListado.SelectedCells.Item(1).Value
        txtNombres.Text = dataListado.SelectedCells.Item(2).Value
        txtApellidos.Text = dataListado.SelectedCells.Item(3).Value
        txtDireccion.Text = dataListado.SelectedCells.Item(4).Value
        txtTelefono.Text = dataListado.SelectedCells.Item(5).Value
        txtDNI.Text = dataListado.SelectedCells.Item(6).Value

        btnEditar.Visible = True
        btnGuardar.Visible = False

    End Sub

    Private Sub dataListado_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellContentClick

        If e.ColumnIndex = Me.dataListado.Columns.Item("Eliminar").Index Then

            Dim chkcell As DataGridViewCheckBoxCell = Me.dataListado.Rows(e.RowIndex).Cells("Eliminar")
            chkcell.Value = Not chkcell.Value
        End If

    End Sub

    Private Sub cbo_eliminar_CheckedChanged(sender As Object, e As EventArgs) Handles cbo_eliminar.CheckedChanged
        If cbo_eliminar.CheckState = CheckState.Checked Then
            dataListado.Columns.Item("Eliminar").Visible = True
        Else
            dataListado.Columns.Item("Eliminar").Visible = False
        End If
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        Dim result As DialogResult

        result = MessageBox.Show("Realmente quiere eliminar los clientes seleccionados",
                                 "Eliminar registros",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question)

        If result = DialogResult.OK Then
            Try
                For Each row As DataGridViewRow In dataListado.Rows
                    Dim marcado As Boolean = Convert.ToBoolean(row.Cells("Eliminar").Value)

                    If marcado Then

                        Dim idCliente As Integer = Convert.ToInt32(row.Cells("idcliente").Value)

                        System.Diagnostics.Debug.WriteLine(idCliente)
                        Dim vdb As New vcliente
                        Dim func As New fcliente
                        vdb.gidcliente = idCliente

                        If func.eliminar(vdb) Then
                        Else
                            MessageBox.Show("Cliente no fue eliminado",
                                            "Eliminar registros",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information)
                        End If

                    End If

                Next
                Call mostrar()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            MessageBox.Show("Cancelando eliminacion de registros",
                            "Eliminar registros",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Call mostrar()
        End If

        Call limpiar()

    End Sub

    Private Sub dataListado_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellDoubleClick
        If txtFlag.Text = "1" Then
            frmventa.txtIdCliente.Text = dataListado.SelectedCells.Item(1).Value
            frmventa.txtNombreCliente.Text = dataListado.SelectedCells.Item(2).Value
            dataListado.DataSource = Nothing
            Me.Close()
        End If

    End Sub
End Class