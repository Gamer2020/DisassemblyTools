Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Module NSELZZ

    Public Enum CheckLz77Type
        Sprite
        Palette
    End Enum


    Public Function DecompressBytes(Data__1 As Byte()) As Byte()
        Dim data__2 As Byte()
        If Data__1(0) = &H10 Then
            Dim DataLength As Integer = BitConverter.ToInt32(New [Byte]() {Data__1(1), Data__1(2), Data__1(3), &H0}, 0)
            data__2 = New [Byte](DataLength - 1) {}

            Dim Offset As Integer = 4

            Dim watch As String = ""
            Dim i As Integer = 0
            Dim pos As Byte = 8

            While i < DataLength
                'br.BaseStream.Seek(Offset, SeekOrigin.Begin);
                If pos <> 8 Then
                    If watch(pos) = "0"(0) Then
                        'data[i] = br.ReadByte();
                        data__2(i) = Data__1(Offset)
                    Else
                        'byte[] r = br.ReadBytes(2);
                        Dim r As Byte() = New Byte() {Data__1(Offset), Data__1(Offset + 1)}
                        Dim length As Integer = r(0) >> 4
                        Dim start As Integer = ((r(0) - ((r(0) >> 4) << 4)) << 8) + r(1)
                        AmmendArray(data__2, i, i - start - 1, length + 3)
                        Offset += 1
                    End If
                    Offset += 1
                    i += 1

                    pos += 1
                Else
                    'watch = Convert.ToString(br.ReadByte(), 2);
                    watch = Convert.ToString(Data__1(Offset), 2)
                    While watch.Length <> 8
                        watch = Convert.ToString("0") & watch
                    End While
                    Offset += 1
                    pos = 0
                End If
            End While
            'br.Close();


            Return data__2
        Else
            Throw New Exception("This data is not Lz77 compressed!")
        End If
    End Function

    Private Sub AmmendArray(ByRef Bytes As Byte(), ByRef Index As Integer, Start As Integer, Length As Integer)
        Dim a As Integer = 0
        ' Act
        Dim r As Integer = 0
        ' Rel
        Dim Backup As Byte = 0

        If Index > 0 Then
            Backup = Bytes(Index - 1)
        End If

        While a <> Length
            If Index + r >= 0 AndAlso Start + r >= 0 AndAlso Index + a < Bytes.Length Then
                If Start + r >= Index Then
                    r = 0
                    Bytes(Index + a) = Bytes(Start + r)
                Else
                    Bytes(Index + a) = Bytes(Start + r)
                    Backup = Bytes(Index + r)
                End If
            End If
            a += 1
            r += 1
        End While

        Index += Length - 1
    End Sub


    ' For picking what type of Compression Look-up we want
    Public Enum CompressionMode
            Old
            ' Good
            [New]
            ' Perfect!
        End Enum

    Public Function CompressBytes(Data As Byte(), Optional Mode As CompressionMode = CompressionMode.[New]) As Byte()
        Dim header As Byte() = BitConverter.GetBytes(Data.Length)
        Dim Bytes As New List(Of Byte)()
        Dim PreBytes As New List(Of Byte)()
        Dim Watch As Byte = 0
        Dim ShortPosition As Byte = 2
        Dim ActualPosition As Integer = 2
        Dim match As Integer = -1

        Dim BestLength As Integer = 0

        ' Adds the Lz77 header to the bytes 0x10 3 bytes size reversed
        Bytes.Add(&H10)
        Bytes.Add(header(0))
        Bytes.Add(header(1))
        Bytes.Add(header(2))

        ' Lz77 Compression requires SOME starting data, so we provide the first 2 bytes
        PreBytes.Add(Data(0))
        PreBytes.Add(Data(1))

        ' Compress everything
        While ActualPosition < Data.Length
            'If we've compressed 8 of 8 bytes
            If ShortPosition = 8 Then
                ' Add the Watch Mask
                ' Add the 8 steps in PreBytes
                Bytes.Add(Watch)
                Bytes.AddRange(PreBytes)

                Watch = 0
                PreBytes.Clear()

                ' Back to 0 of 8 compressed bytes
                ShortPosition = 0
            Else
                ' If we are approaching the end
                If ActualPosition + 1 < Data.Length Then
                    ' Old NSE 1.x compression lookup
                    If Mode = CompressionMode.Old Then
                        match = SearchBytesOld(Data, ActualPosition, Math.Min(4096, ActualPosition))
                    ElseIf Mode = CompressionMode.[New] Then
                        ' New NSE 2.x compression lookup
                        match = SearchBytes(Data, ActualPosition, Math.Min(4096, ActualPosition), BestLength)
                    End If
                Else
                    match = -1
                End If

                ' If we have NOT found a match in the compression lookup
                If match = -1 Then
                    ' Add the byte
                    PreBytes.Add(Data(ActualPosition))
                    ' Add a 0 to the mask
                    Watch = BitConverter.GetBytes(CInt(Watch) << 1)(0)

                    ActualPosition += 1
                Else
                    ' How many bytes match
                    Dim length As Integer = -1

                    Dim start As Integer = match
                    If Mode = CompressionMode.Old OrElse BestLength = -1 Then
                        ' Old look-up technique
                        '#Region "GetLength_Old"
                        start = match

                        Dim Compatible As Boolean = True

                        While Compatible = True AndAlso length < 18 AndAlso length + ActualPosition < Data.Length - 1
                            length += 1
                            If Data(ActualPosition + length) <> Data(ActualPosition - start + length) Then
                                Compatible = False
                            End If
                            '#End Region
                        End While
                    ElseIf Mode = CompressionMode.[New] Then
                        ' New lookup (Perfect Compression!)
                        length = BestLength
                    End If

                    ' Add the rel-compression pointer (P) and length of bytes to copy (L)
                    ' Format: L P P P
                    Dim b As Byte() = BitConverter.GetBytes(((length - 3) << 12) + (start - 1))

                    b = New Byte() {b(1), b(0)}
                    PreBytes.AddRange(b)

                    ' Move to the next position
                    ActualPosition += length

                    ' Add a 1 to the bit Mask
                    Watch = BitConverter.GetBytes((CInt(Watch) << 1) + 1)(0)
                End If

                ' We've just compressed 1 more 8
                ShortPosition += 1


            End If
        End While

        ' Finnish off the compression
        If ShortPosition <> 0 Then
            'Tyeing up any left-over data compression
            Watch = BitConverter.GetBytes(CInt(Watch) << (8 - ShortPosition))(0)

            Bytes.Add(Watch)
            Bytes.AddRange(PreBytes)
        End If

        ' Return the Compressed bytes as an array!
        Return Bytes.ToArray()
    End Function

    Private Function SearchBytesOld(Data As Byte(), Index As Integer, Length As Integer) As Integer
        Dim found As Integer = -1
        Dim pos As Integer = 2

        If Index + 2 < Data.Length Then
            While pos < Length + 1 AndAlso found = -1
                If Data(Index - pos) = Data(Index) AndAlso Data(Index - pos + 1) = Data(Index + 1) Then

                    If Index > 2 Then
                        If Data(Index - pos + 2) = Data(Index + 2) Then
                            found = pos
                        Else
                            pos += 1
                        End If
                    Else
                        found = pos


                    End If
                Else
                    pos += 1
                End If
            End While

            Return found
        Else
            Return -1
        End If

    End Function

    Private Function SearchBytes(Data As Byte(), Index As Integer, Length As Integer, ByRef match As Integer) As Integer

        Dim pos As Integer = 2
        match = 0
        Dim found As Integer = -1

        If Index + 2 < Data.Length Then
            While pos < Length + 1 AndAlso match <> 18
                If Data(Index - pos) = Data(Index) AndAlso Data(Index - pos + 1) = Data(Index + 1) Then

                    If Index > 2 Then
                        If Data(Index - pos + 2) = Data(Index + 2) Then
                            Dim _match As Integer = 2
                            Dim Compatible As Boolean = True
                            While Compatible = True AndAlso _match < 18 AndAlso _match + Index < Data.Length - 1
                                _match += 1
                                If Data(Index + _match) <> Data(Index - pos + _match) Then
                                    Compatible = False
                                End If
                            End While
                            If _match > match Then
                                match = _match
                                found = pos

                            End If
                        End If
                        pos += 1
                    Else
                        found = pos
                        match = -1
                        pos += 1


                    End If
                Else
                    pos += 1
                End If
            End While

            Return found
        Else
            Return -1
        End If

    End Function



    'Public Shared Function CheckLz77(read As Read, Offset As Integer, Type As CheckLz77Type) As Integer
    '    Return CheckLz77(read.ReadBytes(Offset, 5), Type)
    'End Function

    Public Function CheckLz77(Header5Bytes As Byte(), Type As CheckLz77Type) As Integer
        If Header5Bytes(0) = &H10 Then
            Dim length As Integer = BitConverter.ToInt32(New [Byte]() {Header5Bytes(1), Header5Bytes(2), Header5Bytes(3), &H0}, 0)

            If Type = CheckLz77Type.Sprite AndAlso Header5Bytes(4) <= 63 AndAlso length >= 64 AndAlso length Mod 8 = 0 Then
                Return length
            ElseIf Type = CheckLz77Type.Palette AndAlso length = &H20 AndAlso Header5Bytes(4) <= 63 Then
                Return length
            Else
                Return -1


            End If
        Else
            Return -1
        End If
    End Function

    '=======================================================
    'Service provided by Telerik (www.telerik.com)
    'Conversion powered by NRefactory.
    'Twitter: @telerik
    'Facebook: facebook.com/telerik
    '=======================================================

End Module
