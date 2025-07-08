Imports TacticaCRM.AccesoDatos
Imports TacticaCRM.Entidades

Public Class FrmVentas
    Inherits FrmBase

    Private clienteSeleccionado As ClienteDTO
    Private ventaSeleccionada As VentaConDetalleDTO
    Private totalGeneralDecimal As Decimal

    Private WithEvents DgvDetalleProductos As DataGridView
    Private WithEvents BtnBuscarCliente As Button
    Private WithEvents BtnAgregarProducto As Button
    Private WithEvents LblCliente As Label
    Private WithEvents LblTotalGeneral As Label
    Private WithEvents DtpFechaVenta As DateTimePicker
    Private WithEvents BtnLimpiarCarrito As Button
    Public Sub New()
        MyBase.New()
        LblTitulo.Text = "Gestión de Ventas"
        InicializarControles()
    End Sub
    Private Sub FrmVentas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarVentas()
    End Sub

    Private Sub InicializarControles()
        LblCliente = New Label With {.Text = "Cliente", .Font = New Font("Segoe UI", 12, FontStyle.Bold), .AutoSize = True}
        BtnBuscarCliente = New Button With {.Text = "Buscar Cliente", .AutoSize = True}
        BtnAgregarProducto = New Button With {.Text = "Agregar Producto", .AutoSize = True}
        BtnLimpiarCarrito = New Button With {.Text = "Limpiar Carrito", .AutoSize = True}
        DtpFechaVenta = New DateTimePicker With {.Format = DateTimePickerFormat.Short, .Width = 150}

        AddHandler BtnAgregarProducto.Click, AddressOf BtnAgregarProducto_Click

        Dim panelSuperior As New FlowLayoutPanel With {
        .Dock = DockStyle.Top,
        .AutoSize = True,
        .FlowDirection = FlowDirection.LeftToRight,
        .WrapContents = False,
        .Padding = New Padding(0, 0, 0, 10)
    }
        Dim lblFechaVenta As New Label With {
            .Text = "Fecha de venta: ",
            .AutoSize = True,
            .Font = LblCliente.Font,
            .Margin = New Padding(0, 3, 0, 0)
        }
        panelSuperior.Controls.AddRange({LblCliente, New Label With {.Width = 150}, lblFechaVenta, DtpFechaVenta})

        DgvDetalleProductos = New DataGridView With {
        .Dock = DockStyle.Fill,
        .AllowUserToAddRows = False,
        .[ReadOnly] = True,
        .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        .SelectionMode = DataGridViewSelectionMode.FullRowSelect
    }
        With DgvDetalleProductos.Columns
            .Add("ProductoID", "ID")
            .Add("ProductoNombre", "Producto")
            .Add("Cantidad", "Cantidad")
            .Add("PrecioUnitario", "Precio Unitario")
            .Add("PrecioTotal", "Precio Total")
        End With

        LblTotalGeneral = New Label With {
        .Text = "Total general: ",
        .Font = New Font("Segoe UI", 12, FontStyle.Bold),
        .AutoSize = True,
        .ForeColor = Color.DarkGreen,
        .Padding = New Padding(5)
    }
        Dim panelTotal As New FlowLayoutPanel With {
        .Dock = DockStyle.Bottom,
        .AutoSize = True,
        .FlowDirection = FlowDirection.LeftToRight
    }
        panelTotal.Controls.Add(LblTotalGeneral)

        Dim panelCentral As New Panel With {
        .Dock = DockStyle.Fill,
        .Padding = New Padding(0, 0, 0, 5)
    }
        panelCentral.Controls.Add(DgvDetalleProductos)

        Dim panelMedio As New FlowLayoutPanel With {
        .Dock = DockStyle.Top,
        .AutoSize = True,
        .FlowDirection = FlowDirection.LeftToRight,
        .WrapContents = False,
        .Padding = New Padding(0, 0, 0, 1)
        }
        panelMedio.Controls.Add(BtnBuscarCliente)
        panelMedio.Controls.Add(BtnAgregarProducto)

        PanelCampos.Controls.Clear()
        PanelCampos.Controls.Add(panelSuperior)
        PanelCampos.Controls.Add(panelCentral)
        PanelCampos.Controls.Add(panelMedio)
        PanelCampos.Controls.Add(panelTotal)

        If Not PanelBotones.Controls.Contains(BtnLimpiarCarrito) Then
            PanelBotones.Controls.Add(BtnLimpiarCarrito)
        End If

        AddHandler BtnBuscarCliente.Click, AddressOf BtnBuscarCliente_Click
        AddHandler BtnAgregar.Click, AddressOf BtnAgregar_Click
        AddHandler BtnBuscar.Click, AddressOf BtnBuscar_Click
        AddHandler BtnModificar.Click, AddressOf BtnModificar_Click
        AddHandler BtnEliminar.Click, AddressOf BtnEliminar_Click
        AddHandler BtnLimpiarCarrito.Click, AddressOf BtnLimpiarCarrito_Click
    End Sub

    Private Sub BtnLimpiarCarrito_Click(sender As Object, e As EventArgs)
        DgvDetalleProductos.Rows.Clear()
        totalGeneralDecimal = 0
        LblTotalGeneral.Text = "Total general: "
        clienteSeleccionado = Nothing
        LblCliente.Text = "Cliente"
    End Sub

    Private Sub BtnBuscarCliente_Click(sender As Object, e As EventArgs)
        Dim buscador As New FrmBuscador()
        buscador.TipoSeleccionado = "Clientes"
        buscador.TipoEditable = False

        If buscador.ShowDialog() = DialogResult.OK Then
            clienteSeleccionado = CType(buscador.ItemSeleccionado, ClienteDTO)
            LblCliente.Text = $"Cliente: {clienteSeleccionado.Nombre}"
        End If
    End Sub

    Private Sub BtnAgregarProducto_Click(sender As Object, e As EventArgs)
        Dim buscador As New FrmBuscador()
        buscador.TipoSeleccionado = "Productos"
        buscador.TipoEditable = False

        If buscador.ShowDialog() = DialogResult.OK Then
            Dim productoSeleccionado = CType(buscador.ItemSeleccionado, ProductoDTO)
            Dim cantidadStr = InputBox($"Ingrese la cantidad para '{productoSeleccionado.Nombre}': ", "Cantidad", "1")
            If Integer.TryParse(cantidadStr, Nothing) Then
                Dim cantidad As Integer = Convert.ToInt32(cantidadStr)
                Dim precioTotal = cantidad * productoSeleccionado.Precio
                DgvDetalleProductos.Rows.Add(
                    productoSeleccionado.ID,
                    productoSeleccionado.Nombre,
                    cantidad,
                    productoSeleccionado.Precio,
                    precioTotal
                )
                CalcularTotalGeneral()
            End If
        End If
    End Sub

    Private Sub CargarVentas()
        Try
            DgvDatos.DataSource = VentaDAL.ObtenerVentas()
            DgvDatos.Columns("ID").Visible = False
        Catch ex As Exception
            MessageBox.Show("Error al cargar ventas: " & ex.Message)
        End Try
    End Sub
    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs)
        If Not ValidarVenta() Then Exit Sub

        Dim nuevaVentaDTO As New CreacionVentaDTO With {
            .IDCliente = clienteSeleccionado.ID,
            .Fecha = DtpFechaVenta.Value,
            .Total = totalGeneralDecimal,
            .Detalles = ObtenerDetallesDesdeGrilla()
        }
        Try
            Dim nuevaVenta As Venta = CreacionVentaDTOAVenta(nuevaVentaDTO)
            Dim idGenerado = VentaDAL.Insertar(nuevaVenta)
            MessageBox.Show("Venta registrada correctamente. ID: " & idGenerado.ToString())
            CargarVentas()
        Catch ex As Exception
            MessageBox.Show("Error al registrar venta: " & ex.Message)

        End Try
    End Sub

    Private Sub BtnModificar_Click(sender As Object, e As EventArgs)
        If Not ValidarVenta() Then Exit Sub

        Dim idVenta As Integer = Convert.ToInt32(DgvDatos.CurrentRow.Cells("ID").Value)
        ventaSeleccionada = VentaDAL.ObtenerVentaPorID(idVenta)

        If ventaSeleccionada Is Nothing Then
            MessageBox.Show("No se pudo obtener la venta seleccionada.")
            Exit Sub
        End If

        Dim detalles = ObtenerDetallesDesdeGrilla()
        Dim total = totalGeneralDecimal

        ventaSeleccionada.IDCliente = clienteSeleccionado.ID
        ventaSeleccionada.Fecha = DtpFechaVenta.Value
        ventaSeleccionada.Total = total
        ventaSeleccionada.Detalles = detalles

        Try
            Dim ventaSeleccionadaEntity As Venta = VentasConDetallesDTOAVenta(ventaSeleccionada)
            VentaDAL.Modificar(ventaSeleccionadaEntity)
            MessageBox.Show("Venta modificada correctamente.")
            CargarVentas()
        Catch ex As Exception
            MessageBox.Show("Error al modificar la venta: " & ex.Message)
        End Try
    End Sub
    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs)
        If Not ValidarVenta() Then Exit Sub
        If MessageBox.Show("Está seguro que desea eliminar la venta seleccionada?", "Eliminar venta",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                VentaDAL.Eliminar(ventaSeleccionada.IDVenta)
                MessageBox.Show("Venta eliminada correctamente.")
                CargarVentas()
            Catch ex As Exception
                MessageBox.Show("Error al eliminar la venta: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub BtnBuscar_Click(sender As Object, e As EventArgs)
        Dim buscador As New FrmBuscador()
        buscador.TipoSeleccionado = "Ventas"
        buscador.TipoEditable = False
        If buscador.ShowDialog() = DialogResult.OK Then
            Dim ventaSeleccionada = TryCast(buscador.ItemSeleccionado, VentaConDetalleDTO)
            If ventaSeleccionada IsNot Nothing Then
                Dim frmDetalle As New FrmDetalleVentas(ventaSeleccionada)
                frmDetalle.ShowDialog()
            End If
        End If
    End Sub

    Private Sub CalcularTotalGeneral()
        totalGeneralDecimal = 0
        For Each fila As DataGridViewRow In DgvDetalleProductos.Rows
            totalGeneralDecimal += Convert.ToDecimal(fila.Cells("PrecioTotal").Value)
        Next
        LblTotalGeneral.Text = "Total general: " & totalGeneralDecimal.ToString("C")
    End Sub
    Private Function ObtenerDetallesDesdeGrilla() As List(Of VentaItemDTO)
        Dim detalles As New List(Of VentaItemDTO)()
        For Each fila As DataGridViewRow In DgvDetalleProductos.Rows
            Dim detalle As New VentaItemDTO With {
                .IDProducto = Convert.ToInt32(fila.Cells("ProductoID").Value),
                .Cantidad = Convert.ToInt32(fila.Cells("Cantidad").Value),
                .PrecioUnitario = Convert.ToDecimal(fila.Cells("PrecioUnitario").Value)
            }
            detalles.Add(detalle)
        Next
        Return detalles

    End Function
    Private Function ValidarVenta() As Boolean
        If clienteSeleccionado Is Nothing Then
            MessageBox.Show("Debe seleccionar un cliente.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        If DgvDetalleProductos.Rows.Count = 0 Then
            MessageBox.Show("Debe agregar al menos un producto a la venta.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If
        Return True
    End Function
End Class