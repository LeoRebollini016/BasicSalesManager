Public Module MapeadorHelper
    ' Cliente
    Public Function ClienteAClienteDTO(cliente As Cliente) As ClienteDTO
        Return New ClienteDTO With {
            .ID = cliente.ID,
            .Nombre = cliente.Nombre,
            .Telefono = cliente.Telefono,
            .Correo = cliente.Correo
        }
    End Function

    Public Function ClienteDTOACliente(clienteDTO As ClienteDTO) As Cliente
        Return New Cliente With {
            .ID = clienteDTO.ID,
            .Nombre = clienteDTO.Nombre,
            .Telefono = clienteDTO.Telefono,
            .Correo = clienteDTO.Correo
        }
    End Function

    ' Producto
    Public Function ProductoAProductoDTO(producto As Producto) As ProductoDTO
        Return New ProductoDTO With {
            .ID = producto.ID,
            .Nombre = producto.Nombre,
            .Categoria = producto.Categoria,
            .Precio = producto.Precio
        }
    End Function
    Public Function ProductoDTOAProducto(productoDTO As ProductoDTO) As Producto
        Return New Producto With {
            .ID = productoDTO.ID,
            .Nombre = productoDTO.Nombre,
            .Precio = productoDTO.Precio,
            .Categoria = productoDTO.Categoria
        }
    End Function

    ' Venta
    Public Function CreacionVentaDTOAVenta(creacionVentaDTo As CreacionVentaDTO) As Venta
        Dim venta As New Venta With {
            .IDCliente = creacionVentaDTo.IDCliente,
            .Fecha = creacionVentaDTo.Fecha,
            .Total = creacionVentaDTo.Total,
            .Detalles = ItemsVentaDTOAItemsVenta(creacionVentaDTo.Detalles)
        }
        Return venta
    End Function
    Public Function VentaAVentasConDetallesDTO(venta As Venta) As VentaConDetalleDTO
        Dim ventaConDetalle As New VentaConDetalleDTO With {
            .IDVenta = venta.ID,
            .IDCliente = venta.IDCliente,
            .Fecha = venta.Fecha,
            .Total = venta.Total,
            .Detalles = ItemsVentaAItemsVentaDTO(venta.Detalles)
        }
        Return ventaConDetalle
    End Function

    Public Function VentasConDetallesDTOAVenta(ventaConDetalleDTO As VentaConDetalleDTO) As Venta
        Dim venta As New Venta With {
            .ID = ventaConDetalleDTO.IDVenta,
            .IDCliente = ventaConDetalleDTO.IDCliente,
            .Fecha = ventaConDetalleDTO.Fecha,
            .Total = ventaConDetalleDTO.Total,
            .Detalles = ItemsVentaDTOAItemsVenta(ventaConDetalleDTO.Detalles)
        }
        Return venta
    End Function
    Public Function ItemsVentaAItemsVentaDTO(items As List(Of VentaItem)) As List(Of VentaItemDTO)
        Dim result As New List(Of VentaItemDTO)()
        For Each item In items
            result.Add(New VentaItemDTO With {
                .IDProducto = item.IDProducto,
                .Cantidad = item.Cantidad,
                .PrecioUnitario = item.PrecioUnitario
            })
        Next
        Return result
    End Function
    Public Function ItemsVentaDTOAItemsVenta(items As List(Of VentaItemDTO)) As List(Of VentaItem)
        Dim result As New List(Of VentaItem)()
        For Each item In items
            result.Add(New VentaItem With {
                .IDProducto = item.IDProducto,
                .Cantidad = item.Cantidad,
                .PrecioUnitario = item.PrecioUnitario
            })
        Next
        Return result
    End Function
    ' VentaItemDTO y VentaItemConDetalleDTO
    Public Function VentaReportesDTO(items As List(Of VentaItem), productos As List(Of Producto)) As List(Of VentaItemDTO)
        Dim result As New List(Of VentaItemDTO)()
        For Each item In items
            Dim productoNombre = productos.FirstOrDefault(Function(p) p.ID = item.IDProducto)?.Nombre
            result.Add(New VentaItemDTO With {
                .Producto = productoNombre,
                .Cantidad = item.Cantidad,
                .PrecioUnitario = item.PrecioUnitario
            })
        Next
        Return result
    End Function
End Module
