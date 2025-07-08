Imports System.Data.SqlClient
Imports System.Configuration
Imports TacticaCRM.Entidades
Public Class ClienteDAL
    Public Function ObtenerTodos() As List(Of ClienteDTO)
        Dim lista As New List(Of Cliente)

        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            Using comando As New SqlCommand(ObtenerClientesQuery, conn)
                conn.Open()
                Using lector As SqlDataReader = comando.ExecuteReader()
                    While lector.Read()
                        lista.Add(New Cliente With {
                            .ID = Convert.ToInt32(lector("ID")),
                            .Nombre = lector("cliente").ToString(),
                            .Telefono = lector("telefono").ToString(),
                            .Correo = lector("correo").ToString()
                        })
                    End While
                End Using
            End Using
        End Using

        Return lista.Select(Function(c) ClienteAClienteDTO(c)).ToList()
    End Function

    Public Shared Function ObtenerClientePorID(id As Integer) As ClienteDTO
        Dim cliente As Cliente = Nothing
        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            Using comando As New SqlCommand(ObtenerClientePorIDQuery, conn)
                comando.Parameters.AddWithValue("@id", id)
                conn.Open()
                Using lector As SqlDataReader = comando.ExecuteReader()
                    If lector.Read() Then
                        cliente = New Cliente With {
                            .ID = id,
                            .Nombre = lector("Cliente").ToString(),
                            .Telefono = lector("Telefono").ToString(),
                            .Correo = lector("Correo").ToString()
                        }
                    End If
                End Using
            End Using
        End Using
        Return If(cliente IsNot Nothing, ClienteAClienteDTO(cliente), Nothing)
    End Function
    Public Function Insertar(cliente As Cliente) As Boolean
        Dim resultado As Boolean = False

        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()

            Using comando As New SqlCommand(InsertarClienteQuery, conn)
                comando.Parameters.AddWithValue("@Cliente", cliente.Nombre)
                comando.Parameters.AddWithValue("@Telefono", cliente.Telefono)
                comando.Parameters.AddWithValue("@Correo", cliente.Correo)

                Try
                    conn.Open()
                    resultado = comando.ExecuteNonQuery() > 0
                Catch ex As Exception
                    Throw New Exception("Error al insertar cliente: " & ex.Message)
                End Try

            End Using
        End Using
        Return resultado
    End Function


    Public Function Modificar(cliente As Cliente) As Boolean
        Dim resultado As Boolean = False

        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()

            Using comando As New SqlCommand(ActualizarClienteQuery, conn)
                comando.Parameters.AddWithValue("@Cliente", cliente.Nombre)
                comando.Parameters.AddWithValue("@Telefono", cliente.Telefono)
                comando.Parameters.AddWithValue("@Correo", cliente.Correo)
                comando.Parameters.AddWithValue("@ID", cliente.ID)
                Try
                    conn.Open()
                    resultado = comando.ExecuteNonQuery() > 0
                Catch ex As Exception
                    Throw New Exception("Error al modificar cliente: " & ex.Message)
                End Try
            End Using
        End Using

        Return resultado
    End Function

    Public Function Eliminar(id As Integer) As Boolean
        Dim resultado As Boolean = False

        Using conn As SqlConnection = ConexionDAL.ObtenerConexion()
            Using comando As New SqlCommand(EliminarClienteQuery, conn)
                comando.Parameters.AddWithValue("@ID", id)
                Try
                    conn.Open()
                    resultado = comando.ExecuteNonQuery() > 0
                Catch ex As Exception
                    Throw New Exception("Error al eliminar cliente: " & ex.Message)
                End Try
            End Using
        End Using

        Return resultado
    End Function
End Class
