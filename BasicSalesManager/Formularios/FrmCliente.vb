Imports TacticaCRM.AccesoDatos
Imports TacticaCRM.Entidades

Public Class FrmCliente
    Inherits FrmBase

    Private TxtNombre As TextBox
    Private TxtTelefono As TextBox
    Private TxtCorreo As TextBox

    Public Sub New()
    End Sub

    Private Sub FormularioCliente_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InicializarControles()
        CargarClientes()
    End Sub
    Private Sub InicializarControles()
        LblTitulo.Text = "Gestión de Clientes"

        Dim LblNombre As New Label()
        LblNombre.Text = "Nombre:"
        LblNombre.AutoSize = True
        TxtNombre = New TextBox()
        TxtNombre.Width = 200

        Dim LblTelefono As New Label()
        LblTelefono.Text = "Telefono:"
        LblTelefono.AutoSize = True
        TxtTelefono = New TextBox()
        TxtTelefono.Width = 200

        Dim LblCorreo As New Label()
        LblCorreo.Text = "Correo:"
        LblCorreo.AutoSize = True
        TxtCorreo = New TextBox()
        TxtCorreo.Width = 200

        PanelCampos.Controls.AddRange({LblNombre, TxtNombre, LblTelefono, TxtTelefono, LblCorreo, TxtCorreo})
    End Sub
    Private Sub CargarClientes()
        Dim clienteDAL As New ClienteDAL()
        Dim listaClientes As List(Of ClienteDTO) = clienteDAL.ObtenerTodos()
        DgvDatos.DataSource = Nothing
        DgvDatos.DataSource = listaClientes
        DgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub
    Private Sub DgvClientes_SelectionChanged(sender As Object, e As EventArgs) Handles DgvDatos.SelectionChanged
        If DgvDatos.CurrentRow IsNot Nothing Then
            TxtNombre.Text = DgvDatos.CurrentRow.Cells("Nombre").Value.ToString()
            TxtTelefono.Text = DgvDatos.CurrentRow.Cells("Telefono").Value.ToString()
            TxtCorreo.Text = DgvDatos.CurrentRow.Cells("Correo").Value.ToString()
        End If
    End Sub
    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles BtnAgregar.Click
        If Not ValidarCampos() Then Exit Sub
        Dim clienteDto As New ClienteDTO With {
            .Nombre = TxtNombre.Text.Trim(),
            .Telefono = TxtTelefono.Text.Trim(),
            .Correo = TxtCorreo.Text.Trim()
        }
        Dim clienteDAL As New ClienteDAL()
        Dim cliente As Cliente = ClienteDTOACliente(clienteDto)
        clienteDAL.Insertar(cliente)
        LimpiarFormulario()
        CargarClientes()
    End Sub

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If DgvDatos.CurrentRow IsNot Nothing Then
            Dim idCliente As Integer = Convert.ToInt32(DgvDatos.CurrentRow.Cells("ID").Value)
            Dim clienteDAL As New ClienteDAL()
            Dim resultado As Boolean = clienteDAL.Eliminar(idCliente)

            If resultado Then
                MessageBox.Show("Cliente eliminado correctamente.")
                CargarClientes()
                LimpiarFormulario()
            Else
                MessageBox.Show("No se pudo eliminar el cliente.")
            End If
        Else
            MessageBox.Show("Seleccione un cliente para eliminar.")
        End If
    End Sub
    Private Sub BtnModificar_Click(sender As Object, e As EventArgs) Handles BtnModificar.Click
        If DgvDatos.CurrentRow IsNot Nothing Then
            If Not ValidarCampos() Then Exit Sub

            Dim clienteDto As New ClienteDTO With {
                .ID = Convert.ToInt32(DgvDatos.CurrentRow.Cells("ID").Value),
                .Nombre = TxtNombre.Text.Trim(),
                .Telefono = TxtTelefono.Text.Trim(),
                .Correo = TxtCorreo.Text.Trim()
            }
            Dim clienteDAL As New ClienteDAL()
            Dim cliente As Cliente = ClienteDTOACliente(clienteDto)
            Dim resultado As Boolean = clienteDAL.Modificar(cliente)

            If resultado Then
                MessageBox.Show("Cliente modificado correctamente.")
                CargarClientes()
                LimpiarFormulario()
            Else
                MessageBox.Show("No se pudo modificar el cliente.")
            End If
        Else
            MessageBox.Show("Seleccione un cliente para modificar.")
        End If
    End Sub
    Private Function ValidarCampos() As Boolean
        Dim campos As New Dictionary(Of String, TextBox) From {
            {"Nombre", TxtNombre},
            {"Telefono", TxtTelefono},
            {"Correo", TxtCorreo}
        }
        Return ValidadorHelper.ValidarCamposObligatorios(campos)
    End Function

    Private Sub BtnBuscarProducto_Click(sender As Object, e As EventArgs) Handles BtnBuscar.Click
        Dim buscador As New FrmBuscador()
        buscador.TipoSeleccionado = "Clientes"
        buscador.TipoEditable = False
        If buscador.ShowDialog() = DialogResult.OK Then
            Dim cliente As ClienteDTO = CType(buscador.ItemSeleccionado, ClienteDTO)
            DataGridViewHelper.SeleccionarFilaPorId(DgvDatos, cliente.ID)
            TxtNombre.Text = cliente.Nombre
            TxtTelefono.Text = cliente.Telefono.ToString()
            TxtCorreo.Text = cliente.Correo
        End If
    End Sub

    Private Sub LimpiarFormulario()
        TxtNombre.Text = ""
        TxtTelefono.Text = ""
        TxtCorreo.Text = ""
        DgvDatos.ClearSelection()
    End Sub
End Class