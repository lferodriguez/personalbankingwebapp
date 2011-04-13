﻿Public Class clsStatistics
    Protected _dbCon As clsDbWrapper
    Protected _dsConsulta As DataSet
    Protected _informacionAdicional As String
    Enum Period
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
    Public Sub getDateRageByPeriod(ByRef startDate As DateTime, _
                                     ByRef endDate As DateTime, _
                                     ByVal period As Period)
        Dim mes As Integer = 0
        Dim anio As Integer = 0

        Select Case period
            Case period.ThisMonth
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

    Private Function createDataStructureForAnalyzeConcepts(ByVal periodToConsider As Period) As DataSet
        Dim result As DataSet
        Dim counter As Integer = periodToConsider
        result = New DataSet("Results")
        result.Tables.Add(New DataTable("Result"))
        result.Tables(0).Columns.Add(New DataColumn("Concept", System.Type.GetType("System.String")))
        While counter > 0
            result.Tables(0).Columns.Add(New DataColumn("Transactions-" & counter, System.Type.GetType("System.Double")))
            result.Tables(0).Columns.Add(New DataColumn("AVG-" & counter, System.Type.GetType("System.Double")))
            result.Tables(0).Columns.Add(New DataColumn("TansactionsPerDay-" & counter, System.Type.GetType("System.Double")))
            result.Tables(0).Columns.Add(New DataColumn("AVGPerDay-" & counter, System.Type.GetType("System.Double")))
            result.Tables(0).Columns.Add(New DataColumn("Total-" & counter, System.Type.GetType("System.Double")))
            counter = counter - 1
        End While
        Return result
    End Function

    Public Function analyzeIncomesAndExpenses(ByVal concepts As DataSet, ByVal firstPeriod As Period, ByVal currency As clsAccount.accountCurrencies)
        Dim booldev As Boolean = False
        Dim firstdate As DateTime
        Dim lastdate As DateTime
        Dim strQry As String = ""
        Dim period As Integer = 0
        Dim dsTemp As DataSet
        Dim drTemp As DataRow
        Dim evaluatedDates As Integer = 0


        Try
            period = firstPeriod
            dsTemp = createDataStructureForAnalyzeConcepts(firstPeriod)
            For Each concept As DataRow In concepts.Tables(0).Rows
                drTemp = dsTemp.Tables(0).NewRow
                drTemp.Item("Concept") = concept.Item("name").ToString.Trim()

                While (period > 0)
                    getDateRageByPeriod(firstdate, lastdate, period)
                    Try
                        evaluatedDates = 1
                        evaluatedDates = DateDiff(DateInterval.Day, firstdate, lastdate)
                    Catch ex As Exception
                        evaluatedDates = 1
                    End Try
                    strQry = "select tcft.transactionConceptFlowType, COUNT(at.value) as cnt, avg (at.value) av," & _
                             " sm = case " & _
                             " when tcft.transactionConceptFlowType = 1 then SUM (at.value) " & _
                             " when tcft.transactionConceptFlowType = 2 then SUM (at.value) *-1" & _
                             " end " & _
                                " from " & _
                                "AccountTransaction at, TransactionConcept tc, account a, transactionConceptFlowType tcft" & _
                                " where " & _
                                " at.account = a.account and a.currency = " & currency & _
                                " and at.transactionConcept = tc.TransactionConcept And " & _
                                " tc.isConsiderableInConceptFlowType = 'Y' and " & _
                                " tc.TransactionConceptFlowType = tcft.transactionConceptFlowtype and " & _
                                " tc.TransactionConceptFlowType =" & concept.Item("transactionConceptFlowtype") & _
                                " and at.transactionDate between '" & firstdate.ToString("MM/dd/yyyy") & _
                                "' and '" & lastdate.ToString("MM/dd/yyyy") & " 23:59:59' " & _
                                " group by tcft.transactionConceptFlowType order by tcft.transactionConceptFlowType DESC"
                    _dbCon.ejecutarInstruccionSQL(strQry, False)
                    If _dbCon.resultadoConsulta.Tables(0).Rows.Count > 0 Then
                        drTemp.Item("Transactions-" & period) = CDbl(_dbCon.resultadoConsulta.Tables(0).Rows(0).Item("cnt").ToString)
                        drTemp.Item("AVG-" & period) = CDbl(_dbCon.resultadoConsulta.Tables(0).Rows(0).Item("av").ToString)
                        drTemp.Item("TansactionsPerDay-" & period) = CDbl(_dbCon.resultadoConsulta.Tables(0).Rows(0).Item("cnt").ToString) / evaluatedDates
                        drTemp.Item("AVGPerDay-" & period) = CDbl(_dbCon.resultadoConsulta.Tables(0).Rows(0).Item("sm").ToString) / evaluatedDates
                        drTemp.Item("Total-" & period) = CDbl(_dbCon.resultadoConsulta.Tables(0).Rows(0).Item("sm").ToString)
                    Else
                        drTemp.Item("Transactions-" & period) = 0.0
                        drTemp.Item("AVG-" & period) = 0.0
                        drTemp.Item("TansactionsPerDay-" & period) = 0.0
                        drTemp.Item("AVGPerDay-" & period) = 0.0
                        drTemp.Item("Total-" & period) = 0.0

                    End If
                    period = period - 1
                End While
                dsTemp.Tables(0).Rows.Add(drTemp)
                period = firstPeriod
            Next
            _dsConsulta = dsTemp
            booldev = True
        Catch ex As Exception
            _informacionAdicional = "Impossible to analyze incomes and expenses. More Information: " & ex.Message.ToString()
        End Try
        Return booldev
    End Function
    Public Function analyzeConcepts(ByVal concepts As DataSet,
                                    ByVal firstPeriod As Period, _
                                    ByVal currency As clsAccount.accountCurrencies) As Boolean
        Dim booldev As Boolean = False
        Dim firstdate As DateTime
        Dim lastdate As DateTime
        Dim strQry As String = ""
        Dim period As Integer = 0
        Dim dsTemp As DataSet
        Dim drTemp As DataRow
        Dim evaluatedDates As Integer = 0


        Try
            period = firstPeriod
            dsTemp = createDataStructureForAnalyzeConcepts(firstPeriod)
            For Each concept As DataRow In concepts.Tables(0).Rows
                drTemp = dsTemp.Tables(0).NewRow
                drTemp.Item("Concept") = concept.Item("name").ToString.Trim()

                While (period > 0)
                    getDateRageByPeriod(firstdate, lastdate, period)
                    Try
                        evaluatedDates = 1
                        evaluatedDates = DateDiff(DateInterval.Day, firstdate, lastdate)
                    Catch ex As Exception
                        evaluatedDates = 1
                    End Try
                    strQry = "select tc.transactionConcept, COUNT(at.value) as cnt, SUM(at.value) sm, avg (at.value) av" & _
                                " from " & _
                                "AccountTransaction at, TransactionConcept tc, account a" & _
                                " where " & _
                                " at.account = a.account and a.currency = " & currency & _
                                " and at.transactionConcept = tc.TransactionConcept And " & _
                                " tc.TransactionConcept =" & concept.Item("transactionConcept") & _
                                " and at.transactionDate between '" & firstdate.ToString("MM/dd/yyyy") & _
                                "' and '" & lastdate.ToString("MM/dd/yyyy") & " 23:59:59' " & _
                                " group by tc.TransactionConcept "
                    _dbCon.ejecutarInstruccionSQL(strQry, False)
                    If _dbCon.resultadoConsulta.Tables(0).Rows.Count > 0 Then

                        drTemp.Item("Transactions-" & period) = CDbl(_dbCon.resultadoConsulta.Tables(0).Rows(0).Item("cnt").ToString)
                        drTemp.Item("AVG-" & period) = CDbl(_dbCon.resultadoConsulta.Tables(0).Rows(0).Item("av").ToString)
                        drTemp.Item("TansactionsPerDay-" & period) = CDbl(_dbCon.resultadoConsulta.Tables(0).Rows(0).Item("cnt").ToString) / evaluatedDates
                        drTemp.Item("AVGPerDay-" & period) = CDbl(_dbCon.resultadoConsulta.Tables(0).Rows(0).Item("sm").ToString) / evaluatedDates
                        drTemp.Item("Total-" & period) = CDbl(_dbCon.resultadoConsulta.Tables(0).Rows(0).Item("sm").ToString)

                    Else
                        drTemp.Item("Transactions-" & period) = 0.0
                        drTemp.Item("AVG-" & period) = 0.0
                        drTemp.Item("TansactionsPerDay-" & period) = 0.0
                        drTemp.Item("AVGPerDay-" & period) = 0.0
                        drTemp.Item("Total-" & period) = 0.0

                    End If
                    period = period - 1
                End While
                dsTemp.Tables(0).Rows.Add(drTemp)
                period = firstPeriod
            Next
            _dsConsulta = dsTemp
            booldev = True
        Catch ex As Exception
            _informacionAdicional = "Impossible to analyze concepts. More Information: " & ex.Message.ToString()
        End Try
        Return booldev
    End Function
    
End Class
