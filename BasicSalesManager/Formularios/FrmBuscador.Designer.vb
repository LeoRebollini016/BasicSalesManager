<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmBuscador
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.cmbTipoBusqueda = New System.Windows.Forms.ComboBox()
        Me.dgvResultados = New System.Windows.Forms.DataGridView()
        Me.txtFiltro = New System.Windows.Forms.TextBox()
        Me.btnSeleccionar = New System.Windows.Forms.Button()
        Me.btnCancelar = New System.Windows.Forms.Button()
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmbTipoBusqueda
        '
        Me.cmbTipoBusqueda.FormattingEnabled = True
        Me.cmbTipoBusqueda.Items.AddRange(New Object() {"Clientes", "Productos", "Ventas"})
        Me.cmbTipoBusqueda.Location = New System.Drawing.Point(12, 12)
        Me.cmbTipoBusqueda.Name = "cmbTipoBusqueda"
        Me.cmbTipoBusqueda.Size = New System.Drawing.Size(121, 21)
        Me.cmbTipoBusqueda.TabIndex = 0
        '
        'dgvResultados
        '
        Me.dgvResultados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResultados.Location = New System.Drawing.Point(12, 132)
        Me.dgvResultados.Name = "dgvResultados"
        Me.dgvResultados.Size = New System.Drawing.Size(776, 306)
        Me.dgvResultados.TabIndex = 2
        '
        'txtFiltro
        '
        Me.txtFiltro.Location = New System.Drawing.Point(223, 12)
        Me.txtFiltro.Name = "txtFiltro"
        Me.txtFiltro.Size = New System.Drawing.Size(100, 20)
        Me.txtFiltro.TabIndex = 3
        '
        'btnSeleccionar
        '
        Me.btnSeleccionar.Location = New System.Drawing.Point(414, 9)
        Me.btnSeleccionar.Name = "btnSeleccionar"
        Me.btnSeleccionar.Size = New System.Drawing.Size(75, 23)
        Me.btnSeleccionar.TabIndex = 4
        Me.btnSeleccionar.Text = "Seleccionar"
        Me.btnSeleccionar.UseVisualStyleBackColor = True
        '
        'btnCancelar
        '
        Me.btnCancelar.Location = New System.Drawing.Point(414, 38)
        Me.btnCancelar.Name = "btnCancelar"
        Me.btnCancelar.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelar.TabIndex = 5
        Me.btnCancelar.Text = "Cancelar"
        Me.btnCancelar.UseVisualStyleBackColor = True
        '
        'FormularioBuscador
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnCancelar)
        Me.Controls.Add(Me.btnSeleccionar)
        Me.Controls.Add(Me.txtFiltro)
        Me.Controls.Add(Me.dgvResultados)
        Me.Controls.Add(Me.cmbTipoBusqueda)
        Me.Name = "FormularioBuscador"
        Me.Text = "FormularioBuscador"
        CType(Me.dgvResultados, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmbTipoBusqueda As ComboBox
    Friend WithEvents dgvResultados As DataGridView
    Friend WithEvents txtFiltro As TextBox
    Friend WithEvents btnSeleccionar As Button
    Friend WithEvents btnCancelar As Button
End Class
