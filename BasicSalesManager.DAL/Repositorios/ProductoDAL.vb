Imports System.Configuration
Imports System.Data.SqlClient
Imports TacticaCRM.Entidades
Public Class ProductoDAL

    Public Function ObtenerTodos() As List(Of ProductoDTO)
        Dim lista As New List(Of Producto)
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            Using comando As New SqlCommand(ObtenerProductosQuery, conn)
                conn.Open()
                Using lector As SqlDataReader = comando.ExecuteReader()
                    While lector.Read()
                        lista.Add(New Producto With {
                            .ID = lector("ID").ToString(),
                            .Nombre = lector("Nombre").ToString(),
                            .Precio = lector("Precio").ToString(),
                            .Categoria = lector("Categoria").ToString()
                        })
                    End While
                End Using
            End Using
        End Using
        Return lista.Select(Function(p) ProductoAProductoDTO(p)).ToList()
    End Function

    Public Function Insertar(producto As Producto) As Boolean
        Dim resultado As Boolean = False
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            Using comando As New SqlCommand(InsertarProductoQuery, conn)
                comando.Parameters.AddWithValue("@nombre", producto.Nombre)
                comando.Parameters.AddWithValue("@precio", producto.Precio)
                comando.Parameters.AddWithValue("@categoria", producto.Categoria)

                Try
                    conn.Open()
                    resultado = comando.ExecuteNonQuery() > 0
                Catch ex As Exception
                    Throw New Exception("Error al insertar producto: " & ex.Message)
                End Try
            End Using
        End Using
        Return resultado
    End Function

    Public Function Actualizar(producto As Producto) As Boolean
        Dim resultado As Boolean = False
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            Using comando As New SqlCommand(ActualizarProductoQuery, conn)
                comando.Parameters.AddWithValue("@ID", producto.ID)
                comando.Parameters.AddWithValue("@Nombre", producto.Nombre)
                comando.Parameters.AddWithValue("@Precio", producto.Precio)
                comando.Parameters.AddWithValue("@Categoria", producto.Categoria)

                Try
                    conn.Open()
                    resultado = comando.ExecuteNonQuery() > 0
                Catch ex As Exception
                    Throw New Exception("Error al actualizar producto: " & ex.Message)
                End Try
            End Using
        End Using
        Return resultado
    End Function

    Public Function Eliminar(id As Integer) As Boolean
        Dim resultado As Boolean = False
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            Using cmd As New SqlCommand(EliminarProductoQuery, conn)
                cmd.Parameters.AddWithValue("@id", id)
                Try
                    conn.Open()
                    resultado = cmd.ExecuteNonQuery() > 0
                Catch ex As Exception
                    Throw New Exception("Error al eliminar producto: " & ex.Message)
                End Try
            End Using
        End Using
        Return resultado
    End Function

    Public Function ObtenerCategorias() As List(Of String)
        Dim categorias As New List(Of String)
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            conn.Open()
            Using cmd As New SqlCommand(ObtenerCategoriasQuery, conn)
                Using lector As SqlDataReader = cmd.ExecuteReader()
                    While lector.Read()
                        categorias.Add(lector(0).ToString())
                    End While
                End Using
            End Using
        End Using

        Return categorias
    End Function


End Class
