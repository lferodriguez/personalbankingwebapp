''' SpParameters
'''     Estructura de datos que contiene una lista de parametros para enviar a un store procedure
'''     en la clase ConexionBdd
'''
''' Autor(es): 
'''     Lenin Fernando Rodr�guez Conde, Pedro Ruiz
''' 
''' Fecha de creaci�n: 
'''     30 de Mayo de 2007
'''
''' C�digo propiedad de Canella, S.A.

Public Class SpParameters
    Private alParameters As ArrayList
    Private strMessage As String

    ''' New(Constructor de la clase)
    '''     Inicializa todas las variables "string"
    Public Sub New()
        strMessage = ""
        alParameters = New ArrayList
    End Sub

    ''' Add
    '''     Agrega un par�metro nuevo a la lista 
    '''
    ''' Par�metros:
    '''     strNombre,  nombre del par�metro a agregar
    '''     strValor,   valor del par�metro a agregar (convertido a string)
    ''' 
    ''' Valor de retorno:
    '''     Ninguno
    Public Sub Add(ByVal strNombre As String, ByVal strValor As String, ByVal intTipo As SpParameter.tipoParametro)
        Try
            Dim sppParameter As New SpParameter
            sppParameter.Nombre = strNombre
            sppParameter.Valor = strValor
            sppParameter.Tipo = intTipo
            alParameters.Add(sppParameter)
        Catch ex As Exception
            strMessage = ex.Message
        End Try
    End Sub

    ''' Add
    '''     Agrega un par�metro nuevo a la lista 
    '''
    ''' Par�metros:
    '''     sppParameter, parametro nuevo a agregar (de tipo SpParameter)
    ''' 
    ''' Valor de retorno:
    '''     Ninguno
    Public Sub Add(ByVal sppParameter As SpParameter)
        Try
            alParameters.Add(sppParameter)
        Catch ex As Exception
            strMessage = ex.Message
        End Try
    End Sub

    ''' GetParameterAt
    '''     Devuelve el parametro en la posicion especificada
    '''
    ''' Par�metros:
    '''     index,  ind�ce de la posici�n del par�metro
    ''' 
    ''' Valor de retorno:
    '''     SpParameter, estructura de la forma Nombre, Valor
    Public Function GetParameterAt(ByVal index As Integer) As SpParameter
        Try
            Return alParameters.Item(index)
        Catch ex As Exception
            strMessage = ex.Message
            Return Nothing
        End Try
    End Function

    ''' Count
    '''     Indica la cantidad de parametros que contiene el objeto
    '''
    ''' Par�metros:
    '''     Ninguno
    ''' 
    ''' Valor de retorno:
    '''     Integer
    Public Function Count() As Integer
        Return alParameters.Count
    End Function

    ''' FormParametersString
    '''     Forma el string de par�metros de la forma:
    '''     Ejemplo:
    '''     @Cliente=1,@Instalacion=1332
    '''
    ''' Par�metros:
    '''     Ninguno
    ''' 
    ''' Valor de retorno:
    '''     String
    Public Function FormParametersString() As String
        Try
            Dim sppParametro As SpParameter
            Dim strResultado As String

            strResultado = ""
            If alParameters.Count > 0 Then
                For Each sppParametro In alParameters
                    ' Se recorre el array list con los par�metros, formando un string de la forma:
                    ' @<NombreParametro1>=<Valor1>, @<NombreParametro2>=<Valor2>, (...) @<NombreParametroN>=<ValorN>,
                    ' 1 Es un parametro de tipo Numerico 
                    ' 2 es un parametro de tipo alfanumerico, debe ser cadena
                    If (sppParametro.Tipo = SpParameter.tipoParametro.entero) Then
                        strResultado &= " @" & sppParametro.Nombre & "=" & sppParametro.Valor & ","
                    Else
                        strResultado &= " @" & sppParametro.Nombre & "='" & sppParametro.Valor & "',"
                    End If
                Next
                'Se elimina la �ltima coma
                strResultado = strResultado.Substring(0, strResultado.Length - 1)
            End If
            Return strResultado
        Catch ex As Exception
            strMessage = ex.Message
            Return ""
        End Try
    End Function
End Class
