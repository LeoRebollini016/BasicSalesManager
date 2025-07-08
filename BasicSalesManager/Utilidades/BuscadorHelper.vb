Imports TacticaCRM.AccesoDatos
Imports TacticaCRM.Entidades

Public Class BuscadorHelper

    Public Shared Function BuscarClientes(filtro As String) As List(Of ClienteDTO)
        Dim dal As New ClienteDAL()
        Return dal.ObtenerTodos().
            Where(Function(c) c.Nombre.ToLower().Contains(filtro.ToLower())).
            ToList()
    End Function

    Public Shared Function BuscarProductos(filtro As String, categoriaFiltro As String) As List(Of ProductoDTO)
        Dim dal As New ProductoDAL()
        Return dal.ObtenerTodos().
            Where(Function(p) p.Nombre.ToLower().Contains(filtro.ToLower()) AndAlso
                (categoriaFiltro = "Todas" OrElse p.Categoria = categoriaFiltro)
            ).ToList()
    End Function

    Public Shared Function BuscarVentas(filtro As String) As List(Of VentaConDetalleDTO)
        Dim dal As New VentaDAL()
        Return dal.ObtenerVentaDetallesConFiltro(filtro).Cast(Of VentaConDetalleDTO)().ToList()
    End Function
End Class
