Imports System.Data.SqlClient
Imports TacticaCRM.Entidades

Public Class VentaDAL

    Public Shared Function ObtenerVentas() As List(Of VentaDTO)
        Dim lista As New List(Of VentaDTO)()
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            conn.Open()
            Using cmd As New SqlCommand(SelectVentasConClienteQuery, conn)
                Using lector As SqlDataReader = cmd.ExecuteReader()
                    While lector.Read()
                        lista.Add(New VentaDTO With {
                        .Cliente = lector("Cliente").ToString(),
                        .Fecha = Convert.ToDateTime(lector("Fecha")),
                        .Total = Convert.ToDecimal(lector("Total"))
                    })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    Public Shared Function ObtenerVentaPorID(idVenta As Integer) As VentaConDetalleDTO
        Dim venta As Venta = Nothing
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            conn.Open()
            Using cmd As New SqlCommand(SelectVentaPorIDQuery, conn)
                cmd.Parameters.AddWithValue("@ID", idVenta)
                Using lector As SqlDataReader = cmd.ExecuteReader()
                    If lector.Read() Then
                        venta = New Venta With {
                            .ID = Convert.ToInt32(lector("ID")),
                            .IDCliente = Convert.ToInt32(lector("IDCliente")),
                            .Fecha = Convert.ToDateTime(lector("Fecha")),
                            .Total = Convert.ToDecimal(lector("Total")),
                            .Detalles = New List(Of VentaItem)()
                        }
                    End If
                End Using
            End Using
            If venta Is Nothing Then Return Nothing

            Using cmdDetalle As New SqlCommand(SelectDetallesPorVentaIDQuery, conn)
                cmdDetalle.Parameters.AddWithValue("@IDVenta", idVenta)
                Using lector As SqlDataReader = cmdDetalle.ExecuteReader()
                    While lector.Read()
                        Dim item As New VentaItem With {
                            .IDProducto = Convert.ToInt32(lector("IDProducto")),
                            .PrecioUnitario = Convert.ToDecimal(lector("PrecioUnitario")),
                            .Cantidad = Convert.ToInt32(lector("Cantidad")),
                            .PrecioTotal = Convert.ToDecimal(lector("PrecioTotal"))
                        }
                        venta.Detalles.Add(item)
                    End While
                End Using
            End Using
        End Using
        Return VentaAVentasConDetallesDTO(venta)
    End Function

    Public Shared Function Insertar(venta As Venta) As Integer
        Dim ventaID As Integer = 0
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            conn.Open()
            Dim trans As SqlTransaction = conn.BeginTransaction()
            Try
                Dim cmdVenta As New SqlCommand(InsertVentaQuery, conn, trans)
                cmdVenta.Parameters.AddWithValue("@IDCliente", venta.IDCliente)
                cmdVenta.Parameters.AddWithValue("@Fecha", venta.Fecha)
                cmdVenta.Parameters.AddWithValue("@Total", venta.Total)
                ventaID = Convert.ToInt32(cmdVenta.ExecuteScalar)

                For Each item In venta.Detalles
                    Dim cmdItem As New SqlCommand(InsertVentasItemsQuery, conn, trans)
                    cmdItem.Parameters.AddWithValue("@IDVenta", ventaID)
                    cmdItem.Parameters.AddWithValue("@IDProducto", item.IDProducto)
                    cmdItem.Parameters.AddWithValue("@PrecioUnitario", item.PrecioUnitario)
                    cmdItem.Parameters.AddWithValue("@Cantidad", item.Cantidad)
                    cmdItem.Parameters.AddWithValue("@PrecioTotal", item.PrecioTotal)
                    cmdItem.ExecuteNonQuery()
                Next

                trans.Commit()
            Catch ex As Exception
                trans.Rollback()
                Throw New Exception("Error al insertar la venta: " & ex.Message)
            End Try
        End Using
        Return ventaID
    End Function

    Public Shared Sub Eliminar(ventaID As Integer)
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            conn.Open()
            Using trans As SqlTransaction = conn.BeginTransaction()
                Try
                    Dim cmdDetalles As New SqlCommand(DeleteVentasItemsByVentaIDQuery, conn, trans)
                    cmdDetalles.Parameters.AddWithValue("@VentaID", ventaID)
                    cmdDetalles.ExecuteNonQuery()
                    Dim cmdVenta As New SqlCommand(DeleteVentasByIDQuery, conn, trans)
                    cmdVenta.Parameters.AddWithValue("@ID", ventaID)
                    cmdVenta.ExecuteNonQuery()

                    trans.Commit()
                Catch ex As Exception
                    trans.Rollback()
                    Throw New Exception("Error al eliminar la venta: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Public Shared Function Modificar(venta As Venta) As Boolean
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            conn.Open()
            Using trans As SqlTransaction = conn.BeginTransaction()
                Try
                    Dim cmdVenta As New SqlCommand(UpdateVentaQuery, conn, trans)
                    cmdVenta.Parameters.AddWithValue("@Fecha", venta.Fecha)
                    cmdVenta.Parameters.AddWithValue("@ClienteID", venta.IDCliente)
                    cmdVenta.Parameters.AddWithValue("@Total", venta.Total)
                    cmdVenta.Parameters.AddWithValue("@ID", venta.ID)
                    cmdVenta.ExecuteNonQuery()

                    Dim cmdEliminarDetalles As New SqlCommand(DeleteVentasItemsByVentaIDQuery, conn, trans)
                    cmdEliminarDetalles.Parameters.AddWithValue("@IDVenta", venta.ID)
                    cmdEliminarDetalles.ExecuteNonQuery()

                    For Each detalle In venta.Detalles
                        Dim cmdDetalle As New SqlCommand(InsertVentasItemsQuery, conn, trans)
                        cmdDetalle.Parameters.AddWithValue("@IDVenta", venta.ID)
                        cmdDetalle.Parameters.AddWithValue("@IDProducto", detalle.IDProducto)
                        cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad)
                        cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario)
                        cmdDetalle.Parameters.AddWithValue("@PrecioTotal", detalle.PrecioTotal)
                        cmdDetalle.ExecuteNonQuery()
                    Next

                    trans.Commit()
                Catch ex As Exception
                    trans.Rollback()
                    Throw New Exception("Error al modificar la venta: " & ex.Message)
                End Try
            End Using
        End Using
        Return True
    End Function

    Public Shared Function ObtenerVentaDetallesConFiltro(filtro As String) As List(Of VentaConDetalleDTO)
        Dim ventasDic As New Dictionary(Of Integer, VentaConDetalleDTO)

        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            conn.Open()
            Using cmd As New SqlCommand(SelectVentaDetallePorFiltroQuery, conn)
                cmd.Parameters.AddWithValue("@Filtro", $"%{filtro}%")
                Using lector As SqlDataReader = cmd.ExecuteReader()
                    While lector.Read()
                        Dim idVenta = Convert.ToInt32(lector("IDVenta"))

                        If Not ventasDic.ContainsKey(idVenta) Then
                            Dim venta = New VentaConDetalleDTO With {
                                .IDVenta = idVenta,
                                .Cliente = lector("Cliente").ToString(),
                                .Fecha = Convert.ToDateTime(lector("Fecha")),
                                .Total = Convert.ToDecimal(lector("Total")),
                                .Detalles = New List(Of VentaItemDTO)
                            }
                            ventasDic.Add(idVenta, venta)
                        End If
                        Dim item As New VentaItemDTO With {
                            .Producto = lector("Producto").ToString(),
                            .Cantidad = Convert.ToInt32(lector("Cantidad")),
                            .PrecioUnitario = Convert.ToDecimal(lector("PrecioUnitario"))
                        }
                        ventasDic(idVenta).Detalles.Add(item)
                    End While
                End Using
            End Using
        End Using

        Return ventasDic.Values.ToList()
    End Function

    Public Shared Function ObtenerProductosVendidosMensual() As List(Of ProductoVentaMensualDTO)
        Dim lista As New List(Of ProductoVentaMensualDTO)

        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            conn.Open()
            Using cmd As New SqlCommand(SelectProductosVendidosMensualQuery, conn)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        lista.Add(New ProductoVentaMensualDTO With {
                            .Producto = reader("Producto").ToString(),
                            .Año = Convert.ToInt32(reader("Año")),
                            .Mes = Convert.ToInt32(reader("Mes")),
                            .CantidadVendida = Convert.ToInt32(reader("CantidadVendida"))
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function
End Class
