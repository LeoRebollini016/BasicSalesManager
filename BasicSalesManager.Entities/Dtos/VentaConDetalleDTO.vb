Public Class VentaConDetalleDTO
    Public Property IDVenta As Integer
    Public Property IDCliente As Integer
    Public Property Cliente As String
    Public Property Fecha As DateTime
    Public Property Total As Decimal
    Public Property Detalles As List(Of VentaItemDTO)
End Class
