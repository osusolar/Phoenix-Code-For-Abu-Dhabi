Public Class frmMain
    'Public definitions
    Const EFF_RED As Integer = 25
    Const EFF_YELLOW As Integer = 50
    Const PWR_MAX = 7500


    'Public Declarations
    Dim bolViewSOC As Boolean = True                        'View SOC or kWH (default SOC)
    Dim bolError As Boolean = False                         'Error message visible or not visible
    Dim intEfficiencyHistory(6) As Integer                  'Efficiency history array
    Dim intCurrentEfficiency As Integer = 50                'Current efficiency
    Dim floBattPower As Double                              'Battery Power
    Dim floArrayPower As Double                             'Array Power
    Dim floArrayOffset As Double = -2.702702703             'Array Offset
    Dim floMPPTCurrent As Double                            'MPPT current
    Dim floMaxkWH As Double                                 'Maximum kWH
    Dim BodyCurrent As String                               'Body Controller Current Usage
    Dim BodyCurrentValue As Array                           'Body Controller Power Array
    Dim BodyControllerOutput As Array                       'Body Controller Output States

    Dim camRear As New WebCamControl2.WebCamControl2

    Dim CANMessageCount As Integer = 0                      'Used in counter for enabling when we want DecodeBCM to run
    Dim startOfCANMessageCalibration As Boolean = False     'Used as a switch in serBCM_DataReceived method to calibrate the beginning of our 40 messages to see and 4 to read

    Public PhoenixBCM As BCM            'BCM 
    Public PhoenixGPS As GPS            'GPS

    Public Structure GPS
        Public Latitude As String                               'Latitude (from GPRMC)
        Public Longitude As String                              'Longitude (from GPRMC)
        Public SatCount As String                               'Number of satelites, AKA NumInUse (from GPGGA)
        Public Velocity As String                               'Velocity Correct to MPH (from GPRMC)
        Public Altitude As String                               'Altitude (from GPGGA) above or below MEAN sea level data (ellipsoid shaped earth) in METERS
        Public Bearing As String                                'Bearing... not yet populated........................................
        Public Quality As String                                'GPS quality indicator (0=invalid; 1=GPS fix; 2=Diff. GPS fix) (from GPGGA)
        Public NoS As String                                    'North or South
        Public EoW As String                                    'East or West
        Public UTCDate As String                                    'UTC at the position of the GPS 
        Public UTCTime As String
    End Structure

    Public Structure BCM
        Public Ready As Boolean
        Public EPO As Boolean
        Public ACChargerPlugged As Boolean
        Public HVIL As Boolean
        Public MainContactorState As Integer
        Public ChargeContactorState As Integer
        Public PowerRelayCommandState As Boolean
        Public PowerRelayRelayMonitor As Boolean
        Public BCMAlarmCondition As Integer
        Public StateOfCharge As Double
        Public PackCurrent As Double
        Public ChargingDone As Boolean
        Public PackVoltage As Double
        Public DischargeMax As Double
        Public ChargeMax As Double
        Public DischargeBuffer As Double
        Public ChargeBuffer As Double
        Public MaxBatteryAirTemperature As Double
        Public BCMCellUnderVolt As Boolean
        Public BCMCellOverVolt As Boolean
        Public MaxCellVoltage As Double
        Public VehicleWake As Boolean
        Public MinCellVoltage As Double
        Public MaxCellTemp As Double
        Public MinCellTemp As Double
        Public ChargeEnabled As Double
        Public LVBatteryVoltage As Double
        Public EEPROM_WIP As Boolean
        Public MainContactorStep As Integer
        Public FaultMonitor As Boolean
        Public BCMBalancing As Boolean
        Public FGD As Double
        Public BatteryCurrent2 As Double
        Public VBusPositive As Double
        Public VBusNegative As Double
        Public BalancingCount As Double
        Public kWH As Double
        Public ChargeComplete As Boolean

    End Structure


    'FUNCTION: OpenCOMPorts()
    'INPUTS: NONE
    'OUTPUTS: NONE
    'PURPOSE: Sets the COM ports for 
    Private Sub OpenCOMPorts()
        'Declarations

        'Sets the COM port settings - GPS
        serGPS.PortName = "COM19"
        serGPS.BaudRate = "4800"

        'Sets the COM port settings - Body Controller
        serBodyController.PortName = "COM7"
        serBodyController.BaudRate = "9600"

        'Sets the COM port settings - BCM -CAN bus
        serBCM.PortName = "COM20"
        serBCM.BaudRate = "57600"

        'Opens the serial port
        serBCM.Open()
        serGPS.Open()
        serBodyController.Open()


        'Initializes the BCM
        serBCM.Write("V") 'version
        serBCM.Write(vbCr)
        serBCM.Write("S6")
        serBCM.Write(vbCr)
        serBCM.Write("O")
        serBCM.Write(vbCr)


        'Enables the BCM timer
        tmrBCMQuery.Enabled = True
    End Sub

    'FUNCTION: InitCamera()
    'INPUTS: NONE
    'OUTPUTS: NONE
    'PURPOSE: Initializes web camera

    Private Sub InitCamera()
        'Declarations
        Dim camRearLocation As Point
        Dim camRearSize As Size

        'Error handling
        On Error Resume Next

        'Adds the control
        Me.Controls.Add(camRear)

        'Sets the location
        camRearLocation.X = 150
        camRearLocation.Y = 238
        camRear.Location = camRearLocation

        'Sets the size
        camRearSize.Height = 553
        camRearSize.Width = 973
        camRear.Size = camRearSize


    End Sub

    'FUNCTION: InitEfficiencyHistory ()
    'INPUTS: NONE
    'OUTPUTS: NONE
    'PURPOSE: Initializes the efficiency history

    Private Sub InitEfficiencyHistory()
        'Adds blanks into the efficiency history array
        intEfficiencyHistory(0) = 120
        intEfficiencyHistory(1) = 60
        intEfficiencyHistory(2) = 20
        intEfficiencyHistory(3) = 40
        intEfficiencyHistory(4) = 10
        intEfficiencyHistory(5) = 70


    End Sub

    'FUNCTION: ToggleSOC ()
    'INPUTS: NONE
    'OUTPUTS: NONE
    'PURPOSE: Toggles SOC vs. kWH

    Private Sub ToggleSOC()
        'Toggles between SOC and kWH
        If bolViewSOC = False Then
            'Goes to SOC state
            bolViewSOC = True

            'Toggles the GUI
            lblSOC1.Text = "SOC:"
        Else
            'Goes to kWH state
            bolViewSOC = False

            'Toggles the GUI
            lblSOC1.Text = "kWH:"
        End If
    End Sub


    'FUNCTION: RefreshEfficiency()
    'INPUTS: NONE
    'OUTPUTS: NONE
    'PURPOSE: Refreshes the energy efficiency
    Private Sub RefreshEfficiency()
        'Declarations
        Dim ptSize As Size
        Const BAR_MAX As Integer = 121
        Dim EffRed As Color
        Dim EffYellow As Color
        Dim EffGreen As Color

        'Sets the values to the colors involved
        EffRed = Color.FromArgb(CType(255, Byte), CType(51, Byte), CType(0, Byte))
        EffYellow = Color.FromArgb(CType(255, Byte), CType(204, Byte), CType(102, Byte))
        EffGreen = Color.FromArgb(CType(153, Byte), CType(102, Byte), CType(255, Byte))




    End Sub


    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load



        'Initializes the efficiency history
        Call InitEfficiencyHistory()

        'Initializes the COM ports
        Call OpenCOMPorts()

        'Passes a sample of the BCM code through
        DecodeBCM("4108093FFE599C40018A")
        DecodeBCM("42085A3314C8C8FF0000")
        DecodeBCM("43080CDEACDC78750076")
        DecodeBCM("440800FE9C510217C800")

        'Sets the charge completed flag to false initially
        PhoenixBCM.ChargeComplete = False


        'Fills the table adapter
        Me.SolarCarTableAdapter.Fill(Me.PhoenixDataSetMain.SolarCar)
    End Sub

    Private Sub lblSOC1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblSOC1.Click
        'Toggles the SOC
        Call ToggleSOC()


        'Plays the panel tone
        My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Panel.wav")
    End Sub

    Private Sub lblSOC2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblSOC2.Click
        'Toggles the SOC
        Call ToggleSOC()

        'Plays the panel tone
        My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Panel.wav")
    End Sub

    Private Sub lblHist1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        RefreshEfficiency()
    End Sub

    Private Sub tmrMain_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMain.Tick
        'Main Timer

        'Declarations
        Dim PwrRed As Color
        Dim PwrYellow As Color
        Dim PwrGreen As Color
        Dim sizTemp As Size
        Dim ptTemp As Point



        'Try Block
        Try

            'Sets the values to the colors involved
            PwrRed = Color.FromArgb(CType(255, Byte), CType(51, Byte), CType(0, Byte))
            PwrYellow = Color.FromArgb(CType(255, Byte), CType(204, Byte), CType(102, Byte))
            PwrGreen = Color.FromArgb(CType(32, Byte), CType(98, Byte), CType(238, Byte))

            'Updates time
            lblTime.Text = Now.ToShortTimeString.ToString

            'Displays SOC/kWH
            If bolViewSOC = False Then
                'Displays kW
                lblSOC2.Text = Math.Round(PhoenixBCM.kWH, 2)
            Else
                'Displays SOC
                lblSOC2.Text = Math.Round(PhoenixBCM.StateOfCharge, 0) & "%"
            End If

            'Updates the speed
            lblSpeed.Text = PhoenixGPS.Velocity


            'Updates the battery power
            lblBattPower2.Text = Math.Round(PhoenixBCM.PackCurrent * PhoenixBCM.PackVoltage / 1000, 3).ToString
            If Math.Round(PhoenixBCM.PackCurrent * PhoenixBCM.PackVoltage / 1000, 3) > -0.02 Then
                lblBattPower.BackColor = PwrGreen
                lblBattPower2.BackColor = PwrGreen
            Else
                lblBattPower.BackColor = PwrRed
                lblBattPower2.BackColor = PwrRed

            End If

            'Sets the current date with GPS date
            Dim CurrentDate As New Date("20" + PhoenixGPS.UTCDate.Substring(4, 2), PhoenixGPS.UTCDate.Substring(2, 2), PhoenixGPS.UTCDate.Substring(0, 2), PhoenixGPS.UTCTime.Substring(0, 2), PhoenixGPS.UTCTime.Substring(2, 2), PhoenixGPS.UTCTime.Substring(4, 2))

            'Store Values into SQL database
            SolarCarTableAdapter.Insert(CurrentDate, PhoenixGPS.Latitude, PhoenixGPS.Longitude, PhoenixGPS.Altitude, PhoenixGPS.Velocity, Math.Round(PhoenixBCM.StateOfCharge, 1), Math.Round(PhoenixBCM.PackVoltage, 1).ToString, Math.Round(PhoenixBCM.PackCurrent, 2).ToString, Math.Round(floArrayPower / PhoenixBCM.PackVoltage, 4).ToString, Math.Round(PhoenixBCM.LVBatteryVoltage, 1).ToString)


            'Updates array power
            floArrayPower = Split(BodyControllerOutput(2), "=")(1).Substring(0, 4)
            floArrayPower = Math.Round((floArrayPower * 5.405405405 + floArrayOffset) * PhoenixBCM.PackVoltage)
            lblMPPTPower2.Text = floArrayPower.ToString + "W"

            'Updates the aux pack voltage
            lblAuxPackVoltage.Text = "Aux. Pack Voltage: " + PhoenixBCM.LVBatteryVoltage.ToString + "V"

            'Update the battery temp
            lblBMSTemp.Text = "Battery Temp: " + PhoenixBCM.MaxCellTemp.ToString + " deg. C"
        Catch ex As Exception
        End Try

    End Sub


    Private Sub lblExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblExit.Click
        On Error Resume Next
        serGPS.Close()
        serBodyController.Close()
        serBCM.Close()
        Me.Close()
    End Sub


    Private Sub lblLeftBlinker_Click(sender As Object, e As System.EventArgs) Handles lblLeftBlinker.Click
        'Declarations
        Dim colBlinkerDefault = Color.FromArgb(CType(204, Byte), CType(102, Byte), CType(102, Byte))

        'Opens the COM port if port is closed
        If serBodyController.IsOpen = False Then
            serBodyController.Open()
        End If

        'Plays the panel tone
        My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Panel.wav")

        'Disables the right blinker
        If tmrRightBlinker.Enabled = True Then
            tmrRightBlinker.Enabled = False

            'Resets the button's color
            lblRightBlinker.BackColor = colBlinkerDefault
        End If


        'Enables/disables the left turn timer
        If tmrLeftBlinker.Enabled = False Then
            'Flashes blinker
            tmrLeftBlinker.Enabled = True
        ElseIf tmrLeftBlinker.Enabled = True Then
            'Stops blinker flash
            tmrLeftBlinker.Enabled = False

            'Resets the button's color
            lblLeftBlinker.BackColor = colBlinkerDefault
        End If
    End Sub


    Private Sub tmrLeftBlinker_Tick(sender As System.Object, e As System.EventArgs) Handles tmrLeftBlinker.Tick
        'Declarations
        Dim colBlinkerDefault As Color
        Dim colFlash As Color


        'Sets the values to the colors for the blinker
        colBlinkerDefault = Color.FromArgb(CType(204, Byte), CType(102, Byte), CType(102, Byte))
        colFlash = Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte))


        'Sends test blinker code out to serial port
        If tmrLeftBlinker.Tag = "" Then
            'Sends the serial string out
            serBodyController.WriteLine("$b,8,2,10,2")


            'Plays music tone
            My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Turn.wav")

            'Sets the timer tag so that we go half time
            tmrLeftBlinker.Tag = "You're it!"
        Else
            'Clears the tag
            tmrLeftBlinker.Tag = ""
        End If


        'Flashes the label
        If lblLeftBlinker.BackColor = colBlinkerDefault Then
            'Changes background to white
            lblLeftBlinker.BackColor = colFlash
        Else
            'Changes background to default
            lblLeftBlinker.BackColor = colBlinkerDefault
        End If


    End Sub

    Private Sub lblRightBlinker_Click(sender As System.Object, e As System.EventArgs) Handles lblRightBlinker.Click
        'Declarations
        Dim colBlinkerDefault = Color.FromArgb(CType(204, Byte), CType(102, Byte), CType(102, Byte))

        'Opens the COM port if port is closed
        If serBodyController.IsOpen = False Then
            serBodyController.Open()
        End If

        'Plays the panel tone
        My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Panel.wav")


        'Disables the left blinker
        If tmrLeftBlinker.Enabled = True Then
            tmrLeftBlinker.Enabled = False

            'Resets the button's color
            lblLeftBlinker.BackColor = colBlinkerDefault
        End If


        'Enables/disables the right turn timer
        If tmrRightBlinker.Enabled = False Then
            'Flashes blinker
            tmrRightBlinker.Enabled = True
        ElseIf tmrRightBlinker.Enabled = True Then
            'Stops blinker flash
            tmrRightBlinker.Enabled = False

            'Resets the button's color
            lblRightBlinker.BackColor = colBlinkerDefault
        End If
    End Sub

    Private Sub tmrRightBlinker_Tick(sender As System.Object, e As System.EventArgs) Handles tmrRightBlinker.Tick
        'Declarations
        Dim colBlinkerDefault As Color
        Dim colFlash As Color
        'Dim myString As Array


        'Sets the values to the colors for the blinker
        colBlinkerDefault = Color.FromArgb(CType(204, Byte), CType(102, Byte), CType(102, Byte))
        colFlash = Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte))


        'Sends test blinker code out to serial port
        If tmrRightBlinker.Tag = "" Then
            'Sends the serial string out
            serBodyController.WriteLine("$b,7,2,9,2")


            'Plays music tone
            My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Turn.wav")

            'Sets the timer tag so that we go half time
            tmrRightBlinker.Tag = "You're it!"
        Else
            'Clears the tag
            tmrRightBlinker.Tag = ""
        End If


        'Flashes the label
        If lblRightBlinker.BackColor = colBlinkerDefault Then
            'Changes background to white
            lblRightBlinker.BackColor = colFlash
        Else
            'Changes background to default
            lblRightBlinker.BackColor = colBlinkerDefault
        End If


    End Sub


    Private Sub serMain_DataReceived(sender As Object, e As System.IO.Ports.SerialDataReceivedEventArgs) Handles serBodyController.DataReceived
        'Reads the current line from the serial port
        BodyControllerOutput = Split(serBodyController.ReadLine, ",")

        'Parses data into array
        If BodyControllerOutput(0) = "$BodyController" Then
            'Splits the array and removes everything but the numeric value (0) and word amps (1) both as strings
            BodyCurrentValue = Split(Split(BodyControllerOutput(3), "=")(1), " ")
        End If

    End Sub


    Private Sub serGPS_DataReceived(sender As Object, e As System.IO.Ports.SerialDataReceivedEventArgs) Handles serGPS.DataReceived
        'Declarations
        Dim GPSArray As Array
        Dim VelocityRound As Integer

        'Try/Catch to keep it from crashing apon exit
        Try
            GPSArray = serGPS.ReadLine.Split(",")
        Catch ex As Exception

        End Try

        '1    = UTC of position fix
        '2    = Data status (V=navigation receiver warning)
        '3    = Latitude of fix
        '4    = N or S
        '5    = Longitude of fix
        '6    = E or W
        '7    = Speed over ground in knots
        '8    = Track made good in degrees True
        '9    = UT time
        '10   = Magnetic variation degrees (Easterly var. subtracts from true course)
        '11   = E or W
        '12   = Checksum

        'Read and Split each line piece of data and store it into an array
        Try
            If GPSArray(0) = "$GPRMC" And GPSArray(2) = "A" Then

                'Display Speed Section
                PhoenixGPS.Velocity = Math.Round((Convert.ToDecimal(GPSArray(7)) * 1.15077945)).ToString 'Convert into MPH
                VelocityRound = Val(Velocity) 'Get the integer value


                'Store each value, Velocity is stored as a ROUNDED value from above
                PhoenixGPS.UTCTime = GPSArray(1)
                PhoenixGPS.Latitude = GPSArray(3)
                PhoenixGPS.NoS = GPSArray(4)
                PhoenixGPS.Longitude = GPSArray(5)
                PhoenixGPS.EoW = GPSArray(6)
                PhoenixGPS.UTCDate = GPSArray(9)
                PhoenixGPS.Velocity = Velocity.ToString
            End If

        Catch ex As Exception


        End Try



        Try
            If GPSArray(0) = "$GPGGA" Then

                '1    = UTC of Position
                '2    = Latitude
                '3    = N or S
                '4    = Longitude
                '5    = E or W
                '6    = GPS quality indicator (0=invalid; 1=GPS fix; 2=Diff. GPS fix)
                '7    = Number of satellites in use [not those in view]
                '8    = Horizontal dilution of position
                '9    = Antenna altitude above/below mean sea level (geoid)
                '10   = Meters  (Antenna height unit)
                '11   = Geoidal separation (Diff. between WGS-84 earth ellipsoid and
                '       mean sea level.  -=geoid is below WGS-84 ellipsoid)
                '12   = Meters  (Units of geoidal separation)
                '13   = Age in seconds since last update from diff. reference station
                '14   = Diff. reference station ID#
                '15   = Checksum

                'Store each value, Velocity is stored as a ROUNDED value from above
                PhoenixGPS.Quality = GPSArray(6)
                PhoenixGPS.SatCount = GPSArray(7)
                PhoenixGPS.Altitude = GPSArray(9)
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub lblHorn_ControlAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.ControlEventArgs) Handles lblHorn.ControlAdded

    End Sub


    Private Sub lblhorn_click(sender As System.Object, e As System.EventArgs) Handles lblHorn.MouseDown
        'Declarations
        Dim colDefault As Color = Color.Plum
        Dim colHold As Color = Color.MediumVioletRed

        'Enable the timer horn which outputs on the serial string for the horn
        If tmrHorn.Enabled = False Then
            tmrHorn.Enabled = True
        End If

        'Plays the panel tone
        My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Panel.wav")

        'Flashes the label
        If lblHorn.BackColor = colDefault Then
            'Changes background to 
            lblHorn.BackColor = colHold
        End If


    End Sub

    Private Sub lblhorn_unclick(sender As System.Object, e As System.EventArgs) Handles lblHorn.MouseUp
        'Declarations
        Dim colDefault As Color = Color.Plum
        Dim colHold As Color = Color.MediumVioletRed

        'Disables the timer horn which outputs on the serial string for the horn
        If tmrHorn.Enabled = True Then
            tmrHorn.Enabled = False
        End If

        If lblHorn.BackColor <> colDefault Then
            'Changes background to default
            lblHorn.BackColor = colDefault
        End If

    End Sub

    Private Sub tmrHorn_Tick(sender As System.Object, e As System.EventArgs) Handles tmrHorn.Tick
        'Sends a pulse COMand for the horn(output 1) for 5 tenths of a second
        serBodyController.WriteLine("$p,1,10")
    End Sub


    Public Sub DecodeBCM(strCANMessage As String)
        'Declaration
        Dim strID As String = ""
        Dim strLength As String = ""
        Dim binary As String = ""
        Dim binary2 As String = ""
        Dim Data_Seg(8) As String

        '410 ID
        If strCANMessage.Substring(0, 3) = "410" Then
            workTxt = strCANMessage
            strID = "410"
            ' binary = Convert.ToString(strCANMessage.Substring(4, 5), 2)
            strLength = workTxt.Substring(3, 1)

            If Convert.ToDecimal(strLength) > 7 Then 'strLength is actually the length value sent from the CAN message. Then this line checks to make sure the message is complete.
                Data_Seg(0) = workTxt.Substring(4, 2)
                Data_Seg(1) = workTxt.Substring(6, 2)
                Data_Seg(2) = workTxt.Substring(8, 2)
                Data_Seg(3) = workTxt.Substring(10, 2)
                Data_Seg(4) = workTxt.Substring(12, 2)
                Data_Seg(5) = workTxt.Substring(14, 2)
                Data_Seg(6) = workTxt.Substring(16, 2)
                Data_Seg(7) = workTxt.Substring(18, 2)
            End If

            '=======================================
            'Byte 0
            '=======================================
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(0), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Check Ready
            PhoenixBCM.Ready = Convert.ToBoolean(binary.Substring(7, 1) * -1)

            'Check Emergency Shut Off
            PhoenixBCM.EPO = Convert.ToBoolean(binary.Substring(6, 1) * -1)

            'Charger Plugged into AC Socket
            PhoenixBCM.ACChargerPlugged = Convert.ToBoolean(binary.Substring(5, 1) * -1)

            'HVIL Monitor
            PhoenixBCM.HVIL = Convert.ToBoolean(binary.Substring(4, 1) * -1)

            'Main Contactor State
            PhoenixBCM.MainContactorState = Convert.ToDecimal(BinaryToDecimal(binary.Substring(2, 2)))

            'Charge Contactor State
            PhoenixBCM.ChargeContactorState = Convert.ToDecimal(BinaryToDecimal(binary.Substring(0, 2)))


            '=======================================
            'Byte 1
            '=======================================
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(1), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Contactor Power Relay Command State
            PhoenixBCM.PowerRelayCommandState = Convert.ToBoolean(-1 * (binary.Substring(3, 1)))

            'Contactor Power Relay Monitor
            PhoenixBCM.PowerRelayRelayMonitor = Convert.ToBoolean(-1 * (binary.Substring(2, 1)))

            'BCM Alarm Condition
            PhoenixBCM.BCMAlarmCondition = Convert.ToDecimal(BinaryToDecimal(binary.Substring(0, 2)))

            '=======================================
            'Byte 2 - skip
            '=======================================
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(2), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            '=======================================
            'Byte 3 - skip
            '=======================================
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(3), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'SOC in %
            PhoenixBCM.StateOfCharge = 0.5 * Convert.ToDecimal(BinaryToDecimal(binary))

            'Battery Current (****Special Uses 2 bytes****)
            PhoenixBCM.PackCurrent = 0.025 * CLng("&h" + Data_Seg(4) + Data_Seg(5)) - 1000


            'Byte 4 (*** Nothing Done ***)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(4), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Byte 5 (*** Nothing Done ***)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(5), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Byte 6
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(6), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Charging Done
            PhoenixBCM.ChargingDone = Convert.ToBoolean(-1 * (binary.Substring(3, 1)))

            'Vbat (*** Uses 2 bytes ***)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(6), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While
            binary2 = Convert.ToString(Convert.ToInt32(Data_Seg(7), 16), 2) : While 8 - binary2.Length > 0 : binary2 = "0" + binary2 : End While

            binary = binary.Substring(4, 4) + binary2
            PhoenixBCM.PackVoltage = 0.25 * Convert.ToDecimal(BinaryToDecimal(binary))

            'Pack kWH
            PhoenixBCM.kWH = (PhoenixBCM.StateOfCharge / 100) * (99 * 40) / 1000

        End If


        '420 ID
        If strCANMessage.Substring(0, 3) = "420" Then
            workTxt = strCANMessage
            strID = "420"
            strLength = workTxt.Substring(3, 1)
            If Convert.ToDecimal(strLength) > 7 Then
                Data_Seg(0) = workTxt.Substring(4, 2)
                Data_Seg(1) = workTxt.Substring(6, 2)
                Data_Seg(2) = workTxt.Substring(8, 2)
                Data_Seg(3) = workTxt.Substring(10, 2)
                Data_Seg(4) = workTxt.Substring(12, 2)
                Data_Seg(5) = workTxt.Substring(14, 2)
                Data_Seg(6) = workTxt.Substring(16, 2)
                Data_Seg(7) = workTxt.Substring(18, 2)
            End If

            'byte 0 
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(0), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While
            binary2 = Convert.ToString(Convert.ToInt32(Data_Seg(1), 16), 2) : While 8 - binary2.Length > 0 : binary2 = "0" + binary2 : End While

            'Discharge Max
            binary = binary + binary2.Substring(0, 4)
            PhoenixBCM.DischargeMax = 0.25 * Convert.ToDecimal(BinaryToDecimal(binary))

            'byte 1
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(1), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While
            binary2 = Convert.ToString(Convert.ToInt32(Data_Seg(2), 16), 2) : While 8 - binary2.Length > 0 : binary2 = "0" + binary2 : End While

            'Charge Max
            binary = binary.Substring(4, 4) + binary2
            PhoenixBCM.ChargeMax = 0.25 * Convert.ToDecimal(BinaryToDecimal(binary))

            'byte 2 (*** Nothing to Be Done ***)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(2), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'byte 3
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(3), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Discharge Buffer
            PhoenixBCM.DischargeBuffer = 0.5 * Convert.ToDecimal(BinaryToDecimal(binary))

            'byte 4
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(4), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Charge Buffer
            PhoenixBCM.ChargeBuffer = 0.5 * Convert.ToDecimal(BinaryToDecimal(binary))

            'byte 5
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(5), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Battery Air Temprature
            PhoenixBCM.MaxBatteryAirTemperature = (0.5 * Convert.ToDecimal(BinaryToDecimal(binary))) - 40

            'byte 6
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(6), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'BCM Cell Undervolt (flag)
            PhoenixBCM.BCMCellUnderVolt = Convert.ToBoolean(-1 * (binary.Substring(1, 1)))

            'BCM Cel OverVolt (flag)
            PhoenixBCM.BCMCellOverVolt = Convert.ToBoolean(-1 * (binary.Substring(0, 1)))

            'byte 7 (*** Nothing to Be Done ***)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(7), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

        End If

        '430 ID
        If strCANMessage.Substring(0, 3) = "430" Then
            workTxt = strCANMessage
            strID = "430"
            strLength = workTxt.Substring(3, 1)
            If Convert.ToDecimal(strLength) > 7 Then
                Data_Seg(0) = workTxt.Substring(4, 2)
                Data_Seg(1) = workTxt.Substring(6, 2)
                Data_Seg(2) = workTxt.Substring(8, 2)
                Data_Seg(3) = workTxt.Substring(10, 2)
                Data_Seg(4) = workTxt.Substring(12, 2)
                Data_Seg(5) = workTxt.Substring(14, 2)
                Data_Seg(6) = workTxt.Substring(16, 2)
                Data_Seg(7) = workTxt.Substring(18, 2)
            End If

            'byte 0 
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(0), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Charger Enable Command
            PhoenixBCM.ChargeEnabled = Convert.ToBoolean(-1 * (binary.Substring(0, 1)))

            'Max Cell Voltage (*** Byte 1 and Byte 2 ***)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(0), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While
            binary2 = Convert.ToString(Convert.ToInt32(Data_Seg(1), 16), 2) : While 8 - binary2.Length > 0 : binary2 = "0" + binary2 : End While

            binary = binary.Substring(3, 5) + binary2
            PhoenixBCM.MaxCellVoltage = 0.001 * Convert.ToDecimal(BinaryToDecimal(binary))

            'byte 1 (*** Nothing to Be done ***)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(1), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'byte 2
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(2), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Vehicle Wake Monitor
            PhoenixBCM.VehicleWake = Convert.ToBoolean(-1 * (binary.Substring(2, 1)))

            'MIN Cell Volt (*** Byte 2 and Byte 3 ***)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(2), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While
            binary2 = Convert.ToString(Convert.ToInt32(Data_Seg(3), 16), 2) : While 8 - binary2.Length > 0 : binary2 = "0" + binary2 : End While

            binary = binary.Substring(3, 5) + binary2
            PhoenixBCM.MinCellVoltage = 0.001 * Convert.ToDecimal(BinaryToDecimal(binary))

            'byte 3 (*** Nothing to Be Done ***)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(3), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'byte 4 
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(4), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Max Cell Temp (-40 offset . 25 init ?) ****** MIGHT WORK********
            PhoenixBCM.MaxCellTemp = (0.5 * Convert.ToDecimal(BinaryToDecimal(binary))) - 40

            'byte 5
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(5), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Min Cell Temp (-40 offset . 25 init ?) ****** MIGHT WORK********
            PhoenixBCM.MinCellTemp = (0.5 * Convert.ToDecimal(BinaryToDecimal(binary))) - 40

            'byte 6 (*** Nothing to Be Done ***)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(6), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'byte 7
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(7), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'LV Battery Voltage
            PhoenixBCM.LVBatteryVoltage = 0.1 * Convert.ToDecimal(BinaryToDecimal(binary))

        End If

        '440 ID
        If strCANMessage.Substring(0, 3) = "440" Then
            workTxt = strCANMessage
            strID = "440"
            strLength = workTxt.Substring(3, 1)
            If Convert.ToDecimal(strLength) > 7 Then
                Data_Seg(0) = workTxt.Substring(4, 2)
                Data_Seg(1) = workTxt.Substring(6, 2)
                Data_Seg(2) = workTxt.Substring(8, 2)
                Data_Seg(3) = workTxt.Substring(10, 2)
                Data_Seg(4) = workTxt.Substring(12, 2)
                Data_Seg(5) = workTxt.Substring(14, 2)
                Data_Seg(6) = workTxt.Substring(16, 2)
                Data_Seg(7) = workTxt.Substring(18, 2)

            End If

            'byte 0 
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(0), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'EEPROM WIP
            PhoenixBCM.EEPROM_WIP = Convert.ToBoolean(binary.Substring(7, 1) * -1)

            'Main Contactor Step
            PhoenixBCM.MainContactorStep = Convert.ToDecimal(BinaryToDecimal(binary.Substring(2, 4)))

            'Fault monitor
            PhoenixBCM.FaultMonitor = Convert.ToBoolean(binary.Substring(1, 1) * -1)

            'BCM Balacing 
            PhoenixBCM.BCMBalancing = Convert.ToBoolean(binary.Substring(0, 1) * -1)


            'byte 1
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(1), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'FGD
            PhoenixBCM.FGD = 0.5 * Convert.ToDecimal(BinaryToDecimal(binary))

            'byte 2 and 3

            'Battery Current 2
            binary = binary + binary2
            PhoenixBCM.BatteryCurrent2 = 0.025 * CLng("&h" + Data_Seg(2) + Data_Seg(3)) - 1000

            'byte 4 and byte 5 (half)
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(4), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While
            binary2 = Convert.ToString(Convert.ToInt32(Data_Seg(5), 16), 2) : While 8 - binary2.Length > 0 : binary2 = "0" + binary2 : End While

            'V Bus Positive
            binary = binary + binary2.Substring(0, 4)
            PhoenixBCM.VBusPositive = 0.25 * Convert.ToDecimal(BinaryToDecimal(binary))


            'byte 5 (half), byte 6
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(5), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While
            binary2 = Convert.ToString(Convert.ToInt32(Data_Seg(6), 16), 2) : While 8 - binary2.Length > 0 : binary2 = "0" + binary2 : End While

            'V Bus Negative
            binary = binary.Substring(4, 4) + binary2
            PhoenixBCM.VBusNegative = 0.25 * Convert.ToDecimal(BinaryToDecimal(binary)) - 500


            'byte 7
            binary = Convert.ToString(Convert.ToInt32(Data_Seg(7), 16), 2) : While 8 - binary.Length > 0 : binary = "0" + binary : End While

            'Balancing count
            PhoenixBCM.BalancingCount = 0.25 * Convert.ToDecimal(BinaryToDecimal(binary))

        End If
    End Sub



    Public Function BinaryToDecimal(Binary As String) As Long
        Dim n As Long
        Dim s As Integer

        For s = 1 To Len(Binary)
            n = n + (Mid(Binary, Len(Binary) - s + 1, 1) * (2 ^ _
                (s - 1)))
        Next s

        BinaryToDecimal = n
    End Function


    Private Sub serBCM_DataReceived(sender As Object, e As System.IO.Ports.SerialDataReceivedEventArgs) Handles serBCM.DataReceived
        'Declarations
        Dim strBCM As String = ""
        Dim arrBCM As Array
        Dim intBCM As Integer

        'Recieves the BCM string
        Try
            strBCM = serBCM.ReadExisting
        Catch ex As Exception
        End Try

        'Takes a message
        arrBCM = strBCM.Split("t")

        'Loops through all the data
        For intBCM = 0 To arrBCM.Length - 1
            'Parses the BCM data
            If arrBCM(intBCM).Length = 21 Then
                'drop 4*9 packets here.
                'read 4*1 packets here.
                ' "4108093FFE599C40018A"
                'if substring 1,3 == 410 then read that and count to 3 while reading then let it go through to 3*9 until read again

                'DecodeBCM(strBCM.Substring(1, 20))

                If Not startOfCANMessageCalibration Then
                    If (strBCM.Substring(1, 20)).Substring(0, 3) = "410" Then
                        startOfCANMessageCalibration = True
                    End If
                Else
                    If (CANMessageCount < 39) Then
                        If (CANMessageCount <= 3) Then
                            DecodeBCM(strBCM.Substring(1, 20))
                        End If
                    Else
                        CANMessageCount = -1
                    End If
                    CANMessageCount += 1
                End If
            End If
        Next
    End Sub

    Private Sub tmrBCMQuery_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrBCMQuery.Tick
        'Queries the BCM
        serBCM.Write("A")
        serBCM.Write(vbCr)
    End Sub

    Private Sub tmrInit_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrInit.Tick
        'Init timer

        'Initializes camera
        Call InitCamera()

        'Plays the ready tone
        My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Ready.wav")

        'Enables main timer
        tmrMain.Enabled = True

        'Enables the array contactor
        tmrEnableArray.Enabled = True

        'Disables init timer
        tmrInit.Enabled = False

        'Sets default values for GPS
        PhoenixGPS.UTCDate = "060613"
        PhoenixGPS.Latitude = "00.00"
        PhoenixGPS.NoS = "N"
        PhoenixGPS.Longitude = "00.00"
        PhoenixGPS.EoW = "W"
        PhoenixGPS.UTCTime = "120100"
        PhoenixGPS.Velocity = "0"
    End Sub

    Private Sub tmrEnableArray_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrEnableArray.Tick
        'Enables the array contactor 

        'Checks for contactor closure
        'If PhoenixBCM.MainContactorState = 0 Then
        'Exit Sub
        'End If

        'Displays battery voltage 
        lblBattVoltage.Text = "Battery Voltage: " + PhoenixBCM.PackVoltage.ToString() + "V"

        'Checks forcharge complete
        If PhoenixBCM.ChargeComplete = False Then
            'Checks the state of charge and enables array contactor
            If PhoenixBCM.PackVoltage < 103 Then
                'Enables the array contactors
                'If serBodyController.IsOpen = False Then
                'serBodyController.Open()
                'End If
                ' serBodyController.WriteLine("$p,5,30")


                'Displays array contactor status
                lblArrayContactor.Text = "Array Contactor: On"
            Else
                'Charge complete
                PhoenixBCM.ChargeComplete = True

                'Displays array contactor status
                lblArrayContactor.Text = "Array Contactor: Off"
            End If
        ElseIf PhoenixBCM.ChargeComplete = True Then
            'Displays array contactor status
            lblArrayContactor.Text = "Array Contactor: Off"

            'Restarts charging
            If PhoenixBCM.StateOfCharge <= 99 Then
                PhoenixBCM.ChargeComplete = False
            End If
        End If
    End Sub

    Private Sub lblHazard_Click(sender As System.Object, e As System.EventArgs) Handles lblHazard.Click
        'Declarations
        Dim colHazardDefault = Color.FromArgb(CType(255, Byte), CType(51, Byte), CType(0, Byte))
        Dim colBlinkerDefault = Color.FromArgb(CType(204, Byte), CType(102, Byte), CType(102, Byte))

        'Plays the panel sound
        My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Panel.wav")


        'Opens the COM port if port is closed
        If serBodyController.IsOpen = False Then
            serBodyController.Open()
        End If


        'Disables both blinkers
        tmrRightBlinker.Enabled = False
        tmrLeftBlinker.Enabled = False
        lblRightBlinker.BackColor = colBlinkerDefault
        lblLeftBlinker.BackColor = colBlinkerDefault

        'Enables/disables the left turn timer
        If tmrHazard.Enabled = False Then
            'Flashes blinker
            tmrHazard.Enabled = True
        ElseIf tmrHazard.Enabled = True Then
            'Stops blinker flash
            tmrHazard.Enabled = False

            'Resets the blinker color
            lblHazard.BackColor = colHazardDefault


            'Resets the button's color
            lblLeftBlinker.BackColor = colBlinkerDefault
        End If
    End Sub

    Private Sub tmrHazard_Tick(sender As System.Object, e As System.EventArgs) Handles tmrHazard.Tick
        'Declarations
        Dim colHazardDefault = Color.FromArgb(CType(255, Byte), CType(51, Byte), CType(0, Byte))
        Dim colFlash = Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(255, Byte))




        'Sends test blinker code out to serial port
        If tmrHazard.Tag = "" Then
            'Sends the serial string out
            serBodyController.WriteLine("$b,8,2,10,2,7,2,9,2")

            'Plays music tone
            My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Red Alert.wav")

            'Sets the timer tag so that we go half time
            tmrHazard.Tag = "You're it!"
        Else
            'Clears the tag
            tmrHazard.Tag = ""
        End If


        'Flashes the label
        If lblHazard.BackColor = colHazardDefault Then
            'Changes background to white
            lblHazard.BackColor = colFlash
        Else
            'Changes background to default
            lblHazard.BackColor = colHazardDefault
        End If
    End Sub

    Private Sub lblLight_Click(sender As System.Object, e As System.EventArgs) Handles lblLight.Click
        'Declarations
        Dim colLightDefault = Color.FromArgb(CType(255, Byte), CType(255, Byte), CType(193, Byte))

        'Plays the panel sound
        My.Computer.Audio.Play(My.Application.Info.DirectoryPath & "\Panel.wav")


        'Opens the COM port if port is closed
        If serBodyController.IsOpen = False Then
            serBodyController.Open()
        End If

        'Enables/disables the left turn timer
        If tmrHeadLight.Enabled = False Then
            'Flashes blinker
            tmrHeadLight.Enabled = True


        ElseIf tmrHeadLight.Enabled = True Then
            'Stops blinker flash
            tmrHeadLight.Enabled = False

            'Resets the button's color
            lblLight.BackColor = colLightDefault
        End If
    End Sub

    Private Sub tmrHeadLight_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrHeadLight.Tick
        'Declarations
        Dim colLightEnabled = Color.FromArgb(CType(255, Byte), CType(153, Byte), CType(0, Byte))


        'Enables the headlights
        serBodyController.WriteLine("$p,2,40,3,40")

        'Changes the headlight color
        lblLight.BackColor = colLightEnabled

    End Sub

End Class

