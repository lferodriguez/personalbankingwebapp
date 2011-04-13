Public Class clsAccountTransaction
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
    Enum transactionTypes
        MakeADepositOrWithdrawl = 1
        PayACreditCard = 2
        PayALoan = 3
        ElectronicTransfers = 4
    End Enum

    Function executeATransaction(ByVal account As Integer, _
                                 ByVal concept As Integer, ByVal sdate As String, _
                                 ByVal value As Double, _
                                 ByVal comments As String, _
                                 ByRef transactionRef As Long)
        Dim booldev As Boolean = False
        Dim spparameters As New SpParameters

        spparameters.Add("trasanctionConcept", concept, SpParameter.tipoParametro.entero)
        spparameters.Add("account", account, SpParameter.tipoParametro.entero)
        spparameters.Add("transactionDate", sdate, SpParameter.tipoParametro.cadena)
        spparameters.Add("value", value, SpParameter.tipoParametro.entero)
        spparameters.Add("webUserComments", comments, SpParameter.tipoParametro.cadena)
        booldev = _dbCon.ejecutarProcedimientoAlmacenado("addAccountTransaction", spparameters)
        _informacionAdicional = _dbCon.informacionAdicional
        If booldev Then
            _dsConsulta = _dbCon.resultadoConsulta
            If _dsConsulta.Tables(0).Rows(0).Item("accountTransaction") > 0 Then
                transactionRef = _dsConsulta.Tables(0).Rows(0).Item("accountTransaction")
                booldev = True
            Else
                booldev = False
                _informacionAdicional = "Unable to get a transaction reference id."
            End If
        End If
        Return booldev
    End Function
    Function executeACompoundTransaction(ByVal type As transactionTypes, _
                                         ByVal TransactionDate As String, _
                                         ByVal sourceAccountId As String, _
                                         ByVal destinationAccountId As String,
                                         ByVal value As Double, _
                                         ByVal exchangeRate As Double, _
                                         ByVal comments As String, _
                                         ByRef firstTransactionRef As Long, _
                                         ByRef secondTransactionRef As Long) As Boolean


        Dim booldev As Boolean = False
        Dim spparameters As New SpParameters

        Dim firstTransactionConcept As Integer = 0
        Dim secondTransactionConcept As Integer = 0
        Dim sourceAccountCurrency As Integer = 0
        Dim sourceAccount As clsAccount
        Dim sourceValue As Double
        Dim destinationAccountcurrency As Integer = 0
        Dim destinationAccount As clsAccount
        Dim destinationValue As Double

        '------------------------------
        '1. Finding Transaction Type.
        '------------------------------
        Select Case type
            Case transactionTypes.PayACreditCard
                firstTransactionConcept = mci_transactionConcept.PartialCreditCardPayment
                secondTransactionConcept = mci_transactionConcept.CreditCardPayment
            Case transactionTypes.PayALoan
                firstTransactionConcept = mci_transactionConcept.PartialLoanSubscription
                secondTransactionConcept = mci_transactionConcept.LoanSubscription
            Case transactionTypes.ElectronicTransfers
                firstTransactionConcept = mci_transactionConcept.ViaElectronicTransferWithdrawl
                secondTransactionConcept = mci_transactionConcept.ViaElectronicTransferDeposit
        End Select


        '------------------------------------
        '2. Finding values and exchange rate
        '------------------------------------
        sourceValue = value
        destinationValue = value

        sourceAccount = New clsAccount
        If (sourceAccountId > 0) Then
            sourceAccount.accountDescription(sourceAccountId)
            If sourceAccount.resultadoConsulta.Tables(0).Rows.Count > 0 Then
                sourceAccountCurrency = sourceAccount.resultadoConsulta.Tables(0).Rows(0).Item("currency")
            End If
        End If
        destinationAccount = New clsAccount
        If (destinationAccountId > 0) Then
            destinationAccount.accountDescription(destinationAccountId)
            If destinationAccount.resultadoConsulta.Tables(0).Rows.Count > 0 Then
                destinationAccountcurrency = destinationAccount.resultadoConsulta.Tables(0).Rows(0).Item("currency")
            End If
        End If

        If (sourceAccountCurrency = clsAccount.accountCurrencies.Que) And _
            destinationAccountcurrency = clsAccount.accountCurrencies.Dol Then
            'Purchase Foreign Currency
            sourceValue = value
            destinationValue = value / exchangeRate
            If type = transactionTypes.ElectronicTransfers Then
                firstTransactionConcept = mci_transactionConcept.PurchaseForeignCurrency
                secondTransactionConcept = mci_transactionConcept.ViaElectronicTransferDeposit
            End If
        End If

        If (sourceAccountCurrency = clsAccount.accountCurrencies.Dol) And _
            destinationAccountcurrency.ToString = clsAccount.accountCurrencies.Que Then
            'Sell Foreign Currency
            sourceValue = value
            destinationValue = value * exchangeRate
            If type = transactionTypes.ElectronicTransfers Then
                firstTransactionConcept = mci_transactionConcept.SellForeignCurrency
                secondTransactionConcept = mci_transactionConcept.ViaElectronicTransferDeposit
            End If

        End If

        '------------------------------------
        '3. Write on database
        '------------------------------------
        spparameters.Add("sourceAccount", sourceAccountId, SpParameter.tipoParametro.entero)
        spparameters.Add("sourceTransactionConcept", firstTransactionConcept, SpParameter.tipoParametro.entero)
        spparameters.Add("sourceValue", sourceValue, SpParameter.tipoParametro.entero)
        spparameters.Add("destinationAccount", destinationAccountId, SpParameter.tipoParametro.entero)
        spparameters.Add("destinationTransactionConcept", secondTransactionConcept, SpParameter.tipoParametro.entero)
        spparameters.Add("destinationValue", destinationValue, SpParameter.tipoParametro.entero)
        spparameters.Add("transactionDate", TransactionDate, SpParameter.tipoParametro.cadena)
        spparameters.Add("webUserComments", comments, SpParameter.tipoParametro.cadena)

        booldev = _dbCon.ejecutarProcedimientoAlmacenado("addDoubleAccountTransaction", spparameters)
        _informacionAdicional = _dbCon.informacionAdicional

        If booldev Then
            _dsConsulta = _dbCon.resultadoConsulta
            If _dsConsulta.Tables(0).Rows(0).Item("FirstID") > 0 And _
                _dsConsulta.Tables(0).Rows(0).Item("SecondID") > 0 Then

                firstTransactionRef = _dsConsulta.Tables(0).Rows(0).Item("FirstID")
                secondTransactionRef = _dsConsulta.Tables(0).Rows(0).Item("SecondID")
                booldev = True

            Else
                booldev = False
                _informacionAdicional = "Unable to get a transaction reference id."
            End If
        End If
        Return booldev
    End Function

End Class
