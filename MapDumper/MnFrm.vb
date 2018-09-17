Imports System.IO
Imports VB = Microsoft.VisualBasic

Public Class MnFrm

<<<<<<< HEAD
    Public OutPutHeaderText As String


    Public outputtextFooter As String
    Public Outputprimaryts As String
    Public Outputsecondaryts As String
    Public Outputmetatiles As String
    Public Outputgraphicsfile As String
    Public LayoutsTableText As String
    Public LayoutsText As String

    Public outputlevel2 As String
    Public outputlevel4 As String
=======
    Dim outputtext As String
    Dim outputtextFooter As String
    Dim Outputprimaryts As String
    Dim Outputsecondaryts As String
    Dim Outputmetatiles As String
    Dim Outputgraphicsfile As String
    Dim LayoutsTableText As String
    Dim LayoutsText As String
    Dim outputlevel2 As String
    Dim outputlevel4 As String
>>>>>>> parent of 4138b8c... Starting over. Trying to be neater.

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

<<<<<<< HEAD
            GetInitialPointers()
            GenerateHeader()
            GenerateEvents()
            GenerateLayout()
=======
            MapBank = (MapsAndBanks.SelectedNode.Parent.Index)
            MapNumber = (MapsAndBanks.SelectedNode.Index)
>>>>>>> parent of 4138b8c... Starting over. Trying to be neater.

            Point2MapBankPointers = Int32.Parse(GetString(GetINIFileLocation(), header, "Pointer2PointersToMapBanks", ""), System.Globalization.NumberStyles.HexNumber)

            MapBankPointers = ((Val(("&H" & ReverseHEX(ReadHEX(LoadedROM, Point2MapBankPointers, 4)))) - &H8000000))


            BankPointer = ((Val(("&H" & ReverseHEX(ReadHEX(LoadedROM, MapBankPointers + (MapBank * 4), 4)))) - &H8000000))


            HeaderPointer = ((Val(("&H" & ReverseHEX(ReadHEX(LoadedROM, BankPointer + (MapNumber * 4), 4)))) - &H8000000))

            If ((header2 = "BPR") Or (header2 = "BPG")) Then

                ExportName = MapNameList.Items.Item(("&H" & (ReadHEX(LoadedROM, HeaderPointer + 20, 1))) - &H58).replace(" ", "_")

            ElseIf (mMain.header2 = "BPE") Then

                ExportName = MapNameList.Items.Item("&H" & (ReadHEX(LoadedROM, HeaderPointer + 20, 1))).replace(" ", "_")

            ElseIf ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then

                ExportName = MapNameList.Items.Item("&H" & (ReadHEX(LoadedROM, HeaderPointer + 20, 1))).replace(" ", "_")

            End If

            'If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then


            '    FRLoadOutput2()


            'ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then

            '    EMLoadOutput2()

            'End If

            DoOutput()

            If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/")) Then
                System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/tilesets/")
            End If

            If File.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/headers.inc") Then
                File.AppendAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/headers.inc", Outputprimaryts & Outputsecondaryts)
            Else
                File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/headers.inc", Outputprimaryts & Outputsecondaryts)
            End If

            If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")) Then
                System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")
            End If

            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "layout" & ".inc", outputtextFooter)
            WriteHEX(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "border.bin", 0, BorderData)
            WriteHEX(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "map.bin", 0, MapPermData)

            If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/maps/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")) Then
                System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/maps/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")
            End If

            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/maps/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "header" & ".inc", outputtext)

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

                    'behaviors2 = behaviors2 & VB.Right("00" & ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin", 0 + loopvar, 1), 2)
                    'behaviors2 = behaviors2 & VB.Right("00" & ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin", 0 + loopvar + 1, 2), 2)

                    'loopvar = loopvar + 4

                    behaviors2 = behaviors2 & VB.Right("00" & ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin", 0 + loopvar, 1), 2)
                    'behaviors2 = behaviors2 & VB.Right("00" & ReadHEX(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin", 0 + loopvar + 1, 2), 2)

                    loopvar = loopvar + 2

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

            If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")) Then
                System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")
            End If

            If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")) Then
                System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")
            End If

            If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/")) Then
                System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/")
            End If

            If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/")) Then
                System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/")
            End If

            Outputmetatiles = vbTab & ".align 1" & vbLf
            Outputmetatiles = Outputmetatiles & "gMetatiles_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary::" & vbLf
            Outputmetatiles = Outputmetatiles & vbTab & " .incbin " & """" & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatiles.bin" & """" & vbLf
            Outputmetatiles = Outputmetatiles & vbLf
            Outputmetatiles = Outputmetatiles & vbTab & ".align 1" & vbLf
            Outputmetatiles = Outputmetatiles & "gMetatileAttributes_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary::" & vbLf
            Outputmetatiles = Outputmetatiles & vbTab & " .incbin " & """" & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatile_attributes.bin" & """" & vbLf
            Outputmetatiles = Outputmetatiles & vbLf

            Outputmetatiles = Outputmetatiles & vbTab & ".align 1" & vbLf
            Outputmetatiles = Outputmetatiles & "gMetatiles_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Secondary::" & vbLf
            Outputmetatiles = Outputmetatiles & vbTab & " .incbin " & """" & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatiles.bin" & """" & vbLf
            Outputmetatiles = Outputmetatiles & vbLf
            Outputmetatiles = Outputmetatiles & vbTab & ".align 1" & vbLf
            Outputmetatiles = Outputmetatiles & "gMetatileAttributes_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Secondary::" & vbLf
            Outputmetatiles = Outputmetatiles & vbTab & " .incbin " & """" & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatile_attributes.bin" & """" & vbLf
            Outputmetatiles = Outputmetatiles & vbLf

            If File.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/metatiles.inc") Then
                File.AppendAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/metatiles.inc", Outputmetatiles)
            Else
                File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/metatiles.inc", Outputmetatiles)
            End If

            If File.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatiles.bin") Then
                File.Delete(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatiles.bin")
            End If

            If File.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatile_attributes.bin") Then
                File.Delete(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatile_attributes.bin")
            End If

            If File.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatiles.bin") Then
                File.Delete(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatiles.bin")
            End If

            If File.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatile_attributes.bin") Then
                File.Delete(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatile_attributes.bin")
            End If

            Outputgraphicsfile = vbTab & ".align 2" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & "gTilesetTiles_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary::" & "" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/tiles.4bpp.lz" & """" & vbLf & vbLf

            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".align 2" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & "gTilesetPalettes_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary::" & "" & vbLf

            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/00.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/01.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/02.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/03.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/04.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/05.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/06.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/07.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/08.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/09.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/10.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/11.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/12.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/13.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/14.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/primary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/15.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbLf

            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".align 2" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & "gTilesetTiles_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Secondary::" & "" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/tiles.4bpp.lz" & """" & vbLf & vbLf

            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".align 2" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & "gTilesetPalettes_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Secondary::" & "" & vbLf

            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/00.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/01.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/02.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/03.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/04.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/05.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/06.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/07.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/08.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/09.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/10.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/11.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/12.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/13.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/14.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbTab & ".incbin " & """" & "data/tilesets/secondary/palettes/" & ExportName & "_" & MapBank & "_" & MapNumber & "/15.gbapal" & """" & vbLf
            Outputgraphicsfile = Outputgraphicsfile & vbLf

            If File.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/graphics.inc") Then
                File.AppendAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/graphics.inc", Outputgraphicsfile)
            Else
                File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/graphics.inc", Outputgraphicsfile)
            End If

            File.Move(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBlocks.bin", FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatiles.bin")
            File.Move(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryBehaviors.bin", FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatile_attributes.bin")
            File.Move(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBlocks.bin", FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatiles.bin")
            File.Move(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryBehaviors.bin", FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/metatile_attributes.bin")

            Dim palsvar00 As Color() = New Color(15) {}
            Dim palsvar01 As Color() = New Color(15) {}
            Dim palsvar02 As Color() = New Color(15) {}
            Dim palsvar03 As Color() = New Color(15) {}
            Dim palsvar04 As Color() = New Color(15) {}
            Dim palsvar05 As Color() = New Color(15) {}
            Dim palsvar06 As Color() = New Color(15) {}
            Dim palsvar07 As Color() = New Color(15) {}
            Dim palsvar08 As Color() = New Color(15) {}
            Dim palsvar09 As Color() = New Color(15) {}
            Dim palsvar10 As Color() = New Color(15) {}
            Dim palsvar11 As Color() = New Color(15) {}
            Dim palsvar12 As Color() = New Color(15) {}
            Dim palsvar13 As Color() = New Color(15) {}
            Dim palsvar14 As Color() = New Color(15) {}
            Dim palsvar15 As Color() = New Color(15) {}

            Using fs As New FileStream(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin", FileMode.Open, FileAccess.Read)
                Using r As New BinaryReader(fs)
                    fs.Position = 0
                    palsvar00 = LoadPaletteFromROM(fs)
                    palsvar01 = LoadPaletteFromROM(fs)
                    palsvar02 = LoadPaletteFromROM(fs)
                    palsvar03 = LoadPaletteFromROM(fs)
                    palsvar04 = LoadPaletteFromROM(fs)
                    palsvar05 = LoadPaletteFromROM(fs)
                    r.Close()
                    fs.Close()
                End Using
            End Using

            Using fs As New FileStream(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal.bin", FileMode.Open, FileAccess.Read)
                Using r As New BinaryReader(fs)
                    fs.Position = 0
                    palsvar06 = LoadPaletteFromROM(fs)
                    palsvar07 = LoadPaletteFromROM(fs)
                    palsvar08 = LoadPaletteFromROM(fs)
                    palsvar09 = LoadPaletteFromROM(fs)
                    palsvar10 = LoadPaletteFromROM(fs)
                    palsvar11 = LoadPaletteFromROM(fs)
                    palsvar12 = LoadPaletteFromROM(fs)
                    palsvar13 = LoadPaletteFromROM(fs)
                    palsvar14 = LoadPaletteFromROM(fs)
                    palsvar15 = LoadPaletteFromROM(fs)
                    r.Close()
                    fs.Close()
                End Using
            End Using

            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/00.pal", ColorToPalText(palsvar00))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/01.pal", ColorToPalText(palsvar01))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/02.pal", ColorToPalText(palsvar02))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/03.pal", ColorToPalText(palsvar03))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/04.pal", ColorToPalText(palsvar04))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/05.pal", ColorToPalText(palsvar05))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/06.pal", ColorToPalText(palsvar06))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/07.pal", ColorToPalText(palsvar07))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/08.pal", ColorToPalText(palsvar08))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/09.pal", ColorToPalText(palsvar09))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/10.pal", ColorToPalText(palsvar10))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/11.pal", ColorToPalText(palsvar11))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/12.pal", ColorToPalText(palsvar12))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/13.pal", ColorToPalText(palsvar13))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/14.pal", ColorToPalText(palsvar14))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/15.pal", ColorToPalText(palsvar15))

            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/00.pal", ColorToPalText(palsvar00))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/01.pal", ColorToPalText(palsvar01))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/02.pal", ColorToPalText(palsvar02))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/03.pal", ColorToPalText(palsvar03))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/04.pal", ColorToPalText(palsvar04))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/05.pal", ColorToPalText(palsvar05))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/06.pal", ColorToPalText(palsvar06))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/07.pal", ColorToPalText(palsvar07))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/08.pal", ColorToPalText(palsvar08))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/09.pal", ColorToPalText(palsvar09))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/10.pal", ColorToPalText(palsvar10))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/11.pal", ColorToPalText(palsvar11))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/12.pal", ColorToPalText(palsvar12))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/13.pal", ColorToPalText(palsvar13))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/14.pal", ColorToPalText(palsvar14))
            File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/palettes/15.pal", ColorToPalText(palsvar15))

            If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin") Then
                File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryPal.bin")
            End If

            If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal.bin") Then
                File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryPal.bin")
            End If

            'Image code goes here.
            Dim PrimRect As New Rectangle(0, 0, 8, (20480 * 2) / 8)

            Dim PrimTile As Bitmap = New Bitmap(PrimRect.Width, PrimRect.Height, Imaging.PixelFormat.Format4bppIndexed)

            Dim ColorpalForIndex As Imaging.ColorPalette

            ColorpalForIndex = PrimTile.Palette

            ColorpalForIndex.Entries(0) = palsvar00(0)
            ColorpalForIndex.Entries(1) = palsvar00(1)
            ColorpalForIndex.Entries(2) = palsvar00(2)
            ColorpalForIndex.Entries(3) = palsvar00(3)
            ColorpalForIndex.Entries(4) = palsvar00(4)
            ColorpalForIndex.Entries(5) = palsvar00(5)
            ColorpalForIndex.Entries(6) = palsvar00(6)
            ColorpalForIndex.Entries(7) = palsvar00(7)
            ColorpalForIndex.Entries(8) = palsvar00(8)
            ColorpalForIndex.Entries(9) = palsvar00(9)
            ColorpalForIndex.Entries(10) = palsvar00(10)
            ColorpalForIndex.Entries(11) = palsvar00(11)
            ColorpalForIndex.Entries(12) = palsvar00(12)
            ColorpalForIndex.Entries(13) = palsvar00(13)
            ColorpalForIndex.Entries(14) = palsvar00(14)
            ColorpalForIndex.Entries(15) = palsvar00(15)

            PrimTile.Palette = ColorpalForIndex

            Dim bmpdataprim As System.Drawing.Imaging.BitmapData = PrimTile.LockBits(PrimRect, Drawing.Imaging.ImageLockMode.ReadWrite, PrimTile.PixelFormat)
            Dim pointerprim As IntPtr = bmpdataprim.Scan0

            Dim numofbytesprim As Integer = Math.Abs(bmpdataprim.Stride) * PrimTile.Height
            Dim Datavaluse(numofbytesprim - 1) As Byte

            System.Runtime.InteropServices.Marshal.Copy(pointerprim, Datavaluse, 0, numofbytesprim)

            Datavaluse = LoadTilesToBitsforimage(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles.bin", palsvar00, True, PrimRect.Height, PrimRect.Width, numofbytesprim)

            System.Runtime.InteropServices.Marshal.Copy(Datavaluse, 0, pointerprim, numofbytesprim)

            PrimTile.UnlockBits(bmpdataprim)

            PrimTile.Save(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/tiles.png", Imaging.ImageFormat.Png)

            Dim SecRect As New Rectangle(0, 0, 8, ((16384 * 2) / 8))

            Dim SecTile As Bitmap = New Bitmap(SecRect.Width, SecRect.Height, Imaging.PixelFormat.Format4bppIndexed)

            SecTile.Palette = ColorpalForIndex

            Dim bmpdatasec As System.Drawing.Imaging.BitmapData = SecTile.LockBits(SecRect, Drawing.Imaging.ImageLockMode.ReadWrite, SecTile.PixelFormat)
            Dim pointersec As IntPtr = bmpdataSec.Scan0

            Dim numofbytessec As Integer = Math.Abs(bmpdataSec.Stride) * SecTile.Height
            Dim Datavaluse2(numofbytessec - 1) As Byte

            System.Runtime.InteropServices.Marshal.Copy(pointersec, Datavaluse2, 0, numofbytessec)

            Datavaluse2 = LoadTilesToBitsforimage2(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles.bin", palsvar00, True, SecRect.Height, SecRect.Width, numofbytessec)

            System.Runtime.InteropServices.Marshal.Copy(Datavaluse2, 0, pointersec, numofbytessec)

            SecTile.UnlockBits(bmpdatasec)

            SecTile.Save(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/tiles.png", Imaging.ImageFormat.Png)


            If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles.bin") Then
                File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_PrimaryTiles.bin")
            End If

            If File.Exists(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles.bin") Then
                File.Delete(FolderBrowserDialog1.SelectedPath & "\Bank" & MapBank & "_Map" & MapNumber & "_SecondaryTiles.bin")
            End If

            'layouts text stuff

            LayoutsText = vbTab & ".include " & """" & "/data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/layouts.inc" & """" & vbLf

            LayoutsTableText = vbTab & ".4byte " & ExportName & "_" & MapBank & "_" & MapNumber & "_Layout" & vbLf

            If File.Exists(FolderBrowserDialog1.SelectedPath & "/data/layouts.inc") Then
                File.AppendAllText(FolderBrowserDialog1.SelectedPath & "/data/layouts.inc", LayoutsText)
            Else
                File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/layouts.inc", LayoutsText)
            End If

            If File.Exists(FolderBrowserDialog1.SelectedPath & "/data/layouts_table.inc") Then
                File.AppendAllText(FolderBrowserDialog1.SelectedPath & "/data/layouts_table.inc", LayoutsTableText)
            Else

                LayoutsTableText = vbTab & ".align 2" & vbLf & "gMapLayouts::" & vbLf & LayoutsTableText

                File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/layouts_table.inc", LayoutsTableText)
            End If

            Me.Text = "Map Dumper"
            Me.UseWaitCursor = False
            Me.Enabled = True
            Me.BringToFront()

        End If
    End Sub


<<<<<<< HEAD
        OutPutHeaderText = OutPutHeaderText & vbTab & ".4byte " & ExportName & "_" & MapBank & "_" & MapNumber & "_Layout" & "  @Footer" & vbLf
        OutPutHeaderText = OutPutHeaderText & vbTab & ".4byte " & ExportName & "_" & MapBank & "_" & MapNumber & "_MapEvents" & "  @Events" & vbLf
        OutPutHeaderText = OutPutHeaderText & vbTab & ".4byte " & "0x0" & "  @Level Scripts" & vbLf
        OutPutHeaderText = OutPutHeaderText & vbTab & ".4byte " & "0x0" & "  @Connections" & vbLf
=======
    Private Sub EMLoadOutput2()
>>>>>>> parent of 4138b8c... Starting over. Trying to be neater.

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
        outputtext = outputtext & ExportName & "_" & MapBank & "_" & MapNumber & "_Header:" & vbLf

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

        outputtextFooter = outputtextFooter & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBorder::" & vbLf
        outputtextFooter = outputtextFooter & vbTab & ".incbin " & """" & "data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "border.bin" & """" & vbLf & vbLf

        outputtextFooter = outputtextFooter & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBlockdata::" & vbLf
        outputtextFooter = outputtextFooter & vbTab & ".incbin " & """" & "data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "map.bin" & """" & vbLf & vbLf

        outputtextFooter = outputtextFooter & vbTab & ".align 2" & vbLf

        outputtextFooter = outputtextFooter & ExportName & "_" & MapBank & "_" & MapNumber & "_Layout::" & vbLf

        MapWidth = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer, 4)))

        outputtextFooter = outputtextFooter & "    .4byte    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer, 4)) & "  @Map Width" & vbLf

        MapHeight = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 4, 4)))

        outputtextFooter = outputtextFooter & "    .4byte    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 4, 4)) & "  @Map Height" & vbLf

        BorderPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 8, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    " & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBorder" & "  @Border Data" & vbLf

        MapDataPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 12, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    " & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBlockdata" & "  @Map data / Movement Permissons" & vbLf

        PrimaryTilesetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 16, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    " & ExportName & "_" & MapBank & "_" & MapNumber & "_PrimaryTileset" & "  @Primary Tileset" & vbLf

        SecondaryTilesetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 20, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    " & ExportName & "_" & MapBank & "_" & MapNumber & "_SecondaryTileset" & "  @Secondary Tileset" & vbLf

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

    Private Sub DoOutput()

        Dim loopvar As Integer

        'outputtext = ".align 2" & vbLf & vbLf
        outputtext = outputtext & ExportName & "_" & MapBank & "_" & MapNumber & "_Header:" & vbLf

        'Header

        Map_Footer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer, 4))) - &H8000000

        'outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Footer" & "  @Footer" & vbLf
        outputtext = outputtext & "    .4byte    " & ExportName & "_" & MapBank & "_" & MapNumber & "_Layout" & "  @Footer" & vbLf

        Map_Events = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 4, 4))) - &H8000000

        'outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Events" & "  @Events" & vbLf
        outputtext = outputtext & "    .4byte    " & "0x0" & "  @Events" & vbLf

        Map_Level_Scripts = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 8, 4))) - &H8000000

        'outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Level_Scripts" & "  @Level Scripts" & vbLf
        outputtext = outputtext & "    .4byte    " & "0x0" & "  @Level Scripts" & vbLf

        Map_Connection_Header = ("&H" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 12, 4))) - &H8000000

        'outputtext = outputtext & "    .4byte    " & "Bank" & MapBank & "_Map" & MapNumber & "_Connections_Header" & "  @Connections" & vbLf
        outputtext = outputtext & "    .4byte    " & "0x0" & "  @Connections" & vbLf

        outputtext = outputtext & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 16, 2)) & "  @Music" & vbLf
        outputtext = outputtext & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, HeaderPointer + 18, 2)) & "  @Footer ID" & vbLf
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

        outputtextFooter = outputtextFooter & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBorder::" & vbLf
        outputtextFooter = outputtextFooter & vbTab & ".incbin " & """" & "data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "border.bin" & """" & vbLf & vbLf

        outputtextFooter = outputtextFooter & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBlockdata::" & vbLf
        outputtextFooter = outputtextFooter & vbTab & ".incbin " & """" & "data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "map.bin" & """" & vbLf & vbLf

        outputtextFooter = outputtextFooter & vbTab & ".align 2" & vbLf

        outputtextFooter = outputtextFooter & ExportName & "_" & MapBank & "_" & MapNumber & "_Layout::" & vbLf

        MapWidth = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer, 4)))

        outputtextFooter = outputtextFooter & "    .4byte    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer, 4)) & "  @Map Width" & vbLf

        MapHeight = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 4, 4)))

        outputtextFooter = outputtextFooter & "    .4byte    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 4, 4)) & "  @Map Height" & vbLf

        BorderPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 8, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    " & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBorder" & "  @Border Data" & vbLf

        MapDataPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 12, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    " & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBlockdata" & "  @Map data / Movement Permissons" & vbLf

        PrimaryTilesetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 16, 4))) - &H8000000

        outputtextFooter = outputtextFooter & "    .4byte    gTileset_" & ExportName & "_" & MapBank & "_" & MapNumber & "_PrimaryTileset" & "  @Primary Tileset" & vbLf

<<<<<<< HEAD
        SecondaryTilesetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 20, 4))) - &H8000000
=======
            GetInitialPointers()
            GenerateHeader()
>>>>>>> parent of 45db6ae... Starting over...

        outputtextFooter = outputtextFooter & "    .4byte    gTileset_" & ExportName & "_" & MapBank & "_" & MapNumber & "_SecondaryTileset" & "  @Secondary Tileset" & vbLf

        'BorderHeight = ("&H" & (ReadHEX(LoadedROM, Map_Footer + 24, 1)))
        'BorderWidth = ("&H" & (ReadHEX(LoadedROM, Map_Footer + 25, 1)))

        BorderHeight = 2
        BorderWidth = 2

        BorderData = ReadHEX(LoadedROM, BorderPointer, (BorderHeight * BorderWidth) * 2)

        MapPermData = ReadHEX(LoadedROM, MapDataPointer, (MapHeight * MapWidth) * 2)

        'Primary Tileset

        'outputtext = outputtext & vbLf

        Outputprimaryts = vbTab & ".align 2" & vbLf

        Outputprimaryts = Outputprimaryts & "gTileset_" & ExportName & "_" & MapBank & "_" & MapNumber & "_PrimaryTileset::" & vbLf

        PrimaryTilesetCompression = "&H" & (ReadHEX(LoadedROM, PrimaryTilesetPointer, 1))

        Outputprimaryts = Outputprimaryts & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer, 1)) & "  @Is Compressed?" & vbLf
        Outputprimaryts = Outputprimaryts & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer + 1, 1)) & "  @Is secondary tileset" & vbLf
        Outputprimaryts = Outputprimaryts & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 2, 2)) & "  @Padding?" & vbLf

        PrimaryImagePointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 4, 4))) - &H8000000

        Outputprimaryts = Outputprimaryts & "    .4byte    gTilesetTiles_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary" & "  @Image Pointer" & vbLf

        PrimaryPalPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 8, 4))) - &H8000000

        Outputprimaryts = Outputprimaryts & "    .4byte    gTilesetPalettes_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary" & "  @Pallete Pointer" & vbLf

        PrimaryBlockSetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 12, 4))) - &H8000000

        Outputprimaryts = Outputprimaryts & "    .4byte    gMetatiles_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary" & "  @blockset_data" & vbLf


        If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
            PrimaryBehaviourPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 20, 4))) - &H8000000
        ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
            PrimaryBehaviourPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 16, 4))) - &H8000000
        End If

        Outputprimaryts = Outputprimaryts & "    .4byte    gMetatileAttributes_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary" & "  @behavioural_bg_bytes" & vbLf

        Outputprimaryts = Outputprimaryts & "    .4byte    0x0" & "  @Animation routine" & vbLf

        Outputprimaryts = Outputprimaryts & vbLf

        'If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
        'outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 16, 4)) & "  @Animation routine" & vbLf
        'ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
        'outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 20, 4)) & "  @Animation routine" & vbLf
        'End If

        If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
            PrimaryPals = ReadHEX(LoadedROM, PrimaryPalPointer, 7 * (16 * 2))
        ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
            PrimaryPals = ReadHEX(LoadedROM, PrimaryPalPointer, 6 * (16 * 2))
        End If

        If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
            If PrimaryTilesetCompression = 1 Then

                PrimaryTilesImg = MapTilesCompressedtoHexStringFRPrim(PrimaryImagePointer, PrimaryPalPointer)

            ElseIf PrimaryTilesetCompression = 0 Then

                PrimaryTilesImg = ReadHEX(LoadedROM, PrimaryImagePointer, 20480)

            End If
        ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
            If PrimaryTilesetCompression = 1 Then

                PrimaryTilesImg = MapTilesCompressedtoHexString(PrimaryImagePointer, PrimaryPalPointer)

            ElseIf PrimaryTilesetCompression = 0 Then

                PrimaryTilesImg = ReadHEX(LoadedROM, PrimaryImagePointer, 16384)

            End If
        End If

        If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
            PrimaryBlocks = ReadHEX(LoadedROM, PrimaryBlockSetPointer, 16 * 640)

<<<<<<< HEAD
            PrimaryBehaviors = ReadHEX(LoadedROM, PrimaryBehaviourPointer, 4 * 640)
        ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
            PrimaryBlocks = ReadHEX(LoadedROM, PrimaryBlockSetPointer, 16 * 512)
=======
        OutPutHeaderText = OutPutHeaderText & vbTab & ".4byte " & ExportName & "_" & MapBank & "_" & MapNumber & "_Layout" & "  @Footer" & vbLf
        OutPutHeaderText = OutPutHeaderText & vbTab & ".4byte " & "0x0" & "  @Events" & vbLf
        OutPutHeaderText = OutPutHeaderText & vbTab & ".4byte " & "0x0" & "  @Level Scripts" & vbLf
        OutPutHeaderText = OutPutHeaderText & vbTab & ".4byte " & "0x0" & "  @Connections" & vbLf
>>>>>>> parent of 45db6ae... Starting over...

            PrimaryBehaviors = ReadHEX(LoadedROM, PrimaryBehaviourPointer, 2 * 512)
        End If



        'Secondary Tileset

        Outputsecondaryts = vbTab & ".align 2" & vbLf

        Outputsecondaryts = Outputsecondaryts & "gTileset_" & ExportName & "_" & MapBank & "_" & MapNumber & "_SecondaryTileset::" & vbLf

        SecondaryTilesetCompression = "&H" & (ReadHEX(LoadedROM, SecondaryTilesetPointer, 1))

        Outputsecondaryts = Outputsecondaryts & "    .byte    0x" & (ReadHEX(LoadedROM, SecondaryTilesetPointer, 1)) & "  @Is Compressed?" & vbLf
        Outputsecondaryts = Outputsecondaryts & "    .byte    0x" & (ReadHEX(LoadedROM, SecondaryTilesetPointer + 1, 1)) & "  @Is secondary tileset" & vbLf
        Outputsecondaryts = Outputsecondaryts & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 2, 2)) & "  @Padding?" & vbLf

        SecondaryImagePointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 4, 4))) - &H8000000

        Outputsecondaryts = Outputsecondaryts & "    .4byte    gTilesetTiles_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Secondary" & "  @Image Pointer" & vbLf

        SecondaryPalPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 8, 4))) - &H8000000

        Outputsecondaryts = Outputsecondaryts & "    .4byte    gTilesetPalettes_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Secondary" & "  @Pallete Pointer" & vbLf

        SecondaryBlockSetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 12, 4))) - &H8000000

        Outputsecondaryts = Outputsecondaryts & "    .4byte    gMetatiles_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Secondary" & "  @blockset_data" & vbLf

        If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
            SecondaryBehaviourPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 20, 4))) - &H8000000
        ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
            SecondaryBehaviourPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 16, 4))) - &H8000000
        End If

        Outputsecondaryts = Outputsecondaryts & "    .4byte    gMetatileAttributes_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Secondary" & "  @behavioural_bg_bytes" & vbLf

        Outputsecondaryts = Outputsecondaryts & "    .4byte    0x0" & "  @Animation routine" & vbLf

        Outputsecondaryts = Outputsecondaryts & vbLf

        'If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
        'outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 16, 4)) & "  @Animation routine" & vbLf
        'ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
        'outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, SecondaryTilesetPointer + 20, 4)) & "  @Animation routine" & vbLf
        'End If

        SecondaryPals = ReadHEX(LoadedROM, SecondaryPalPointer, 13 * (16 * 2))

        If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
            If SecondaryTilesetCompression = 1 Then

                SecondaryTilesImg = MapTilesCompressedtoHexStringFRSec(SecondaryImagePointer, SecondaryPalPointer)


            ElseIf SecondaryTilesetCompression = 0 Then

                SecondaryTilesImg = ReadHEX(LoadedROM, SecondaryImagePointer, 12288)

            End If
        ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
            If SecondaryTilesetCompression = 1 Then

                SecondaryTilesImg = MapTilesCompressedtoHexString(SecondaryImagePointer, SecondaryPalPointer)


            ElseIf SecondaryTilesetCompression = 0 Then

                SecondaryTilesImg = ReadHEX(LoadedROM, SecondaryImagePointer, 16384)

            End If
        End If

        SecondaryBlocks = ReadHEX(LoadedROM, SecondaryBlockSetPointer, (Int32.Parse((GetString(AppPath & "ini\roms.ini", header, "NumberOfTilesInTilset" & Hex(SecondaryTilesetPointer), "")), System.Globalization.NumberStyles.HexNumber) + 1) * 16)

        SecondaryBehaviors = ReadHEX(LoadedROM, SecondaryBehaviourPointer, (Int32.Parse((GetString(AppPath & "ini\roms.ini", header, "NumberOfTilesInTilset" & Hex(SecondaryTilesetPointer), "")), System.Globalization.NumberStyles.HexNumber) + 1) * 2)

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

<<<<<<< HEAD
    Private Sub GenerateEvents()
        OutPutEventText = ExportName & "_" & MapBank & "_" & MapNumber & "_MapEvents::" & vbLf
        File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/maps/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "events" & ".inc", OutPutEventText)
    End Sub

    Private Sub GenerateLayout()
        OutPutLayoutText = ""

        OutPutLayoutText = OutPutLayoutText & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBorder::" & vbLf
        OutPutLayoutText = OutPutLayoutText & vbTab & ".incbin " & """" & "data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "border.bin" & """" & vbLf & vbLf

        OutPutLayoutText = OutPutLayoutText & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBlockdata::" & vbLf
        OutPutLayoutText = OutPutLayoutText & vbTab & ".incbin " & """" & "data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "map.bin" & """" & vbLf & vbLf

        OutPutLayoutText = OutPutLayoutText & vbTab & ".align 2" & vbLf
        OutPutLayoutText = OutPutLayoutText & ExportName & "_" & MapBank & "_" & MapNumber & "_Layout::" & vbLf
        OutPutLayoutText = OutPutLayoutText & "    .4byte    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer, 4)) & "  @Map Width" & vbLf
        OutPutLayoutText = OutPutLayoutText & "    .4byte    0x" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 4, 4)) & "  @Map Height" & vbLf
        OutPutLayoutText = OutPutLayoutText & "    .4byte    " & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBorder" & "  @Border Data" & vbLf
        OutPutLayoutText = OutPutLayoutText & "    .4byte    " & ExportName & "_" & MapBank & "_" & MapNumber & "_MapBlockdata" & "  @Map data / Movement Permissons" & vbLf
        OutPutLayoutText = OutPutLayoutText & "    .4byte    gTileset_" & ExportName & "_" & MapBank & "_" & MapNumber & "_PrimaryTileset" & "  @Primary Tileset" & vbLf
        OutPutLayoutText = OutPutLayoutText & "    .4byte    gTileset_" & ExportName & "_" & MapBank & "_" & MapNumber & "_SecondaryTileset" & "  @Secondary Tileset" & vbLf


        MapWidth = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer, 4)))
        MapHeight = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 4, 4)))
        BorderPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 8, 4))) - &H8000000
        MapDataPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 12, 4))) - &H8000000
        PrimaryTilesetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 16, 4))) - &H8000000
        SecondaryTilesetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, Map_Footer + 20, 4))) - &H8000000

        BorderHeight = 2
        BorderWidth = 2

        BorderData = ReadHEX(LoadedROM, BorderPointer, (BorderHeight * BorderWidth) * 2)

        MapPermData = ReadHEX(LoadedROM, MapDataPointer, (MapHeight * MapWidth) * 2)

        If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")) Then
            System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")
        End If

        File.WriteAllText(FolderBrowserDialog1.SelectedPath & "/data/layouts/" & ExportName & "_" & MapBank & "_" & MapNumber & "/" & "layout" & ".inc", OutPutLayoutText)


    End Sub

    Private Sub GeneratePrimaryTileset()

        Outputprimaryts = vbTab & ".align 2" & vbLf

        Outputprimaryts = Outputprimaryts & "gTileset_" & ExportName & "_" & MapBank & "_" & MapNumber & "_PrimaryTileset::" & vbLf

        PrimaryTilesetCompression = "&H" & (ReadHEX(LoadedROM, PrimaryTilesetPointer, 1))

        Outputprimaryts = Outputprimaryts & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer, 1)) & "  @Is Compressed?" & vbLf
        Outputprimaryts = Outputprimaryts & "    .byte    0x" & (ReadHEX(LoadedROM, PrimaryTilesetPointer + 1, 1)) & "  @Is secondary tileset" & vbLf
        Outputprimaryts = Outputprimaryts & "    .2byte    0x" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 2, 2)) & "  @Padding?" & vbLf

        PrimaryImagePointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 4, 4))) - &H8000000

        Outputprimaryts = Outputprimaryts & "    .4byte    gTilesetTiles_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary" & "  @Image Pointer" & vbLf

        PrimaryPalPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 8, 4))) - &H8000000

        Outputprimaryts = Outputprimaryts & "    .4byte    gTilesetPalettes_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary" & "  @Pallete Pointer" & vbLf

        PrimaryBlockSetPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 12, 4))) - &H8000000

        Outputprimaryts = Outputprimaryts & "    .4byte    gMetatiles_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary" & "  @blockset_data" & vbLf


        If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
            PrimaryBehaviourPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 20, 4))) - &H8000000
        ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
            PrimaryBehaviourPointer = ("&H" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 16, 4))) - &H8000000
        End If

        Outputprimaryts = Outputprimaryts & "    .4byte    gMetatileAttributes_" & ExportName & "_" & MapBank & "_" & MapNumber & "_Primary" & "  @behavioural_bg_bytes" & vbLf

        Outputprimaryts = Outputprimaryts & "    .4byte    0x0" & "  @Animation routine" & vbLf

        Outputprimaryts = Outputprimaryts & vbLf

        'If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
        'outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 16, 4)) & "  @Animation routine" & vbLf
        'ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
        'outputtext = outputtext & "    .long    0x" & ReverseHEX(ReadHEX(LoadedROM, PrimaryTilesetPointer + 20, 4)) & "  @Animation routine" & vbLf
        'End If

        If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
            PrimaryPals = ReadHEX(LoadedROM, PrimaryPalPointer, 7 * (16 * 2))
        ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
            PrimaryPals = ReadHEX(LoadedROM, PrimaryPalPointer, 6 * (16 * 2))
        End If

        If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
            If PrimaryTilesetCompression = 1 Then

                PrimaryTilesImg = MapTilesCompressedtoHexStringFRPrim(PrimaryImagePointer, PrimaryPalPointer)

            ElseIf PrimaryTilesetCompression = 0 Then

                PrimaryTilesImg = ReadHEX(LoadedROM, PrimaryImagePointer, 20480)

            End If
        ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
            If PrimaryTilesetCompression = 1 Then

                PrimaryTilesImg = MapTilesCompressedtoHexString(PrimaryImagePointer, PrimaryPalPointer)

            ElseIf PrimaryTilesetCompression = 0 Then

                PrimaryTilesImg = ReadHEX(LoadedROM, PrimaryImagePointer, 16384)

            End If
        End If

        If ((mMain.header2 = "BPR") Or (mMain.header2 = "BPG")) Then
            PrimaryBlocks = ReadHEX(LoadedROM, PrimaryBlockSetPointer, 16 * 640)

            PrimaryBehaviors = ReadHEX(LoadedROM, PrimaryBehaviourPointer, 4 * 640)
        ElseIf (mMain.header2 = "BPE") Or ((mMain.header2 = "AXP") Or (mMain.header2 = "AXV")) Then
            PrimaryBlocks = ReadHEX(LoadedROM, PrimaryBlockSetPointer, 16 * 512)

            PrimaryBehaviors = ReadHEX(LoadedROM, PrimaryBehaviourPointer, 2 * 512)
        End If

        If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")) Then
            System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/tilesets/primary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")
        End If

        If (Not System.IO.Directory.Exists(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")) Then
            System.IO.Directory.CreateDirectory(FolderBrowserDialog1.SelectedPath & "/data/tilesets/secondary/" & ExportName & "_" & MapBank & "_" & MapNumber & "/")
        End If

    End Sub

=======
>>>>>>> parent of 45db6ae... Starting over...
End Class
