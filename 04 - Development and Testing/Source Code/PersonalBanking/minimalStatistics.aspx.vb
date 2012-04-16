Public Class minimalStatistics
    Inherits System.Web.UI.Page
    Enum analyzeSense
        increases = 1
        decreases = 2
    End Enum
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        getIncomesAndExpensesToCompare(clsAccount.accountCurrencies.Que, lblResumeQ, analyzeSense.decreases)
        getIncomesAndExpensesToCompare(clsAccount.accountCurrencies.Dol, lblResumeUS, analyzeSense.increases)

        getResultsToCompare(clsAccount.accountCurrencies.Que, clsCatalogs.catalogTransactionConceptTypeFlowType.Income, lblincomesQue, analyzeSense.decreases)
        getResultsToCompare(clsAccount.accountCurrencies.Que, clsCatalogs.catalogTransactionConceptTypeFlowType.Expense, lblexpensesQue, analyzeSense.increases)
        getResultsToCompare(clsAccount.accountCurrencies.Dol, clsCatalogs.catalogTransactionConceptTypeFlowType.Income, lblincomesDol, analyzeSense.decreases)
        getResultsToCompare(clsAccount.accountCurrencies.Dol, clsCatalogs.catalogTransactionConceptTypeFlowType.Expense, lblexpensesdol, analyzeSense.increases)
    End Sub
    Function getIncomesAndExpensesToCompare(ByVal currency As clsAccount.accountCurrencies,
                                 ByVal writeTo As Label,
                                 ByVal ToAnalyze As analyzeSense)
        Dim booldev As Boolean = False
        Dim statistics As clsStatistics
        Dim basecatalog As New clsCatalogs
        Dim firstDate As DateTime
        Dim lastDate As DateTime
        Dim writer As StringBuilder
        Dim lines As StringBuilder
        Dim counter As Integer = 0
        Dim totalsCounter As Integer = 0
        Dim totals(7) As Double
        Dim cssClass As String = ""

        basecatalog.TransactionConceptFlowType(clsCatalogs.catalogTransactionConceptTypeFlowType.IncomeAndExpenses)

        writer = New StringBuilder
        writer.AppendLine("<table cellpadding=""0"" cellspacing=""0"" border=""0"" class=""statistics_results"">")
        writer.AppendLine("<tr>")
        writer.AppendLine("<td style=""width:20%;""></td>")
        statistics = New clsStatistics
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.SixMonthsAgo)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.FiveMonthsago)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.FourMonthsago)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.ThreeMonthsago)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.TwoMonthsago)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.LastMonth)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.ThisMonth)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        writer.AppendLine("</tr>")
        writer.AppendLine("<tr>")
        writer.AppendLine("<td class=""header"">Concept</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("</tr>")
        lines = New StringBuilder
        statistics = New clsStatistics
        If (statistics.analyzeIncomesAndExpenses(basecatalog.resultadoConsulta, clsStatistics.Period.SixMonthsAgo, currency)) Then
            For Each drTemp As DataRow In statistics.resultadoConsulta.Tables(0).Rows
                totalsCounter = 0
                lines.AppendLine("<tr>")
                lines.AppendLine("<td class=""cell_concept"">" & drTemp.Item("Concept") & "</td>")
                For counter = 1 To statistics.resultadoConsulta.Tables(0).Columns.Count - 1
                    '+------------------------------------
                    '| Getting totals and analyzing.
                    '+------------------------------------
                    cssClass = "cell"

                    If (statistics.resultadoConsulta.Tables(0).Columns(counter).ColumnName.StartsWith("Total-")) Then
                        lines.AppendLine("<td class=""" & cssClass & """>" & CDbl(drTemp.Item(counter)).ToString("N2") & "</td>")
                        totals(totalsCounter) = totals(totalsCounter) + CDbl(drTemp.Item(counter))
                        totalsCounter = totalsCounter + 1
                    End If
                Next
                lines.AppendLine("</tr>")
            Next
            lines.AppendLine("<tr>")
            lines.AppendLine("<td class=""cell_concept"">Totals</td>")
            For totalsCounter = 0 To 6
                lines.AppendLine("<td class=""cell_total"">" & totals(totalsCounter).ToString("N2") & " </td>")
            Next
            lines.AppendLine("</tr>")
        Else
            'Error. Imposible to get the data
            lines.AppendLine("<tr>")
            lines.AppendLine("<td class=""cell_msg"">" & statistics.informacionAdicional & " </td>")
            lines.AppendLine("</tr>")

        End If

        writer.AppendLine(lines.ToString)
        writer.AppendLine("</table>")
        writeTo.Text = writer.ToString
        Return booldev
    End Function
    Function getResultsToCompare(ByVal currency As clsAccount.accountCurrencies,
                                 ByVal whatType As clsCatalogs.catalogTransactionConceptTypeFlowType,
                                 ByVal writeTo As Label,
                                 ByVal ToAnalyze As analyzeSense)
        Dim booldev As Boolean = False
        Dim statistics As clsStatistics
        Dim basecatalog As New clsCatalogs
        Dim writer As StringBuilder
        Dim lines As StringBuilder
        Dim counter As Integer = 0
        Dim totalsCounter As Integer = 0
        Dim totals(7) As Double
        Dim firstDate As DateTime
        Dim lastDate As DateTime
        Dim cssClass As String = ""
        Select Case whatType
            Case clsCatalogs.catalogTransactionConceptTypeFlowType.Income

                basecatalog.TransactionConcept(clsCatalogs.catalogTransactionConceptType.Deposit,
                                       clsCatalogs.catalogTransactionConceptTypeFlowType.Income,
                                       clsCatalogs.catalogTransactionConceptisConsiderableInConceptFlowType.Yes)

            Case clsCatalogs.catalogTransactionConceptTypeFlowType.Expense
                basecatalog.TransactionConcept(clsCatalogs.catalogTransactionConceptType.Withdrawl,
                                                       clsCatalogs.catalogTransactionConceptTypeFlowType.Expense,
                                                       clsCatalogs.catalogTransactionConceptisConsiderableInConceptFlowType.Yes)

        End Select

        writer = New StringBuilder
        writer.AppendLine("<table cellpadding=""0"" cellspacing=""0"" border=""0"" class=""statistics_results"">")
        writer.AppendLine("<tr>")
        writer.AppendLine("<td style=""width:20%;""></td>")
        statistics = New clsStatistics
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.SixMonthsAgo)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.FiveMonthsago)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.FourMonthsago)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.ThreeMonthsago)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.TwoMonthsago)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.LastMonth)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        statistics.getDateRageByPeriod(firstDate, lastDate, clsStatistics.Period.ThisMonth)
        writer.AppendLine("<td class=""max_header"" style=""width:12%;"">" & MonthName(firstDate.Month) & "/" & firstDate.Year.ToString() & "</td>")
        writer.AppendLine("</tr>")
        writer.AppendLine("<tr>")
        writer.AppendLine("<td class=""header"">Concept</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("<td class=""header"">Total</td>")
        writer.AppendLine("</tr>")
        lines = New StringBuilder
        statistics = New clsStatistics
        If (statistics.analyzeConcepts(basecatalog.resultadoConsulta, clsStatistics.Period.SixMonthsAgo, currency)) Then
            For Each drTemp As DataRow In statistics.resultadoConsulta.Tables(0).Rows
                totalsCounter = 0
                lines.AppendLine("<tr>")
                lines.AppendLine("<td class=""cell_concept"">" & drTemp.Item("Concept") & "</td>")
                For counter = 1 To statistics.resultadoConsulta.Tables(0).Columns.Count - 1
                    '+------------------------------------
                    '| Getting totals and analyzing.
                    '+------------------------------------
                    cssClass = "cell"

                    If (statistics.resultadoConsulta.Tables(0).Columns(counter).ColumnName.StartsWith("Total-")) Then
                        lines.AppendLine("<td class=""" & cssClass & """>" & CDbl(drTemp.Item(counter)).ToString("N2") & "</td>")
                        totals(totalsCounter) = totals(totalsCounter) + CDbl(drTemp.Item(counter))
                        totalsCounter = totalsCounter + 1
                    End If
                Next
                lines.AppendLine("</tr>")
            Next
            lines.AppendLine("<tr>")
            lines.AppendLine("<td class=""cell_concept"">Totals</td>")
            For totalsCounter = 0 To 6
                lines.AppendLine("<td class=""cell_total"">" & totals(totalsCounter).ToString("N2") & " </td>")
            Next
            lines.AppendLine("</tr>")
        Else
            'Error. Imposible to get the data
            lines.AppendLine("<tr>")
            lines.AppendLine("<td class=""cell_msg"">" & statistics.informacionAdicional & " </td>")
            lines.AppendLine("</tr>")

        End If

        writer.AppendLine(lines.ToString)
        writer.AppendLine("</table>")
        writeTo.Text = writer.ToString
        Return booldev
    End Function

End Class