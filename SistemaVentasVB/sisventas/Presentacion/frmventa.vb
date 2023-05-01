Imports System.ComponentModel
Imports System.Windows.Forms

Public Class frmventa

    Private dt As New DataTable
    Private Sub frmventa_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mostrar()
    End Sub

    Private Sub limpiar()
        btnGuardar.Visible = True
        btnEditar.Visible = False

        txtIdCliente.Text = ""
        txtNombreCliente.Text = ""
        txtNumeroDocumento.Text = ""
        txtIdVenta.Text = ""

    End Sub

    Private Sub mostrar()

        Try
            Dim func As New fventa
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

        'buscar()

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
        dataListado.Columns(2).Visible = False
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        limpiar()
        mostrar()

    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Dim result As DialogResult

        result = MessageBox.Show("Realmente quiere editar los datos de la venta",
                                 "Editar registro?",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question)

        If result = DialogResult.OK Then

            If Me.ValidateChildren = True And
               txtIdVenta.Text <> "" And
               txtIdCliente.Text <> "" And
               txtNumeroDocumento.Text <> "" Then

                Try
                    Dim dts As New vventa
                    Dim func As New fventa

                    dts.gidventa = txtIdVenta.Text
                    dts.gidcliente = txtIdCliente.Text
                    dts.gfecha_venta = txtFecha.Text
                    dts.gtipo_documento = cboTipoDocumento.Text
                    dts.gnum_documento = txtNumeroDocumento.Text

                    If func.editar(dts) Then
                        MessageBox.Show("Venta editado correctamente",
                                        "Editar registro",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Question)
                        mostrar()
                        limpiar()
                    Else
                        MessageBox.Show("Venta no editado",
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
           txtIdCliente.Text <> "" And
           txtNombreCliente.Text <> "" Then

            Try
                Dim dts As New vventa
                Dim func As New fventa

                dts.gidcliente = txtIdCliente.Text
                dts.gfecha_venta = txtFecha.Text
                dts.gtipo_documento = cboTipoDocumento.Text
                dts.gnum_documento = txtNumeroDocumento.Text

                If func.insertar(dts) Then
                    MessageBox.Show("Venta registrado correctamente",
                                    "Guardar registros",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question)
                    mostrar()
                    limpiar()
                    cargar_detalle()
                Else
                    MessageBox.Show("Venta no registrado",
                                    "Error al registrar",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)

                End If

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        Else
            MessageBox.Show("Venta registrado correctamente",
                                    "Guardar registros",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)

        End If

    End Sub

    Private Sub dataListado_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellClick
        txtIdVenta.Text = dataListado.SelectedCells.Item(1).Value
        txtIdCliente.Text = dataListado.SelectedCells.Item(2).Value
        txtNombreCliente.Text = dataListado.SelectedCells.Item(3).Value
        txtFecha.Text = dataListado.SelectedCells.Item(5).Value
        cboTipoDocumento.Text = dataListado.SelectedCells.Item(6).Value
        txtNumeroDocumento.Text = dataListado.SelectedCells.Item(7).Value

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

    Private Sub txtBuscar_TextChanged(sender As Object, e As EventArgs) Handles txtBuscar.TextChanged
        buscar()
    End Sub

    Private Sub cargar_detalle()
        frmdetalle_venta.txtIdVenta.Text = dataListado.SelectedCells.Item(1).Value
        frmdetalle_venta.txtIdCliente.Text = dataListado.SelectedCells.Item(2).Value
        frmdetalle_venta.txtNombreCliente.Text = dataListado.SelectedCells.Item(3).Value
        frmdetalle_venta.txtFecha.Text = dataListado.SelectedCells.Item(5).Value
        frmdetalle_venta.cboTipoDocumento.Text = dataListado.SelectedCells.Item(6).Value
        frmdetalle_venta.txtNumeroDocumento.Text = dataListado.SelectedCells.Item(7).Value

        frmdetalle_venta.ShowDialog()
    End Sub

    Private Sub dataListado_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellDoubleClick
        cargar_detalle()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        frmcliente.txtFlag.Text = "1"
        frmcliente.ShowDialog()
    End Sub
End Class