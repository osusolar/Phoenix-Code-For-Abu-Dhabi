Imports Microsoft.VisualBasic

Public Class gps_api
    Private GPSport As New System.IO.Ports.SerialPort()
    Public Latitude As String
    Public Longitude As String
    Public SatCount As String
    Public Velocity As String
    Public Altitude As String
    Public Bearing As String
    Public Function open(ByVal port As String)
        GPSport.PortName = port
        GPSport.BaudRate = 4800
        Try
            GPSport.Open()
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function is_open()
        Return GPSport.IsOpen
    End Function


    Public Function close()
        GPSport.Close()
        Return True
    End Function


    Private Function get_data()
        Try
            Static Dim old_data As String
            If GPSport.IsOpen Then
                Dim data As String = GPSport.ReadExisting()
                If (data <> "") Then
                    Dim strArr As Array = data.Split("$") 'split all of the data by line(each line of the gps feed starts with $)
                    old_data = data
                    Return strArr
                Else
                    Dim strArr As Array = old_data.Split("$") 'split all of the data by line(each line of the gps feed starts with $)
                    Return strArr
                End If
            Else
                Return ""
            End If
        Catch e As NullReferenceException
            Return " "
        End Try
    End Function


    Public Function get_location(ByVal UnitType As Double)
        Dim strArr() As String = get_data() 'get a list off all the data
        If strArr.Length > 1 Then 'if there is any info yet
            Try
                For i = 0 To strArr.Length 'loop through each peice of data backwords(so that you get the last record first)
                    Dim strTemp As String = strArr(i).ToString() 'get the current item
                    Dim lineArr() As String = strTemp.Split(",") 'split it by comma to seperate data
                    If (lineArr(0) = "GPGGA") Then 'check to make sure that it is a location code
                        'The array piece hold the folling information:
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

                        Try
                            ' Latitude
                            Dim dLat As Double = Convert.ToDouble(lineArr(2)) 'get the lat and turn it into a number
                            dLat = dLat / 100
                            Dim lat() As String = dLat.ToString().Split(".") 'split at the decimal point
                            Latitude = lineArr(3).ToString() + " " +
                            lat(0).ToString() + _
                            "." + ((Convert.ToDouble(lat(1)) /
                            60)).ToString("#####") ' combine all of them together in the correct format

                            ' Longitude
                            Dim dLon As Double = Convert.ToDouble(lineArr(4)) 'get lon and turn it into a number
                            dLon = dLon / 100
                            Dim lon() As String =
                            dLon.ToString().Split(".") 'split at decimal
                            Longitude = lineArr(5).ToString() + " " +
                            lon(0).ToString() + _
                            "." + ((Convert.ToDouble(lon(1)) /
                            60)).ToString("#####") 'combine all of them together into the correct format

                            'Number of Satalites
                            SatCount = lineArr(7).ToString() 'get the number of satalites

                            'Altitude
                            Dim dAlt As Double = Convert.ToDouble(lineArr(9)) 'get the altitude
                            dAlt = dAlt * UnitType 'multiply it by the conversion factor
                            Altitude = Convert.ToString(dAlt)
                            ' add all the values to the Collection
                            Dim collect As New gps_data()
                            collect.Latitude = Latitude
                            collect.Longitude = Longitude
                            collect.SatCount = SatCount
                            collect.Altitude = Altitude
                            collect.status = True
                            'return the Collection
                            get_location = collect
                            Return get_location
                        Catch e As Exception

                        End Try
                    End If
                Next

            Catch
                'do nothing
            End Try
        End If
        Return New gps_data()
    End Function


    ' This function returns collection with the Velocity, Bearing, and BearingDirection(N,SE ect.)
    Public Function get_other(ByVal unitType As Double, ByVal vunitType As Double)
            Dim strArr() As String = get_data()

            If strArr.Length >= 1 Then 'if there is any info yet
                Try
                    For i = 0 To strArr.Length 'loop through each peice of data backwords(so that you get the last record first)
                        Dim strTemp As String = strArr(i).ToString() 'get the current item
                        Dim lineArr() As String = strTemp.Split(",") 'split it by comma to seperate data
                        If (lineArr(0) = "GPRMC") Then 'If Current line starts with GPRMC then....
                            '1    = UTC of position fix
                            '2    = Data status (V=navigation receiver warning)
                            '3    = Latitude of fix
                            '4    = N or S
                            '5    = Longitude of fix
                            '6    = E or W
                            '7    = Speed over ground in knots
                            '8    = Track made good in degrees True
                            '9    = UT date
                            '10   = Magnetic variation degrees (Easterly var. subtracts from true course)
                            '11   = E or W
                            '12   = Checksum
                            Try
                                'Velocity
                                Velocity = Math.Round(Convert.ToDouble(lineArr(7)) * vunitType, 2) 'Velocity in the selected unit
                                Velocity = Velocity.ToString()
                                'Get the Bearing based on degrees with a 15 degree tolerance(intial +- 15 degrees) on N,E,S,W and west
                                Bearing = Convert.ToDecimal(lineArr(8))
                                Select Case Bearing
                                    Case 346 To 360, 0 To 15
                                        Bearing = "N (" + Bearing.ToString() + ")"
                                    Case 16 To 75
                                        Bearing = "NE (" + Bearing.ToString() + ")"
                                    Case 76 To 105
                                        Bearing = "E (" + Bearing.ToString() + ")"
                                    Case 106 To 165
                                        Bearing = "SE (" + Bearing.ToString() + ")"
                                    Case 166 To 195
                                        Bearing = "S (" + Bearing.ToString() + ")"
                                    Case 196 To 255
                                        Bearing = "SW (" + Bearing.ToString() + ")"
                                    Case 256 To 285
                                        Bearing = "W (" + Bearing.ToString() + ")"
                                    Case 286 To 345
                                        Bearing = "NW (" + Bearing.ToString() + ")"
                                    Case Else
                                        Bearing = Bearing.ToString()
                                End Select

                                'the latitude
                                Dim dLat As Double = Convert.ToDouble(lineArr(3)) 'get the lat and turn it into a number
                                dLat = dLat / 100
                                Dim lat() As String = dLat.ToString().Split(".") 'split at the decimal point
                                Latitude = lineArr(4).ToString() + " " +
                                lat(0).ToString() + _
                                "." + ((Convert.ToDouble(lat(1)) /
                                60)).ToString("#####") ' combine all of them together in the correct format

                                ' Longitude
                                Dim dLon As Double = Convert.ToDouble(lineArr(5)) 'get lon and turn it into a number
                                dLon = dLon / 100
                                Dim lon() As String =
                                dLon.ToString().Split(".") 'split at decimal
                                Longitude = lineArr(6).ToString() + " " +
                                lon(0).ToString() + _
                                "." + ((Convert.ToDouble(lon(1)) /
                                60)).ToString("#####") 'combine all of them together into the correct format

                                'put the data in to a collection
                                Dim storage As New gps_data()
                                storage.Velocity = Velocity
                                storage.Bearing = Convert.ToDecimal(lineArr(8)) 'just the raw bearing degree
                                storage.BearingDirection = Bearing
                                storage.Latitude = Latitude
                                storage.Longitude = Longitude
                                storage.status = True
                                'return the data
                                get_other = storage
                                Return get_other
                            Catch
                                Dim storage As New gps_data()
                                storage.status = False
                                get_other = storage
                                Return get_other
                                'data error
                            End Try
                        End If
                    Next
                Catch
                    'The strArr could not be read
                End Try
            End If
            Return New gps_data()
    End Function


End Class
