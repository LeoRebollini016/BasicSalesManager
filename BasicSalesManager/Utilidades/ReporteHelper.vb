Imports System.IO
Imports System.Text
Imports TacticaCRM.Entidades

Public Class ReporteHelper

    Public Shared Sub GenerarReporteVenta(venta As VentaReportesDTO, ruta As String)
        Try
            Using sw As New StreamWriter(ruta, False, Encoding.UTF8)
                sw.WriteLine("Reporte de ventas")
                sw.WriteLine($"Cliente:{venta.Cliente.Nombre}")
                sw.WriteLine($"Telefono:{venta.Cliente.Telefono}")
                sw.WriteLine($"Correo:{venta.Cliente.Correo}")
                sw.WriteLine($"Fecha:{venta.Fecha:dd/MM/yyyy}")
                sw.WriteLine($"Total:${venta.Total}")
                sw.WriteLine()
                sw.WriteLine("Detalle de productos")
                sw.WriteLine("Producto:Cantidad:Precio Unitario")
                For Each item In venta.Detalles
                    sw.WriteLine($"{item.Producto}:{item.Cantidad}:{item.PrecioUnitario}")
                Next
            End Using
        Catch ex As Exception
            Throw New Exception("Error al generar el reporte de venta: " & ex.Message)
        End Try
    End Sub
    Public Shared Sub GenerarReporteProductosMensual(productosMensual As List(Of ProductoVentaMensualDTO), ruta As String)
        Try
            Using sw As New StreamWriter(ruta, False, Encoding.UTF8)
                sw.WriteLine("Producto:Año:Mes:CantidadVendida")
                For Each item In productosMensual
                    sw.WriteLine($"{item.Producto}:{item.Año}:{item.Mes}:{item.CantidadVendida}")
                Next
            End Using
        Catch ex As Exception
            Throw New Exception("Error al generar el reporte mensual de productos: " & ex.Message)
        End Try
    End Sub
End Class
