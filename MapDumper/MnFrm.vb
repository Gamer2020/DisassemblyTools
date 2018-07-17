Imports System.IO
Imports VB = Microsoft.VisualBasic

Public Class MnFrm

    Dim outputtext As String
    Dim outputtextFooter As String
    Dim outputlevel2 As String
    Dim outputlevel4 As String

    Private Sub LoadButton_Click(sender As Object, e As EventArgs) Handles LoadButton.Click
        fileOpenDialog.FileName = ""
        fileOpenDialog.CheckFileExists = True

        ' Check to ensure that the selected path exists.  Dialog box displays 
        ' a warning otherwise.
        fileOpenDialog.CheckPathExists = True

        ' Get or set default extension. Doesn't include the leading ".".
        fileOpenDialog.DefaultExt = "GBA"

        ' Return the file referenced by a link? If False, simply returns the selected link
        ' file. If True, returns the file linked to the LNK file.
        fileOpenDialog.DereferenceLinks = True

        ' Just as in VB6, use a set of pairs of filters, separated with "|". Each 
        ' pair consists of a description|file spec. Use a "|" between pairs. No need to put a
        ' trailing "|". You can set the FilterIndex property as well, to select the default
        ' filter. The first filter is numbered 1 (not 0). The default is 1. 
        fileOpenDialog.Filter =
            "(*.gba)|*.gba*"

        fileOpenDialog.Multiselect = False

        ' Restore the original directory when done selecting
        ' a file? If False, the current directory changes
        ' to the directory in which you selected the file.
        ' Set this to True to put the current folder back
        ' where it was when you started.
        ' The default is False.
        '.RestoreDirectory = False

        ' Show the Help button and Read-Only checkbox?
        fileOpenDialog.ShowHelp = False
        fileOpenDialog.ShowReadOnly = False

        ' Start out with the read-only check box checked?
        ' This only make sense if ShowReadOnly is True.
        fileOpenDialog.ReadOnlyChecked = False

        fileOpenDialog.Title = "Select ROM to open:"

        ' Only accept valid Win32 file names?
        fileOpenDialog.ValidateNames = True


        If fileOpenDialog.ShowDialog = DialogResult.OK Then

            LoadedROM = fileOpenDialog.FileName

            HandleOpenedROM()

        End If
    End Sub

    Private Sub HandleOpenedROM()
        FileNum = FreeFile()

        FileOpen(FileNum, LoadedROM, OpenMode.Binary)
        'Opens the ROM as binary
        FileGet(FileNum, header, &HAD, True)
        header2 = Mid(header, 1, 3)
        header3 = Mid(header, 4, 1)
        FileClose(FileNum)

        If header2 = "BPR" Or header2 = "BPG" Or header2 = "BPE" Or header2 = "AXP" Or header2 = "AXV" Then
            If header3 = "J" Then
                ROMNameLabel.Text = ""
                LoadedROM = ""
                MessageBox.Show("I haven't added Jap support out of pure lazziness. I will though if it get's highly Demanded.")
                End
            Else
                ROMNameLabel.Text = header & " - " & GetString(GetINIFileLocation(), header, "ROMName", "")
            End If
        Else
            ROMNameLabel.Text = ""
            LoadedROM = ""
            MessageBox.Show("Not one of the Pokemon games...")
            End
        End If

        LoadMapList()
        LoadBanksAndMaps()

    End Sub

    Private Sub LoadMapList()
        MapNameList.Items.Clear()
        Dim i As Integer
        For i = 0 To (GetString((AppPath & "ini\roms.ini"), header, "NumberOfMapLabels", "")) - 1
            MapNameList.Items.Add(GetMapLabelName(i))
        Next i
    End Sub

    Private Sub LoadBanksAndMaps()

        MapsAndBanks.Nodes.Clear()

        Point2MapBankPointers = Int32.Parse(GetString(GetINIFileLocation(), header, "Pointer2PointersToMapBanks", ""), System.Globalization.NumberStyles.HexNumber)

        MapBankPointers = ((Val(("&H" & ReverseHEX(ReadHEX(LoadedROM, Point2MapBankPointers, 4)))) - &H8000000))

        i = 0

        While (ReadHEX(LoadedROM, MapBankPointers + (i * 4), "4") <> "02000000") And (ReadHEX(LoadedROM, MapBankPointers + (i * 4), "4") <> "FFFFFFFF") 'And ((("&H" & ReverseHEX(ReadHEX(LoadedROM, MapBankPointers + (i * 4), 4)))) < &H8000000)

            MapsAndBanks.Nodes.Add(i)

            Dim OriginalBankPointer As String = GetString((AppPath & "ini\roms.ini"), header, ("OriginalBankPointer" & i), "")
            Dim NumberOfMapsInBank As String = GetString((AppPath & "ini\roms.ini"), header, ("NumberOfMapsInBank" & i), "")


            x = 0

            BankPointer = ((Val(("&H" & ReverseHEX(ReadHEX(LoadedROM, MapBankPointers + (i * 4), 4)))) - &H8000000))

            While (x <= 299)

                HeaderPointer = ((Val(("&H" & ReverseHEX(ReadHEX(LoadedROM, BankPointer + (x * 4), 4)))) - &H8000000))

                If (ReadHEX(LoadedROM, BankPointer + (x * 4), 4) = "F7F7F7F7") Then
                    Exit While
                End If

                If OriginalBankPointer = Hex(BankPointer) Then

                    Dim maplabelvar As Integer

                    maplabelvar = CInt((Val(("&H" & ReadHEX(LoadedROM, HeaderPointer + 20, 1)))))

                    If ((header2 = "BPR") Or (header2 = "BPG")) Then

                        MapsAndBanks.Nodes.Item(i).Nodes.Add(New TreeNode(x & " - " & MapNameList.Items.Item(maplabelvar - &H58)))

                    ElseIf (mMain.header2 = "BPE") Then

                        MapsAndBanks.Nodes.Item(i).Nodes.Add(New TreeNode(x & " - " & MapNameList.Items.Item(maplabelvar)))

                    ElseIf ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then

                        MapsAndBanks.Nodes.Item(i).Nodes.Add(New TreeNode(x & " - " & MapNameList.Items.Item(maplabelvar)))

                    End If
                    'MapsAndBanks.Nodes.Item(i).Nodes.Add(New TreeNode(x & " - " & GetMapLabelName(1)))

                    If NumberOfMapsInBank = x Then

                        Exit While

                    End If

                Else

                    If (ReadHEX(LoadedROM, BankPointer + (x * 4), 4) = "77777777") Then
                        MapsAndBanks.Nodes.Item(i).Nodes.Add(New TreeNode((x & " - Reserved")))
                    Else

                        Dim maplabelvar As Integer

                        maplabelvar = CInt((Val(("&H" & ReadHEX(LoadedROM, HeaderPointer + 20, 1)))))

                        If ((header2 = "BPR") Or (header2 = "BPG")) Then

                            MapsAndBanks.Nodes.Item(i).Nodes.Add(New TreeNode(x & " - " & MapNameList.Items.Item(maplabelvar - &H58)))

                        ElseIf (mMain.header2 = "BPE") Then

                            MapsAndBanks.Nodes.Item(i).Nodes.Add(New TreeNode(x & " - " & MapNameList.Items.Item(maplabelvar)))

                        ElseIf ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then

                            MapsAndBanks.Nodes.Item(i).Nodes.Add(New TreeNode(x & " - " & MapNameList.Items.Item(maplabelvar)))

                        End If

                    End If

                End If

                x = x + 1
            End While

            i = i + 1

        End While


    End Sub

    Private Sub ExportBttn_Click(sender As Object, e As EventArgs) Handles ExportBttn.Click
        FolderBrowserDialog1.Description = "Select folder to export to:"

        If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
            SelectedPath = FolderBrowserDialog1.SelectedPath

            Me.Text = "Please wait..."
            Me.UseWaitCursor = True

            MapBank = (MapsAndBanks.SelectedNode.Parent.Index)
            MapNumber = (MapsAndBanks.SelectedNode.Index)

            Point2MapBankPointers = Int32.Parse(GetString(GetINIFileLocation(), header, "Pointer2PointersToMapBanks", ""), System.Globalization.NumberStyles.HexNumber)

            MapBankPointers = ((Val(("&H" & ReverseHEX(ReadHEX(LoadedROM, Point2MapBankPointers, 4)))) - &H8000000))


            BankPointer = ((Val(("&H" & ReverseHEX(ReadHEX(LoadedROM, MapBankPointers + (MapBank * 4), 4)))) - &H8000000))


            HeaderPointer = ((Val(("&H" & ReverseHEX(ReadHEX(LoadedROM, BankPointer + (MapNumber * 4), 4)))) - &H8000000))



            If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then


                FRLoadOutput2()


            ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then

                EMLoadOutput2()

            End If

            If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & "Bank" & MapBank & "_Map" & MapNumber & "/")) Then
                System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & "Bank" & MapBank & "_Map" & MapNumber & "/")
            End If

            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & "Bank" & MapBank & "_Map" & MapNumber & "/" & "layout" & ".inc", outputtextFooter)
            WriteHEX(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & "Bank" & MapBank & "_Map" & MapNumber & "/" & "border.bin", 0, BorderData)
            WriteHEX(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & "Bank" & MapBank & "_Map" & MapNumber & "/" & "map.bin", 0, MapPermData)

            If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/maps/" & "Bank" & MapBank & "_Map" & MapNumber & "/")) Then
                System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/maps/" & "Bank" & MapBank & "_Map" & MapNumber & "/")
            End If

            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/maps/" & "Bank" & MapBank & "_Map" & MapNumber & "/" & "header" & ".inc", outputtext)

            WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin", 0, PrimaryPals)
            WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal.bin", 0, SecondaryPals)

            WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles.bin", 0, PrimaryTilesImg)
            WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles.bin", 0, SecondaryTilesImg)

            WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks.bin", 0, PrimaryBlocks)
            WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks.bin", 0, SecondaryBlocks)

            WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors.bin", 0, PrimaryBehaviors)
            WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin", 0, SecondaryBehaviors)

            'Conversion code

            If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then

                'Tile Conversion
                Dim tiles1 As String
                Dim tiles2 As String
                Dim tilecomb As String

                tiles1 = MapTilesCompressedtoHexStringFRPrim2(0, 0, FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles.bin")
                tiles2 = MapTilesCompressedtoHexStringFRSec2(0, 0, FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles.bin")

                tilecomb = tiles1 & tiles2

                tiles1 = tilecomb.Substring(0, 16384 * 2)
                tiles2 = tilecomb.Substring(16384 * 2, 16384 * 2)

                tiles1 = ByteArrayToHexString(CompressBytes(HexStringToByteArray(tiles1)))
                tiles2 = ByteArrayToHexString(CompressBytes(HexStringToByteArray(tiles2)))

                If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles.bin") Then
                    File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles.bin")
                End If

                If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles.bin") Then
                    File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles.bin")
                End If

                WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles.bin", 0, tiles1)
                WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles.bin", 0, tiles2)

                'Block Conversion
                Dim blocks1 As String
                Dim blocks2 As String
                Dim blockscomb As String

                Dim info1 As New FileInfo(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks.bin")
                Dim info2 As New FileInfo(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks.bin")

                blocks1 = ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks.bin", 0, info1.Length)
                blocks2 = ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks.bin", 0, info2.Length)

                blockscomb = blocks1 & blocks2

                blocks1 = blockscomb.Substring(0, (512 * 2) * 16)
                blocks2 = blockscomb.Substring(((512 * 2) * 16), (blockscomb.Length - ((512 * 2) * 16)))

                If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks.bin") Then
                    File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks.bin")
                End If

                If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks.bin") Then
                    File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks.bin")
                End If

                WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks.bin", 0, blocks1)
                WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks.bin", 0, blocks2)

                'Pal Conversion
                Dim pals1 As String
                Dim pals2 As String

                pals1 = ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin", 0, ((16 * 2) * 6))
                pals2 = ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin", 0, ((16 * 2) * 7)) & ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal.bin", ((16 * 2) * 7), ((16 * 2) * 6))

                If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin") Then
                    File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin")
                End If

                If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal.bin") Then
                    File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal.bin")
                End If

                WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin", 0, pals1)
                WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal.bin", 0, pals2)

                'Behavior Conversion
                Dim behaviors1 As String = ""
                Dim behaviors2 As String = ""
                Dim behaviorscomb As String = ""

                Dim info3 As New FileInfo(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors.bin")
                Dim info4 As New FileInfo(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin")

                Dim loopvar As Integer

                loopvar = 0

                While loopvar < info3.Length

                    behaviors1 = behaviors1 & VB.Right("00" & ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors.bin", 0 + loopvar, 1), 2)
                    behaviors1 = behaviors1 & VB.Right("00" & ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors.bin", 0 + loopvar + 2, 1), 2)

                    loopvar = loopvar + 4

                End While

                loopvar = 0

                While loopvar < info4.Length

                    behaviors2 = behaviors2 & VB.Right("00" & ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin", 0 + loopvar, 1), 2)
                    behaviors2 = behaviors2 & VB.Right("00" & ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin", 0 + loopvar + 1, 2), 2)

                    loopvar = loopvar + 4

                End While

                behaviorscomb = behaviors1 & behaviors2

                Dim conbehaviors1 As String = ""
                Dim conbehaviors2 As String = ""

                conbehaviors1 = behaviorscomb.Substring(0, (512 * 2) * 2)
                conbehaviors2 = behaviorscomb.Substring(((512 * 2) * 2), (behaviorscomb.Length - ((512 * 2) * 2)) - 1)

                If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors.bin") Then
                    File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors.bin")
                End If

                If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin") Then
                    File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin")
                End If

                WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors.bin", 0, conbehaviors1)
                WriteHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin", 0, conbehaviors2)

            End If

            'Make Files compatible...


            Me.Text = "Map Dumper"
            Me.UseWaitCursor = False
            Me.Enabled = True
            Me.BringToFront()

        End If
    End Sub


    Private Sub EMLoadOutput2()

        Dim loopvar As Integer

        'outputtext = ".align 2" & vbLf & vbLf
        outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Header:" & vbLf

        'Header

        Map_Footer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer, 4))) - &H8000000

        outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Footer" & "  @Footer" & vbLf

        Map_Events = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 4, 4))) - &H8000000

        outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Events" & "  @Events" & vbLf

        Map_Level_Scripts = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 8, 4))) - &H8000000

        outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts" & "  @Level Scripts" & vbLf

        Map_Connection_Header = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 12, 4))) - &H8000000

        outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Connections_Header" & "  @Connections" & vbLf

        outputtext = outputtext & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 16, 2)) & "  @Music" & vbLf
        outputtext = outputtext & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 18, 2)) & "  @Foorter ID" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 20, 1)) & "  @Name" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 21, 1)) & "  @Light" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 22, 1)) & "  @Weather" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 23, 1)) & "  @Type" & vbLf
        outputtext = outputtext & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 24, 2)) & "  @Can_Dig" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 25, 1)) & "  @Can_Dig" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 26, 1)) & "  @Show_Name" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 27, 1)) & "  @BattleType" & vbLf

        outputtext = outputtext & vbLf

        'Footer

        outputtextFooter = outputtextFooter & "Bank" & MapBank & "_Map" & MapNumber & "_MapBorder::" & vbLf
        outputtextFooter = outputtextFooter & vbTab & ".incbin " & """" & "data/layouts/" & "Bank" & MapBank & "_Map" & MapNumber & "/" & "border.bin" & """" & vbLf & vbLf

        outputtextFooter = outputtextFooter & "Bank" & MapBank & "_Map" & MapNumber & "_MapBlockdata::" & vbLf
        outputtextFooter = outputtextFooter & vbTab & ".incbin " & """" & "data/layouts/" & "Bank" & MapBank & "_Map" & MapNumber & "/" & "map.bin" & """" & vbLf & vbLf

        outputtextFooter = outputtextFooter & vbTab & ".align 2" & vbLf


        outputtextFooter = outputtextFooter & "Bank" & MapBank & "_Map" & MapNumber & "_Layout::" & vbLf

        MapWidth = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer, 4)))

        outputtextFooter = outputtextFooter & "    .4byte    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer, 4)) & "  @Map Width" & vbLf

        MapHeight = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 4, 4)))

        outputtextFooter = outputtextFooter & "    .4byte    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 4, 4)) & "  @Map Height" & vbLf

        BorderPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 8, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    Bank" & MapBank & "_Map" & MapNumber & "_MapBorder" & "  @Border Data" & vbLf

        MapDataPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 12, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    Bank" & MapBank & "_Map" & MapNumber & "_MapBlockdata" & "  @Map data / Movement Permissons" & vbLf

        PrimaryTilesetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 16, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTileset" & "  @Primary Tileset" & vbLf

        SecondaryTilesetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 20, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTileset" & "  @Secondary Tileset" & vbLf

        BorderHeight = 2
        BorderWidth = 2

        BorderData = ReadHEX(LoadedROM, BorderPointer, (BorderHeight * BorderWidth) * 2)

        MapPermData = ReadHEX(LoadedROM, MapDataPointer, (MapHeight * MapWidth) * 2)

        'Primary Tileset

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTileset:" & vbLf

        PrimaryTilesetCompression = "&H" & (ReadHEX(LoadedROM, PrimaryTilesetPointer, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer, 1)) & "  @Is Compressed?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer + 1, 1)) & "  @Pallete mode?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer + 2, 1)) & "  @Field 2?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer + 3, 1)) & "  @Field 3?" & vbLf

        PrimaryImagePointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 4, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles" & "  @Image Pointer" & vbLf

        PrimaryPalPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 8, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal" & "  @Pallete Pointer" & vbLf

        PrimaryBlockSetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 12, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks" & "  @blockset_data" & vbLf

        PrimaryBehaviourPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 16, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors" & "  @behavioural_bg_bytes" & vbLf
        'outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 20, 4)) & "  @Animation routine" & vbLf

        PrimaryPals = ReadHEX(LoadedROM, PrimaryPalPointer, 6 * (16 * 2))

        If PrimaryTilesetCompression = 1 Then

            PrimaryTilesImg = MapTilesCompressedtoHexString(PrimaryImagePointer, PrimaryPalPointer)

        ElseIf PrimaryTilesetCompression = 0 Then

            PrimaryTilesImg = ReadHEX(LoadedROM, PrimaryImagePointer, 16384)

        End If

        PrimaryBlocks = ReadHEX(LoadedROM, PrimaryBlockSetPointer, 16 * 512)

        PrimaryBehaviors = ReadHEX(LoadedROM, PrimaryBehaviourPointer, 2 * 512)

        'Secondary Tileset

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTileset:" & vbLf

        SecondaryTilesetCompression = "&H" & (ReadHEX(LoadedROM, SecondaryTilesetPointer, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SecondaryTilesetPointer, 1)) & "  @Is Compressed?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SecondaryTilesetPointer + 1, 1)) & "  @Pallete mode?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SecondaryTilesetPointer + 2, 1)) & "  @Field 2?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SecondaryTilesetPointer + 3, 1)) & "  @Field 3?" & vbLf

        SecondaryImagePointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 4, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles" & "  @Image Pointer" & vbLf

        SecondaryPalPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 8, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal" & "  @Pallete Pointer" & vbLf

        SecondaryBlockSetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 12, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks" & "  @blockset_data" & vbLf

        SecondaryBehaviourPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 16, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors" & "  @behavioural_bg_bytes" & vbLf
        'outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 20, 4)) & "  @Animation routine" & vbLf

        SecondaryPals = ReadHEX(LoadedROM, SecondaryPalPointer, 13 * (16 * 2))

        If SecondaryTilesetCompression = 1 Then

            SecondaryTilesImg = MapTilesCompressedtoHexString(SecondaryImagePointer, SecondaryPalPointer)


        ElseIf SecondaryTilesetCompression = 0 Then

            SecondaryTilesImg = ReadHEX(LoadedROM, SecondaryImagePointer, 16384)

        End If

        SecondaryBlocks = ReadHEX(LoadedROM, SecondaryBlockSetPointer, (Int32.Parse((GetString(AppPath & "ini\roms.ini", header, "NumberOfTilesInTilset" & Hex(SecondaryTilesetPointer), "")), System.Globalization.NumberStyles.HexNumber) + 1) * 16)

        SecondaryBehaviors = ReadHEX(LoadedROM, SecondaryBehaviourPointer, (Int32.Parse((GetString(AppPath & "ini\roms.ini", header, "NumberOfTilesInTilset" & Hex(SecondaryTilesetPointer), "")), System.Globalization.NumberStyles.HexNumber) + 1) * 2)

        ''Events

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Events:" & vbLf

        NPC_Num = "&H" & (ReadHEX(LoadedROM, Map_Events, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Events, 1)) & "  @Number of NPC Events" & vbLf

        Warp_Num = "&H" & (ReadHEX(LoadedROM, Map_Events + 1, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Events + 1, 1)) & "  @Number of Warps" & vbLf

        Script_Event_Num = "&H" & (ReadHEX(LoadedROM, Map_Events + 2, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Events + 2, 1)) & "  @Number of Script Events" & vbLf

        SignPost_Num = "&H" & (ReadHEX(LoadedROM, Map_Events + 3, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Events + 3, 1)) & "  @Number of Signposts" & vbLf

        NPC_Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Events + 4, 4))) - &H8000000

        'outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_NPCs" & "  @Pointer to NPC Events" & vbLf

        Warp_Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Events + 8, 4))) - &H8000000

        'outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Warps" & "  @Pointer to Warps" & vbLf

        Script_Event_Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Events + 12, 4))) - &H8000000

        'outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Script_Events" & "  @Pointer to Script Events" & vbLf

        SignPost_Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Events + 16, 4))) - &H8000000

        'outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Sign_Posts" & "  @Pointer to Signposts" & vbLf

        ' Load NPCS

        'loopvar = 0

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_NPCs:" & vbLf

        'While loopvar < NPC_Num

        '    outputtext = outputtext & vbLf & "@NPC " & (loopvar + 1) & vbLf

        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + (loopvar * 24), 1)) & "  @NPC Number" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + 1 + (loopvar * 24), 1)) & "  @Sprite ID" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 2 + (loopvar * 24), 2)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 4 + (loopvar * 24), 2)) & "  @X Position" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 6 + (loopvar * 24), 2)) & "  @Y Position" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + 8 + (loopvar * 24), 1)) & "  @Height" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + 9 + (loopvar * 24), 1)) & "  @Behaviour" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 10 + (loopvar * 24), 2)) & "  @Behaviour Property" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + 12 + (loopvar * 24), 1)) & "  @Is_Trainer" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + 13 + (loopvar * 24), 1)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 14 + (loopvar * 24), 2)) & "  @radius_or_plantID" & vbLf
        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 16 + (loopvar * 24), 4)) & "  @Script Pointer" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 20 + (loopvar * 24), 2)) & "  @Flag" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 22 + (loopvar * 24), 2)) & "  @???" & vbLf

        '    loopvar = loopvar + 1
        'End While

        'Load Warps

        'loopvar = 0

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Warps:" & vbLf

        'While loopvar < Warp_Num

        '    outputtext = outputtext & vbLf & "@Warp " & (loopvar + 1) & vbLf

        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Warp_Pointer + (loopvar * 8), 2)) & "  @X Position" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Warp_Pointer + 2 + (loopvar * 8), 2)) & "  @Y Position" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Warp_Pointer + 4 + (loopvar * 8), 1)) & "  @Height" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Warp_Pointer + 5 + (loopvar * 8), 1)) & "  @Target Warp" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Warp_Pointer + 6 + (loopvar * 8), 1)) & "  @Target Map" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Warp_Pointer + 7 + (loopvar * 8), 1)) & "  @Target Bank" & vbLf

        '    loopvar = loopvar + 1
        'End While

        'Load Scripts

        'loopvar = 0

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Script_Events:" & vbLf

        'While loopvar < Script_Event_Num

        '    outputtext = outputtext & vbLf & "@Script Event " & (loopvar + 1) & vbLf

        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + (loopvar * 16), 2)) & "  @X Position" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + 2 + (loopvar * 16), 2)) & "  @Y Position" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Script_Event_Pointer + 4 + (loopvar * 16), 1)) & "  @Height" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Script_Event_Pointer + 5 + (loopvar * 16), 1)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + 6 + (loopvar * 16), 2)) & "  @Variable" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + 8 + (loopvar * 16), 2)) & "  @Value" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + 10 + (loopvar * 16), 2)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + 12 + (loopvar * 16), 4)) & "  @Script Pointer" & vbLf

        '    loopvar = loopvar + 1
        'End While

        'Sign posts

        'loopvar = 0

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Sign_Posts:" & vbLf

        'While loopvar < SignPost_Num

        '    outputtext = outputtext & vbLf & "@Sign Post " & (loopvar + 1) & vbLf

        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, SignPost_Pointer + (loopvar * 12), 2)) & "  @X Position" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, SignPost_Pointer + 2 + (loopvar * 12), 2)) & "  @Y Position" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SignPost_Pointer + 4 + (loopvar * 12), 1)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SignPost_Pointer + 5 + (loopvar * 12), 1)) & "  @Hidden Item" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SignPost_Pointer + 6 + (loopvar * 12), 1)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SignPost_Pointer + 7 + (loopvar * 12), 1)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, SignPost_Pointer + 8 + (loopvar * 12), 4)) & "  @Script Pointer" & vbLf

        '    loopvar = loopvar + 1
        'End While

        'Level Scripts

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts:" & vbLf



        'loopvar = 0

        'While (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1) <> "00")



        '    If (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) = "02" Then

        '        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) & "  @Type" & vbLf
        '        outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts_2" & "  @Pointer" & vbLf

        '        LevelScript2Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Level_Scripts + 1 + (loopvar * 5), 4))) - &H8000000

        '        Dim loopvar2 As Integer

        '        loopvar2 = 0

        '        outputlevel2 = outputlevel2 & vbLf

        '        outputlevel2 = outputlevel2 & ".align 2" & vbLf

        '        outputlevel2 = outputlevel2 & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts_2:" & vbLf

        '        While ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + (loopvar2 * 8), 2)) <> "0000"

        '            outputlevel2 = outputlevel2 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + (loopvar2 * 8), 2)) & "  @Variable" & vbLf
        '            outputlevel2 = outputlevel2 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + 2 + (loopvar2 * 8), 2)) & "  @Value to run" & vbLf
        '            outputlevel2 = outputlevel2 & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + 4 + (loopvar2 * 8), 4)) & "  @Pointer" & vbLf

        '            loopvar2 = loopvar2 + 1
        '        End While

        '        outputlevel2 = outputlevel2 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + (loopvar2 * 8), 2)) & "  @Terminator" & vbLf

        '    ElseIf (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) = "04" Then

        '        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) & "  @Type" & vbLf
        '        outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts_4" & "  @Pointer" & vbLf

        '        LevelScript4Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Level_Scripts + 1 + (loopvar * 5), 4))) - &H8000000

        '        Dim loopvar2 As Integer

        '        loopvar2 = 0

        '        outputlevel4 = outputlevel4 & vbLf

        '        outputlevel4 = outputlevel4 & ".align 2" & vbLf

        '        outputlevel4 = outputlevel4 & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts_4:" & vbLf

        '        While ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + (loopvar2 * 8), 2)) <> "0000"

        '            outputlevel4 = outputlevel4 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript4Pointer + (loopvar2 * 8), 2)) & "  @Variable" & vbLf
        '            outputlevel4 = outputlevel4 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript4Pointer + 2 + (loopvar2 * 8), 2)) & "  @Value to run" & vbLf
        '            outputlevel4 = outputlevel4 & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript4Pointer + 4 + (loopvar2 * 8), 4)) & "  @Pointer" & vbLf

        '            loopvar2 = loopvar2 + 1
        '        End While

        '        outputlevel4 = outputlevel4 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript4Pointer + (loopvar2 * 8), 2)) & "  @Terminator" & vbLf

        '    Else

        '        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) & "  @Type" & vbLf
        '        outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Level_Scripts + 1 + (loopvar * 5), 4)) & "  @Pointer" & vbLf

        '    End If

        'loopvar = loopvar + 1
        'End While

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) & "  @Terminator" & vbLf

        'outputtext = outputtext & outputlevel2
        'outputtext = outputtext & outputlevel4

        'Connections

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Connections_Header:" & vbLf

        'If Map_Connection_Header = -134217728 Then
        '    Connection_Num = 0
        '    Connection_Pointer = 0
        '    outputtext = outputtext & "    .long    0x0" & "  @Number of Connections" & vbLf
        '    outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Connections" & "  @Pointer to Connections" & vbLf

        'Else

        '    Connection_Num = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Connection_Header, 4)))
        '    Connection_Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Connection_Header + 4, 4))) - &H8000000

        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Connection_Header, 4)) & "  @Number of Connections" & vbLf
        '    outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Connections" & "  @Pointer to Connections" & vbLf

        'End If

        'loopvar = 0

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Connections:" & vbLf

        'While loopvar < Connection_Num

        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, Connection_Pointer + (loopvar * 12), 4)) & "  @Direction" & vbLf
        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, Connection_Pointer + 4 + (loopvar * 12), 4)) & "  @Offset Reference" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Connection_Pointer + 8 + (loopvar * 12), 1)) & "  @Map Bank" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Connection_Pointer + 9 + (loopvar * 12), 1)) & "  @Map Number" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Connection_Pointer + 10 + (loopvar * 12), 2)) & "  @Filler" & vbLf

        '    loopvar = loopvar + 1
        'End While

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Border:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_Border.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_MapData:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_MapData.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin""" & vbLf

    End Sub

    Private Sub FRLoadOutput2()

        Dim loopvar As Integer

        'outputtext = ".align 2" & vbLf & vbLf
        outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Header:" & vbLf

        'Header

        Map_Footer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer, 4))) - &H8000000

        outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Footer" & "  @Footer" & vbLf

        Map_Events = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 4, 4))) - &H8000000

        outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Events" & "  @Events" & vbLf

        Map_Level_Scripts = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 8, 4))) - &H8000000

        outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts" & "  @Level Scripts" & vbLf

        Map_Connection_Header = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 12, 4))) - &H8000000

        outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Connections_Header" & "  @Connections" & vbLf

        outputtext = outputtext & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 16, 2)) & "  @Music" & vbLf
        outputtext = outputtext & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 18, 2)) & "  @Foorter ID" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 20, 1)) & "  @Name" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 21, 1)) & "  @Light" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 22, 1)) & "  @Weather" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 23, 1)) & "  @Type" & vbLf
        outputtext = outputtext & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 24, 2)) & "  @Can_Dig" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 25, 1)) & "  @Can_Dig" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 26, 1)) & "  @Show_Name" & vbLf
        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, HeaderPointer + 27, 1)) & "  @BattleType" & vbLf

        outputtext = outputtext & vbLf

        'Footer

        outputtextFooter = outputtextFooter & "Bank" & MapBank & "_Map" & MapNumber & "_MapBorder::" & vbLf
        outputtextFooter = outputtextFooter & vbTab & ".incbin " & """" & "data/layouts/" & "Bank" & MapBank & "_Map" & MapNumber & "/" & "border.bin" & """" & vbLf & vbLf

        outputtextFooter = outputtextFooter & "Bank" & MapBank & "_Map" & MapNumber & "_MapBlockdata::" & vbLf
        outputtextFooter = outputtextFooter & vbTab & ".incbin " & """" & "data/layouts/" & "Bank" & MapBank & "_Map" & MapNumber & "/" & "map.bin" & """" & vbLf & vbLf

        outputtextFooter = outputtextFooter & vbTab & ".align 2" & vbLf

        outputtextFooter = outputtextFooter & "Bank" & MapBank & "_Map" & MapNumber & "_Layout::" & vbLf

        MapWidth = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer, 4)))

        outputtextFooter = outputtextFooter & "    .4byte    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer, 4)) & "  @Map Width" & vbLf

        MapHeight = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 4, 4)))

        outputtextFooter = outputtextFooter & "    .4byte    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 4, 4)) & "  @Map Height" & vbLf

        BorderPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 8, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    Bank" & MapBank & "_Map" & MapNumber & "_MapBorder" & "  @Border Data" & vbLf

        MapDataPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 12, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    Bank" & MapBank & "_Map" & MapNumber & "_MapBlockdata" & "  @Map data / Movement Permissons" & vbLf

        PrimaryTilesetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 16, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTileset" & "  @Primary Tileset" & vbLf

        SecondaryTilesetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 20, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTileset" & "  @Secondary Tileset" & vbLf

        'BorderHeight = ("&H" & (ReadHEX(LoadedROM, Map_Footer + 24, 1)))
        'BorderWidth = ("&H" & (ReadHEX(LoadedROM, Map_Footer + 25, 1)))

        'outputtext = outputtext & "    .byte    " & BorderHeight & "  @Border Height" & vbLf
        'outputtext = outputtext & "    .byte    " & BorderWidth & "  @Border Width" & vbLf

        BorderData = ReadHEX(LoadedROM, BorderPointer, (2 * 2) * 2)

        MapPermData = ReadHEX(LoadedROM, MapDataPointer, (MapHeight * MapWidth) * 2)

        'Primary Tileset

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTileset:" & vbLf

        PrimaryTilesetCompression = "&H" & (ReadHEX(LoadedROM, PrimaryTilesetPointer, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer, 1)) & "  @Is Compressed?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer + 1, 1)) & "  @Pallete mode?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer + 2, 1)) & "  @Field 2?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer + 3, 1)) & "  @Field 3?" & vbLf

        PrimaryImagePointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 4, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles" & "  @Image Pointer" & vbLf

        PrimaryPalPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 8, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal" & "  @Pallete Pointer" & vbLf

        PrimaryBlockSetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 12, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks" & "  @blockset_data" & vbLf

        PrimaryBehaviourPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 20, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors" & "  @behavioural_bg_bytes" & vbLf

        'outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 16, 4)) & "  @Animation routine" & vbLf

        PrimaryPals = ReadHEX(LoadedROM, PrimaryPalPointer, 7 * (16 * 2))

        If PrimaryTilesetCompression = 1 Then

            PrimaryTilesImg = MapTilesCompressedtoHexStringFRPrim(PrimaryImagePointer, PrimaryPalPointer)


        ElseIf PrimaryTilesetCompression = 0 Then

            PrimaryTilesImg = ReadHEX(LoadedROM, PrimaryImagePointer, 20480)

        End If

        PrimaryBlocks = ReadHEX(LoadedROM, PrimaryBlockSetPointer, 16 * 640)

        PrimaryBehaviors = ReadHEX(LoadedROM, PrimaryBehaviourPointer, 4 * 640)

        'Secondary Tileset

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTileset:" & vbLf

        SecondaryTilesetCompression = "&H" & (ReadHEX(LoadedROM, SecondaryTilesetPointer, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SecondaryTilesetPointer, 1)) & "  @Is Compressed?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SecondaryTilesetPointer + 1, 1)) & "  @Pallete mode?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SecondaryTilesetPointer + 2, 1)) & "  @Field 2?" & vbLf
        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SecondaryTilesetPointer + 3, 1)) & "  @Field 3?" & vbLf

        SecondaryImagePointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 4, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles" & "  @Image Pointer" & vbLf

        SecondaryPalPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 8, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal" & "  @Pallete Pointer" & vbLf

        SecondaryBlockSetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 12, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks" & "  @blockset_data" & vbLf

        SecondaryBehaviourPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 20, 4))) - &H8000000

        'outputtext = outputtext & "    .long    Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors" & "  @behavioural_bg_bytes" & vbLf

        'outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 16, 4)) & "  @Animation routine" & vbLf

        SecondaryPals = ReadHEX(LoadedROM, SecondaryPalPointer, 13 * (16 * 2))

        If SecondaryTilesetCompression = 1 Then

            SecondaryTilesImg = MapTilesCompressedtoHexStringFRSec(SecondaryImagePointer, SecondaryPalPointer)


        ElseIf SecondaryTilesetCompression = 0 Then

            SecondaryTilesImg = ReadHEX(LoadedROM, SecondaryImagePointer, 12288)

        End If

        SecondaryBlocks = ReadHEX(LoadedROM, SecondaryBlockSetPointer, (Int32.Parse((GetString(AppPath & "ini\roms.ini", header, "NumberOfTilesInTilset" & Hex(SecondaryTilesetPointer), "")), System.Globalization.NumberStyles.HexNumber) + 1) * 16)

        'MsgBox(Int32.Parse((GetString(AppPath & "ini\roms.ini", header, "NumberOfTilesInTilset" & Hex(SecondaryTilesetPointer), ""))))
        'MsgBox((Int32.Parse((GetString(AppPath & "ini\roms.ini", header, "NumberOfTilesInTilset" & Hex(SecondaryTilesetPointer), "")), System.Globalization.NumberStyles.HexNumber)))

        SecondaryBehaviors = ReadHEX(LoadedROM, SecondaryBehaviourPointer, (Int32.Parse((GetString(AppPath & "ini\roms.ini", header, "NumberOfTilesInTilset" & Hex(SecondaryTilesetPointer), "")), System.Globalization.NumberStyles.HexNumber) + 1) * 4)

        'Events

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Events:" & vbLf

        'NPC_Num = "&H" & (ReadHEX(LoadedROM, Map_Events, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Events, 1)) & "  @Number of NPC Events" & vbLf

        'Warp_Num = "&H" & (ReadHEX(LoadedROM, Map_Events + 1, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Events + 1, 1)) & "  @Number of Warps" & vbLf

        'Script_Event_Num = "&H" & (ReadHEX(LoadedROM, Map_Events + 2, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Events + 2, 1)) & "  @Number of Script Events" & vbLf

        'SignPost_Num = "&H" & (ReadHEX(LoadedROM, Map_Events + 3, 1))

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Events + 3, 1)) & "  @Number of Signposts" & vbLf

        'NPC_Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Events + 4, 4))) - &H8000000

        'outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_NPCs" & "  @Pointer to NPC Events" & vbLf

        'Warp_Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Events + 8, 4))) - &H8000000

        'outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Warps" & "  @Pointer to Warps" & vbLf

        'Script_Event_Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Events + 12, 4))) - &H8000000

        'outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Script_Events" & "  @Pointer to Script Events" & vbLf

        'SignPost_Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Events + 16, 4))) - &H8000000

        'outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Sign_Posts" & "  @Pointer to Signposts" & vbLf

        'loopvar = 0

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_NPCs:" & vbLf

        'While loopvar < NPC_Num

        '    outputtext = outputtext & vbLf & "@NPC " & (loopvar + 1) & vbLf

        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + (loopvar * 24), 1)) & "  @NPC Number" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + 1 + (loopvar * 24), 1)) & "  @Sprite ID" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 2 + (loopvar * 24), 2)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 4 + (loopvar * 24), 2)) & "  @X Position" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 6 + (loopvar * 24), 2)) & "  @Y Position" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + 8 + (loopvar * 24), 1)) & "  @Height" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + 9 + (loopvar * 24), 1)) & "  @Behaviour" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 10 + (loopvar * 24), 2)) & "  @Behaviour Property" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + 12 + (loopvar * 24), 1)) & "  @Is_Trainer" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, NPC_Pointer + 13 + (loopvar * 24), 1)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 14 + (loopvar * 24), 2)) & "  @radius_or_plantID" & vbLf
        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 16 + (loopvar * 24), 4)) & "  @Script Pointer" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 20 + (loopvar * 24), 2)) & "  @Flag" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, NPC_Pointer + 22 + (loopvar * 24), 2)) & "  @???" & vbLf

        '    loopvar = loopvar + 1
        'End While

        'loopvar = 0

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Warps:" & vbLf

        'While loopvar < Warp_Num

        '    outputtext = outputtext & vbLf & "@Warp " & (loopvar + 1) & vbLf

        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Warp_Pointer + (loopvar * 8), 2)) & "  @X Position" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Warp_Pointer + 2 + (loopvar * 8), 2)) & "  @Y Position" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Warp_Pointer + 4 + (loopvar * 8), 1)) & "  @Height" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Warp_Pointer + 5 + (loopvar * 8), 1)) & "  @Target Warp" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Warp_Pointer + 6 + (loopvar * 8), 1)) & "  @Target Map" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Warp_Pointer + 7 + (loopvar * 8), 1)) & "  @Target Bank" & vbLf

        '    loopvar = loopvar + 1
        'End While

        'loopvar = 0

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Script_Events:" & vbLf

        'While loopvar < Script_Event_Num

        '    outputtext = outputtext & vbLf & "@Script Event " & (loopvar + 1) & vbLf

        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + (loopvar * 16), 2)) & "  @X Position" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + 2 + (loopvar * 16), 2)) & "  @Y Position" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Script_Event_Pointer + 4 + (loopvar * 16), 1)) & "  @Height" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Script_Event_Pointer + 5 + (loopvar * 16), 1)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + 6 + (loopvar * 16), 2)) & "  @Variable" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + 8 + (loopvar * 16), 2)) & "  @Value" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + 10 + (loopvar * 16), 2)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, Script_Event_Pointer + 12 + (loopvar * 16), 4)) & "  @Script Pointer" & vbLf

        '    loopvar = loopvar + 1
        'End While

        'loopvar = 0

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Sign_Posts:" & vbLf

        'While loopvar < SignPost_Num

        '    outputtext = outputtext & vbLf & "@Sign Post " & (loopvar + 1) & vbLf

        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, SignPost_Pointer + (loopvar * 12), 2)) & "  @X Position" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, SignPost_Pointer + 2 + (loopvar * 12), 2)) & "  @Y Position" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SignPost_Pointer + 4 + (loopvar * 12), 1)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SignPost_Pointer + 5 + (loopvar * 12), 1)) & "  @Hidden Item" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SignPost_Pointer + 6 + (loopvar * 12), 1)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, SignPost_Pointer + 7 + (loopvar * 12), 1)) & "  @???" & vbLf
        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, SignPost_Pointer + 8 + (loopvar * 12), 4)) & "  @Script Pointer" & vbLf

        '    loopvar = loopvar + 1
        'End While

        ''Level Scripts

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts:" & vbLf



        'loopvar = 0

        'While (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1) <> "00")



        '    If (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) = "02" Then

        '        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) & "  @Type" & vbLf
        '        outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts_2" & "  @Pointer" & vbLf

        '        LevelScript2Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Level_Scripts + 1 + (loopvar * 5), 4))) - &H8000000

        '        Dim loopvar2 As Integer

        '        loopvar2 = 0

        '        outputlevel2 = outputlevel2 & vbLf

        '        outputlevel2 = outputlevel2 & ".align 2" & vbLf

        '        outputlevel2 = outputlevel2 & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts_2:" & vbLf

        '        While ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + (loopvar2 * 8), 2)) <> "0000"

        '            outputlevel2 = outputlevel2 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + (loopvar2 * 8), 2)) & "  @Variable" & vbLf
        '            outputlevel2 = outputlevel2 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + 2 + (loopvar2 * 8), 2)) & "  @Value to run" & vbLf
        '            outputlevel2 = outputlevel2 & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + 4 + (loopvar2 * 8), 4)) & "  @Pointer" & vbLf

        '            loopvar2 = loopvar2 + 1
        '        End While

        '        outputlevel2 = outputlevel2 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + (loopvar2 * 8), 2)) & "  @Terminator" & vbLf

        '    ElseIf (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) = "04" Then

        '        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) & "  @Type" & vbLf
        '        outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts_4" & "  @Pointer" & vbLf

        '        LevelScript4Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Level_Scripts + 1 + (loopvar * 5), 4))) - &H8000000

        '        Dim loopvar2 As Integer

        '        loopvar2 = 0

        '        outputlevel4 = outputlevel4 & vbLf

        '        outputlevel4 = outputlevel4 & ".align 2" & vbLf

        '        outputlevel4 = outputlevel4 & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts_4:" & vbLf

        '        While ReverseHEX(ReadHEX(LoadedROM, LevelScript2Pointer + (loopvar2 * 8), 2)) <> "0000"

        '            outputlevel4 = outputlevel4 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript4Pointer + (loopvar2 * 8), 2)) & "  @Variable" & vbLf
        '            outputlevel4 = outputlevel4 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript4Pointer + 2 + (loopvar2 * 8), 2)) & "  @Value to run" & vbLf
        '            outputlevel4 = outputlevel4 & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript4Pointer + 4 + (loopvar2 * 8), 4)) & "  @Pointer" & vbLf

        '            loopvar2 = loopvar2 + 1
        '        End While

        '        outputlevel4 = outputlevel4 & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, LevelScript4Pointer + (loopvar2 * 8), 2)) & "  @Terminator" & vbLf

        '    Else

        '        outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) & "  @Type" & vbLf
        '        outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Level_Scripts + 1 + (loopvar * 5), 4)) & "  @Pointer" & vbLf

        '    End If

        '    loopvar = loopvar + 1
        'End While

        'outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Map_Level_Scripts + (loopvar * 5), 1)) & "  @Terminator" & vbLf

        'outputtext = outputtext & outputlevel2
        'outputtext = outputtext & outputlevel4

        ''Connections

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Connections_Header:" & vbLf

        'If Map_Connection_Header = -134217728 Then
        '    Connection_Num = 0
        '    Connection_Pointer = 0
        '    outputtext = outputtext & "    .long    0x0" & "  @Number of Connections" & vbLf
        '    outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Connections" & "  @Pointer to Connections" & vbLf

        'Else

        '    Connection_Num = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Connection_Header, 4)))
        '    Connection_Pointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Connection_Header + 4, 4))) - &H8000000

        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Connection_Header, 4)) & "  @Number of Connections" & vbLf
        '    outputtext = outputtext & "    .long    " & "Bank" & MapBank & "_Map" & MapNumber & "_Connections" & "  @Pointer to Connections" & vbLf

        'End If

        'loopvar = 0

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Connections:" & vbLf

        'While loopvar < Connection_Num

        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, Connection_Pointer + (loopvar * 12), 4)) & "  @Direction" & vbLf
        '    outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, Connection_Pointer + 4 + (loopvar * 12), 4)) & "  @Offset Reference" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Connection_Pointer + 8 + (loopvar * 12), 1)) & "  @Map Bank" & vbLf
        '    outputtext = outputtext & "    .byte    0x" & (ReadHEX(LoadedROM, Connection_Pointer + 9 + (loopvar * 12), 1)) & "  @Map Number" & vbLf
        '    outputtext = outputtext & "    .short    0x" & ReverseHEX(ReadHEX(LoadedROM, Connection_Pointer + 10 + (loopvar * 12), 2)) & "  @Filler" & vbLf

        '    loopvar = loopvar + 1
        'End While

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_Border:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_Border.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_MapData:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_MapData.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks.bin""" & vbLf

        'outputtext = outputtext & vbLf

        'outputtext = outputtext & ".align 2" & vbLf

        'outputtext = outputtext & "Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors:" & vbLf
        'outputtext = outputtext & " .incbin ""map_data/Bank" & MapBank & "_Map" & MapNumber & "/Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin""" & vbLf

    End Sub

End Class
