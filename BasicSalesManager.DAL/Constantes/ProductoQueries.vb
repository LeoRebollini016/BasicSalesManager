Module ProductoQueries

    Public Const ObtenerProductosQuery As String =
        "SELECT ID, Nombre, Precio, Categoria 
        FROM productos"

    Public Const InsertarProductoQuery As String =
        "INSERT INTO productos (Nombre, Precio, Categoria) 
        VALUES (@nombre, @precio, @categoria)"

    Public Const ActualizarProductoQuery As String =
        "UPDATE productos 
        SET Nombre = @Nombre, Precio = @Precio, Categoria = @Categoria 
        WHERE ID = @ID"

    Public Const EliminarProductoQuery As String =
        "DELETE 
        FROM productos 
        WHERE ID = @id"

    Public Const ObtenerCategoriasQuery As String =
        "SELECT DISTINCT Categoria 
        FROM productos"
End Module
