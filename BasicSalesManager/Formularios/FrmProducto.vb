Imports TacticaCRM.Entidades
Imports TacticaCRM.AccesoDatos

Public Class FrmProducto
    Inherits FrmBase

    Private TxtNombre As TextBox
    Private TxtPrecio As TextBox
    Private TxtCategoria As TextBox

    Private _evitarRellenoCampos As Boolean = False

    Public Sub New()
        InicializarControles()
    End Sub

    Private Sub FormularioProducto_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarProductos()
    End Sub

    Private Sub InicializarControles()
        LblTitulo.Text = "Gestión de Productos"

        Dim LblNombre As New Label With {.Text = "Nombre:", .AutoSize = True}
        TxtNombre = New TextBox With {.Width = 200}

        Dim LblPrecio As New Label With {.Text = "Precio:", .AutoSize = True}
        TxtPrecio = New TextBox With {.Width = 200}

        Dim LblCategoria As New Label With {.Text = "Categoria:", .AutoSize = True}
        TxtCategoria = New TextBox With {.Width = 200}

        PanelCampos.Controls.AddRange({LblNombre, TxtNombre, LblPrecio, TxtPrecio, LblCategoria, TxtCategoria})
    End Sub

    Private Sub CargarProductos()
        Dim productoDAL As New ProductoDAL()
        Dim listaProductos As List(Of ProductoDTO) = productoDAL.ObtenerTodos()
        _evitarRellenoCampos = True
        DgvDatos.DataSource = Nothing
        DgvDatos.DataSource = listaProductos
        DgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DgvDatos.ClearSelection()
        _evitarRellenoCampos = False
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles BtnAgregar.Click
        If Not ValidarCampos() Then Exit Sub
        Dim productoDto As New ProductoDTO With {
            .Nombre = TxtNombre.Text.Trim(),
            .Precio = Convert.ToDecimal(TxtPrecio.Text.Trim()),
            .Categoria = TxtCategoria.Text.Trim()
        }
        Dim productoDAL As New ProductoDAL()
        Dim producto As Producto = ProductoDTOAProducto(productoDto)
        productoDAL.Insertar(producto)

        LimpiarFormulario()
        CargarProductos()
    End Sub

    Private Sub BtnModificar_Click(sender As Object, e As EventArgs) Handles BtnModificar.Click
        If DgvDatos.CurrentRow IsNot Nothing Then
            If Not ValidarCampos() Then Exit Sub

            Dim productoDto As New ProductoDTO With {
                .ID = Convert.ToInt32(DgvDatos.CurrentRow.Cells("ID").Value),
                .Nombre = TxtNombre.Text.Trim(),
                .Precio = Convert.ToDecimal(TxtPrecio.Text.Trim()),
                .Categoria = TxtCategoria.Text.Trim()
            }
            Dim producto As Producto = ProductoDTOAProducto(productoDto)
            Dim productoDAL As New ProductoDAL()
            productoDAL.Actualizar(producto)

            LimpiarFormulario()
            CargarProductos()
        Else
            MessageBox.Show("Seleccione un producto para modificar.")
        End If
    End Sub

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If DgvDatos.CurrentRow IsNot Nothing Then
            Dim idProducto As Integer = Convert.ToInt32(DgvDatos.CurrentRow.Cells("ID").Value)

            Dim productoDAL As New ProductoDAL()
            productoDAL.Eliminar(idProducto)

            LimpiarFormulario()
            CargarProductos()
        Else
            MessageBox.Show("Seleccione un producto para eliminar.")
        End If
    End Sub

    Private Sub DgvDatos_SelectionChanged(sender As Object, e As EventArgs) Handles DgvDatos.SelectionChanged
        If _evitarRellenoCampos Then Exit Sub
        If DgvDatos.CurrentRow IsNot Nothing AndAlso DgvDatos.SelectedRows.Count > 0 Then
            TxtNombre.Text = DgvDatos.CurrentRow.Cells("Nombre").Value.ToString()
            TxtPrecio.Text = DgvDatos.CurrentRow.Cells("Precio").Value.ToString()
            TxtCategoria.Text = DgvDatos.CurrentRow.Cells("Categoria").Value.ToString()
        End If
    End Sub

    Private Function ValidarCampos() As Boolean
        Dim campos As New Dictionary(Of String, TextBox) From {
            {"Nombre", TxtNombre},
            {"Precio", TxtPrecio},
            {"Categoria", TxtCategoria}
        }
        Return ValidadorHelper.ValidarCamposObligatorios(campos)
    End Function

    Private Sub LimpiarFormulario()
        TxtNombre.Clear()
        TxtPrecio.Clear()
        TxtCategoria.Clear()
        _evitarRellenoCampos = True
        DgvDatos.ClearSelection()
        _evitarRellenoCampos = False
    End Sub
    Private Sub BtnBuscarProducto_Click(sender As Object, e As EventArgs) Handles BtnBuscar.Click
        Dim buscador As New FrmBuscador()
        buscador.TipoSeleccionado = "Productos"
        buscador.TipoEditable = False
        If buscador.ShowDialog() = DialogResult.OK Then
            Dim producto As ProductoDTO = CType(buscador.ItemSeleccionado, ProductoDTO)
            DataGridViewHelper.SeleccionarFilaPorId(DgvDatos, producto.ID)
            TxtNombre.Text = producto.Nombre
            TxtPrecio.Text = producto.Precio.ToString()
            TxtCategoria.Text = producto.Categoria
        End If
    End Sub
End Class