Imports System.ComponentModel

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
End Class