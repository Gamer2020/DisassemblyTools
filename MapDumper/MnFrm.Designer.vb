<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MnFrm
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
        Me.LoadButton = New System.Windows.Forms.Button()
        Me.fileOpenDialog = New System.Windows.Forms.OpenFileDialog()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.ROMNameLabel = New System.Windows.Forms.Label()
        Me.MapNameList = New System.Windows.Forms.ComboBox()
        Me.MapsAndBanks = New System.Windows.Forms.TreeView()
        Me.ExportBttn = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.GroupBox4.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LoadButton
        '
        Me.LoadButton.Location = New System.Drawing.Point(12, 12)
        Me.LoadButton.Name = "LoadButton"
        Me.LoadButton.Size = New System.Drawing.Size(136, 35)
        Me.LoadButton.TabIndex = 0
        Me.LoadButton.Text = "Load ROM"
        Me.LoadButton.UseVisualStyleBackColor = True
        '
        'fileOpenDialog
        '
        Me.fileOpenDialog.FileName = "OpenFileDialog1"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.ROMNameLabel)
        Me.GroupBox4.Location = New System.Drawing.Point(13, 63)
        Me.GroupBox4.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox4.Size = New System.Drawing.Size(179, 78)
        Me.GroupBox4.TabIndex = 13
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Game Loaded"
        '
        'ROMNameLabel
        '
        Me.ROMNameLabel.Location = New System.Drawing.Point(8, 31)
        Me.ROMNameLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ROMNameLabel.Name = "ROMNameLabel"
        Me.ROMNameLabel.Size = New System.Drawing.Size(137, 33)
        Me.ROMNameLabel.TabIndex = 0
        '
        'MapNameList
        '
        Me.MapNameList.FormattingEnabled = True
        Me.MapNameList.Location = New System.Drawing.Point(12, 159)
        Me.MapNameList.Name = "MapNameList"
        Me.MapNameList.Size = New System.Drawing.Size(121, 24)
        Me.MapNameList.TabIndex = 15
        '
        'MapsAndBanks
        '
        Me.MapsAndBanks.Location = New System.Drawing.Point(199, 12)
        Me.MapsAndBanks.Name = "MapsAndBanks"
        Me.MapsAndBanks.Size = New System.Drawing.Size(309, 368)
        Me.MapsAndBanks.TabIndex = 16
        '
        'ExportBttn
        '
        Me.ExportBttn.Location = New System.Drawing.Point(514, 12)
        Me.ExportBttn.Name = "ExportBttn"
        Me.ExportBttn.Size = New System.Drawing.Size(71, 52)
        Me.ExportBttn.TabIndex = 17
        Me.ExportBttn.Text = "Export"
        Me.ExportBttn.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(597, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(252, 466)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 18
        Me.PictureBox1.TabStop = False
        '
        'MnFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(861, 393)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.ExportBttn)
        Me.Controls.Add(Me.MapsAndBanks)
        Me.Controls.Add(Me.MapNameList)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.LoadButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "MnFrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Map Dumper"
        Me.GroupBox4.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents LoadButton As Button
    Friend WithEvents fileOpenDialog As OpenFileDialog
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents ROMNameLabel As Label
    Friend WithEvents MapNameList As ComboBox
    Friend WithEvents MapsAndBanks As TreeView
    Friend WithEvents ExportBttn As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents PictureBox1 As PictureBox
End Class
