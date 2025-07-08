Module ClienteQuery
    Public Const ObtenerClientesQuery As String =
        "SELECT ID, cliente, telefono, correo 
        FROM clientes"

    Public Const ObtenerClientePorIDQuery As String =
        "SELECT Cliente, Telefono, Correo 
        FROM clientes 
        WHERE ID = @id"

    Public Const InsertarClienteQuery As String =
        "INSERT INTO clientes (cliente, telefono, correo) 
        VALUES (@Cliente, @Telefono, @Correo)"

    Public Const ActualizarClienteQuery As String =
        "UPDATE clientes 
        SET Cliente = @Cliente, Telefono = @Telefono, Correo = @Correo 
        WHERE ID = @ID"

    Public Const EliminarClienteQuery As String =
        "DELETE FROM clientes 
        WHERE ID = @ID"
End Module
