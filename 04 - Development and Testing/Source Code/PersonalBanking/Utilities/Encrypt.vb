''' Encrypt
'''     Maneja los algoritmos de encritpación y decriptación de la banca en línea
'''
''' Autor:  Pedro Ruiz - código basado en la versión en Vb6 por cspsr y csphf -
'''
''' Fecha de creación: 31 de Mayo de 2007
'''
''' Código propiedad de Canella, S.A.
Public Class Encrypt

#Region "Constantes"
    Private Const GKey = 10799  ' Constante para Encryp/Decryp
    Private Const GHash = 6791  ' Constante para obtener el "Hash"
    Private Const HexCHU = "0123456789ABCDEF"   ' Dígitos Hexadecimales.

#End Region

#Region "Atributos"
    ''' Llaves de encriptacion
    Private C1 As Long
    Private C2 As Long
#End Region


    ' PadZeroL:  Rellena de caracteres "0" a la izquierda a un string
    ' enviado (Cadena), para una longitud específica (Longitud).
    Private Function PadZeroL(ByVal cadena As String, ByVal Longitud As Integer) As String
        Dim Result As String

        Result = cadena
        While (Len(Result) < Longitud)
            Result = "0" & Result
        End While

        PadZeroL = Result
    End Function

    ' Hash:  Obtiene un número Hash entre 0 y 65536 a partir de un string
    ' enviado (Cadena).  Para ello utiliza la constante "GHash" y corrimiento
    ' de bits a la izquierda.
    Private Function Hash(ByVal cadena As String) As Integer
        Dim i As Integer
        Dim Result As Long
        Result = 0
        For i = 1 To Len(cadena)
            Result = ((Result * 8) Xor Asc(Mid(cadena, i, 1))) Mod GHash
            If (Result >= 65536) Then
                Result = Result Mod 65536
            End If
        Next
        ' (Result shl 3) = (Result * 2^3)
        Hash = Result
    End Function

    ' Encrypt:  Función de encripción 1.  Utiliza la llave inicial "GKey".
    Private Function Encrypt(ByVal cadena As String) As String
        Dim i As Integer
        Dim L As Integer
        Dim S As String
        Dim Key As Double

        On Error GoTo ErrorHandler
        L = Len(cadena) ' Longitud de la cadena a encriptar.
        Key = GKey      ' Inicialización de la llave.

        For i = 1 To L
            S = S & Chr(Asc(Mid(cadena, i, 1)) Xor Int(Key / 256))
            ' Referencias:
            '   . Asc(Mid(Cadena, I, 1))    = Ord(S[I])
            '   . (Key / 256)               = (Key shr 8)
            '   . X shr Y                   = X / (2^Y)   -> 2^8 = 256
            Key = (Asc(Mid(S, i, 1)) + Key) * C1 + C2
            If (Key >= 65536) Then
                Key = Key Mod 65536
            End If
        Next

        Encrypt = S
        Exit Function

ErrorHandler:
        If (Err.Number = 6) Then ' Al calcular el valor "Key" da un 'Overflow'.
            Key = C1 + C2
            Err.Clear()
            Resume
        End If
    End Function

    ' Decrypt:  Función de desencripción 1.
    Private Function Decrypt(ByVal cadena As String) As String
        Dim i As Integer
        Dim L As Integer
        Dim S As String
        Dim Key As Double

        On Error GoTo ErrorHandler
        L = Len(cadena) ' Longitud de la cadena a desencriptar.
        Key = GKey      ' Inicialización de la llave.

        For i = 1 To L
            S = S & Chr(Asc(Mid(cadena, i, 1)) Xor Int(Key / 256))
            ' Referencias:
            '   . Asc(Mid(Cadena, I, 1))    = Ord(S[I])
            '   . (Key / 256)               = (Key shr 8)
            '   . X shr Y                   = X / (2^Y)   -> 2^8 = 256
            Key = (Asc(Mid(cadena, i, 1)) + Key) * C1 + C2
            If (Key >= 65536) Then
                Key = Key Mod 65536
            End If
        Next

        Decrypt = S
        Exit Function

ErrorHandler:
        If (Err.Number = 6) Then ' Al calcular el valor "Key" da un 'Overflow'.
            Key = C1 + C2
            Err.Clear()
            Resume
        End If
    End Function

    ' EncryptSS2:  Función de encripción 2.  Solamente se aplica a
    ' cadenas de longitud par.
    Private Function EncryptSS2(ByVal cadena As String) As String
        Dim i, LDIV2 As Integer
        Dim SResult, S1, S2 As String

        If (Len(cadena) Mod 2) = 0 Then
            LDIV2 = Int(Len(cadena) / 2)    ' LDIV2 := Length(S) div 2
            S1 = Mid(cadena, 1, LDIV2)
            S2 = Mid(cadena, LDIV2 + 1, LDIV2)
            For i = 1 To LDIV2
                SResult = SResult & Mid(S1, i, 1) & Mid(S2, i, 1)
            Next
        End If

        EncryptSS2 = SResult
    End Function

    ' DecryptSS2:  Función de desencripción 2.
    Private Function DecryptSS2(ByVal cadena As String) As String
        Dim i, J, K, LDIV2 As Integer
        Dim S1, S2, SResult As String

        If (Len(cadena) Mod 2) = 0 Then
            LDIV2 = Int(Len(cadena) / 2)
            For i = 1 To LDIV2
                J = (i - 1) * 2 + 1
                K = J + 1
                S1 = S1 & Mid(cadena, J, 1)
                S2 = S2 & Mid(cadena, K, 1)
            Next
            SResult = S1 & S2
        End If

        DecryptSS2 = SResult
    End Function

    ' VoltearTexto:  Da vuelta al string enviado (Cadena).
    Private Function VoltearTexto(ByVal cadena As String) As String
        Dim i As Integer
        Dim S As String

        For i = Len(cadena) To 1 Step -1
            S = S & Mid(cadena, i, 1)
        Next
        VoltearTexto = S
    End Function

    ' CharAHex:  Traslada el código ASCII de los caracteres del string
    ' enviado (Cadena) a notación Hexadecimal.
    Private Function CharAHex(ByVal cadena As String) As String
        ' Convertir a Hex
        Dim i, J As Integer
        Dim Hex, Hex1, Hex2 As String
        Dim L As Integer

        L = Len(cadena)
        For i = 1 To L
            J = Asc(Mid(cadena, i, 1))
            Hex1 = UCase(Mid(HexCHU, Int(J / 16) + 1, 1))
            ' Referencia:
            '   . Hex1 := UpCase( HexCHU[ (J shr 4) + 1 ] )
            Hex2 = UCase(Mid(HexCHU, (J And 15) + 1, 1))
            ' Referencia:
            '   . Hex2 := UpCase( HexCHU[ (J and 15) + 1 ] )
            Hex = Hex & Hex1 & Hex2
        Next

        CharAHex = Hex
    End Function

    ' HexAChar:  Traslada un string enviado (Cadena) con caracteres en
    ' notación Hexadecimal a su código ASCII.
    Private Function HexAChar(ByVal cadena As String) As String
        ' Convertir a Char
        Dim i, J, K, LDIV2, Value As Integer
        Dim bytChar As Byte
        Dim SResult As String

        If (Len(cadena) Mod 2) = 0 Then
            LDIV2 = Int(Len(cadena) / 2)
            For i = 1 To LDIV2
                Value = 0
                J = (i - 1) * 2 + 1
                K = J + 1
                bytChar = Asc(Mid(cadena, J, 1))
                Select Case bytChar
                    Case 48 To 57 ' "0".."9"
                        Value = (bytChar - 48) * 16

                    Case 65 To 70 ' "A".."F" ~ 10..15
                        Value = (bytChar - 55) * 16
                End Select
                bytChar = Asc(Mid(cadena, K, 1))
                Select Case bytChar
                    Case 48 To 57 ' "0".."9"
                        Value = Value + (bytChar - 48)

                    Case 65 To 70 ' "A".."F" ~ 10..15
                        Value = Value + (bytChar - 55)
                End Select
                If IsNothing(Chr(Value)) Then
                    SResult = SResult & " "
                Else
                    SResult = SResult & Chr(Value)
                End If
            Next
        End If
        HexAChar = SResult
    End Function

    'CSPhf(11/10/2000): Fusiòn ESTGNF: Esconder texto en texto
    Private Function FusionESTGNF(ByVal cadena1 As String, ByVal cadena2 As String) As String
        Dim SResult As String
        Dim i, L As Integer
        Dim srand As Single

        L = Len(cadena1)
        For i = 1 To L
            Randomize(Asc(Mid(cadena1, i, 1)))
            srand = CInt((10) * (Rnd())) Mod 10
            SResult = SResult & CStr(srand) & Mid(cadena2, i, 1)
        Next

        FusionESTGNF = SResult
    End Function

    'CSPhf(11/10/2000)
    Private Function FisionESTGNF(ByVal cadena As String)
        Dim i, L As Integer
        Dim SResult As String

        L = Len(cadena)

        For i = 1 To L
            If (i Mod 2 = 0) Then
                SResult = SResult & Mid(cadena, i, 1)
            End If
        Next

        FisionESTGNF = SResult
    End Function

    '-------------------------------------------------------------------
    ' Funciones Públicas:

    ' CSPsr(10/X/2000):  Documentación de la función.
    ' Nombre:
    '     Encriptar.
    ' Descripción:
    '     Algoritmo para la encripción de los parámetros enviados a los CGIs
    '     para la consulta de las imágenes de los cheques.
    ' Parámetros:
    '     Cadena = String a encriptar.
    ' Resultado:
    '     Se regresa la cadena encriptada.
    Public Function Encriptar(ByVal cadena As String, _
                              ByVal key1 As Long, _
                              ByVal key2 As Long) As String
        Dim SResult As String

        '  Asignamos los valores a las llaves necesarias para encriptar
        C1 = key1
        C2 = key2
        If (Len(cadena) > 0) Then
            SResult = Hash(cadena)                          ' 1. Cálculo del HASH del string.
            SResult = PadZeroL(CInt(SResult), 10) & cadena  ' 2. El HASH ocupa 10 dígitos.
            SResult = Encrypt(SResult)                      ' 3. Encripción con la llave inicial "GKey".
            SResult = CharAHex(SResult)                     ' 4. Traslado a Hexadecimal.
            SResult = EncryptSS2(SResult)                   ' 5. Segunda encripción.
            SResult = VoltearTexto(SResult)                 ' 6. Voltea la cadena.
        End If

        Encriptar = SResult
    End Function ' Encriptar

    ' CSPsr(10/X/2000):  Documentación de la función.
    ' Nombre:
    '     Desencriptar.
    ' Descripción:
    '     Algoritmo para la desencripción de los parámetros enviados a los CGIs
    '     para la consulta de las imágenes de los cheques.
    ' Parámetros:
    '     Cadena = String a desencriptar.
    ' Resultado:
    '     Se regresa la cadena desencriptada.
    Public Function Desencriptar(ByVal cadena As String, _
                                 ByVal key1 As Long, _
                                 ByVal key2 As Long) As String
        Dim SResult As String

        '  Asignamos los valores necesarios para desencriptar la cadena
        C1 = key1
        C2 = key2
        If (Len(cadena) > 0) Then
            SResult = VoltearTexto(cadena)
            SResult = DecryptSS2(SResult)
            SResult = HexAChar(SResult)
            SResult = Decrypt(SResult)  ' La llave a utilizar es "GKey".
        End If

        SResult = Right(SResult, Len(SResult) - 10)
        Desencriptar = SResult
    End Function ' Desencriptar

    '  CSPhf(11/10/2000): Encripciòn de parámetros
    Public Function Encrypt_param(ByVal cadena As String) As String
        Dim SResult, S2 As String

        If (Len(cadena) > 0) Then
            S2 = VoltearTexto(cadena)
            SResult = FusionESTGNF(S2, cadena)
            SResult = CharAHex(SResult)
            SResult = VoltearTexto(SResult)
            SResult = EncryptSS2(SResult)
        End If

        Encrypt_param = SResult
    End Function

    ' CSPhf(11/10/2000): Desencripciòn de parámetros
    Public Function Decrypt_param(ByVal cadena As String) As String
        Dim SResult As String

        If Len(cadena) > 0 Then
            SResult = DecryptSS2(cadena)
            SResult = VoltearTexto(SResult)
            SResult = HexAChar(SResult)
            SResult = FisionESTGNF(SResult)
        End If

        Decrypt_param = SResult
    End Function
End Class
