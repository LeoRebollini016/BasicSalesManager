Module VentaQueries
    Public Const InsertVentaQuery As String = "
        INSERT INTO ventas (IDCliente, Fecha, Total) 
        VALUES (@IDCliente, @Fecha, @Total); 
        SELECT SCOPE_IDENTITY();"

    Public Const InsertVentasItemsQuery As String =
        "INSERT INTO ventasitems (IDVenta, IDProducto, PrecioUnitario, Cantidad, PrecioTotal)
        VALUES (@IDVenta, @IDProducto, @PrecioUnitario, @Cantidad, @PrecioTotal);"

    Public Const UpdateVentaQuery As String =
        "UPDATE Ventas 
        SET Fecha = @Fecha, IDCliente = @ClienteID, Total = @Total 
        WHERE ID = @ID"

    Public Const DeleteVentasByIDQuery As String =
        "DELETE FROM Ventas 
        WHERE ID = @ID"

    Public Const DeleteVentasItemsByVentaIDQuery As String =
        "DELETE 
        FROM ventasitems 
        WHERE IDVenta = @IDVenta"
    Public Const SelectVentasConClienteQuery As String =
        "SELECT v.ID, v.Fecha, C.Cliente, V.Total
        FROM Ventas V
        INNER JOIN Clientes c ON V.IDCliente = C.ID"
    Public Const SelectVentaPorIDQuery As String =
        "SELECT ID, IDCliente, Fecha, Total
        FROM ventas V
        WHERE V.ID = @ID"
    Public Const SelectDetallesPorVentaIDQuery As String =
        "SELECT IDProducto, PrecioUnitario, Cantidad, PrecioTotal FROM ventasitems WHERE IDVenta = @IDVenta;"

    Public Const SelectVentaDetallePorFiltroQuery As String =
        "SELECT
            v.ID as IDVenta, v.Fecha, v.Total,
            c.Cliente,
            p.Nombre AS Producto,
            vi.Cantidad, vi.PrecioUnitario
        FROM ventas v
        JOIN clientes c ON v.IDCliente = c.ID
        JOIN ventasitems vi ON vi.IDVenta = v.ID
        JOIN productos p ON p.ID = vi.IDProducto
        WHERE c.Cliente LIKE @Filtro OR p.Nombre LIKE @Filtro
        ORDER BY v.Fecha DESC, v.ID;"

    Public Const SelectProductosVendidosMensualQuery As String =
        "SELECT p.Nombre AS Producto, YEAR(v.Fecha) AS Año, MONTH(v.Fecha) AS Mes, SUM(vi.Cantidad) AS CantidadVendida " &
        "FROM ventas v " &
        "JOIN ventasitems vi ON vi.IDVenta = v.ID " &
        "JOIN productos p ON p.ID = vi.IDProducto " &
        "GROUP BY p.Nombre, YEAR(v.Fecha), MONTH(v.Fecha) " &
        "ORDER BY p.Nombre, Año, Mes"
End Module
