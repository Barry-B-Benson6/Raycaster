<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ServerBrowser
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnCreateGame = New System.Windows.Forms.Button()
        Me.lstServer = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'btnCreateGame
        '
        Me.btnCreateGame.Location = New System.Drawing.Point(77, 364)
        Me.btnCreateGame.Name = "btnCreateGame"
        Me.btnCreateGame.Size = New System.Drawing.Size(118, 35)
        Me.btnCreateGame.TabIndex = 0
        Me.btnCreateGame.Text = "CreateGame"
        Me.btnCreateGame.UseVisualStyleBackColor = True
        '
        'lstServer
        '
        Me.lstServer.FormattingEnabled = True
        Me.lstServer.ItemHeight = 15
        Me.lstServer.Location = New System.Drawing.Point(157, 86)
        Me.lstServer.Name = "lstServer"
        Me.lstServer.Size = New System.Drawing.Size(120, 94)
        Me.lstServer.TabIndex = 1
        '
        'ServerBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.lstServer)
        Me.Controls.Add(Me.btnCreateGame)
        Me.Name = "ServerBrowser"
        Me.Text = "ServerBrowser"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnCreateGame As Button
    Friend WithEvents lstServer As ListBox
End Class
