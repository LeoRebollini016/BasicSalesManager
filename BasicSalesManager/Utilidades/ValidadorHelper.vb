Public Module ValidadorHelper
    Public Function ValidarCamposObligatorios(controles As Dictionary(Of String, TextBox)) As Boolean
        Dim errores As String = ""

        For Each kvp In controles
            Dim nombreCampo As String = kvp.Key
            Dim textBox As TextBox = kvp.Value

            If String.IsNullOrWhiteSpace(textBox.Text) Then
                errores &= $"El campo '{nombreCampo}' es obligatorio." & Environment.NewLine
            End If
        Next

        If errores <> "" Then
            MessageBox.Show("Se encontraron los siguientes errores: " & Environment.NewLine & Environment.NewLine & errores,
                                "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function
End Module
