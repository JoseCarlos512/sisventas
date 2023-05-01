Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmproducto

    Private dt As New DataTable
    Private Sub frmproducto_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        mostrar()
    End Sub

    Private Sub limpiar()
        btnGuardar.Visible = True
        btnEditar.Visible = False

        txtNombre.Text = ""
        txtDescripcion.Text = ""
        txtStock.Text = "0"
        txtPrecioCompra.Text = "0"
        txtPrecioVenta.Text = ""
        txtIdProducto.Text = ""

        pbImagen.Image = Nothing
        pbImagen.BackgroundImage = My.Resources.blanco
        pbImagen.SizeMode = PictureBoxSizeMode.StretchImage

    End Sub

    Private Sub mostrar()

        Try
            Dim func As New fproducto
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
        dataListado.Columns(2).Visible = False
    End Sub

    Private Sub txtNombre_Validating(sender As Object, e As CancelEventArgs) Handles txtNombre.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese el nombre del producto porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub txtDescripcion_Validating(sender As Object, e As CancelEventArgs) Handles txtDescripcion.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese la descripcion del producto porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub txtStock_Validating(sender As Object, e As CancelEventArgs) Handles txtStock.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese el Stock del producto porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub txtPrecioCompra_Validating(sender As Object, e As CancelEventArgs) Handles txtPrecioCompra.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese el Precio de compra del producto porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub txtPrecioVenta_Validating(sender As Object, e As CancelEventArgs) Handles txtPrecioVenta.Validating
        If DirectCast(sender, Windows.Forms.TextBox).Text.Length > 0 Then
            Me.erroricono.SetError(sender, "")
        Else
            Me.erroricono.SetError(sender, "Ingrese el Precio de Venta del producto porfavor, es un dato requerido")
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        limpiar()
        mostrar()

    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        Dim result As DialogResult

        result = MessageBox.Show("Realmente quiere editar los datos del Producto",
                                 "Editar registro?",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question)

        If result = DialogResult.OK Then

            If Me.ValidateChildren = True And
               txtIdCategoria.Text <> "" And
               txtIdProducto.Text <> "" And
               txtNombre.Text <> "" And
               txtDescripcion.Text <> "" And
               txtPrecioCompra.Text <> "" And
               txtStock.Text <> "" And
               txtPrecioVenta.Text <> "" Then

                Try

                    Dim dts As New vproducto
                    Dim func As New fproducto

                    dts.gidproducto = txtIdProducto.Text
                    dts.gidcategoria = txtIdCategoria.Text
                    dts.gnombre = txtNombre.Text
                    dts.gdescripcion = txtDescripcion.Text
                    dts.gStock = txtStock.Text
                    dts.gprecio_compra = txtPrecioCompra.Text
                    dts.gprecio_venta = txtPrecioVenta.Text
                    dts.gfecha_vencimiento = txtFechaVencimiento.Text

                    ' Crea una secuencia de respalo en la memoria de la imagen
                    Dim ms As New IO.MemoryStream()

                    If Not pbImagen.Image Is Nothing Then
                        pbImagen.Image.Save(ms, pbImagen.Image.RawFormat)
                    Else
                        pbImagen.Image = My.Resources.blanco
                        pbImagen.Image.Save(ms, pbImagen.Image.RawFormat)
                    End If
                    dts.gimagen = ms.GetBuffer


                    If func.editar(dts) Then
                        MessageBox.Show("Producto editado correctamente",
                                        "Editar registro",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Question)
                        mostrar()
                        limpiar()
                    Else
                        MessageBox.Show("Producto no editado",
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
           txtNombre.Text <> "" And
           txtDescripcion.Text <> "" And
           txtPrecioCompra.Text <> "" And
           txtStock.Text <> "" And
           txtPrecioVenta.Text <> "" Then

            Try
                Dim dts As New vproducto
                Dim func As New fproducto

                dts.gnombre = txtNombre.Text
                dts.gidcategoria = txtIdCategoria.Text
                dts.gdescripcion = txtDescripcion.Text
                dts.gStock = txtStock.Text
                dts.gprecio_compra = txtPrecioCompra.Text
                dts.gprecio_venta = txtPrecioVenta.Text
                dts.gfecha_vencimiento = txtFechaVencimiento.Text

                ' Crea una secuencia de respalo en la memoria de la imagen
                Dim ms As New IO.MemoryStream()

                If Not pbImagen.Image Is Nothing Then
                    pbImagen.Image.Save(ms, pbImagen.Image.RawFormat)
                Else
                    pbImagen.Image = My.Resources.blanco
                    pbImagen.Image.Save(ms, pbImagen.Image.RawFormat)
                End If
                dts.gimagen = ms.GetBuffer

                If func.insertar(dts) Then
                    MessageBox.Show("Producto registrado correctamente",
                                    "Editar registro",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Question)
                    mostrar()
                    limpiar()
                Else
                    MessageBox.Show("Producto no registrado",
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error)

                End If

            Catch ex As Exception
                MessageBox.Show("Faltan agregar algunos campos",
                                "Editar registro",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning)
            End Try

        Else
            MsgBox("Faltan ingresar algunos datos")

        End If

    End Sub

    Private Sub dataListado_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dataListado.CellClick

        txtIdProducto.Text = dataListado.SelectedCells.Item(1).Value
        txtIdCategoria.Text = dataListado.SelectedCells.Item(2).Value
        txtNombreCategoria.Text = dataListado.SelectedCells.Item(3).Value
        txtNombre.Text = dataListado.SelectedCells.Item(4).Value
        txtDescripcion.Text = dataListado.SelectedCells.Item(5).Value
        txtStock.Text = dataListado.SelectedCells.Item(6).Value
        txtPrecioCompra.Text = dataListado.SelectedCells.Item(7).Value
        txtPrecioVenta.Text = dataListado.SelectedCells.Item(8).Value
        txtFechaVencimiento.Text = dataListado.SelectedCells.Item(9).Value

        pbImagen.BackgroundImage = Nothing
        Dim b() As Byte = dataListado.SelectedCells.Item(10).Value
        Dim ms As New IO.MemoryStream(b)

        pbImagen.Image = Image.FromStream(ms)
        pbImagen.SizeMode = PictureBoxSizeMode.StretchImage

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

        result = MessageBox.Show("Realmente quiere eliminar los Productos seleccionados",
                                 "Eliminar registros",
                                 MessageBoxButtons.OKCancel,
                                 MessageBoxIcon.Question)

        If result = DialogResult.OK Then
            Try
                For Each row As DataGridViewRow In dataListado.Rows
                    Dim marcado As Boolean = Convert.ToBoolean(row.Cells("Eliminar").Value)

                    If marcado Then

                        Dim idproducto As Integer = Convert.ToInt32(row.Cells("idproducto").Value)
                        Dim vdb As New vproducto
                        Dim func As New fproducto
                        vdb.gidproducto = idproducto

                        If func.eliminar(vdb) Then
                        Else
                            MessageBox.Show("Producto no fue eliminado",
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

    Private Sub pbxCargar_Click(sender As Object, e As EventArgs) Handles pbxCargar.Click
        If dlgDialogo.ShowDialog() = DialogResult.OK Then
            pbImagen.BackgroundImage = Nothing
            pbImagen.Image = New Bitmap(dlgDialogo.FileName)
            pbImagen.SizeMode = PictureBoxSizeMode.StretchImage
        End If
    End Sub

    Private Sub pbxDelete_Click(sender As Object, e As EventArgs) Handles pbxDelete.Click
        pbImagen.Image = Nothing
        pbImagen.BackgroundImage = My.Resources.blanco
        pbImagen.SizeMode = PictureBoxSizeMode.StretchImage
    End Sub

    Private Sub btnBuscarCategoria_Click(sender As Object, e As EventArgs) Handles btnBuscarCategoria.Click
        frmcategoria.txtFlag.Text = "1"
        frmcategoria.ShowDialog()
    End Sub
End Class