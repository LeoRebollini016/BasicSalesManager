Imports System.Configuration
Imports System.Data.SqlClient

Public Class ConexionDAL
    Public Shared Function ObtenerConexion() As SqlConnection
        Dim cadena As String = ConfigurationManager.ConnectionStrings("CadenaConexion").ConnectionString
        Return New SqlConnection(cadena)
    End Function
End Class
