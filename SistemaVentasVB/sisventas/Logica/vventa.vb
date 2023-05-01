Public Class vventa
    Dim idventa, idcliente As Integer
    Dim fecha_venta As Date
    Dim tipo_documento, num_documento As String


    Public Property gidventa
        Get
            Return idventa
        End Get
        Set(value)
            idventa = value
        End Set
    End Property
    Public Property gidcliente
        Get
            Return idcliente
        End Get
        Set(value)
            idcliente = value
        End Set
    End Property

    Public Property gfecha_venta
        Get
            Return fecha_venta
        End Get
        Set(value)
            fecha_venta = value
        End Set
    End Property

    Public Property gtipo_documento
        Get
            Return tipo_documento
        End Get
        Set(value)
            tipo_documento = value
        End Set
    End Property

    Public Property gnum_documento
        Get
            Return num_documento
        End Get
        Set(value)
            num_documento = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal idventa As Integer, ByVal idcliente As Integer, ByVal fecha_venta As Date,
                   ByVal tipo_documento As String, ByVal num_documento As String)
        gidventa = idventa
        gidcliente = idcliente
        gfecha_venta = fecha_venta
        gtipo_documento = tipo_documento
        gnum_documento = num_documento
    End Sub
End Class
