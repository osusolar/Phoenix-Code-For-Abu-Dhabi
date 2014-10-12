<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Me.components = New System.ComponentModel.Container()
        Me.lblSpeed = New System.Windows.Forms.Label()
        Me.lblBorder1 = New System.Windows.Forms.Label()
        Me.lblExit = New System.Windows.Forms.Label()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.lblBorder2 = New System.Windows.Forms.Label()
        Me.picLeftBorder = New System.Windows.Forms.PictureBox()
        Me.lblSOC1 = New System.Windows.Forms.Label()
        Me.lblSOC2 = New System.Windows.Forms.Label()
        Me.lblLeftBlinker = New System.Windows.Forms.Label()
        Me.lblRightBlinker = New System.Windows.Forms.Label()
        Me.tmrMain = New System.Windows.Forms.Timer(Me.components)
        Me.lblBattPower2 = New System.Windows.Forms.Label()
        Me.lblBattPower = New System.Windows.Forms.Label()
        Me.lblMPPTPower2 = New System.Windows.Forms.Label()
        Me.lblMPPTPower = New System.Windows.Forms.Label()
        Me.serBodyController = New System.IO.Ports.SerialPort(Me.components)
        Me.tmrLeftBlinker = New System.Windows.Forms.Timer(Me.components)
        Me.tmrRightBlinker = New System.Windows.Forms.Timer(Me.components)
        Me.serGPS = New System.IO.Ports.SerialPort(Me.components)
        Me.lblHorn = New System.Windows.Forms.Label()
        Me.tmrHorn = New System.Windows.Forms.Timer(Me.components)
        Me.serBCM = New System.IO.Ports.SerialPort(Me.components)
        Me.tmrBCMQuery = New System.Windows.Forms.Timer(Me.components)
        Me.tmrInit = New System.Windows.Forms.Timer(Me.components)
        Me.tmrEnableArray = New System.Windows.Forms.Timer(Me.components)
        Me.srcMain = New System.Windows.Forms.BindingSource(Me.components)
        Me.PhoenixDataSetMain = New Phoenix.PhoenixDataSetMain()
        Me.SolarCarTableAdapter = New Phoenix.PhoenixDataSetMainTableAdapters.SolarCarTableAdapter()
        Me.lblHazard = New System.Windows.Forms.Label()
        Me.lblLight = New System.Windows.Forms.Label()
        Me.lblMusic = New System.Windows.Forms.Label()
        Me.tmrHazard = New System.Windows.Forms.Timer(Me.components)
        Me.tmrHeadLight = New System.Windows.Forms.Timer(Me.components)
        Me.lblEfficiency = New System.Windows.Forms.Label()
        Me.lblBorder3 = New System.Windows.Forms.Label()
        Me.lblAuxPackVoltage = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblBMSTemp = New System.Windows.Forms.Label()
        Me.lblBattVoltage = New System.Windows.Forms.Label()
        Me.lblArrayContactor = New System.Windows.Forms.Label()
        CType(Me.picLeftBorder, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.srcMain, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PhoenixDataSetMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblSpeed
        '
        Me.lblSpeed.AutoSize = True
        Me.lblSpeed.Font = New System.Drawing.Font("Microsoft Sans Serif", 120.0!)
        Me.lblSpeed.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblSpeed.Location = New System.Drawing.Point(134, 34)
        Me.lblSpeed.Margin = New System.Windows.Forms.Padding(0)
        Me.lblSpeed.Name = "lblSpeed"
        Me.lblSpeed.Size = New System.Drawing.Size(255, 181)
        Me.lblSpeed.TabIndex = 1
        Me.lblSpeed.Text = "55"
        Me.lblSpeed.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblBorder1
        '
        Me.lblBorder1.BackColor = System.Drawing.Color.FromArgb(CType(CType(204, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.lblBorder1.Location = New System.Drawing.Point(163, 2)
        Me.lblBorder1.Name = "lblBorder1"
        Me.lblBorder1.Size = New System.Drawing.Size(790, 45)
        Me.lblBorder1.TabIndex = 2
        '
        'lblExit
        '
        Me.lblExit.BackColor = System.Drawing.Color.FromArgb(CType(CType(204, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.lblExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!)
        Me.lblExit.Location = New System.Drawing.Point(1082, 4)
        Me.lblExit.Name = "lblExit"
        Me.lblExit.Size = New System.Drawing.Size(187, 43)
        Me.lblExit.TabIndex = 3
        Me.lblExit.Text = "EXIT"
        Me.lblExit.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTime
        '
        Me.lblTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!)
        Me.lblTime.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblTime.Location = New System.Drawing.Point(974, -1)
        Me.lblTime.Margin = New System.Windows.Forms.Padding(0)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(105, 48)
        Me.lblTime.TabIndex = 4
        Me.lblTime.Text = "12:00 pm"
        Me.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblBorder2
        '
        Me.lblBorder2.BackColor = System.Drawing.Color.FromArgb(CType(CType(204, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.lblBorder2.Font = New System.Drawing.Font("Microsoft Sans Serif", 22.0!)
        Me.lblBorder2.ForeColor = System.Drawing.Color.Black
        Me.lblBorder2.Location = New System.Drawing.Point(147, 199)
        Me.lblBorder2.Name = "lblBorder2"
        Me.lblBorder2.Size = New System.Drawing.Size(1121, 30)
        Me.lblBorder2.TabIndex = 2
        Me.lblBorder2.Text = "REARVIEW CAMERA SYSTEM"
        '
        'picLeftBorder
        '
        Me.picLeftBorder.Image = Global.Phoenix.My.Resources.Resources.Left_Border
        Me.picLeftBorder.ImageLocation = ""
        Me.picLeftBorder.Location = New System.Drawing.Point(3, 2)
        Me.picLeftBorder.Name = "picLeftBorder"
        Me.picLeftBorder.Size = New System.Drawing.Size(154, 81)
        Me.picLeftBorder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLeftBorder.TabIndex = 5
        Me.picLeftBorder.TabStop = False
        '
        'lblSOC1
        '
        Me.lblSOC1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(193, Byte), Integer))
        Me.lblSOC1.Font = New System.Drawing.Font("Microsoft Sans Serif", 35.0!)
        Me.lblSOC1.Location = New System.Drawing.Point(6, 364)
        Me.lblSOC1.Name = "lblSOC1"
        Me.lblSOC1.Size = New System.Drawing.Size(129, 117)
        Me.lblSOC1.TabIndex = 6
        Me.lblSOC1.Text = "SOC:"
        '
        'lblSOC2
        '
        Me.lblSOC2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(193, Byte), Integer))
        Me.lblSOC2.Font = New System.Drawing.Font("Microsoft Sans Serif", 48.0!)
        Me.lblSOC2.Location = New System.Drawing.Point(6, 411)
        Me.lblSOC2.Name = "lblSOC2"
        Me.lblSOC2.Size = New System.Drawing.Size(129, 71)
        Me.lblSOC2.TabIndex = 7
        Me.lblSOC2.Text = "100%"
        Me.lblSOC2.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblLeftBlinker
        '
        Me.lblLeftBlinker.BackColor = System.Drawing.Color.FromArgb(CType(CType(204, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer))
        Me.lblLeftBlinker.Font = New System.Drawing.Font("Microsoft Sans Serif", 35.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLeftBlinker.Location = New System.Drawing.Point(5, 238)
        Me.lblLeftBlinker.Name = "lblLeftBlinker"
        Me.lblLeftBlinker.Size = New System.Drawing.Size(129, 117)
        Me.lblLeftBlinker.TabIndex = 8
        Me.lblLeftBlinker.Text = "LEFT BLINKER"
        '
        'lblRightBlinker
        '
        Me.lblRightBlinker.BackColor = System.Drawing.Color.FromArgb(CType(CType(204, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(102, Byte), Integer))
        Me.lblRightBlinker.Font = New System.Drawing.Font("Microsoft Sans Serif", 35.0!)
        Me.lblRightBlinker.Location = New System.Drawing.Point(1140, 236)
        Me.lblRightBlinker.Name = "lblRightBlinker"
        Me.lblRightBlinker.Size = New System.Drawing.Size(129, 117)
        Me.lblRightBlinker.TabIndex = 9
        Me.lblRightBlinker.Text = "RIGHT BLINKER"
        '
        'tmrMain
        '
        Me.tmrMain.Interval = 1000
        '
        'lblBattPower2
        '
        Me.lblBattPower2.BackColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(98, Byte), Integer), CType(CType(238, Byte), Integer))
        Me.lblBattPower2.Font = New System.Drawing.Font("Microsoft Sans Serif", 40.0!)
        Me.lblBattPower2.ForeColor = System.Drawing.Color.White
        Me.lblBattPower2.Location = New System.Drawing.Point(1140, 410)
        Me.lblBattPower2.Name = "lblBattPower2"
        Me.lblBattPower2.Size = New System.Drawing.Size(129, 71)
        Me.lblBattPower2.TabIndex = 20
        Me.lblBattPower2.Text = "12.500"
        Me.lblBattPower2.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblBattPower
        '
        Me.lblBattPower.BackColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(98, Byte), Integer), CType(CType(238, Byte), Integer))
        Me.lblBattPower.Font = New System.Drawing.Font("Microsoft Sans Serif", 32.0!)
        Me.lblBattPower.ForeColor = System.Drawing.Color.White
        Me.lblBattPower.Location = New System.Drawing.Point(1140, 364)
        Me.lblBattPower.Name = "lblBattPower"
        Me.lblBattPower.Size = New System.Drawing.Size(129, 117)
        Me.lblBattPower.TabIndex = 19
        Me.lblBattPower.Text = "BATT kW"
        '
        'lblMPPTPower2
        '
        Me.lblMPPTPower2.BackColor = System.Drawing.Color.FromArgb(CType(CType(153, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblMPPTPower2.Font = New System.Drawing.Font("Microsoft Sans Serif", 40.0!)
        Me.lblMPPTPower2.ForeColor = System.Drawing.Color.White
        Me.lblMPPTPower2.Location = New System.Drawing.Point(5, 539)
        Me.lblMPPTPower2.Name = "lblMPPTPower2"
        Me.lblMPPTPower2.Size = New System.Drawing.Size(129, 71)
        Me.lblMPPTPower2.TabIndex = 22
        Me.lblMPPTPower2.Text = "1.0kW"
        Me.lblMPPTPower2.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblMPPTPower
        '
        Me.lblMPPTPower.BackColor = System.Drawing.Color.FromArgb(CType(CType(153, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblMPPTPower.Font = New System.Drawing.Font("Microsoft Sans Serif", 32.0!)
        Me.lblMPPTPower.ForeColor = System.Drawing.Color.White
        Me.lblMPPTPower.Location = New System.Drawing.Point(5, 492)
        Me.lblMPPTPower.Name = "lblMPPTPower"
        Me.lblMPPTPower.Size = New System.Drawing.Size(129, 117)
        Me.lblMPPTPower.TabIndex = 21
        Me.lblMPPTPower.Text = "ARRAY:"
        '
        'serBodyController
        '
        Me.serBodyController.PortName = "COM7"
        '
        'tmrLeftBlinker
        '
        Me.tmrLeftBlinker.Interval = 500
        '
        'tmrRightBlinker
        '
        Me.tmrRightBlinker.Interval = 500
        '
        'serGPS
        '
        Me.serGPS.BaudRate = 4800
        Me.serGPS.PortName = "COM3"
        '
        'lblHorn
        '
        Me.lblHorn.BackColor = System.Drawing.Color.Plum
        Me.lblHorn.Font = New System.Drawing.Font("Microsoft Sans Serif", 50.0!)
        Me.lblHorn.Location = New System.Drawing.Point(1139, 674)
        Me.lblHorn.Name = "lblHorn"
        Me.lblHorn.Size = New System.Drawing.Size(129, 117)
        Me.lblHorn.TabIndex = 25
        Me.lblHorn.Text = "Horn"
        Me.lblHorn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tmrHorn
        '
        Me.tmrHorn.Interval = 50
        '
        'serBCM
        '
        Me.serBCM.BaudRate = 57600
        Me.serBCM.PortName = "COM18"
        '
        'tmrBCMQuery
        '
        '
        'tmrInit
        '
        Me.tmrInit.Enabled = True
        Me.tmrInit.Interval = 5000
        '
        'tmrEnableArray
        '
        Me.tmrEnableArray.Interval = 1000
        '
        'srcMain
        '
        Me.srcMain.DataSource = Me.PhoenixDataSetMain
        Me.srcMain.Position = 0
        '
        'PhoenixDataSetMain
        '
        Me.PhoenixDataSetMain.DataSetName = "PhoenixDataSetMain"
        Me.PhoenixDataSetMain.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'SolarCarTableAdapter
        '
        Me.SolarCarTableAdapter.ClearBeforeFill = True
        '
        'lblHazard
        '
        Me.lblHazard.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(51, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblHazard.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHazard.Location = New System.Drawing.Point(6, 716)
        Me.lblHazard.Name = "lblHazard"
        Me.lblHazard.Size = New System.Drawing.Size(129, 75)
        Me.lblHazard.TabIndex = 27
        Me.lblHazard.Text = "HAZARD"
        Me.lblHazard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblLight
        '
        Me.lblLight.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(193, Byte), Integer))
        Me.lblLight.Font = New System.Drawing.Font("Microsoft Sans Serif", 36.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLight.Location = New System.Drawing.Point(6, 623)
        Me.lblLight.Name = "lblLight"
        Me.lblLight.Size = New System.Drawing.Size(129, 75)
        Me.lblLight.TabIndex = 28
        Me.lblLight.Text = "LIGHT"
        Me.lblLight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMusic
        '
        Me.lblMusic.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(193, Byte), Integer))
        Me.lblMusic.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMusic.Location = New System.Drawing.Point(1139, 623)
        Me.lblMusic.Name = "lblMusic"
        Me.lblMusic.Size = New System.Drawing.Size(129, 38)
        Me.lblMusic.TabIndex = 29
        Me.lblMusic.Text = "MUSIC"
        Me.lblMusic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tmrHazard
        '
        Me.tmrHazard.Interval = 500
        '
        'tmrHeadLight
        '
        Me.tmrHeadLight.Interval = 1000
        '
        'lblEfficiency
        '
        Me.lblEfficiency.BackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.lblEfficiency.Font = New System.Drawing.Font("Microsoft Sans Serif", 48.0!, System.Drawing.FontStyle.Bold)
        Me.lblEfficiency.Location = New System.Drawing.Point(1140, 542)
        Me.lblEfficiency.Name = "lblEfficiency"
        Me.lblEfficiency.Size = New System.Drawing.Size(129, 71)
        Me.lblEfficiency.TabIndex = 11
        Me.lblEfficiency.Text = "40"
        Me.lblEfficiency.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblBorder3
        '
        Me.lblBorder3.BackColor = System.Drawing.Color.FromArgb(CType(CType(51, Byte), Integer), CType(CType(102, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.lblBorder3.Font = New System.Drawing.Font("Microsoft Sans Serif", 35.0!)
        Me.lblBorder3.Location = New System.Drawing.Point(1140, 496)
        Me.lblBorder3.Name = "lblBorder3"
        Me.lblBorder3.Size = New System.Drawing.Size(129, 117)
        Me.lblBorder3.TabIndex = 10
        Me.lblBorder3.Text = "MI/kWH"
        '
        'lblAuxPackVoltage
        '
        Me.lblAuxPackVoltage.Font = New System.Drawing.Font("Microsoft Sans Serif", 35.0!)
        Me.lblAuxPackVoltage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblAuxPackVoltage.Location = New System.Drawing.Point(498, 47)
        Me.lblAuxPackVoltage.Margin = New System.Windows.Forms.Padding(0)
        Me.lblAuxPackVoltage.Name = "lblAuxPackVoltage"
        Me.lblAuxPackVoltage.Size = New System.Drawing.Size(455, 67)
        Me.lblAuxPackVoltage.TabIndex = 30
        Me.lblAuxPackVoltage.Text = "Aux. Pack Voltage: 12.0V"
        Me.lblAuxPackVoltage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(204, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(204, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(5, 86)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(126, 142)
        Me.Label1.TabIndex = 31
        '
        'lblBMSTemp
        '
        Me.lblBMSTemp.Font = New System.Drawing.Font("Microsoft Sans Serif", 35.0!)
        Me.lblBMSTemp.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblBMSTemp.Location = New System.Drawing.Point(498, 114)
        Me.lblBMSTemp.Margin = New System.Windows.Forms.Padding(0)
        Me.lblBMSTemp.Name = "lblBMSTemp"
        Me.lblBMSTemp.Size = New System.Drawing.Size(455, 67)
        Me.lblBMSTemp.TabIndex = 32
        Me.lblBMSTemp.Text = "Battery Temp: "
        Me.lblBMSTemp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblBattVoltage
        '
        Me.lblBattVoltage.Font = New System.Drawing.Font("Microsoft Sans Serif", 35.0!)
        Me.lblBattVoltage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblBattVoltage.Location = New System.Drawing.Point(274, 56)
        Me.lblBattVoltage.Margin = New System.Windows.Forms.Padding(0)
        Me.lblBattVoltage.Name = "lblBattVoltage"
        Me.lblBattVoltage.Size = New System.Drawing.Size(455, 67)
        Me.lblBattVoltage.TabIndex = 32
        Me.lblBattVoltage.Text = "Battery Voltage: "
        Me.lblBattVoltage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblArrayContactor
        '
        Me.lblArrayContactor.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblArrayContactor.Font = New System.Drawing.Font("Microsoft Sans Serif", 35.0!)
        Me.lblArrayContactor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(153, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblArrayContactor.Location = New System.Drawing.Point(274, 114)
        Me.lblArrayContactor.Margin = New System.Windows.Forms.Padding(0)
        Me.lblArrayContactor.Name = "lblArrayContactor"
        Me.lblArrayContactor.Size = New System.Drawing.Size(455, 67)
        Me.lblArrayContactor.TabIndex = 33
        Me.lblArrayContactor.Text = "Array Contactor: "
        Me.lblArrayContactor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'frmMain
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(1280, 780)
        Me.Controls.Add(Me.lblArrayContactor)
        Me.Controls.Add(Me.lblBattVoltage)
        Me.Controls.Add(Me.lblBMSTemp)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblAuxPackVoltage)
        Me.Controls.Add(Me.lblMusic)
        Me.Controls.Add(Me.lblLight)
        Me.Controls.Add(Me.lblHazard)
        Me.Controls.Add(Me.lblHorn)
        Me.Controls.Add(Me.lblMPPTPower2)
        Me.Controls.Add(Me.lblMPPTPower)
        Me.Controls.Add(Me.lblBattPower2)
        Me.Controls.Add(Me.lblBattPower)
        Me.Controls.Add(Me.lblEfficiency)
        Me.Controls.Add(Me.lblBorder3)
        Me.Controls.Add(Me.lblRightBlinker)
        Me.Controls.Add(Me.lblLeftBlinker)
        Me.Controls.Add(Me.lblSOC2)
        Me.Controls.Add(Me.lblSOC1)
        Me.Controls.Add(Me.picLeftBorder)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.lblExit)
        Me.Controls.Add(Me.lblBorder2)
        Me.Controls.Add(Me.lblBorder1)
        Me.Controls.Add(Me.lblSpeed)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "frmMain"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.picLeftBorder, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.srcMain, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PhoenixDataSetMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSpeed As System.Windows.Forms.Label
    Friend WithEvents lblBorder1 As System.Windows.Forms.Label
    Friend WithEvents lblExit As System.Windows.Forms.Label
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents lblBorder2 As System.Windows.Forms.Label
    Friend WithEvents picLeftBorder As System.Windows.Forms.PictureBox
    Friend WithEvents lblSOC1 As System.Windows.Forms.Label
    Friend WithEvents lblSOC2 As System.Windows.Forms.Label
    Friend WithEvents lblLeftBlinker As System.Windows.Forms.Label
    Friend WithEvents lblRightBlinker As System.Windows.Forms.Label
    Friend WithEvents tmrMain As System.Windows.Forms.Timer
    Friend WithEvents lblBattPower2 As System.Windows.Forms.Label
    Friend WithEvents lblBattPower As System.Windows.Forms.Label
    Friend WithEvents lblMPPTPower2 As System.Windows.Forms.Label
    Friend WithEvents lblMPPTPower As System.Windows.Forms.Label
    Friend WithEvents serBodyController As System.IO.Ports.SerialPort
    Friend WithEvents tmrLeftBlinker As System.Windows.Forms.Timer
    Friend WithEvents tmrRightBlinker As System.Windows.Forms.Timer
    Friend WithEvents serGPS As System.IO.Ports.SerialPort
    Friend WithEvents lblHorn As System.Windows.Forms.Label
    Friend WithEvents tmrHorn As System.Windows.Forms.Timer
    Friend WithEvents serBCM As System.IO.Ports.SerialPort
    Friend WithEvents tmrBCMQuery As System.Windows.Forms.Timer
    Friend WithEvents tmrInit As System.Windows.Forms.Timer
    Friend WithEvents tmrEnableArray As System.Windows.Forms.Timer
    Friend WithEvents srcMain As System.Windows.Forms.BindingSource
    Friend WithEvents PhoenixDataSetMain As Phoenix.PhoenixDataSetMain
    Friend WithEvents SolarCarTableAdapter As Phoenix.PhoenixDataSetMainTableAdapters.SolarCarTableAdapter
    Friend WithEvents lblHazard As System.Windows.Forms.Label
    Friend WithEvents lblLight As System.Windows.Forms.Label
    Friend WithEvents lblMusic As System.Windows.Forms.Label
    Friend WithEvents tmrHazard As System.Windows.Forms.Timer
    Friend WithEvents tmrHeadLight As System.Windows.Forms.Timer
    Friend WithEvents lblEfficiency As System.Windows.Forms.Label
    Friend WithEvents lblBorder3 As System.Windows.Forms.Label
    Friend WithEvents lblAuxPackVoltage As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblBMSTemp As System.Windows.Forms.Label
    Friend WithEvents lblBattVoltage As System.Windows.Forms.Label
    Friend WithEvents lblArrayContactor As System.Windows.Forms.Label

End Class
