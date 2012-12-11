Public Class clsEvents
    Protected _dbCon As clsDbWrapper
    Protected _dsConsulta As DataSet
    Protected _informacionAdicional As String

    Sub New()
        _dbCon = New clsDbWrapper
    End Sub
    ReadOnly Property informacionAdicional() As String
        Get
            Return _informacionAdicional
        End Get
    End Property
    ReadOnly Property resultadoConsulta() As DataSet
        Get
            Return _dsConsulta
        End Get
    End Property
    Function addEvent(ByVal webUser As Integer, ByVal transactionReference As Integer, ByVal level As clsCatalogs.events) As Boolean
        Dim booldev As Boolean = False

        Dim spparameters As New SpParameters
        spparameters.Add("pWebUser", webUser, SpParameter.tipoParametro.entero)
        spparameters.Add("pTransactionReference", transactionReference, SpParameter.tipoParametro.entero)
        spparameters.Add("pEventLevel", level, SpParameter.tipoParametro.entero)
        spparameters.Add("pSource", System.Configuration.ConfigurationManager.AppSettings.Item("ApplicationName"), SpParameter.tipoParametro.cadena)
        booldev = _dbCon.ejecutarProcedimientoAlmacenado("addEventOneTransaction", spparameters)
        _informacionAdicional = _dbCon.informacionAdicional

        Return booldev
    End Function
    Function addEvent(ByVal webUser As Integer, ByVal FirstTransactionReference As Integer, ByVal SecondTransactionReference As Integer, ByVal level As clsCatalogs.events) As Boolean
        Dim booldev As Boolean = False

        Dim spparameters As New SpParameters
        spparameters.Add("pWebUser", webUser, SpParameter.tipoParametro.entero)
        spparameters.Add("pFirstTransactionReference", FirstTransactionReference, SpParameter.tipoParametro.entero)
        spparameters.Add("pSecondTransactionReference", SecondTransactionReference, SpParameter.tipoParametro.entero)
        spparameters.Add("pEventLevel", level, SpParameter.tipoParametro.entero)
        spparameters.Add("pSource", System.Configuration.ConfigurationManager.AppSettings.Item("ApplicationName"), SpParameter.tipoParametro.cadena)
        booldev = _dbCon.ejecutarProcedimientoAlmacenado("addEventTwoTransactions", spparameters)
        _informacionAdicional = _dbCon.informacionAdicional

        Return booldev
    End Function
    Function lastEvents() As Boolean
        Dim booldev As Boolean = False
        Dim spparameters As New SpParameters
        booldev = _dbCon.ejecutarProcedimientoAlmacenado("queryLastEvents", spparameters)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
    Function searchForEvents(ByVal startDate As String, ByVal endDate As String, ByVal searchCriteria As String) As Boolean
        Dim booldev As Boolean = False
        Dim spparameters As New SpParameters
        spparameters.Add("pStartDate", startDate, SpParameter.tipoParametro.cadena)
        spparameters.Add("pEndDate", endDate, SpParameter.tipoParametro.cadena)
        If Not (searchCriteria = String.Empty) Then
            spparameters.Add("pMessage", searchCriteria, SpParameter.tipoParametro.cadena)
        End If
        booldev = _dbCon.ejecutarProcedimientoAlmacenado("searchEvent", spparameters)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
End Class
