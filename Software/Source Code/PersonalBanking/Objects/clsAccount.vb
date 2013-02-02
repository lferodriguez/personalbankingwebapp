Public Class clsAccount
    Protected _dbCon As clsDbWrapper
    Protected _dsConsulta As DataSet
    Protected _informacionAdicional As String
    Enum accountTypes
        Checks = 1
        Savings = 2
        CreditCards = 3
        Loans = 4
        CertificateOfDeposits = 5
    End Enum
    Enum accountStates
        Enabled = 1
        Disabled = 2
    End Enum
    Enum accountBankSituation
        Enabled = 1
        Canceled = 2
        Blocked = 3
    End Enum
    Enum accountCurrencies
        Que = 1
        Dol = 2
    End Enum
    Enum accountHistoryPeriod
        ThisMonth = 1
        LastMonth = 2
        TwoMonthsago = 3
        ThreeMonthsago = 4
        FourMonthsago = 5
        FiveMonthsago = 6
        SixMonthsAgo = 7
    End Enum
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
    Private Sub getDateRageByPeriod(ByRef startDate As DateTime, _
                                     ByRef endDate As DateTime, _
                                     ByVal period As accountHistoryPeriod)
        Dim mes As Integer = 0
        Dim anio As Integer = 0

        Select Case period
            Case accountHistoryPeriod.ThisMonth
                startDate = DateSerial(Year(Now.Date), Month(Now.Date) + 0, 1)
                endDate = DateSerial(Year(Now.Date), Month(Now.Date) + 1, 0)
            Case Else
                mes = Now.AddMonths(1 - period).Month
                If (Now.Month - mes) < 0 Then
                    anio = Now.AddYears(-1).Year
                Else
                    anio = Now.Year
                End If
                startDate = DateSerial(anio, Now.AddMonths(1 - period).Month() + 0, 1)
                endDate = DateSerial(anio, mes + 1, 0)
        End Select
    End Sub
    Function accountDescription(ByVal account As Integer) As Boolean
        Dim booldev As Boolean = False
        Dim strQry As String = ""

        strQry = "  select " & _
                 " a.number as accountNumber, bc.name as bankName, a.name as accountName, s.name as state, bs.name as situation, " & _
                 " cc.symbol as currencySymbol, t.name as accountType, a.currency " & _
                  " from account a, bankCatalog bc, currencyCatalog cc, accountTypeCatalog t, accountstatecatalog s, accountBankSituationCatalog bs " & _
                    " where a.Account = " & account & " and a.bank = bc.bank and a.currency = cc.currency and a.type = t.type and s.accountstate = a.accountstate " & _
                    " and a.accountbanksituation = bs.accountbanksituation"

        booldev = _dbCon.ejecutarInstruccionSQL(strQry, False)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
    Function accountBalancePerUserPerAccountTypePerCurrencyPerAccountState(ByVal webuser As Integer, _
                                                            ByVal accountType As accountTypes, _
                                                            ByVal currency As accountCurrencies, _
                                                            ByVal accounstate As accountStates) As Boolean
        Dim booldev As Boolean = False
        Dim strQry As String = ""
        strQry = "select * from cnsaccountBalances where webuser=" & webuser & " and accountType=" & accountType & _
                 " and currency = " & currency & " and accountState = " & accounstate
        booldev = _dbCon.ejecutarInstruccionSQL(strQry, False)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
    Function accountBalancePerAccount(ByVal accountId As Integer) As Boolean
        Dim booldev As Boolean = False
        Dim strQry As String = ""
        strQry = "select * from cnsaccountBalances where account=" & accountId
        booldev = _dbCon.ejecutarInstruccionSQL(strQry, False)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
    Function accountResumePerUserPerAccountTypePerState(ByVal webuser As Integer, _
                                                     ByVal accountType As accountTypes, ByVal accountState As accountStates) As Boolean
        Dim booldev As Boolean = False
        Dim strQry As String = ""
        strQry = "select * from cnsAccountsResume where webuser=" & webuser & " and type=" & accountType & " and accountState=" & accountState
        booldev = _dbCon.ejecutarInstruccionSQL(strQry, False)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
    Function accountTransactionConceptTypeResumeByPeriod(ByVal account As Integer, _
                                                     ByVal period As accountHistoryPeriod) As Boolean
        Dim booldev As Boolean = False
        Dim strQry As String = ""
        Dim dtPrimero As DateTime
        Dim dtUltimo As DateTime

        getDateRageByPeriod(dtPrimero, dtUltimo, period)

        strQry = "select tct.name as concept, SUM(at.value) as total   from AccountTransaction at," & _
                    " TransactionConcept tc, TransactionConceptType tct " & _
                    " where at.account = " & account & " And " & _
                    " at.transactionConcept = tc.TransactionConcept And " & _
                    " tc.TransactionConceptType = tct.TransactionConceptType And " & _
                    " at.transactionDate between '" & dtPrimero.ToString("MM/dd/yyyy") & _
                    "' and '" & dtUltimo.ToString("MM/dd/yyyy") & " 23:59:59'" & _
                    " group by tct.name "

        booldev = _dbCon.ejecutarInstruccionSQL(strQry, False)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
    Function accountTransactionConceptResumeByPeriod(ByVal account As Integer, _
                                                     ByVal period As accountHistoryPeriod) As Boolean
        Dim booldev As Boolean = False
        Dim strQry As String = ""
        Dim dtPrimero As DateTime
        Dim dtUltimo As DateTime

        getDateRageByPeriod(dtPrimero, dtUltimo, period)

        strQry = "select tc.name as concept, SUM(at.value) as total   from AccountTransaction at," & _
                    " TransactionConcept tc" & _
                    " where at.account = " & account & " And " & _
                    " at.transactionConcept = tc.TransactionConcept And " & _
                    " at.transactionDate between '" & dtPrimero.ToString("MM/dd/yyyy") & _
                    "' and '" & dtUltimo.ToString("MM/dd/yyyy") & " 23:59:59'" & _
                    " group by tc.name " & _
                    " order by total DESC"

        booldev = _dbCon.ejecutarInstruccionSQL(strQry, False)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
    Function accountTransactionHistoryByPeriod(ByVal account As Integer, _
                                               ByVal period As accountHistoryPeriod)

        Dim booldev As Boolean = False
        Dim strQry As String = ""

        Dim dtPrimero As DateTime
        Dim dtUltimo As DateTime
        getDateRageByPeriod(dtPrimero, dtUltimo, period)

        strQry = "select " & _
                  " at.account, at.transactionDate, at.AccountTransaction,tc.name," & _
                    "(case tc.TransactionConceptType when 1 then at.value else 0.00 end) as Deposits," & _
                    "(case tc.TransactionConceptType when 2 then at.value else 0.00 end) as Withdrawls," & _
                    "       at.webUserComments" & _
                    " from " & _
                    " accounttransaction at, transactionConcept tc " & _
                    " where " & _
                    " at.transactionConcept = tc.TransactionConcept " & _
                    " and at.account = " & account & _
                    " and at.transactionDate between '" & dtPrimero.ToString("MM/dd/yyyy") & _
                    "' and '" & dtUltimo.ToString("MM/dd/yyyy") & " 23:59:59' order by at.transactionDate ASC "

        booldev = _dbCon.ejecutarInstruccionSQL(strQry, False)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
End Class
