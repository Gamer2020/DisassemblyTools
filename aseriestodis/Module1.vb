Imports System.IO
Imports System.Drawing
Imports VB = Microsoft.VisualBasic

Module Module1

    Private FrontPalette As Color() = New Color(&H10 - 1) {}
    Private BackPalette As Color() = New Color(&H10 - 1) {}

    Private FrontSprite As Byte()
    Private BackSprite As Byte()

    Private AnimationNormalPalette As Color() = New Color(&H10 - 1) {}
    Private AnimationShinyPalette As Color() = New Color(&H10 - 1) {}

    Private AnimationNormalSprite As Byte()
    Private AnimationShinySprite As Byte()

    Sub Main()
        'Load arguments
        Dim strArg() As String
        strArg = Command().Split(" ")

        Dim LoadAnimationFlag As Boolean

        'Display help
        If strArg(0) = "" Then
            Console.WriteLine("Write help...")
            End
        End If

        If File.Exists(strArg(0)) Then

            Dim ImgFile As String = strArg(0)

            Dim SplitName As String() = ImgFile.Split("\")
            Dim JustFileName As String = SplitName(SplitName.Count - 1)

            Dim mainbitmap As New Bitmap(ImgFile)

            If Directory.Exists(AppPath & "output\graphics\pokemon\anim_front_pics\") = False Then
                Directory.CreateDirectory(AppPath & "output\graphics\pokemon\anim_front_pics\")
            End If

            If Directory.Exists(AppPath & "output\graphics\pokemon\back_pics\") = False Then
                Directory.CreateDirectory(AppPath & "output\graphics\pokemon\back_pics\")
            End If

            If Directory.Exists(AppPath & "output\graphics\pokemon\front_pics\") = False Then
                Directory.CreateDirectory(AppPath & "output\graphics\pokemon\front_pics\")
            End If

            If Directory.Exists(AppPath & "output\graphics\pokemon\palettes\") = False Then
                Directory.CreateDirectory(AppPath & "output\graphics\pokemon\palettes\")
            End If

            If mainbitmap.Height = 128 And mainbitmap.Width = 256 Then
                LoadAnimationFlag = True
            Else
                LoadAnimationFlag = False

            End If

            FrontSprite = New Byte(2048) {}
            BackSprite = New Byte(2048) {}

            Dim ONormalBackBitmap As Bitmap = New Bitmap(&H40, &H40)
            Dim ONormalFrontBitmap As Bitmap = New Bitmap(&H40, &H40)
            Dim OShinyBackBitmap As Bitmap = New Bitmap(&H40, &H40)
            Dim OShinyFrontBitmap As Bitmap = New Bitmap(&H40, &H40)



            Dim ONormalBackBitmapAnimation As Bitmap = New Bitmap(&H40, &H40)
            Dim ONormalFrontBitmapAnimation As Bitmap = New Bitmap(&H40, &H80)
            Dim OShinyBackBitmapAnimation As Bitmap = New Bitmap(&H40, &H40)
            Dim OShinyFrontBitmapAnimation As Bitmap = New Bitmap(&H40, &H80)

            AnimationNormalSprite = New Byte(4096) {}
            AnimationShinySprite = New Byte(4096) {}

            BitmapBLT(mainbitmap, ONormalFrontBitmap, 0, 0, 0, 0, &H40, &H40, Color.FromArgb(&HFF, 200, 200, &HA8))
            BitmapBLT(mainbitmap, OShinyFrontBitmap, 0, 0, &H40, 0, &H40, &H40, Color.FromArgb(&HFF, 200, 200, &HA8))
            BitmapBLT(mainbitmap, ONormalBackBitmap, 0, 0, &H80, 0, &H40, &H40, Color.FromArgb(&HFF, 200, 200, &HA8))
            BitmapBLT(mainbitmap, OShinyBackBitmap, 0, 0, &HC0, 0, &H40, &H40, Color.FromArgb(&HFF, 200, 200, &HA8))

            If LoadAnimationFlag = True Then
                BitmapBLT(mainbitmap, ONormalFrontBitmapAnimation, 0, 0, 0, 0, &H40, &H80, Color.FromArgb(&HFF, 200, 200, &HA8))
                BitmapBLT(mainbitmap, OShinyFrontBitmapAnimation, 0, 0, &H40, 0, &H40, &H80, Color.FromArgb(&HFF, 200, 200, &HA8))
                BitmapBLT(mainbitmap, ONormalBackBitmapAnimation, 0, 0, &H80, 0, &H40, &H40, Color.FromArgb(&HFF, 200, 200, &HA8))
                BitmapBLT(mainbitmap, OShinyBackBitmapAnimation, 0, 0, &HC0, 0, &H40, &H40, Color.FromArgb(&HFF, 200, 200, &HA8))

            End If

            'synchpals

            Dim num As Byte
            Dim palcolor As Color
            Dim flag As Boolean = False

            Array.Clear(FrontPalette, 0, &H10 - 1)
            Array.Clear(BackPalette, 0, &H10 - 1)

            If LoadAnimationFlag = True Then
                Array.Clear(AnimationNormalPalette, 0, &H10 - 1)
                Array.Clear(AnimationShinyPalette, 0, &H10 - 1)

            End If

            Dim num11 As Integer = ((1 * &H40) - 1)
            Dim ivar As UInteger = 0
            Do While (ivar <= num11)
                Dim num3 As UInteger = 0
                Do
                    palcolor = GetQuantizedPixel(ONormalFrontBitmap, num3, ivar)
                    If Not Enumerable.Contains(Of Color)(FrontPalette, palcolor) Then
                        FrontPalette(num) = palcolor
                        BackPalette(num) = GetQuantizedPixel(OShinyFrontBitmap, num3, ivar)
                        num = ((num + 1))
                        If (num > 15) Then
                            flag = True
                            Exit Do
                        End If
                    End If
                    num3 += 1
                Loop While (num3 <= &H3F)
                If flag Then
                    Exit Do
                End If
                ivar += 1
            Loop

            Dim num12 As Integer = ((1 * &H40) - 1)
            Dim j As UInteger = 0
            Do While (j <= num12)
                Dim num5 As UInteger = 0
                Do
                    palcolor = GetQuantizedPixel(ONormalBackBitmap, num5, j)
                    If Not Enumerable.Contains(Of Color)(FrontPalette, palcolor) Then
                        FrontPalette(num) = palcolor
                        BackPalette(num) = GetQuantizedPixel(OShinyBackBitmap, num5, j)
                        num = ((num + 1))
                        If (num > 15) Then
                            flag = True
                            Exit Do
                        End If
                    End If
                    num5 += 1
                Loop While (num5 <= &H3F)
                If flag Then
                    Exit Do
                End If
                j += 1
            Loop

            Dim num13 As Integer = ((1 * &H40) - 1)
            Dim k As UInteger = 0
            Do While (k <= num13)
                Dim num7 As UInteger = 0
                Do
                    palcolor = GetQuantizedPixel(OShinyFrontBitmap, num7, k)
                    If Not Enumerable.Contains(Of Color)(BackPalette, palcolor) Then
                        BackPalette(num) = palcolor
                        FrontPalette(num) = GetQuantizedPixel(ONormalFrontBitmap, num7, k)
                        num = CByte((num + 1))
                        If (num > 15) Then
                            flag = True
                            Exit Do
                        End If
                    End If
                    num7 += 1
                Loop While (num7 <= &H3F)
                If flag Then
                    Exit Do
                End If
                k += 1
            Loop

            Dim num14 As Integer = ((1 * &H40) - 1)
            Dim m As UInteger = 0
            Do While (m <= num14)
                Dim num9 As UInteger = 0
                Do
                    palcolor = GetQuantizedPixel(OShinyBackBitmap, num9, m)
                    If Not Enumerable.Contains(Of Color)(BackPalette, palcolor) Then
                        BackPalette(num) = palcolor
                        FrontPalette(num) = GetQuantizedPixel(ONormalBackBitmap, num9, m)
                        num = CByte((num + 1))
                        If (num > 15) Then
                            flag = True
                            Exit Do
                        End If
                    End If
                    num9 += 1
                Loop While (num9 <= &H3F)
                If flag Then
                    Exit Do
                End If
                m += 1
            Loop

            Dim n As Integer = num
            Do While (n <= 15)
                FrontPalette(n) = Color.Black
                BackPalette(n) = Color.Black
                n += 1
            Loop

            If LoadAnimationFlag = True Then
                AnimationNormalPalette = FrontPalette
                AnimationShinyPalette = BackPalette
            End If

            ConvertBitmapToPalette(ONormalFrontBitmap, FrontPalette, True)
            ConvertBitmapToPalette(OShinyFrontBitmap, BackPalette, True)
            ConvertBitmapToPalette(ONormalBackBitmap, FrontPalette, True)
            ConvertBitmapToPalette(OShinyBackBitmap, BackPalette, True)

            If LoadAnimationFlag = True Then
                ConvertBitmapToPalette(ONormalFrontBitmapAnimation, AnimationNormalPalette, True)
                ConvertBitmapToPalette(OShinyFrontBitmapAnimation, AnimationShinyPalette, True)
                ConvertBitmapToPalette(ONormalBackBitmapAnimation, AnimationNormalPalette, True)
                ConvertBitmapToPalette(OShinyBackBitmapAnimation, AnimationShinyPalette, True)
            End If

            SynchSprite(FrontSprite, ONormalFrontBitmap, OShinyFrontBitmap)
            SynchSprite(BackSprite, ONormalBackBitmap, OShinyBackBitmap)

            If LoadAnimationFlag = True Then
                'SynchSprite2(AnimationNormalSprite, ONormalFrontBitmapAnimation, OShinyFrontBitmapAnimation)
            End If

            Dim convertedimage1 As Byte()
            Dim convertedpal1 As Byte()
            Dim convertedimage2 As Byte()
            Dim convertedpal2 As Byte()

            BitmapBLT(mainbitmap, OShinyBackBitmap, 0, 0, &HC0, 0, &H40, &H40, Color.FromArgb(&HFF, 200, 200, &HA8))

            ConvertBitmapToPalette(ONormalFrontBitmapAnimation, AnimationNormalPalette, True)

            'convertedimage1 = CompressBytes(SaveBitmapToArray(ONormalFrontBitmapAnimation, AnimationNormalPalette))
            ' convertedimage2 = CompressBytes(BackSprite)

            'convertedpal1 = (CompressBytes(HexStringToByteArray(ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(0))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(1))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(2))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(3))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(4))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(5))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(6))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(7))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(8))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(9))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(10))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(11))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(12))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(13))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(14))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(AnimationNormalPalette(15))), 4)))))
            'convertedpal2 = (CompressBytes(HexStringToByteArray(ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(0))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(1))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(2))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(3))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(4))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(5))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(6))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(7))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(8))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(9))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(10))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(11))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(12))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(13))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(14))), 4)) & ReverseHEX(VB.Right("0000" & Hex(ColorToRGB16(BackPalette(15))), 4)))))

            'File.WriteAllBytes(ImgFile.Replace(".png", "_Animation.bin"), convertedimage1)
            'File.WriteAllBytes(ImgFile.Replace(".png", "_Normal.pal"), convertedpal1)
            'File.WriteAllBytes(ImgFile.Replace(".png", "_Back.bin"), convertedimage2)
            'File.WriteAllBytes(ImgFile.Replace(".png", "_Shiny.pal"), convertedpal2)


            Dim ColorpalForIndex As Imaging.ColorPalette
            Dim aniRect As New Rectangle(0, 0, 64, 128)
            Dim backfrontRect As New Rectangle(0, 0, 64, 64)

            Dim FrontSpriteBitmap As Bitmap = New Bitmap(64, 64)
            Dim BackSpriteBitmap As Bitmap = New Bitmap(64, 64)

            LoadBitmapFromArray(FrontSprite, FrontPalette, FrontSpriteBitmap, 64, 64)

            Dim indexedFrontSprite As Bitmap = FrontSpriteBitmap.Clone(backfrontRect, Imaging.PixelFormat.Format4bppIndexed)

            ColorpalForIndex = indexedFrontSprite.Palette

            ColorpalForIndex.Entries(0) = FrontPalette(0)
            ColorpalForIndex.Entries(1) = FrontPalette(1)
            ColorpalForIndex.Entries(2) = FrontPalette(2)
            ColorpalForIndex.Entries(3) = FrontPalette(3)
            ColorpalForIndex.Entries(4) = FrontPalette(4)
            ColorpalForIndex.Entries(5) = FrontPalette(5)
            ColorpalForIndex.Entries(6) = FrontPalette(6)
            ColorpalForIndex.Entries(7) = FrontPalette(7)
            ColorpalForIndex.Entries(8) = FrontPalette(8)
            ColorpalForIndex.Entries(9) = FrontPalette(9)
            ColorpalForIndex.Entries(10) = FrontPalette(10)
            ColorpalForIndex.Entries(11) = FrontPalette(11)
            ColorpalForIndex.Entries(12) = FrontPalette(12)
            ColorpalForIndex.Entries(13) = FrontPalette(13)
            ColorpalForIndex.Entries(14) = FrontPalette(14)
            ColorpalForIndex.Entries(15) = FrontPalette(15)

            indexedFrontSprite.Palette = ColorpalForIndex

            Dim bmpdatafront As System.Drawing.Imaging.BitmapData = indexedFrontSprite.LockBits(backfrontRect, Drawing.Imaging.ImageLockMode.ReadWrite, indexedFrontSprite.PixelFormat)
            Dim pointerfront As IntPtr = bmpdatafront.Scan0

            Dim numofbytesFront As Integer = Math.Abs(bmpdatafront.Stride) * indexedFrontSprite.Height
            Dim DatavalFront(numofbytesFront - 1) As Byte

            System.Runtime.InteropServices.Marshal.Copy(pointerfront, DatavalFront, 0, numofbytesFront)


            DatavalFront = WriteImageToBits(DatavalFront, FrontSpriteBitmap, FrontPalette)

            System.Runtime.InteropServices.Marshal.Copy(DatavalFront, 0, pointerfront, numofbytesFront)

            indexedFrontSprite.UnlockBits(bmpdatafront)

            'indexedFrontSprite.MakeTransparent()

            'backsprite

            LoadBitmapFromArray(BackSprite, BackPalette, BackSpriteBitmap, 64, 64)

            Dim indexedBackSprite As Bitmap = BackSpriteBitmap.Clone(backfrontRect, Imaging.PixelFormat.Format4bppIndexed)

            ColorpalForIndex = indexedBackSprite.Palette


            ColorpalForIndex.Entries(0) = BackPalette(0)
            ColorpalForIndex.Entries(1) = BackPalette(1)
            ColorpalForIndex.Entries(2) = BackPalette(2)
            ColorpalForIndex.Entries(3) = BackPalette(3)
            ColorpalForIndex.Entries(4) = BackPalette(4)
            ColorpalForIndex.Entries(5) = BackPalette(5)
            ColorpalForIndex.Entries(6) = BackPalette(6)
            ColorpalForIndex.Entries(7) = BackPalette(7)
            ColorpalForIndex.Entries(8) = BackPalette(8)
            ColorpalForIndex.Entries(9) = BackPalette(9)
            ColorpalForIndex.Entries(10) = BackPalette(10)
            ColorpalForIndex.Entries(11) = BackPalette(11)
            ColorpalForIndex.Entries(12) = BackPalette(12)
            ColorpalForIndex.Entries(13) = BackPalette(13)
            ColorpalForIndex.Entries(14) = BackPalette(14)
            ColorpalForIndex.Entries(15) = BackPalette(15)

            indexedBackSprite.Palette = ColorpalForIndex

            Dim bmpdataback As System.Drawing.Imaging.BitmapData = indexedBackSprite.LockBits(backfrontRect, Drawing.Imaging.ImageLockMode.ReadWrite, indexedBackSprite.PixelFormat)
            Dim pointerback As IntPtr = bmpdataback.Scan0

            Dim numofbytesback As Integer = Math.Abs(bmpdataback.Stride) * indexedBackSprite.Height
            Dim DatavalBack(numofbytesback - 1) As Byte

            System.Runtime.InteropServices.Marshal.Copy(pointerback, DatavalBack, 0, numofbytesback)

            DatavalBack = WriteImageToBits(DatavalBack, BackSpriteBitmap, BackPalette)

            System.Runtime.InteropServices.Marshal.Copy(DatavalBack, 0, pointerback, numofbytesback)

            indexedBackSprite.UnlockBits(bmpdataback)

            'indexedBackSprite.MakeTransparent()

            'animation

            If LoadAnimationFlag = True Then

                Dim indexedONormalFrontBitmapAnimation As Bitmap = ONormalFrontBitmapAnimation.Clone(aniRect, Imaging.PixelFormat.Format4bppIndexed)

                ColorpalForIndex = indexedONormalFrontBitmapAnimation.Palette

                ColorpalForIndex.Entries(0) = AnimationNormalPalette(0)
                ColorpalForIndex.Entries(1) = AnimationNormalPalette(1)
                ColorpalForIndex.Entries(2) = AnimationNormalPalette(2)
                ColorpalForIndex.Entries(3) = AnimationNormalPalette(3)
                ColorpalForIndex.Entries(4) = AnimationNormalPalette(4)
                ColorpalForIndex.Entries(5) = AnimationNormalPalette(5)
                ColorpalForIndex.Entries(6) = AnimationNormalPalette(6)
                ColorpalForIndex.Entries(7) = AnimationNormalPalette(7)
                ColorpalForIndex.Entries(8) = AnimationNormalPalette(8)
                ColorpalForIndex.Entries(9) = AnimationNormalPalette(9)
                ColorpalForIndex.Entries(10) = AnimationNormalPalette(10)
                ColorpalForIndex.Entries(11) = AnimationNormalPalette(11)
                ColorpalForIndex.Entries(12) = AnimationNormalPalette(12)
                ColorpalForIndex.Entries(13) = AnimationNormalPalette(13)
                ColorpalForIndex.Entries(14) = AnimationNormalPalette(14)
                ColorpalForIndex.Entries(15) = AnimationNormalPalette(15)


                indexedONormalFrontBitmapAnimation.Palette = ColorpalForIndex

                Dim bmpdataani As System.Drawing.Imaging.BitmapData = indexedONormalFrontBitmapAnimation.LockBits(aniRect, Drawing.Imaging.ImageLockMode.ReadWrite, indexedONormalFrontBitmapAnimation.PixelFormat)
                Dim pointerani As IntPtr = bmpdataani.Scan0

                Dim numofbytesani As Integer = Math.Abs(bmpdataani.Stride) * indexedONormalFrontBitmapAnimation.Height
                Dim Datavaluse(numofbytesani - 1) As Byte

                System.Runtime.InteropServices.Marshal.Copy(pointerani, Datavaluse, 0, numofbytesani)


                Datavaluse = WriteImageToBits(Datavaluse, ONormalFrontBitmapAnimation, AnimationNormalPalette)

                System.Runtime.InteropServices.Marshal.Copy(Datavaluse, 0, pointerani, numofbytesani)

                indexedONormalFrontBitmapAnimation.UnlockBits(bmpdataani)

                'indexedONormalFrontBitmapAnimation.MakeTransparent()

                indexedONormalFrontBitmapAnimation.Save(AppPath & "output\graphics\pokemon\anim_front_pics\" & JustFileName.Replace(".png", "_front_pic.png"), Imaging.ImageFormat.Png)

            End If

            indexedFrontSprite.Save(AppPath & "output\graphics\pokemon\front_pics\" & JustFileName.Replace(".png", "_still_front_pic.png"), Imaging.ImageFormat.Png)
            indexedBackSprite.Save(AppPath & "output\graphics\pokemon\back_pics\" & JustFileName.Replace(".png", "_back_pic.png"), Imaging.ImageFormat.Png)

            Dim normalpaltext As String = ""
            Dim shinypaltext As String = ""
            Dim palsaveloop As Integer = 0

            normalpaltext = normalpaltext & "JASC-PAL" & vbCrLf
            normalpaltext = normalpaltext & "0100" & vbCrLf
            normalpaltext = normalpaltext & "16" & vbCrLf

            While palsaveloop < FrontPalette.Count

                normalpaltext = normalpaltext & FrontPalette(palsaveloop).R & " " & FrontPalette(palsaveloop).G & " " & FrontPalette(palsaveloop).B & vbCrLf

                palsaveloop = palsaveloop + 1
            End While

            palsaveloop = 0

            shinypaltext = shinypaltext & "JASC-PAL" & vbCrLf
            shinypaltext = shinypaltext & "0100" & vbCrLf
            shinypaltext = shinypaltext & "16" & vbCrLf

            While palsaveloop < BackPalette.Count

                shinypaltext = shinypaltext & BackPalette(palsaveloop).R & " " & BackPalette(palsaveloop).G & " " & BackPalette(palsaveloop).B & vbCrLf

                palsaveloop = palsaveloop + 1
            End While

            File.WriteAllText(AppPath & "output\graphics\pokemon\palettes\" & JustFileName.Replace(".png", "_palette.pal"), normalpaltext)
            File.WriteAllText(AppPath & "output\graphics\pokemon\palettes\" & JustFileName.Replace(".png", "_shiny_palette.pal"), shinypaltext)

        Else

            Console.WriteLine("File doesn't exist...")

            End

        End If




    End Sub

    Private Sub SynchSprite(ByRef SpriteArray As Byte(), ByRef NormalSprite As Bitmap, ByRef ShinySprite As Bitmap)
        Dim num As Byte
        Dim num1 As UInteger = 0
        Dim length As Double = CDbl(CInt(SpriteArray.Length)) / 256 - 1
        For i As Double = 0 To length Step 1
            Dim num2 As Integer = 0
            Do
                Dim num3 As Integer = 0
                Do
                    Dim num4 As Integer = 0
                    Do
                        Dim num5 As Byte = CByte(Array.IndexOf(Of Color)(FrontPalette, NormalSprite.GetPixel(num2 * 8 + num4 * 2, CInt(Math.Round(i * 8 + CDbl(num3))))))
                        Dim num6 As Byte = CByte(Array.IndexOf(Of Color)(FrontPalette, NormalSprite.GetPixel(num2 * 8 + num4 * 2 + 1, CInt(Math.Round(i * 8 + CDbl(num3))))))
                        Dim num7 As Byte = CByte(Array.IndexOf(Of Color)(BackPalette, ShinySprite.GetPixel(num2 * 8 + num4 * 2, CInt(Math.Round(i * 8 + CDbl(num3))))))
                        Dim num8 As Byte = CByte(Array.IndexOf(Of Color)(BackPalette, ShinySprite.GetPixel(num2 * 8 + num4 * 2 + 1, CInt(Math.Round(i * 8 + CDbl(num3))))))
                        num = If(num7 <= num5, num5, num7)
                        num = If(num8 <= num6, num Or CByte((num6 << 4)), num Or CByte((num8 << 4)))
                        SpriteArray(num1) = num
                        num1 = CUInt((CULng(num1) + CLng(1)))
                        num4 = num4 + 1
                    Loop While num4 <= 3
                    num3 = num3 + 1
                Loop While num3 <= 7
                num2 = num2 + 1
            Loop While num2 <= 7
        Next

    End Sub

    Private Function WriteImageToBits(InputDataBytes() As Byte, InputImage As Bitmap, pals As Color()) As Byte()
        Dim OutputdataBytes As Byte()
        Dim outputwidth As Integer = 0
        Dim outputheight As Integer = 0
        Dim inputwidth As Integer = 0
        Dim inputheight As Integer = 0
        Dim byteloopvar As Integer = 0
        Dim pixelcounter As Integer = 0

        OutputdataBytes = New Byte(InputDataBytes.Count) {}

        While byteloopvar < InputDataBytes.Count

            OutputdataBytes(byteloopvar) = "&H" & Hex(GetIndexOfColor(InputImage.GetPixel(CalcualtePixelWidthLocation(pixelcounter, InputImage.Height, InputImage.Width), CalcualtePixelHeightLocation(pixelcounter, InputImage.Height, InputImage.Width)), pals)) & Hex(GetIndexOfColor(InputImage.GetPixel(CalcualtePixelWidthLocation(pixelcounter + 1, InputImage.Height, InputImage.Width), CalcualtePixelHeightLocation(pixelcounter + 1, InputImage.Height, InputImage.Width)), pals))

            pixelcounter = pixelcounter + 2
            byteloopvar = byteloopvar + 1
        End While
        Return OutputdataBytes
    End Function

    Private Function GetIndexOfColor(InputColor As Color, pal As Color()) As Integer
        Dim output As Integer = 0
        Dim loopvar As Integer = 0

        While loopvar < 16
            If InputColor = pal(loopvar) Then
                output = loopvar
                Exit While
            End If
            loopvar = loopvar + 1
        End While

        Return output
    End Function

    Private Function CalcualtePixelHeightLocation(pixelcount As Integer, imageheight As Integer, imagewidth As Integer) As Integer
        Dim outputpoint As Integer = 0

        Dim counter As Integer = pixelcount
        Dim endcounter As Integer = 0
        While counter > (imagewidth - 1)

            counter = counter - imagewidth

            endcounter = endcounter + 1
        End While
        outputpoint = endcounter
        Return outputpoint
    End Function

    Private Function CalcualtePixelWidthLocation(pixelcount As Integer, imageheight As Integer, imagewidth As Integer) As Integer
        Dim outputpoint As Integer = 0

        Dim counter As Integer = pixelcount

        While counter > (imagewidth - 1)

            counter = counter - (imagewidth)
        End While

        outputpoint = counter
        Return outputpoint
    End Function

End Module
