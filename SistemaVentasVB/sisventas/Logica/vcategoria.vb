Public Class vcategoria

    Dim IdCategoria As Integer
    Dim Nombre As String

    Public Property gIdCategoria
        Get
            Return IdCategoria
        End Get
        Set(value)
            IdCategoria = value
        End Set
    End Property

    Public Property gNombre
        Get
            Return Nombre
        End Get
        Set(value)
            Nombre = value
        End Set
    End Property

    'Constructores
    Public Sub New()

    End Sub

    Public Sub New(idcategoria, nombre)
        gIdCategoria = idcategoria
        gNombre = nombre
    End Sub

End Class
