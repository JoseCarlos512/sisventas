Imports System.ComponentModel
Imports System.Security.Principal
Imports System.Windows.Forms

Public Class frmcategoria

    Private dt As New DataTable

    Private Sub frmcategoria_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mostrar()
    End Sub

    Private Sub limpiar()
        btnGuardar.Visible = True
        btnEditar.Visible = False

        txtNombre.Text = ""
        txtIdCategoria.Text = ""

    End Sub

    Private Sub mostrar()

        Try
            Dim func As New fcategoria
            dt = func.mostrar
            dataListado.Columns.Item("Eliminar").Visible = False

            System.Diagnostics.Debug.WriteLine("Antes del IF")
            If dt.Rows.Count <> 0 Then
                System.Diagnostics.Debug.WriteLine("Despues del IF")
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

    Private Sub txtNombre_Validating(sender As Object, e As CancelEventArgs) Handles txtNombre.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese el nombre de la categoria porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        limpiar()
        mostrar()
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click

        Dim result As DialogResult

        result = MessageBox.Show("Realmente quiere editar la categoria",
                                 "Editar?",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question)

        If result = DialogResult.OK Then

            If Me.ValidateChildren = True And
               txtIdCategoria.Text <> "" And
               txtNombre.Text <> "" Then

                Try
                    Dim dts As New vcategoria
                    Dim func As New fcategoria

                    dts.gIdCategoria = txtIdCategoria.Text
                    dts.gNombre = txtNombre.Text

                    If func.editar(dts) Then
                        MessageBox.Show("Categoria editado correctamente",
                                        "Editar registro",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Question)
                        mostrar()
                        limpiar()
                    Else
                        MessageBox.Show("Categoria no editado",
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

        If Me.ValidateChildren And
           txtNombre.Text <> "" Then

            Try
                Dim objcategoria As New vcategoria
                Dim fcat As New fcategoria

                objcategoria.gNombre = txtNombre.Text

                If fcat.insertar(objcategoria) Then
                    MessageBox.Show("Categoria ingresado correctamente",
                                        "Editar registro",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Question)
                    mostrar()
                    limpiar()
                Else
                    MessageBox.Show("Categoria no insertada",
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

    End Sub

    Private Sub dataListado_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellClick
        txtIdCategoria.Text = dataListado.SelectedCells.Item(1).Value
        txtNombre.Text = dataListado.SelectedCells.Item(2).Value

        btnEditar.Visible = True
        btnGuardar.Visible = False
    End Sub

    Private Sub dataListado_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellContentClick

        ' Leer un poco mas no entiendo
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

        result = MessageBox.Show("Realmente quiere eliminar las categorias seleccionadas",
                                 "Eliminar registros",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question)

        If result = DialogResult.OK Then
            Try
                For Each row As DataGridViewRow In dataListado.Rows
                    Dim marcado As Boolean = Convert.ToBoolean(row.Cells("Eliminar").Value)

                    If marcado Then

                        Dim idCategoria As Integer = Convert.ToInt32(row.Cells("idcategoria").Value)

                        Dim objCategoria As New vcategoria
                        Dim funcCategoria As New fcategoria
                        objCategoria.gIdCategoria = idCategoria

                        If funcCategoria.eliminar(objCategoria) Then
                        Else
                            MessageBox.Show("Categoria no fue eliminado",
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
            frmproducto.txtIdCategoria.Text = dataListado.SelectedCells.Item(1).Value
            frmproducto.txtNombreCategoria.Text = dataListado.SelectedCells.Item(2).Value
            Me.Close()
        End If
    End Sub
End Class