''' SpParameter
'''     Estructura de datos que contiene datos de parámetros, para enviar a un store procedure
'''     en la clase ConexionBdd. Sp = Store Procedure.
'''
''' Autor(es): 
'''     Lenin Fernando Rodríguez Conde, Pedro Ruiz
''' 
''' Fecha de creación: 
'''     30 de Mayo de 2007
'''
''' Código propiedad de Canella, S.A.

Public Class SpParameter
    Private strNombre As String
    Private strValor As String
    Private intTipo As Integer

    Enum tipoParametro
        entero = 1
        cadena = 2
    End Enum
    Public Sub New()
        strNombre = ""
        strValor = ""
        intTipo = 0
    End Sub
    '''Atributos del Parametro
    ''' Estra propiedad especifica la naturaleza del parametro
    ReadOnly Property entero() As Integer
        Get
            Return tipoParametro.entero
        End Get
    End Property
    ReadOnly Property cadena() As Integer
        Get
            Return tipoParametro.cadena
        End Get
    End Property

    ''' Nombre
    '''     Nombre que identifica a un parametro en un store procedure
    Property Nombre() As String
        Get
            Return strNombre
        End Get
        Set(ByVal Value As String)
            strNombre = Value
        End Set
    End Property

    ''' Valor
    '''     Valor de un parametro en un store procedure
    Property Valor() As String
        Get
            Return strValor
        End Get
        Set(ByVal Value As String)
            strValor = Value
        End Set
    End Property
    Property Tipo() As tipoParametro
        Get
            Return intTipo
        End Get
        Set(ByVal Value As tipoParametro)
            intTipo = Value
        End Set
    End Property
End Class
