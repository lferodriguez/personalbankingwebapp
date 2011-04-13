Public Partial Class main
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cargarTodosLosBalances()
        loadResumes()
    End Sub
    Sub cargarTodosLosBalances()
        cargarBalancesCuentas(clsAccount.accountTypes.Checks, clsAccount.accountCurrencies.Que)
        cargarBalancesCuentas(clsAccount.accountTypes.Savings, clsAccount.accountCurrencies.Que)
        cargarBalancesCuentas(clsAccount.accountTypes.CreditCards, clsAccount.accountCurrencies.Que)
        cargarBalancesCuentas(clsAccount.accountTypes.Loans, clsAccount.accountCurrencies.Que)

        cargarBalancesCuentas(clsAccount.accountTypes.Checks, clsAccount.accountCurrencies.Dol)
        cargarBalancesCuentas(clsAccount.accountTypes.Savings, clsAccount.accountCurrencies.Dol)
        cargarBalancesCuentas(clsAccount.accountTypes.CreditCards, clsAccount.accountCurrencies.Dol)
        cargarBalancesCuentas(clsAccount.accountTypes.Loans, clsAccount.accountCurrencies.Dol)
    End Sub
    Sub loadResumes()

        Dim dbconnection As New clsDbWrapper
        Dim strQry As String = ""
        Dim writer As New StringBuilder
        strQry = "select investings.CurrencySymbol, ToInvest, ToPay, (ToInvest + ToPay) as Total " & _
                    "from  " & _
                    "(select CurrencySymbol, SUM(balance) as ToInvest from cnsAccountBalances  " & _
                    " where AccountState = 1 and AccountType in (" & clsAccount.accountTypes.Checks & "," & clsAccount.accountTypes.Savings & " )  " & _
                    "group by CurrencySymbol) as Investings inner join  " & _
                    " (select CurrencySymbol, SUM(balance) as ToPay from cnsAccountBalances  " & _
                    "where AccountState = 1 and AccountType in (" & clsAccount.accountTypes.CreditCards & "," & clsAccount.accountTypes.Loans & " )  " & _
                    " group by CurrencySymbol) as payments " & _
                    " on Investings.CurrencySymbol = payments.CurrencySymbol  "

        If dbconnection.ejecutarInstruccionSQL(strQry, False) Then
            If dbconnection.resultadoConsulta.Tables(0).Rows.Count > 0 Then
                writer.AppendLine("<table cellpadding = ""0"" cellspacing=""0"" border=""0"" class=""homeResume"">")
                writer.AppendLine("<tr>")                
                writer.AppendLine("<th>To Invest</th>")
                writer.AppendLine("<th>To pay</th><th>Total</th>")
                writer.AppendLine("</tr>")
                For Each dr As DataRow In dbconnection.resultadoConsulta.Tables(0).Rows
                    writer.AppendLine("<tr>")
                    writer.AppendLine("<td>" & dr.Item("CurrencySymbol") & " " & CDbl(dr.Item("ToInvest")).ToString("N2") & "</td>")
                    writer.AppendLine("<td>" & dr.Item("CurrencySymbol") & " " & CDbl(dr.Item("ToPay")).ToString("N2") & "</td>")
                    If CDbl(dr.Item("Total")) > 0 Then
                        writer.AppendLine("<td class=""highlight"">" & dr.Item("CurrencySymbol") & " " & CDbl(dr.Item("Total")).ToString("N2") & "</td>")
                    Else
                        writer.AppendLine("<td class=""highlightRed"">" & dr.Item("CurrencySymbol") & " " & CDbl(dr.Item("Total")).ToString("N2") & "</td>")
                    End If
                    writer.AppendLine("</tr>")
                Next
                writer.AppendLine("</table>")
                lblResume.Text = writer.ToString()
            Else
                lblResume.Text = "Querying the account balances resume without any results."
            End If
        Else
            lblResume.Text = "Imposible to get a resume of account balances from database"
        End If
    End Sub
    Sub cargarBalancesCuentas(ByVal tipoCuenta As clsAccount.accountTypes, ByVal moneda As clsAccount.accountCurrencies)
        Dim accts As New clsAccount
        Dim sb As StringBuilder
        Dim itemBalance As Double = 0
        Dim total As Double = 0
        Dim currencySymbol As String = ""
        Dim enc As Encrypt

        sb = New StringBuilder
        If (accts.accountBalancePerUserPerAccountTypePerCurrencyPerAccountState(Session("sWebUserId"), _
                                                                 tipoCuenta, _
                                                                 moneda, clsAccount.accountStates.Enabled)) Then
            If (accts.resultadoConsulta.Tables(0).Rows.Count > 0) Then
                sb.AppendLine("<ul class=""accountBalanceResume"">")
                Select Case tipoCuenta
                    Case clsAccount.accountTypes.Checks
                        sb.AppendLine("<li class=""accountBalanceResume_tittle"">Checks</li>")
                    Case clsAccount.accountTypes.Savings
                        sb.AppendLine("<li class=""accountBalanceResume_tittle"">Savings</li>")
                    Case clsAccount.accountTypes.CreditCards
                        sb.AppendLine("<li class=""accountBalanceResume_tittle"">Credit Cards</li>")
                    Case clsAccount.accountTypes.Loans
                        sb.AppendLine("<li class=""accountBalanceResume_tittle"">Loans</li>")
                End Select
                sb.AppendLine("<li class=""accountBalanceResume_content"">")
                sb.AppendLine("<table cellpadding=""0"" cellspacing=""0"" border=""0"" class=""accountBalance"">")
                sb.AppendLine("<tr>" & _
                              "<th width=""15%"">Account</th><th width=""20%""> Bank Name</th>" & _
                              "<th width=""25%"">Name</th><th width=""10%"">State</th> " & _
                              "<th width=""10%"">Bank Situation</th><th width=""20%"">Avaible Founds</th></tr>")
                enc = New Encrypt
                For Each dr As DataRow In accts.resultadoConsulta.Tables(0).Rows
                    sb.AppendLine("<tr><td><a href=""accountHistory.aspx?ref=" & enc.Encrypt_param(dr.Item("account")) & """>")
                    sb.AppendLine(dr.Item("number"))
                    sb.AppendLine("</a></td>")
                    sb.AppendLine("<td>" & dr.Item("BankName") & "</td>")
                    sb.AppendLine("<td>" & dr.Item("AccountName") & "</td>")
                    sb.AppendLine("<td>" & dr.Item("AccountStateName") & "</td>")
                    sb.AppendLine("<td>" & dr.Item("AccountBankSituationName") & "</td>")
                    itemBalance = dr.Item("balance")
                    total = total + itemBalance
                    currencySymbol = dr.Item("currencySymbol")
                    sb.AppendLine("</a></td><td>" & _
                                   currencySymbol & " " & _
                                  itemBalance.ToString("N2") & "</td></tr>")
                Next
                If (total >= 0) Then
                    sb.AppendLine("<tr><td colspan=""5""></td><td class=""highlight_center""> " & currencySymbol & " " & total.ToString("N2") & "</td></tr>")
                Else
                    sb.AppendLine("<tr><td colspan=""5""></td><td class=""highlight_center"" style=""color:#B40606;""> " & currencySymbol & " " & total.ToString("N2") & "</td></tr>")
                End If
                sb.AppendLine("</table>")
                sb.AppendLine("</li>")
                sb.AppendLine("</ul>")
                lblAccountBalances.Text = lblAccountBalances.Text & sb.ToString
            End If
        End If
    End Sub
End Class