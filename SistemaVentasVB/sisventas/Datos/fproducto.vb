Imports System.Data.SqlClient
Public Class fproducto

    'Herencia de la clase base
    Inherits conexion
    Dim cmd As New SqlCommand

    Public Function mostrar() As DataTable
        Try
            conectado()
            cmd = New SqlCommand("mostrar_producto")
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Connection = cnn

            If cmd.ExecuteNonQuery Then
                Dim dt As New DataTable
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                Return dt
            Else
                Return Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        Finally
            desconectado()
        End Try
    End Function

    Public Function insertar(ByVal objProducto As vproducto) As Boolean
        Try
            conectado()
            cmd = New SqlCommand("insertar_producto")
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = cnn


            cmd.Parameters.AddWithValue("@idcategoria", objProducto.gidcategoria)
            cmd.Parameters.AddWithValue("@nombre", objProducto.gnombre)
            cmd.Parameters.AddWithValue("@descripcion", objProducto.gdescripcion)
            cmd.Parameters.AddWithValue("@stock", objProducto.gStock)
            cmd.Parameters.AddWithValue("@precio_compra", objProducto.gprecio_compra)
            cmd.Parameters.AddWithValue("@precio_venta", objProducto.gprecio_venta)
            cmd.Parameters.AddWithValue("@fecha_vencimiento", objProducto.gfecha_vencimiento)
            cmd.Parameters.AddWithValue("@imagen", objProducto.gimagen)

            If cmd.ExecuteNonQuery Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            Return False

        Finally
            desconectado()
        End Try

    End Function

    Public Function editar(ByVal objProducto As vproducto) As Boolean
        Try
            conectado()
            cmd = New SqlCommand("editar_producto")
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = cnn

            cmd.Parameters.AddWithValue("@idproducto", objProducto.gidproducto)
            cmd.Parameters.AddWithValue("@idcategoria", objProducto.gidcategoria)
            cmd.Parameters.AddWithValue("@nombre", objProducto.gnombre)
            cmd.Parameters.AddWithValue("@descripcion", objProducto.gdescripcion)
            cmd.Parameters.AddWithValue("@stock", objProducto.gStock)
            cmd.Parameters.AddWithValue("@precio_compra", objProducto.gprecio_compra)
            cmd.Parameters.AddWithValue("@precio_venta", objProducto.gprecio_venta)
            cmd.Parameters.AddWithValue("@fecha_vencimiento", objProducto.gfecha_vencimiento)
            cmd.Parameters.AddWithValue("@imagen", objProducto.gimagen)

            If cmd.ExecuteNonQuery Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
            Return False

        Finally
            desconectado()
        End Try

    End Function

    Public Function eliminar(ByVal objProducto As vproducto) As Boolean

        Try
            conectado()
            cmd = New SqlCommand("eliminar_producto")
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = cnn

            cmd.Parameters.Add("@idproducto", SqlDbType.NVarChar, 50).Value = objProducto.gidproducto

            If cmd.ExecuteNonQuery Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try
    End Function


End Class
