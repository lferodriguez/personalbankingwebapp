

Imports System.Data.SqlClient
Imports System.Xml
Imports System.IO
Imports Canella.Encrypt

Public Class SQLServerConnection
#Region "Constantes"
    Const DirectorioArchivoConfiguracion As String = "D:\Benefos\ConfigFiles\ConfiguracionesBdd.xml"
    Const XPathDbConfiguration As String = "dbconfigurations/dbconfiguration"

    Private Const PasswordKey1 As Long = 10799
    Private Const PasswordKey2 As Long = 6791

    Private Enum IndexTag
        Application = 0
        Server
        DataBase
        UserId
        Password
    End Enum
#End Region

#Region "Atributos"
    Private myConnection As SqlConnection
    Private myCommand As SqlCommand
    Private myReader As SqlDataReader
    Private myAdapter As SqlDataAdapter
    Private myDataset As DataSet
    Private myTransaction As SqlTransaction
    Private mySqlText As String

    Private myConnectionString As String
    Private myApplicationId As String
    Private myServerId As String
    Private myUserId As String
    Private myPassword As String
    Private myDataBaseName As String

    Private myErrorMessage As String

    Private blnOnTransaction As Boolean = False
#End Region

    ''' New(Constructor de la clase)
    '''     Inicializa todas las variables "string"
    Public Sub New()
        myConnectionString = ""
        myApplicationId = ""
        myServerId = ""
        myUserId = ""
        myPassword = ""
        myDataBaseName = ""
        myErrorMessage = ""
    End Sub

    ReadOnly Property ResultDataset() As DataSet
        Get
            Return myDataset
        End Get
    End Property

    ReadOnly Property ResultReader() As SqlDataReader
        Get
            Return myReader
        End Get
    End Property

    ReadOnly Property ErrorMessage() As String
        Get
            Return myErrorMessage
        End Get
    End Property

    ReadOnly Property IsOpen() As Boolean
        Get
            If myConnection.State = ConnectionState.Open Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Property Transaction() As SqlTransaction
        Get
            Return myTransaction
        End Get
        Set(ByVal Value As SqlTransaction)
            myTransaction = Value
        End Set
    End Property

    ''' LoadConnectionString
    '''     Carga desde un archivo XML, localizado en el directorio asignado en la constante 
    '''     DirectorioArchivoConfiguracion, la configuración de la conexión a la BDD
    '''     El nodo o configuración a cargar se determina por el parámetro stridConfiguracion
    '''
    ''' Parámetros:
    '''     strNombreAplicacion, Configuracion de Bdd a cargar
    '''
    ''' Valor de retorno:
    '''     Boolean, True si la carga fue exitosa, False de lo contrario
    Private Function LoadConnectionString() As Boolean
        
        Try
            myUserId = System.Configuration.ConfigurationManager.AppSettings.Item("databaseuser")
            myPassword = System.Configuration.ConfigurationManager.AppSettings.Item("databasepassword")
            myServerId = System.Configuration.ConfigurationManager.AppSettings.Item("databaseserver")
            myDataBaseName = System.Configuration.ConfigurationManager.AppSettings.Item("databasename")
            myConnectionString = "user id=" + myUserId + "; pwd=" + myPassword + ";data source=" + myServerId + ";persist security info=False;initial catalog=" + myDataBaseName + ""
            Return True
        Catch ex As Exception
            Return False
            myErrorMessage = ex.Message
        End Try
    End Function

    ''' Open
    '''     Abre una conexión a la base de datos.
    '''
    ''' Parámetros
    '''     stridConfiguracion, configuración de conexión deseada sobre el Archivo Xml 
    '''                         que contiene el conjunto de configuraciones
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function Open() As Boolean
        Try
            If (LoadConnectionString() = True) Then
                myConnection = New SqlConnection(myConnectionString)
                myConnection.Open()
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' Close
    '''     Cierra la conexión activa
    '''
    ''' Parámetros
    '''     Ninguno
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function Close() As Boolean
        Try
            myConnection.Close()
            Return True
        Catch ex As Exception
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' BeginTransaction
    '''     Inicia una transacción
    '''
    ''' Parámetros
    '''     strTransactionName, Nombre de la transaccion
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function BeginTransaction(Optional ByVal strTransactionName As String = "") As Boolean
        Try
            If myConnection.State = ConnectionState.Open Then
                If strTransactionName = "" Then
                    myTransaction = myConnection.BeginTransaction("myTransaction")
                Else
                    myTransaction = myConnection.BeginTransaction(strTransactionName)
                End If
                blnOnTransaction = True
                Return True
            Else
                myErrorMessage = "La conexión a la BDD está cerrada"
                Return False
            End If
        Catch ex As Exception
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' CommitTransaction
    '''     Finaliza una transaccion
    '''
    ''' Parametros:
    '''     Ninguno
    ''' 
    ''' Valor de retorno:
    '''     Boolean
    Public Function CommitTransaction() As Boolean
        Try
            If Not myTransaction Is Nothing Then
                myTransaction.Commit()
                blnOnTransaction = False
                Return True
            Else
                myErrorMessage = "No hay ninguna transacción asignada"
                Return False
            End If
        Catch ex As Exception
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' CommitTransaction
    '''     Cancela y reversa las operaciones de la transaccion
    '''
    ''' Parametros:
    '''     Ninguno
    ''' 
    ''' Valor de retorno:
    '''     Boolean
    Public Function RollBackTransaction() As Boolean
        Try
            If Not myTransaction Is Nothing Then
                myTransaction.Rollback()
                blnOnTransaction = False
                Return True
            Else
                myErrorMessage = "No hay ninguna transacción asignada"
                Return False
            End If
        Catch ex As Exception
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function
    ''' ExecuteSqlReturnReader
    '''     Ejecuta la instrucción SQL parametrizada y asigna el resultado en un DataReader, si la
    '''     instrucción es solamente de consulta. El DataReader resultante se puede consultar posteriormente
    '''     con la propiedad "ResultReader" de esta clase.
    '''
    ''' Parámetros:
    '''     strSqlSentence,     instrucción SQL a ejecutar
    '''     blnModificatesData, bandera para indicar si la instrucción SQL modifica datos (True) o no (False)
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function ExecuteSqlReturnReader(ByVal strSqlSentence As String, ByVal blnModificatesData As Boolean) As Boolean
        Try
            myCommand = New SqlCommand(strSqlSentence, myConnection)
            myCommand.CommandType = CommandType.Text

            'Si no está abierta la conexión se abre
            If Not (myCommand.Connection.State = ConnectionState.Open) Then
                myCommand.Connection.Open()
            End If
            'Si se encuentra dentro de una transacción, se asigna al SqlCommand
            If blnOnTransaction Then myCommand.Transaction = myTransaction
            'Si no modifica datos, es una consulta que se carga al dataReader
            If (blnModificatesData = False) Then
                If Not myReader Is Nothing Then
                    If Not myReader.IsClosed Then myReader.Close()
                End If
                myReader = myCommand.ExecuteReader
            Else
                'Es una modificación de datos.
                myCommand.ExecuteNonQuery()
            End If
            Return True
        Catch ex As Exception
            If blnOnTransaction Then
                myTransaction.Rollback()
                blnOnTransaction = False
            End If
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' ExecuteSqlReturnDataset
    '''     Ejecuta la instrucción SQL parametrizada y asigna el resultado en un DataSet, si la
    '''     instrucción es solamente de consulta. El Dataset resultante se puede consultar posteriormente
    '''     con la propiedad "ResultDataset" de esta clase.
    '''
    ''' Parámetros:
    '''     strSqlSentence,     instrucción SQL a ejecutar
    '''     blnModificatesData, bandera para indicar si la instrucción SQL modifica datos (True) o no (False)
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function ExecuteSqlReturnDataset(ByVal strSqlSentence As String, ByVal blnModificatesData As Boolean) As Boolean
        Try
            myCommand = New SqlCommand(strSqlSentence, myConnection)
            myCommand.CommandType = CommandType.Text
            If Not (myCommand.Connection.State = ConnectionState.Open) Then
                myCommand.Connection.Open()
            End If
            If (myAdapter Is Nothing) Then myAdapter = New SqlDataAdapter
            'Si se encuentra dentro de una trasacción se asigna al SqlCommand
            If blnOnTransaction Then myCommand.Transaction = myTransaction
            myAdapter.SelectCommand = myCommand
            myDataset = New DataSet
            myAdapter.Fill(myDataset)
            Return True
        Catch ex As Exception
            If blnOnTransaction Then
                myTransaction.Rollback()
                blnOnTransaction = False
            End If
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' RunSpReturnDataset
    '''     Ejecuta un Store procedure que devuelve un conjunto de datos. Si contiene parámetros, se
    '''     envían en el arreglo sppParameters. Los datos se pueden acceder por medio de la propiedad
    '''     ResultDataSet de esta clase.
    '''
    ''' Parámetros:
    '''     strNombreSp,    Nombre del Store procedure
    '''     sppParametros,  ArrayList de "SpParametro" que tiene propiedades "Nombre" y "Valor" de tipo string
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function RunSpReturnDataset(ByVal strNombreSp As String, ByVal sppParametros As SpParameters) As Boolean
        Try
            myCommand = New SqlCommand(strNombreSp, myConnection)
            myCommand.CommandType = CommandType.Text
            myCommand.CommandText = "execute " & strNombreSp & sppParametros.FormParametersString
            If Not (myCommand.Connection.State = ConnectionState.Open) Then
                myCommand.Connection.Open()
            End If
            If blnOnTransaction Then myCommand.Transaction = myTransaction
            myAdapter = New SqlDataAdapter
            myAdapter.SelectCommand = myCommand
            myDataset = New DataSet
            myAdapter.Fill(myDataset)
            Return True
        Catch ex As Exception
            If blnOnTransaction Then
                myTransaction.Rollback()
                blnOnTransaction = False
            End If
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' RunSp
    '''     Ejecuta un Store procedure. Si contiene parámetros, se envían en el arreglo sppParameters
    '''
    ''' Parámetros:
    '''     strNombreSp,    Nombre del Store procedure
    '''     sppParametros,  ArrayList de "SpParametro" que tiene propiedades "Nombre" y "Valor" de tipo string
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function RunSp(ByVal strNombreSp As String, ByVal sppParametros As SpParameters) As Boolean
        Try
            myCommand = New SqlCommand(strNombreSp, myConnection)
            myCommand.CommandType = CommandType.Text
            myCommand.CommandText = "execute " & strNombreSp & sppParametros.FormParametersString
            If Not (myCommand.Connection.State = ConnectionState.Open) Then
                myCommand.Connection.Open()
            End If
            If blnOnTransaction Then myCommand.Transaction = myTransaction
            myCommand.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            If blnOnTransaction Then
                myTransaction.Rollback()
                blnOnTransaction = False
            End If
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' Query
    '''     Consulta a la base de datos. El resultado se accede por medio de la propiedad "ResultDataset"
    '''
    ''' Parámetros:
    '''     strTabla,                   Nombre de la tabla
    '''     strCampos,                  Campos, separados por comas
    '''     strRestriccion,             Restricciones de la consulta
    '''     strAgrupamiento,            Sentencia de agrupamiento para la consulta
    '''     strRestriccionAgrupamiento, Restricciones para el agrupamiento
    '''     strOrden,                   Sentencia de ordenamiento
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function Query(ByVal strTabla As String, _
                            Optional ByVal strCampos As String = "", _
                            Optional ByVal strRestriccion As String = "", _
                            Optional ByVal strAgrupamiento As String = "", _
                            Optional ByVal strRestriccionAgrupamiento As String = "", _
                            Optional ByVal strOrdenamiento As String = "") As Boolean
        Try
            mySqlText = "SELECT " & strCampos & " FROM " & strTabla & _
                        IIf(strRestriccion <> "", vbCrLf & " WHERE    " & strRestriccion, "") & _
                        IIf(strAgrupamiento <> "", vbCrLf & " GROUP BY " & strAgrupamiento, "") & _
                        IIf(strRestriccionAgrupamiento <> "", vbCrLf & " HAVING   " & strRestriccionAgrupamiento, "") & _
                        IIf(strOrdenamiento <> "", vbCrLf & " ORDER BY " & strOrdenamiento, "")

            myCommand = New SqlCommand(mySqlText, myConnection)
            myCommand.CommandType = CommandType.Text

            If (myAdapter Is Nothing) Then myAdapter = New SqlDataAdapter
            'Si se encuentra dentro de una trasacción se asigna al SqlCommand
            If blnOnTransaction Then myCommand.Transaction = myTransaction
            myAdapter.SelectCommand = myCommand
            myDataset = New DataSet
            myAdapter.Fill(myDataset)
            Return True
        Catch ex As Exception
            If blnOnTransaction Then
                myTransaction.Rollback()
                blnOnTransaction = False
            End If
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' Insert
    '''     Inserción a la base de datos
    '''
    ''' Parámetros:
    '''     strTabla,       Nombre de la tabla
    '''     strCampos,      Campos, separados por comas, a insertar
    '''     strValores,     Valores, separados por comas, a insertar
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function Insert(ByVal strTabla As String, ByVal strCampos As String, ByVal strValores As String) As Boolean
        Try
            mySqlText = "INSERT INTO " & strTabla & " (" & strCampos & ") " & vbCrLf & _
                        " VALUES (" & strValores & ")"

            myCommand = New SqlCommand(mySqlText, myConnection)
            myCommand.CommandType = CommandType.Text
            If blnOnTransaction Then myCommand.Transaction = myTransaction
            myCommand.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            If blnOnTransaction Then
                myTransaction.Rollback()
                blnOnTransaction = False
            End If
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' Update
    '''     Actualización a la base de datos
    '''
    ''' Parámetros:
    '''     strTabla,           Nombre de la tabla
    '''     strCampos,          Campos y valores a asignar en la actualización
    '''     strRestriccion,     Restricciones para llevar a cabo los cambios
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function Update(ByVal strTabla As String, ByVal strCampos As String, Optional ByVal strRestriccion As String = "") As Boolean
        Try
            mySqlText = "UPDATE " & strTabla & vbCrLf & _
                        " SET " & strCampos & _
                        IIf(strRestriccion <> "", vbCrLf & " WHERE " & strRestriccion, "")

            myCommand = New SqlCommand(mySqlText, myConnection)
            myCommand.CommandType = CommandType.Text
            If blnOnTransaction Then myCommand.Transaction = myTransaction
            myCommand.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            If blnOnTransaction Then
                myTransaction.Rollback()
                blnOnTransaction = False
            End If
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' Delete
    '''     Eliminación a la base de datos
    '''
    ''' Parámetros:
    '''     strTabla,           Nombre de la tabla
    '''     strRestriccion,     Restricciones para llevar a cabo la eliminación
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function Delete(ByVal strTabla As String, Optional ByVal strRestriccion As String = "") As Boolean
        Try
            mySqlText = "DELETE " & strTabla & vbCrLf & _
                     IIf(strRestriccion <> "", vbCrLf & " WHERE " & strRestriccion, "")

            myCommand = New SqlCommand(mySqlText, myConnection)
            myCommand.CommandType = CommandType.Text
            If blnOnTransaction Then myCommand.Transaction = myTransaction
            myCommand.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            If blnOnTransaction Then
                myTransaction.Rollback()
                blnOnTransaction = False
            End If
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' InsertIdentity
    '''     Inserción a la base de datos, con espera de un código de Id y Error.
    '''     Dichos valores se acceden con la propiedad "ResultDataset"
    '''
    ''' Parámetros:
    '''     strTabla,       Nombre de la tabla
    '''     strCampos,      Campos, separados por comas, a insertar
    '''     strValores,     Valores, separados por comas, a insertar
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function InsertIdentity(ByVal strTabla As String, ByVal strCampos As String, ByVal strValores As String) As Boolean
        Try
            mySqlText = "BEGIN " & vbCrLf & _
                    " declare @vintError as int, " & vbCrLf & _
                    " @vbintNuevo as bigint " & vbCrLf & _
                    " SET @vintError = 0 " & vbCrLf & _
                    " SET @vbintNuevo  = 0 " & vbCrLf & _
                    " INSERT INTO " & strTabla & " (" & strCampos & ") " & vbCrLf & _
                    " VALUES(" & strValores & ")" & vbCrLf & _
                    " SELECT @vbintNuevo = @@Identity, @vintError = @@Error " & vbCrLf & _
                    " SELECT @vbintNuevo as 'Id', @vintError as 'Error'" & vbCrLf & _
                    " END"

            myCommand = New SqlCommand(mySqlText, myConnection)
            myCommand.CommandType = CommandType.Text
            If blnOnTransaction Then myCommand.Transaction = myTransaction
            myAdapter = New SqlDataAdapter
            myAdapter.SelectCommand = myCommand
            myDataset = New DataSet
            myAdapter.Fill(myDataset)
            Return True
        Catch ex As Exception
            If blnOnTransaction Then
                myTransaction.Rollback()
                blnOnTransaction = False
            End If
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

    ''' InsertSelect
    '''     Inserción a la base de datos, con base a una consulta en la sentencia de valores
    '''
    ''' Parámetros:
    '''     strTabla,       Nombre de la tabla
    '''     strCampos,      Campos, separados por comas, a insertar
    '''     strSelect,      Valores, representados por una sentencia SQL de consulta
    '''
    ''' Valor de retorno:
    '''     Boolean
    Public Function InsertSelect(ByVal strTabla As String, ByVal strCampos As String, ByVal strQuery As String) As Boolean
        Try
            mySqlText = "INSERT INTO " & strTabla & " (" & strCampos & ") " & vbCrLf & _
            " (" & strQuery & ")"

            myCommand = New SqlCommand(mySqlText, myConnection)
            myCommand.CommandType = CommandType.Text
            If blnOnTransaction Then myCommand.Transaction = myTransaction
            myCommand.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            If blnOnTransaction Then
                myTransaction.Rollback()
                blnOnTransaction = False
            End If
            myErrorMessage = ex.Message
            Return False
        End Try
    End Function

End Class
