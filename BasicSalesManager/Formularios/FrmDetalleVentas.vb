Imports TacticaCRM.AccesoDatos
Imports TacticaCRM.Entidades

Public Class FrmDetalleVentas
    Inherits FrmBase

    Private ReadOnly _venta As VentaConDetalleDTO
    Private BtnGenerarReporte As Button
    Public Sub New(venta As VentaConDetalleDTO)
        MyBase.New()
        _venta = venta
        Me.FormBorderStyle = FormBorderStyle.Sizable
        InicializarDetalle()
        LblTitulo.Text = $"Detalle de Venta #{_venta.IDVenta}"
    End Sub

    Private Sub InicializarDetalle()
        PanelCampos.Controls.Clear()
        Dim lblCliente As New Label With {
            .Text = $"Cliente: {_venta.Cliente}",
            .Font = New Font("Segoe UI", 12, FontStyle.Regular),
            .AutoSize = True
        }
        Dim lblFecha As New Label With {
            .Text = $"Fecha: {_venta.Fecha:d}",
            .Font = New Font("Segoe UI", 12, FontStyle.Regular),
            .AutoSize = True
        }
        Dim lblTotal As New Label With {
            .Text = $"Total: {_venta.Total:C}",
            .Font = New Font("Segoe UI", 12, FontStyle.Bold),
            .AutoSize = True
        }

        PanelCampos.Controls.Add(lblCliente)
        PanelCampos.Controls.Add(lblFecha)
        PanelCampos.Controls.Add(lblTotal)

        DgvDatos.DataSource = Nothing
        DgvDatos.DataSource = _venta.Detalles

        BtnAgregar.Visible = False
        BtnModificar.Visible = False
        BtnEliminar.Visible = False

        BtnGenerarReporte = New Button With {
            .Text = "Generar Reporte",
            .Font = New Font("Segoe UI", 12, FontStyle.Regular),
            .Width = 150
        }
        AddHandler BtnGenerarReporte.Click, AddressOf BtnGenerarReporte_Click

        PanelBotones.Controls.Add(BtnGenerarReporte)
        BtnBuscar.Text = "Cerrar"
        AddHandler BtnBuscar.Click, AddressOf BtnCerrar_Click
    End Sub

    Private Sub BtnGenerarReporte_Click(sender As Object, e As EventArgs)
        If _venta Is Nothing Then
            MessageBox.Show("No hay datos de venta para generar el reporte.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim cliente As ClienteDTO = ClienteDAL.ObtenerClientePorID(_venta.IDCliente)
        If cliente Is Nothing Then
            MessageBox.Show("El cliente no fue encontrado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            Dim path As String = $"Reporte_Venta_{_venta.IDVenta}.csv"
            ReporteHelper.GenerarReporteVenta(New VentaReportesDTO With {
                .Cliente = cliente,
                .Fecha = _venta.Fecha,
                .Total = _venta.Total,
                .Detalles = _venta.Detalles
            }, path)
            MessageBox.Show($"Reporte generado exitosamente en: {path}", "Reporte Generado", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
End Class