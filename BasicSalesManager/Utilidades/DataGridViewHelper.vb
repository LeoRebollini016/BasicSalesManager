Imports TacticaCRM.Entidades

Public Class DataGridViewHelper
    Public Shared Sub SeleccionarFilaPorId(dgv As DataGridView, idBuscado As Integer)
        If dgv Is Nothing OrElse dgv.Rows.Count = 0 Then Exit Sub

        For Each fila As DataGridViewRow In dgv.Rows
            If Convert.ToInt32(fila.Cells("ID").Value) = idBuscado Then
                fila.Selected = True
                dgv.CurrentCell = fila.Cells(0)
                Exit For
            End If
        Next
    End Sub
End Class
