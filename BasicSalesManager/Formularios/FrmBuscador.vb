Imports TacticaCRM.AccesoDatos

Public Class FrmBuscador
    Public Property TipoSeleccionado As String
    Public Property TipoEditable As Boolean = True
    Public Property ItemSeleccionado As Object

    Private WithEvents CmbCategorias As ComboBox
    Private WithEvents PnlLayout As TableLayoutPanel
    Private WithEvents PnlFiltros As FlowLayoutPanel
    Private WithEvents PnlBotones As New FlowLayoutPanel

    Private Sub FormularioBuscador_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Controls.Add(CmbCategorias)
        IniciarlizarControles()

        cmbTipoBusqueda.Items.AddRange({"Clientes", "Productos", "Ventas"})
        cmbTipoBusqueda.SelectedItem = TipoSeleccionado
        cmbTipoBusqueda.Enabled = TipoEditable

        If TipoSeleccionado = "Productos" Then
            CmbCategorias.Visible = True
            CargarCategoriasProductos()
        End If

        CargarDatos()
    End Sub

    Private Sub IniciarlizarControles()
        PnlLayout = New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .RowCount = 3,
            .ColumnCount = 1,
            .AutoSize = True
        }
        PnlLayout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        PnlLayout.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        PnlLayout.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        PnlFiltros = New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .AutoSize = True,
            .FlowDirection = FlowDirection.LeftToRight
        }
        CmbCategorias = New ComboBox With {
            .Width = 150,
            .DropDownStyle = ComboBoxStyle.DropDownList,
            .Visible = False
        }
        CmbCategorias.Location = New Point(txtFiltro.Left + txtFiltro.Width + 10, txtFiltro.Top)

        PnlFiltros.Controls.Add(cmbTipoBusqueda)
        PnlFiltros.Controls.Add(txtFiltro)
        PnlFiltros.Controls.Add(CmbCategorias)
        PnlLayout.Controls.Add(PnlFiltros, 0, 0)

        PnlBotones = New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .AutoSize = True,
            .FlowDirection = FlowDirection.RightToLeft
        }
        PnlBotones.Controls.Add(btnCancelar)
        PnlBotones.Controls.Add(btnSeleccionar)

        PnlLayout.Controls.Add(PnlBotones, 0, 1)

        dgvResultados.Dock = DockStyle.Fill
        dgvResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        PnlLayout.Controls.Add(dgvResultados, 0, 2)

        Me.Controls.Add(PnlLayout)
    End Sub

    Private Sub cmbTipoBusqueda_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbTipoBusqueda.SelectedIndexChanged
        TipoSeleccionado = cmbTipoBusqueda.SelectedItem.ToString()
        CargarDatos()
    End Sub

    Private Sub CmbCategorias_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbCategorias.SelectedIndexChanged
        CargarDatos()
    End Sub

    Private Sub TxtFiltro_TextChanged(sender As Object, e As EventArgs) Handles txtFiltro.TextChanged
        CargarDatos()
    End Sub

    Private Sub CargarDatos()
        Dim filtro As String = txtFiltro.Text.Trim().ToLower()
        Dim categoriaFiltro As String = If(CmbCategorias.Visible, CmbCategorias.SelectedItem?.ToString(), "")

        Select Case TipoSeleccionado
            Case "Clientes"
                dgvResultados.DataSource = BuscadorHelper.BuscarClientes(filtro)
            Case "Productos"
                dgvResultados.DataSource = BuscadorHelper.BuscarProductos(filtro, categoriaFiltro)
            Case "Ventas"
                dgvResultados.DataSource = BuscadorHelper.BuscarVentas(filtro)
        End Select
    End Sub

    Private Sub CargarCategoriasProductos()
        Dim categorias = New ProductoDAL().ObtenerCategorias()
        categorias.Insert(0, "Todas")
        CmbCategorias.DataSource = categorias
        CmbCategorias.SelectedIndex = 0
    End Sub

    Private Sub BtnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        If dgvResultados.CurrentRow IsNot Nothing Then
            ItemSeleccionado = dgvResultados.CurrentRow.DataBoundItem
            Me.DialogResult = DialogResult.OK
            Me.Close()
            dgvResultados.ClearSelection()
        End If
    End Sub
    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class