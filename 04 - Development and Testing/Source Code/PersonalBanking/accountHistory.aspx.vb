Public Partial Class accountHistory
    Inherits System.Web.UI.Page

    Private _messages As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        _messages = ""
        If Not Me.IsPostBack Then
            loadAccountInformation()
            loadHistory(clsAccount.accountHistoryPeriod.ThisMonth)
            loadTransactionConceptTypeResume(clsAccount.accountHistoryPeriod.ThisMonth)
            loadTransactionConceptResume(clsAccount.accountHistoryPeriod.ThisMonth)
            lblMensajes.Text = _messages
        End If
    End Sub
    Sub loadAccountInformation()
        Dim acct As New clsAccount
        Dim enc As New Encrypt
        Dim currency As String = ""

        If IsNumeric(enc.Decrypt_param(Request.QueryString("ref"))) Then
            If (acct.accountDescription(enc.Decrypt_param(Request.QueryString("ref")))) Then
                If acct.resultadoConsulta.Tables(0).Rows.Count > 0 Then
                    lblAccountInformation.Text = "<ul>"
                    With acct.resultadoConsulta.Tables(0).Rows(0)
                        currency = .Item("currencySymbol")
                        lblAccountInformation.Text = lblAccountInformation.Text & _
                                                        "<li>Account Number: " & .Item("accountNumber") & " Name: " & .Item("accountName") & "</li>" & _
                                                        "<li>State: " & .Item("state") & "</li>" & _
                                                        "<li>Situation: " & .Item("situation") & "</li>" & _
                                                        "<li>Currency: " & .Item("currencySymbol") & "</li>" & _
                                                        "<li>Type: " & .Item("accountType") & "</li>" & _
                                                        "<li>Bank: " & .Item("bankName") & "</li>"
                    End With
                    acct.accountBalancePerAccount(enc.Decrypt_param(Request.QueryString("ref")))
                    With acct.resultadoConsulta.Tables(0).Rows(0)
                        lblAccountInformation.Text = lblAccountInformation.Text & "<li><strong>Balance: " & currency & " " & FormatNumber(.Item("balance"), 2) & "</strong></li>"
                    End With
                    lblAccountInformation.Text = lblAccountInformation.Text & "</ul>"

                Else
                    'Qry with no results
                    _messages = _messages & "Account Information Query return 0 results."
                End If
            Else
                ' Impossible to execute query.
                _messages = _messages & "Sorry, something went wrong. We were unable to execute the account information query. Please try again later."
            End If
        Else
            ' Url mismatch
            _messages = _messages & "Sorry, the URL you've entered is no longer avaible."
        End If
    End Sub

    Sub loadHistory(ByVal period As clsAccount.accountHistoryPeriod)
        Dim acct As New clsAccount
        Dim enc As New Encrypt
        _messages = ""

        If IsNumeric(enc.Decrypt_param(Request.QueryString("ref"))) Then
            If (acct.accountTransactionHistoryByPeriod(enc.Decrypt_param(Request.QueryString("ref")), period)) Then
                If acct.resultadoConsulta.Tables(0).Rows.Count > 0 Then
                    gdResults.DataSource = acct.resultadoConsulta
                    gdResults.DataBind()
                Else
                    'Qry with no results
                    _messages = _messages & "Transaction history query return 0 results."
                End If
            Else
                ' Impossible to execute query.
                _messages = _messages & "Sorry, something went wrong. We were unable to execute the transaction history query. Please try again later."
            End If
        Else
            ' Url mismatch
            _messages = _messages & "Sorry, the URL you've entered is no longer avaible."
        End If
    End Sub
    Sub loadTransactionConceptTypeResume(ByVal period As clsAccount.accountHistoryPeriod)
        Dim acct As New clsAccount
        Dim enc As New Encrypt
        _messages = ""


        If IsNumeric(enc.Decrypt_param(Request.QueryString("ref"))) Then
            If (acct.accountTransactionConceptTypeResumeByPeriod(enc.Decrypt_param(Request.QueryString("ref")), period)) Then
                If acct.resultadoConsulta.Tables(0).Rows.Count > 0 Then
                    gdtctresume.DataSource = acct.resultadoConsulta
                    gdtctresume.DataBind()
                Else
                    'Qry with no results
                    _messages = _messages & "<br/> Transaction Concept Type query return 0 results."
                End If
            Else
                ' Impossible to execute query.
                _messages = _messages & "<br/> Sorry, something went wrong. We were unable to execute the Transaction Concept Type query. Please try again later."
            End If
        Else
            ' Url mismatch
            _messages = _messages & "<br/> Sorry, the URL you've entered is no longer avaible."
        End If

    End Sub
    Sub loadTransactionConceptResume(ByVal period As clsAccount.accountHistoryPeriod)
        Dim acct As New clsAccount
        Dim enc As New Encrypt
        _messages = ""


        If IsNumeric(enc.Decrypt_param(Request.QueryString("ref"))) Then
            If (acct.accountTransactionConceptResumeByPeriod(enc.Decrypt_param(Request.QueryString("ref")), period)) Then
                If acct.resultadoConsulta.Tables(0).Rows.Count > 0 Then
                    gdtcresume.DataSource = acct.resultadoConsulta
                    gdtcresume.DataBind()
                Else
                    'Qry with no results
                    _messages = _messages & "<br/> Transaction Concept query return 0 results."
                End If
            Else
                ' Impossible to execute query.
                _messages = _messages & "<br/> Sorry, something went wrong. We were unable to execute the Transaction Concept query. Please try again later."
            End If
        Else
            ' Url mismatch
            _messages = _messages & "<br/> Sorry, the URL you've entered is no longer avaible."
        End If

    End Sub
    Private Sub ddlPeriods_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriods.SelectedIndexChanged
        clearDataGridsOnPage()
        loadHistory(ddlPeriods.SelectedValue)
        loadTransactionConceptTypeResume(ddlPeriods.SelectedValue)
        loadTransactionConceptResume(ddlPeriods.SelectedValue)
        lblMensajes.Text = _messages
    End Sub
    Sub clearDataGridsOnPage()
        gdResults.DataSource = Nothing
        gdtctresume.DataSource = Nothing
        gdtcresume.DataSource = Nothing
        gdResults.DataBind()
        gdtctresume.DataBind()
        gdtcresume.DataBind()
    End Sub

    Private Sub gdResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gdResults.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim checkboxesId As String = ""
            Dim enc As New Encrypt
            Dim divsToShowEffectId As String = ""
            Dim strConfirmDeleteLink As String = ""

            checkboxesId = "chk_" & gdResults.DataKeys(e.Row.RowIndex).Values(0).ToString
            divsToShowEffectId = "div_" & gdResults.DataKeys(e.Row.RowIndex).Values(0).ToString
            strConfirmDeleteLink = "<a href=""deleteTransaction.aspx?ref=" & enc.Encrypt_param(gdResults.DataKeys(e.Row.RowIndex).Values(0).ToString) & """><img src=""images/delete.png"" alt=""Delete"" border=""0""/></a>"
            e.Row.Cells(0).Text = "<table cellpadding=""0"" cellspacing=""0"" border=""0""><tr>" & _
                                  "<td>" & _
                                  "<input class=""controlCheckbox"" type=""checkbox"" id=""" & checkboxesId & """ onClick=""showDeleteOption('" & checkboxesId & "','" & divsToShowEffectId & "');""/>" & _
                                  "</td><td>" & _
                                  "<div style=""display:none;"" class=""deletePanel"" id=""" & divsToShowEffectId & """> " & _
                                    strConfirmDeleteLink & _
                                  "</div></td></tr></table>"
        End If
    End Sub
End Class