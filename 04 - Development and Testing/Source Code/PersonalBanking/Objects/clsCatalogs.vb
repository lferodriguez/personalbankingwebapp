Public Class clsCatalogs
    Private _dbCon As clsDbWrapper
    Private _dsConsulta As DataSet
    Private _informacionAdicional As String

    Enum catalogTransactionConceptisConsiderableInConceptFlowType
        Yes = 1
        No = 2
        All = 3
    End Enum
    Enum catalogTransactionConceptTypeFlowType
        Income = 1
        Expense = 2
        None = 3
        All = 4
        IncomeAndExpenses = 5
    End Enum
    Enum catalogTransactionConceptType
        Deposit = 1
        Withdrawl = 2
        All = 3
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
    Function TransactionConceptFlowType(ByVal flowType As catalogTransactionConceptTypeFlowType) As Boolean
        Dim booldev As Boolean = False
        Dim strQry As String = ""
        Dim strCondition As String = ""
        Select Case flowType
            Case catalogTransactionConceptTypeFlowType.Expense,
                 catalogTransactionConceptTypeFlowType.Income,
                 catalogTransactionConceptTypeFlowType.None
                strCondition = strCondition & " transactionConceptFlowType = " & flowType & " and "
            Case catalogTransactionConceptTypeFlowType.IncomeAndExpenses
                strCondition = strCondition & " transactionConceptFlowType in (" & catalogTransactionConceptTypeFlowType.Income & _
                    "," & catalogTransactionConceptTypeFlowType.Expense & ") and "
        End Select
        strCondition = IIf(strCondition.Length > 0, " where " & strCondition & " 1 = 1", "")
        strQry = "select * from transactionConceptFlowtype " & strCondition & " order by name DESC"
        booldev = _dbCon.ejecutarInstruccionSQL(strQry, False)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function
    Function TransactionConcept(ByVal type As catalogTransactionConceptType, _
                                ByVal flowType As catalogTransactionConceptTypeFlowType, _
                                ByVal isConsiderableInWorkflow As catalogTransactionConceptisConsiderableInConceptFlowType) As Boolean

        Dim booldev As Boolean = False
        Dim strQry As String = ""
        Dim strCondition As String = ""

        Select Case type
            Case catalogTransactionConceptType.Deposit,
                 catalogTransactionConceptType.Withdrawl
                strCondition = " transactionConceptType = " & type & " and "
        End Select

        Select Case flowType
            Case catalogTransactionConceptTypeFlowType.Expense,
                 catalogTransactionConceptTypeFlowType.Income,
                 catalogTransactionConceptTypeFlowType.None
                strCondition = strCondition & " transactionConceptFlowType = " & flowType & " and "
            Case catalogTransactionConceptTypeFlowType.IncomeAndExpenses
                strCondition = strCondition & " transactionConceptFlowType in (" & catalogTransactionConceptTypeFlowType.Income & _
                    "," & catalogTransactionConceptTypeFlowType.Expense & ") and "
        End Select

        Select Case isConsiderableInWorkflow
            Case catalogTransactionConceptisConsiderableInConceptFlowType.Yes
                strCondition = strCondition & " isConsiderableInConceptFlowType='Y' and "
            Case catalogTransactionConceptisConsiderableInConceptFlowType.No
                strCondition = strCondition & " isConsiderableInConceptFlowType='N' and "
        End Select

        strCondition = IIf(strCondition.Length > 0, " where " & strCondition & " 1 = 1", "")
        strQry = "select * from transactionConcept " & strCondition & " order by name ASC"
        booldev = _dbCon.ejecutarInstruccionSQL(strQry, False)
        _dsConsulta = _dbCon.resultadoConsulta
        _informacionAdicional = _dbCon.informacionAdicional
        Return booldev
    End Function

End Class
