Public Class CreacionVentaDTO
    Public Property IDCliente As Integer
    Public Property Fecha As DateTime
    Public Property Total As Decimal
    Public Property Detalles As List(Of VentaItemDTO)
End Class
