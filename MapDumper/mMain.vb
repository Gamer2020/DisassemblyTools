Imports System.IO



Module mMain

    Public LoadedROM As String
    'Public ledrom As ROM
    Public AppPath As String = System.AppDomain.CurrentDomain.BaseDirectory() & IIf(Right(System.AppDomain.CurrentDomain.BaseDirectory(), 1) = "\", "", "\")
    Public i As Integer
    Public FileNum As Integer
    Public header As String = "xxxx"
    Public header2 As String
    Public header3 As String
    Public lwut As String
    Public SkipVar As Integer
    Public x As Integer

    Public ExportName As String

    Public SelectedPath As String

    Public Point2MapBankPointers As Integer
    Public MapBankPointers As Integer
    Public BankPointer As Integer
    Public HeaderPointer As Integer
    Public MapBank As Integer
    Public MapNumber As Integer
    Public BankOffset As Integer
    Public HeaderOffset As Integer
    Public MapData As String
    Public Const TileSetSize As Integer = 16
    Public Const TileSize As Integer = 8 'constant used for tile sizes.
    Public Const TileWidth As Integer = 8
    Public Const TilesPerRow As Integer = 16

    Public Map_Footer As Integer
    Public Map_Events As Integer
    Public Map_Level_Scripts As Integer
    Public Map_Connection_Header As Integer

    Public LevelScript2Pointer As Integer
    Public LevelScript4Pointer As Integer

    Public Connection_Pointer As Integer
    Public Connection_Num As Integer

    Public NPC_Num As Integer
    Public Warp_Num As Integer
    Public Script_Event_Num As Integer
    Public SignPost_Num As Integer

    Public NPC_Pointer As Integer
    Public Warp_Pointer As Integer
    Public Script_Event_Pointer As Integer
    Public SignPost_Pointer As Integer

    Public MapHeight As Integer
    Public MapWidth As Integer
    Public BorderPointer As Integer
    Public MapDataPointer As Integer
    Public PrimaryTilesetPointer As Integer
    Public SecondaryTilesetPointer As Integer
    Public BorderHeight As Integer
    Public BorderWidth As Integer

    Public PrimaryTilesetCompression As Integer
    Public SecondaryTilesetCompression As Integer
    Public PrimaryImagePointer As Integer
    Public SecondaryImagePointer As Integer
    Public PrimaryPalPointer As Integer
    Public SecondaryPalPointer As Integer
    Public PrimaryBlockSetPointer As Integer
    Public SecondaryBlockSetPointer As Integer
    Public PrimaryBehaviourPointer As Integer
    Public SecondaryBehaviourPointer As Integer

    Public PrimaryPals As String
    Public SecondaryPals As String

    Public PrimaryTilesImg As String
    Public SecondaryTilesImg As String

    Public PrimaryBlocks As String
    Public SecondaryBlocks As String

    Public PrimaryBehaviors As String
    Public SecondaryBehaviors As String

    Public BorderData As String
    Public MapPermData As String

    Public Function GetINIFileLocation()

        If System.IO.File.Exists((LoadedROM).Substring(0, LoadedROM.Length - 4) & ".ini") = True Then

            Return (LoadedROM).Substring(0, LoadedROM.Length - 4) & ".ini"
        Else

            Return AppPath & "ini\roms.ini"
        End If

    End Function

    Public Function MakeFreeSpaceString(NeededLength As Integer, Optional NeedByteString As String = "FF")

        Dim PrivLoopVar As Integer
        Dim OutBuffThing As String = ""

        PrivLoopVar = 0

        While (PrivLoopVar < NeededLength)

            OutBuffThing = OutBuffThing & NeedByteString

            PrivLoopVar = PrivLoopVar + 1
        End While

        MakeFreeSpaceString = OutBuffThing
    End Function

    Public Function DecapString(input As String) As String

        Dim LoopVar As Integer
        Dim outputstring As String = ""
        Dim capflag As Boolean = True

        LoopVar = 0

        While LoopVar < Len(input)

            LoopVar = LoopVar + 1

            If GetChar(input, LoopVar) = " " Then
                outputstring = outputstring & " "
                capflag = True
            Else
                If capflag = True Then

                    outputstring = outputstring & UCase(GetChar(input, LoopVar))
                    capflag = False

                ElseIf capflag = False Then

                    outputstring = outputstring & LCase(GetChar(input, LoopVar))

                End If
            End If

        End While

        DecapString = outputstring
    End Function

    Public Sub OutPutError(message As String)

        Dim errorfile As String = AppPath & "errors.txt"

        System.IO.File.AppendAllText(errorfile, message & vbCrLf)

    End Sub

    Public Function ByteToSignedInt(InputByte As Byte) As Integer
        Dim ReturnVar As Integer


        If InputByte > &H7F Then
            Dim BinaryVar As String = (Convert.ToString(InputByte, 2))
            ReturnVar = ((Convert.ToInt32(Mid(BinaryVar, 2, 7), 2)) - 128)
        Else
            ReturnVar = InputByte
        End If

        ByteToSignedInt = ReturnVar

    End Function

    Public Function SignedIntToHex(InputInt As Integer) As String

        Dim ReturnVar As String


        If InputInt < 0 Then

            Dim BinaryVar As String = (Convert.ToString(InputInt + 128, 2))
            BinaryVar = "1" & BinaryVar
            ReturnVar = Hex((Convert.ToInt32(BinaryVar, 2)))

        Else
            ReturnVar = Hex(InputInt)
        End If

        SignedIntToHex = ReturnVar

    End Function

    Public Function ByteArrayToHexString(inputarray As Byte()) As String

        Dim HexString As String = ""

        For Each b As Byte In inputarray
            HexString = HexString & MakeProperByte(b)
        Next

        ByteArrayToHexString = HexString
    End Function

    Public Function MakeProperByte(DaByte As Byte) As String
        Dim OutputByte As String


        If Len(Hex(DaByte)) = 1 Then
            OutputByte = "0" & Hex(DaByte)
        Else
            OutputByte = Hex(DaByte)
        End If

        MakeProperByte = OutputByte

    End Function



    Public Function Get2Bytes(bytesin As Byte(), local As Integer) As String

        Get2Bytes = MakeProperByte(bytesin(local)) & MakeProperByte(bytesin(local + 1))

    End Function

    Public Function HexStringToByteArray(input As String) As Byte()

        Dim output((input.Length / 2) - 1) As Byte
        Dim loopvar As Integer

        loopvar = 0

        While loopvar < (input.Length / 2)

            If loopvar = 0 Then
                output(loopvar) = "&H" & input.Substring(loopvar * 2, 2)
            Else
                output(loopvar) = "&H" & input.Substring(loopvar * 2, 2)
            End If

            loopvar = loopvar + 1

        End While



        HexStringToByteArray = output
    End Function


End Module
