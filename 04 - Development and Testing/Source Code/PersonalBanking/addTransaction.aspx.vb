Public Partial Class addTransaction
    Inherits System.Web.UI.Page

    Private Enum _AccountSelection
        ChecksAndSavings = 1
        ChecksSavingsCreditCards = 2
        CreditCards = 3
        Loans = 4
    End Enum
    Private Enum _userAction
        AddAnIncome = 1
        AddAnExpense = 2
        AddACreditCardPayment = 3
        AddALoanPayment = 4
        MakeATransfer = 5
    End Enum
    Private Enum _validationType
        IncomeAndExpenses = 1
        CreditCardsAndLoansPayments = 2
        CreditCardsAndLoansPaymentsWithExchangeRate = 3
        Transfers = 4
        TransfersWithExchangeRate = 5
    End Enum
    Private Enum _ListBoxChange
        SourceAccount = 1
        DestinationAccount = 2
    End Enum
    Private Enum _userUsedAExchangeRate
        yes = 1
        no = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Dim userAction As Integer = 0
            If (Request.QueryString("userAction") = String.Empty) Then
                mview.SetActiveView(vStep001)
            Else
                Try
                    userAction = CInt(Request.QueryString("userAction"))
                    Select Case userAction
                        Case _userAction.AddAnIncome, _userAction.AddAnExpense,
                            _userAction.MakeATransfer, _userAction.AddALoanPayment,
                            _userAction.AddACreditCardPayment
                            handlesStepOneWorkflow(userAction)
                        Case Else
                            mview.SetActiveView(vStep001)
                    End Select
                Catch ex As Exception
                    mview.SetActiveView(vStep001)
                End Try
            End If
        End If
    End Sub

    Private Sub validacionesEnCliente(ByVal validation As _validationType)
        jqueryvalidateplugin1.cargarJQueryValidatePlugin(True, True)
        Dim _jquery_actions As New HtmlGenericControl("script")
        _jquery_actions.Attributes.Add("type", "text/javascript")
        _jquery_actions.Attributes.Add("language", "javascript")

        Dim strHtml As New StringBuilder
        strHtml.AppendLine("$(document).ready(function(){")
        strHtml.AppendLine("$.metadata.setType(""attr"", ""validate"");")
        strHtml.AppendLine("$('#" & Form.ClientID & "').validate({")

        Select Case validation
            Case _validationType.IncomeAndExpenses
                strHtml.AppendLine("rules:{")
                strHtml.AppendLine(txtDate.UniqueID & ": {required:true,date:true},")
                strHtml.AppendLine(txtAmount.UniqueID & ": {required:true,number:true}")
                strHtml.AppendLine("},")
                strHtml.AppendLine("messages:{")
                strHtml.AppendLine(txtDate.UniqueID & ": {required:""Please enter a date."", date:""Please enter a valid date""},")
                strHtml.AppendLine(txtAmount.UniqueID & ": {required:""Please enter a amount"",number:""Please enter a valid number""}")
                strHtml.AppendLine("}")
                strHtml.AppendLine("});")
                strHtml.AppendLine("});")
                ddlAccounts.Attributes.Add("title", "-Please choose one account-")
                ddlAccounts.Attributes.Add("validate", "required:true")
                ddlConcept.Attributes.Add("title", "-Please choose one concept-")
                ddlConcept.Attributes.Add("validate", "required:true")

            Case _validationType.CreditCardsAndLoansPayments,
                _validationType.CreditCardsAndLoansPaymentsWithExchangeRate,
                _validationType.Transfers,
                _validationType.TransfersWithExchangeRate

                strHtml.AppendLine("rules:{")
                strHtml.AppendLine(txtDate01.UniqueID & ": {required:true,date:true},")
                strHtml.AppendLine(txtAmount01.UniqueID & ": {required:true,number:true}")
                If (validation = _validationType.CreditCardsAndLoansPaymentsWithExchangeRate) Or _
                    (validation = _validationType.TransfersWithExchangeRate) Then
                    strHtml.AppendLine("," & txtexchangeRate.UniqueID & ": {required:true,number:true}")
                End If
                strHtml.AppendLine("},")
                strHtml.AppendLine("messages:{")
                strHtml.AppendLine(txtDate01.UniqueID & ": {required:""Please enter a date."", date:""Please enter a valid date""},")
                strHtml.AppendLine(txtAmount01.UniqueID & ": {required:""Please enter a amount"",number:""Please enter a valid number""}")
                If (validation = _validationType.CreditCardsAndLoansPaymentsWithExchangeRate) Or _
                    (validation = _validationType.TransfersWithExchangeRate) Then
                    strHtml.AppendLine("," & txtexchangeRate.UniqueID & ": {required:""Please enter exchange rate"",number:""Please enter a number""}")
                End If
                strHtml.AppendLine("}")
                strHtml.AppendLine("});")
                strHtml.AppendLine("});")
                ddlDestinationAccount.Attributes.Add("title", "-Please choose one account-")
                ddlDestinationAccount.Attributes.Add("validate", "required:true")
                ddlSourceAccount.Attributes.Add("title", "-Please choose one account-")
                ddlSourceAccount.Attributes.Add("validate", "required:true")
        End Select

        _jquery_actions.InnerHtml = strHtml.ToString

        Page.Header.Controls.Add(_jquery_actions)
    End Sub

    Private Function loadUserAccounts(ByVal selection As _AccountSelection, _
                                      ByRef pageDropDownListbox As DropDownList) As String

        Dim accounts As clsAccount
        Dim strMessage As String = ""
        Dim accountsToLoad As DataSet

        accounts = New clsAccount
        accountsToLoad = New DataSet

        Select Case selection
            Case _AccountSelection.ChecksAndSavings
                accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.Checks, clsAccount.accountStates.Enabled)
                accountsToLoad = accounts.resultadoConsulta
                accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.Savings, clsAccount.accountStates.Enabled)
                accountsToLoad.Merge(accounts.resultadoConsulta)
            Case _AccountSelection.ChecksSavingsCreditCards
                accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.Checks, clsAccount.accountStates.Enabled)
                accountsToLoad = accounts.resultadoConsulta
                accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.Savings, clsAccount.accountStates.Enabled)
                accountsToLoad.Merge(accounts.resultadoConsulta)
                accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.CreditCards, clsAccount.accountStates.Enabled)
                accountsToLoad.Merge(accounts.resultadoConsulta)
            Case _AccountSelection.CreditCards
                accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.CreditCards, clsAccount.accountStates.Enabled)
                accountsToLoad = accounts.resultadoConsulta
            Case _AccountSelection.Loans
                accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.Loans, clsAccount.accountStates.Enabled)
                accountsToLoad = accounts.resultadoConsulta
        End Select
        If accountsToLoad.Tables(0).Rows.Count > 0 Then
            pageDropDownListbox.DataSource = accountsToLoad
            pageDropDownListbox.DataTextField = "accountDescription"
            pageDropDownListbox.DataValueField = "account"
            pageDropDownListbox.DataBind()        
            pageDropDownListbox.Items.Insert(0, New ListItem("-Please choose one-", ""))
        Else
            strMessage = "Logged user doesn't have any account associated."
        End If
        Return strMessage
    End Function

    Private Function loadTransactionConcepts(ByVal selection As _userAction) As String

        Dim cats As New clsCatalogs
        Dim strMessage As String = ""
        Dim boolDev As Boolean = False

        Select Case selection
            Case _userAction.AddAnIncome
                boolDev = cats.TransactionConcept(clsCatalogs.catalogTransactionConceptType.Deposit, _
                                    clsCatalogs.catalogTransactionConceptTypeFlowType.Income, _
                                    clsCatalogs.catalogTransactionConceptisConsiderableInConceptFlowType.Yes
                                    )
            Case _userAction.AddAnExpense
                boolDev = cats.TransactionConcept(clsCatalogs.catalogTransactionConceptType.Withdrawl, _
                                    clsCatalogs.catalogTransactionConceptTypeFlowType.Expense, _
                                    clsCatalogs.catalogTransactionConceptisConsiderableInConceptFlowType.Yes
                                    )
        End Select

        If (boolDev) Then
            If (cats.resultadoConsulta.Tables(0).Rows.Count > 0) Then
                ddlConcept.DataSource = Nothing
                ddlConcept.DataBind()
                ddlConcept.DataSource = cats.resultadoConsulta
                ddlConcept.DataTextField = "name"
                ddlConcept.DataValueField = "TransactionConcept"
                ddlConcept.DataBind()
                ddlConcept.Items.Insert(0, New ListItem("-Please choose one-", ""))
            Else
                strMessage = "Error. There is no records for catalogs used in this page."
            End If
        Else
            strMessage = "Unable to execute query on database. Impossible to find Transaction Concept Catalogs."
        End If
        Return strMessage

    End Function

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        Dim acct As New clsAccountTransaction
        Dim evt As New clsEvents
        Dim idTransaction As Long = 0
        Dim message As String = ""    
        If (acct.executeATransaction(ddlAccounts.SelectedValue, ddlConcept.SelectedValue, _
                                     txtDate.Text, txtAmount.Text, _
                                     txtComment.Text, idTransaction)) Then
            message = "Transaction added succesfully. Transaction reference id: " & idTransaction
            evt.addEvent(Session("sWebUserId"), idTransaction, clsCatalogs.events.Information)
        Else
            message = "Transaction aborted, due technical problems. Try again later. More:" & acct.informacionAdicional
        End If
        endPageWorkflow(message)
    End Sub

    Private Sub handlesStepOneWorkflow(ByVal userAction As Integer)
        Dim strMessage As String = ""
        Session("saddTransaction_userAction") = userAction
        Select Case userAction
            Case _userAction.AddAnExpense  'Expense
                mview.SetActiveView(vStep002)
                lblFirstMessage.Text = "Welcome, please report your expenses."
                jquerydatepicker1.cargarJqueryDatePicker()
                jquerydatepicker1.mostrarDatePickerEnTextBox(txtDate)
                validacionesEnCliente(_validationType.IncomeAndExpenses)
                strMessage = loadUserAccounts(_AccountSelection.ChecksSavingsCreditCards, ddlAccounts)
                strMessage = strMessage & loadTransactionConcepts(_userAction.AddAnExpense)
            Case _userAction.AddAnIncome  'Incomes
                lblFirstMessage.Text = "Welcome, please report your incomes."
                mview.SetActiveView(vStep002)
                jquerydatepicker1.cargarJqueryDatePicker()
                jquerydatepicker1.mostrarDatePickerEnTextBox(txtDate)
                validacionesEnCliente(_validationType.IncomeAndExpenses)
                strMessage = loadUserAccounts(_AccountSelection.ChecksAndSavings, ddlAccounts)
                strMessage = strMessage & loadTransactionConcepts(_userAction.AddAnIncome)
            Case _userAction.AddACreditCardPayment  'Credit card payments
                mview.SetActiveView(vstep003)
                lblFirstMessage.Text = "Welcome, please report your credit card payments."
                jquerydatepicker1.cargarJqueryDatePicker()
                jquerydatepicker1.mostrarDatePickerEnTextBox(txtDate01)
                validacionesEnCliente(_validationType.CreditCardsAndLoansPayments)
                strMessage = loadUserAccounts(_AccountSelection.CreditCards, ddlDestinationAccount)
                strMessage = strMessage & loadUserAccounts(_AccountSelection.ChecksAndSavings, ddlSourceAccount)
            Case _userAction.AddALoanPayment  ' Loan payments
                mview.SetActiveView(vstep003)
                lblFirstMessage.Text = "Welcome, please report your loan subscriptions."
                jquerydatepicker1.cargarJqueryDatePicker()
                jquerydatepicker1.mostrarDatePickerEnTextBox(txtDate01)
                validacionesEnCliente(_validationType.CreditCardsAndLoansPayments)
                strMessage = loadUserAccounts(_AccountSelection.Loans, ddlDestinationAccount)
                strMessage = strMessage & loadUserAccounts(_AccountSelection.ChecksAndSavings, ddlSourceAccount)
            Case _userAction.MakeATransfer
                mview.SetActiveView(vstep003)
                lblFirstMessage.Text = "Welcome, please enter the amout to transfer from one account to another."
                jquerydatepicker1.cargarJqueryDatePicker()
                jquerydatepicker1.mostrarDatePickerEnTextBox(txtDate01)
                validacionesEnCliente(_validationType.Transfers)
                strMessage = loadUserAccounts(_AccountSelection.ChecksAndSavings, ddlDestinationAccount)
                strMessage = strMessage & loadUserAccounts(_AccountSelection.ChecksAndSavings, ddlSourceAccount)
        End Select
        If (strMessage.Length > 0) Then
            endPageWorkflow(strMessage)
        End If
    End Sub

    Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNext.Click
        handlesStepOneWorkflow(rblAction.SelectedValue)
    End Sub

    Private Sub endPageWorkflow(ByVal message As String)
        Dim accounts As New clsAccount
        Dim ds As New DataSet

        mview.SetActiveView(vstep004)
        lblResultado.Text = message
        accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.Checks, clsAccount.accountStates.Enabled)
        ds = accounts.resultadoConsulta
        accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.Savings, clsAccount.accountStates.Enabled)
        ds.Merge(accounts.resultadoConsulta)
        accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.CreditCards, clsAccount.accountStates.Enabled)
        ds.Merge(accounts.resultadoConsulta)
        accounts.accountResumePerUserPerAccountTypePerState(Session("sWebUserId"), clsAccount.accountTypes.Loans, clsAccount.accountStates.Enabled)
        ds.Merge(accounts.resultadoConsulta)

        ddlAccounttoMove.DataSource = ds
        ddlAccounttoMove.DataTextField = "accountDescription"
        ddlAccounttoMove.DataValueField = "account"
        ddlAccounttoMove.DataBind()
        ddlAccounttoMove.Items.Insert(0, New ListItem("-Please choose one-", "-1"))

    End Sub

    Private Sub manageAccountsSelectIndexChanged(ByVal listBox As _ListBoxChange)
        Dim sourceAccountId As Integer = 0
        Dim sourceAccountCurrency As Integer = 0
        Dim destinationAccountId As Integer = 0
        Dim destinationAccountCurrency As Integer = 0

        Dim sourceAccount As New clsAccount
        Dim destinationAccount As New clsAccount

        sourceAccountId = IIf(ddlSourceAccount.SelectedValue = "", 0, ddlSourceAccount.SelectedValue)
        destinationAccountId = IIf(ddlDestinationAccount.SelectedValue = "", 0, ddlDestinationAccount.SelectedValue)


        If (sourceAccountId > 0) Then
            sourceAccount.accountDescription(sourceAccountId)
            If sourceAccount.resultadoConsulta.Tables(0).Rows.Count > 0 Then
                sourceAccountCurrency = sourceAccount.resultadoConsulta.Tables(0).Rows(0).Item("currency")
            End If
        End If
        If (destinationAccountId > 0) Then
            destinationAccount.accountDescription(destinationAccountId)
            If destinationAccount.resultadoConsulta.Tables(0).Rows.Count > 0 Then
                destinationAccountCurrency = destinationAccount.resultadoConsulta.Tables(0).Rows(0).Item("currency")
            End If
        End If

        lblExchangeRate.Visible = False
        txtexchangeRate.Visible = False
        txtexchangeRate.Text = ""

        Session("saddTransaction_requesteExchangeRate") = _userUsedAExchangeRate.no
        If Not (sourceAccountCurrency = destinationAccountCurrency) And _
            (sourceAccountCurrency > 0) And _
            (destinationAccountCurrency > 0) Then

            Session("saddTransaction_requesteExchangeRate") = _userUsedAExchangeRate.yes
            Select Case Session("saddTransaction_userAction")
                Case _userAction.AddACreditCardPayment  'Credit card payments
                    validacionesEnCliente(_validationType.CreditCardsAndLoansPaymentsWithExchangeRate)
                Case _userAction.AddALoanPayment  ' Loan payments
                    validacionesEnCliente(_validationType.CreditCardsAndLoansPaymentsWithExchangeRate)
                Case _userAction.MakeATransfer
                    validacionesEnCliente(_validationType.TransfersWithExchangeRate)
            End Select

            lblExchangeRate.Visible = True
            txtexchangeRate.Visible = True
            txtexchangeRate.Text = ""
        Else
            Select Case Session("saddTransaction_userAction")
                Case _userAction.MakeATransfer
                    validacionesEnCliente(_validationType.Transfers)
                Case _userAction.AddACreditCardPayment
                    validacionesEnCliente(_validationType.CreditCardsAndLoansPayments)
                Case _userAction.AddALoanPayment
                    validacionesEnCliente(_validationType.CreditCardsAndLoansPayments)
            End Select
        End If


        jquerydatepicker1.cargarJqueryDatePicker()
        jquerydatepicker1.mostrarDatePickerEnTextBox(txtDate01)

    End Sub

    Private Sub ddlSourceAccount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSourceAccount.SelectedIndexChanged
        manageAccountsSelectIndexChanged(_ListBoxChange.SourceAccount)
    End Sub

    Private Sub ddlDestinationAccount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDestinationAccount.SelectedIndexChanged
        manageAccountsSelectIndexChanged(_ListBoxChange.DestinationAccount)
    End Sub

    Protected Sub btnPayment_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPayment.Click

        Dim firstReferenceId As Long = 0
        Dim SecondReferenceId As Long = 0
        Dim sourceAccount As Integer = 0
        Dim destinationAccount As Integer = 0
        Dim message As String = ""
        Dim typeOfTransaction As Integer
        Dim accountTransers As New clsAccountTransaction
        Dim evts As New clsEvents

        sourceAccount = ddlSourceAccount.SelectedValue
        destinationAccount = ddlDestinationAccount.SelectedValue

        Select Case Session("saddTransaction_userAction")
            Case _userAction.AddACreditCardPayment
                typeOfTransaction = clsAccountTransaction.transactionTypes.PayACreditCard
            Case _userAction.AddALoanPayment
                typeOfTransaction = clsAccountTransaction.transactionTypes.PayALoan
            Case _userAction.MakeATransfer
                typeOfTransaction = clsAccountTransaction.transactionTypes.ElectronicTransfers                
        End Select
        If (accountTransers.executeACompoundTransaction(typeOfTransaction, txtDate01.Text, _
                                                         sourceAccount, destinationAccount, _
                                                          txtAmount01.Text, _
                                                          IIf(txtexchangeRate.Text = "", 0, txtexchangeRate.Text), txtComment01.Text, _
                                                         firstReferenceId, SecondReferenceId)) Then
            message = "Transaction completed successfully. A withdrawl has been processed with a Transaction Reference Id: " & firstReferenceId & _
                        " A deposit has been processed with a Transaction Reference Id: " & SecondReferenceId
            evts.addEvent(Session("sWebUserId"), firstReferenceId, SecondReferenceId, clsCatalogs.events.Information)
        Else
            message = "Transaction aborted, due technical problems. Try again later. More:" & SecondReferenceId
        End If
            endPageWorkflow(message)
    End Sub

    Protected Sub ddlAccounttoMove_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlAccounttoMove.SelectedIndexChanged
        Dim enc As New Encrypt
        If (ddlAccounttoMove.SelectedValue > 0) Then
            Server.Transfer(ResolveUrl("~/accountHistory.aspx?ref=" & enc.Encrypt_param(ddlAccounttoMove.SelectedValue)))
        End If
    End Sub
End Class