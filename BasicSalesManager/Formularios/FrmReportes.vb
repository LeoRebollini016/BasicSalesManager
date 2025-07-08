Imports TacticaCRM.AccesoDatos
Imports TacticaCRM.Entidades

Public Class FrmReportes
    Inherits FrmBase

    Private WithEvents BtnReporteVentas As Button
    Private WithEvents BtnReporteProductosMensual As Button
    Private SaveDialog As SaveFileDialog

    Public Sub New()
        MyBase.New()
        LblTitulo.Text = "Reportes"
        InicializarControles()
    End Sub

    Private Sub InicializarControles()
        BtnAgregar.Visible = False
        BtnModificar.Visible = False
        BtnEliminar.Visible = False
        BtnBuscar.Visible = False
        DgvDatos.Visible = False

        BtnReporteVentas = New Button With {.Text = "Reporte de Ventas", .Width = 180}
        BtnReporteProductosMensual = New Button With {.Text = "Reporte Productos Mensual", .Width = 220}
        SaveDialog = New SaveFileDialog With {
            .Filter = "CSV Files (*.csv)|*.csv",
            .Title = "Guardar Reporte"
        }

        PanelBotones.Controls.Add(BtnReporteVentas)
        PanelBotones.Controls.Add(BtnReporteProductosMensual)
    End Sub

    Private Sub BtnReporteVentas_Click(sender As Object, e As EventArgs) Handles BtnReporteVentas.Click
        Dim buscador As New FrmBuscador()
        buscador.TipoSeleccionado = "Ventas"
        buscador.TipoEditable = False

        If buscador.ShowDialog() = DialogResult.OK Then
            Dim ventaSeleccionada = TryCast(buscador.ItemSeleccionado, VentaConDetalleDTO)
            If ventaSeleccionada Is Nothing Then
                MessageBox.Show("No se seleccionó ninguna venta.")
                Return
            End If

            If SaveDialog.ShowDialog() = DialogResult.OK Then
                Dim reporteVenta As New VentaReportesDTO With {
                .Cliente = New ClienteDTO With {.Nombre = ventaSeleccionada.Cliente},
                .Fecha = ventaSeleccionada.Fecha,
                .Total = ventaSeleccionada.Total,
                .Detalles = ventaSeleccionada.Detalles
            }
                ReporteHelper.GenerarReporteVenta(reporteVenta, SaveDialog.FileName)
                MessageBox.Show("Reporte de venta generado correctamente.")
            End If
        End If
    End Sub

    Private Sub BtnReporteProductosMensual_Click(sender As Object, e As EventArgs) Handles BtnReporteProductosMensual.Click
        Dim productosMensual = VentaDAL.ObtenerProductosVendidosMensual()
        If productosMensual.Count = 0 Then
            MessageBox.Show("No hay datos de productos vendidos mensualmente.")
            Return
        End If

        If SaveDialog.ShowDialog() = DialogResult.OK Then
            ReporteHelper.GenerarReporteProductosMensual(productosMensual, SaveDialog.FileName)
            MessageBox.Show("Reporte mensual de productos generado correctamente.")
        End If
    End Sub
End Class