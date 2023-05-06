Imports System.Windows.Forms
Imports System.ComponentModel

Public Class frmdetalle_venta

    Private dt As New DataTable
    Private Shared before_cant As Integer = 0

    Private Sub frmdetalle_venta_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mostrar()
    End Sub


    Private Sub limpiar()
        btnGuardar.Visible = True
        btnEditar.Visible = False

        txtIDDetalleVenta.Text = ""

        txtIdProducto.Text = ""
        txtNameProducto.Text = ""
        txtCantidad.Value = 0
        txtStock.Value = 0
        txtPrecioUnitario.Text = ""

    End Sub

    Private Sub mostrar()


        Try
            Dim func As New fdetalle_venta
            Dim dts As New vdetalle_venta

            dts.gidventa = txtIdVenta.Text

            dt = func.mostrar_ventaxdetalle(dts)

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

            dv.RowFilter = txtIdVenta.Text & " like '" & txtBuscar.Text & "%'"

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
               txtIdProducto.Text <> "" Then

                Try
                    Dim dts As New vdetalle_venta
                    Dim func As New fdetalle_venta

                    dts.giddetalle_venta = txtIDDetalleVenta.Text
                    dts.gidventa = txtIdVenta.Text
                    dts.gidproducto = txtIdProducto.Text
                    dts.gcantidad = txtCantidad.Text
                    dts.gprecio_unitario = txtPrecioUnitario.Text

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
           txtIdVenta.Text <> "" And
           txtIdCliente.Text <> "" And
           txtIdProducto.Text <> "" Then

            Try
                Dim dts As New vdetalle_venta
                Dim func As New fdetalle_venta

                dts.gidventa = txtIdVenta.Text
                dts.gidproducto = txtIdProducto.Text
                dts.gcantidad = txtCantidad.Text
                dts.gprecio_unitario = txtPrecioUnitario.Text

                If func.insertar(dts) Then

                    ' Disminuir stock
                    func.disminuir_stock(dts)

                    MessageBox.Show("Articulo añadido correctamente",
                                    "Guardar registros",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question)
                    mostrar()
                    limpiar()
                Else
                    MessageBox.Show("Articulo no añadido",
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
        txtIDDetalleVenta.Text = dataListado.SelectedCells.Item(1).Value
        txtIdVenta.Text = dataListado.SelectedCells.Item(2).Value
        txtIdProducto.Text = dataListado.SelectedCells.Item(3).Value
        txtNameProducto.Text = dataListado.SelectedCells.Item(4).Value
        txtCantidad.Text = dataListado.SelectedCells.Item(5).Value
        txtPrecioUnitario.Text = dataListado.SelectedCells.Item(6).Value

        ' Crear variable de sesion con la cantidad
        before_cant = dataListado.SelectedCells.Item(5).Value


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
        'frmdetalle_venta.txtIdVenta.Text = dataListado.SelectedCells.Item(1).Value
        'frmdetalle_venta.txtIdCliente.Text = dataListado.SelectedCells.Item(2).Value
        'frmdetalle_venta.txtNombreCliente.Text = dataListado.SelectedCells.Item(3).Value
        'frmdetalle_venta.txtFecha.Text = dataListado.SelectedCells.Item(5).Value
        'frmdetalle_venta.cboTipoDocumento.Text = dataListado.SelectedCells.Item(6).Value
        'frmdetalle_venta.txtNumeroDocumento.Text = dataListado.SelectedCells.Item(7).Value

        'frmdetalle_venta.ShowDialog()
    End Sub

    Private Sub dataListado_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellDoubleClick
        cargar_detalle()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btn_buscar.Click
        frmproducto.txtFlag.Text = "1"
        frmproducto.ShowDialog()
    End Sub
End Class