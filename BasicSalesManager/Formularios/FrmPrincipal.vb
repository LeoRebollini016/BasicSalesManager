Public Class FrmPrincipal
    Private Sub FormularioPrincipal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Size = New Size(1024, 728)
        PanelContenedor.Dock = DockStyle.Fill
        Dim formClientes As New FrmCliente()
        AbrirFormularioEnPanel(formClientes)
    End Sub
    Private Sub ClientesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClientesToolStripMenuItem.Click
        Dim formClientes As New FrmCliente()
        AbrirFormularioEnPanel(formClientes)
    End Sub
    Private Sub ProductosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProductosToolStripMenuItem.Click
        Dim formProductos As New FrmProducto()
        AbrirFormularioEnPanel(formProductos)
    End Sub
    Private Sub VentasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VentasToolStripMenuItem.Click
        Dim formVentas As New FrmVentas()
        AbrirFormularioEnPanel(formVentas)
    End Sub
    Private Sub ReportesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportesToolStripMenuItem.Click
        Dim formReportes As New FrmReportes()
        AbrirFormularioEnPanel(formReportes)
    End Sub
    Private Sub AbrirFormularioEnPanel(formulario As Form)
        PanelContenedor.Controls.Clear()

        formulario.TopLevel = False
        formulario.FormBorderStyle = FormBorderStyle.None
        formulario.Dock = DockStyle.Fill

        PanelContenedor.Controls.Add(formulario)
        formulario.Show()
    End Sub
    Private Sub SalirToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalirToolStripMenuItem.Click
        Me.Close()
    End Sub
End Class