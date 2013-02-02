Public Class accountBalances
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cargarTodosLosBalances()        
    End Sub
    Sub cargarTodosLosBalances()
        cargarBalancesCuentas(clsAccount.accountTypes.Checks, clsAccount.accountCurrencies.Que)
        cargarBalancesCuentas(clsAccount.accountTypes.Savings, clsAccount.accountCurrencies.Que)
        cargarBalancesCuentas(clsAccount.accountTypes.CreditCards, clsAccount.accountCurrencies.Que)
        cargarBalancesCuentas(clsAccount.accountTypes.Loans, clsAccount.accountCurrencies.Que)
        cargarBalancesCuentas(clsAccount.accountTypes.CertificateOfDeposits, clsAccount.accountCurrencies.Que)

        cargarBalancesCuentas(clsAccount.accountTypes.Checks, clsAccount.accountCurrencies.Dol)
        cargarBalancesCuentas(clsAccount.accountTypes.Savings, clsAccount.accountCurrencies.Dol)
        cargarBalancesCuentas(clsAccount.accountTypes.CreditCards, clsAccount.accountCurrencies.Dol)
        cargarBalancesCuentas(clsAccount.accountTypes.Loans, clsAccount.accountCurrencies.Dol)
        cargarBalancesCuentas(clsAccount.accountTypes.CertificateOfDeposits, clsAccount.accountCurrencies.Dol)
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
                    Case clsAccount.accountTypes.CertificateOfDeposits
                        sb.AppendLine("<li class=""accountBalanceResume_tittle"">Certificate of Deposits</li>")

                End Select
                sb.AppendLine("<li class=""accountBalanceResume_content"">")
                sb.AppendLine("<table cellpadding=""0"" cellspacing=""0"" border=""0"" class=""accountBalance"">")
                sb.AppendLine("<tr>" & _
                              "<th width=""25%"">Account</th>" & _
                              "<th width=""25%"">Bank Name</th>" & _
                              "<th width=""25%"">Account Name</th>" & _
                              "<th> Avaible Founds</th></tr>")
                enc = New Encrypt
                For Each dr As DataRow In accts.resultadoConsulta.Tables(0).Rows
                    sb.AppendLine("<tr><td><a href=""accountHistory.aspx?ref=" & enc.Encrypt_param(dr.Item("account")) & """>")
                    sb.AppendLine(dr.Item("number"))
                    sb.AppendLine("</a></td>")
                    sb.AppendLine("<td>" & dr.Item("BankName") & "</td>")
                    sb.AppendLine("<td>" & dr.Item("AccountName") & "</td>")                    
                    itemBalance = dr.Item("balance")
                    total = total + itemBalance
                    currencySymbol = dr.Item("currencySymbol")
                    sb.AppendLine("</a></td><td>" & _
                                   currencySymbol & " " & _
                                  itemBalance.ToString("N2") & "</td></tr>")
                Next
                If (total >= 0) Then
                    sb.AppendLine("<tr><td colspan=""3""></td><td class=""highlight_center""> " & currencySymbol & " " & total.ToString("N2") & "</td></tr>")
                Else
                    sb.AppendLine("<tr><td colspan=""3""></td><td class=""highlight_center"" style=""color:#B40606;""> " & currencySymbol & " " & total.ToString("N2") & "</td></tr>")
                End If
                sb.AppendLine("</table>")
                sb.AppendLine("</li>")
                sb.AppendLine("</ul>")
                lblAccountBalances.Text = lblAccountBalances.Text & sb.ToString
            End If
        End If
    End Sub
End Class