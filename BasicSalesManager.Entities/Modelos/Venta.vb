Public Class Venta
    Public Property ID As Integer
    Public Property IDCliente As Integer
    Public Property Fecha As DateTime
    Public Property Total As Decimal
    Public Property Cliente As Cliente
    Public Property Detalles As List(Of VentaItem)
End Class
