Public Class VentaReportesDTO
    Public Property IDVenta As Integer
    Public Property Cliente As ClienteDTO
    Public Property Fecha As DateTime
    Public Property Total As Decimal
    Public Property Detalles As List(Of VentaItemDTO)
End Class
