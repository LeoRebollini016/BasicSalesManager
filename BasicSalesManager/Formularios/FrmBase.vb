Public Class FrmBase
    Inherits Form

    Protected WithEvents LblTitulo As Label
    Protected WithEvents PanelCampos As FlowLayoutPanel
    Protected WithEvents PanelBotones As FlowLayoutPanel
    Protected WithEvents DgvDatos As DataGridView
    Protected WithEvents BtnAgregar As Button
    Protected WithEvents BtnModificar As Button
    Protected WithEvents BtnEliminar As Button
    Protected WithEvents BtnBuscar As Button

    Public Sub New()
        InitializeComponent()
        InicializarControles()
    End Sub

    Private Sub InicializarControles()
        Me.FormBorderStyle = FormBorderStyle.Sizable
        Me.MinimumSize = New Size(800, 600)
        Me.BackColor = Color.White
        Me.Padding = New Padding(20)

        Dim layoutPrincipal As New TableLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .RowCount = 4,
            .ColumnCount = 1
        }
        layoutPrincipal.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        layoutPrincipal.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        layoutPrincipal.RowStyles.Add(New RowStyle(SizeType.AutoSize))
        layoutPrincipal.RowStyles.Add(New RowStyle(SizeType.Percent, 100))

        LblTitulo = New Label With {
            .Text = "Formulario",
            .Font = New Font("Segoe UI", 16, FontStyle.Bold),
            .AutoSize = True,
            .Dock = DockStyle.Fill,
            .Padding = New Padding(10)
        }
        layoutPrincipal.Controls.Add(LblTitulo)

        PanelCampos = New FlowLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .AutoSize = True,
            .Padding = New Padding(10),
            .FlowDirection = FlowDirection.TopDown,
            .WrapContents = False,
            .AutoScroll = True
        }
        layoutPrincipal.Controls.Add(PanelCampos)

        PanelBotones = New FlowLayoutPanel() With {
            .Dock = DockStyle.Fill,
            .AutoSize = True,
            .Padding = New Padding(10),
            .FlowDirection = FlowDirection.LeftToRight
        }
        BtnAgregar = New Button With {.Text = "Agregar"}
        BtnModificar = New Button With {.Text = "Modificar"}
        BtnEliminar = New Button With {.Text = "Eliminar"}
        BtnBuscar = New Button With {.Text = "Buscar"}
        PanelBotones.Controls.AddRange({BtnAgregar, BtnModificar, BtnEliminar, BtnBuscar})
        layoutPrincipal.Controls.Add(PanelBotones)

        DgvDatos = New DataGridView With {
            .Dock = DockStyle.Fill,
            .[ReadOnly] = True,
            .AllowUserToAddRows = False,
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
        }
        layoutPrincipal.Controls.Add(DgvDatos)

        Me.Controls.Add(layoutPrincipal)
    End Sub
End Class