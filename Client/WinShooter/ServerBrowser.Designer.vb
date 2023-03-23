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
        btnCreateGame = New Button()
        lstServer = New ListBox()
        btnJoinGame = New Button()
        SuspendLayout()
        ' 
        ' btnCreateGame
        ' 
        btnCreateGame.Location = New Point(77, 364)
        btnCreateGame.Name = "btnCreateGame"
        btnCreateGame.Size = New Size(118, 35)
        btnCreateGame.TabIndex = 0
        btnCreateGame.Text = "CreateGame"
        btnCreateGame.UseVisualStyleBackColor = True
        ' 
        ' lstServer
        ' 
        lstServer.FormattingEnabled = True
        lstServer.ItemHeight = 15
        lstServer.Location = New Point(157, 86)
        lstServer.Name = "lstServer"
        lstServer.Size = New Size(120, 94)
        lstServer.TabIndex = 1
        ' 
        ' btnJoinGame
        ' 
        btnJoinGame.Location = New Point(341, 208)
        btnJoinGame.Name = "btnJoinGame"
        btnJoinGame.Size = New Size(118, 35)
        btnJoinGame.TabIndex = 2
        btnJoinGame.Text = "JoinGame"
        btnJoinGame.UseVisualStyleBackColor = True
        ' 
        ' ServerBrowser
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 450)
        Controls.Add(btnJoinGame)
        Controls.Add(lstServer)
        Controls.Add(btnCreateGame)
        Name = "ServerBrowser"
        Text = "ServerBrowser"
        ResumeLayout(False)
    End Sub

    Friend WithEvents btnCreateGame As Button
    Friend WithEvents lstServer As ListBox
    Friend WithEvents btnJoinGame As Button
End Class
