Module modCatalogsInDataBase

    ' Objective:
    ' Here are all the values that are used in database in catalog tables. 

    Enum mci_transactionConcept
        CreditCardPayment = 54
        PartialCreditCardPayment = 15
        LoanSubscription = 53
        PartialLoanSubscription = 57
        ViaElectronicTransferDeposit = 48
        ViaElectronicTransferWithdrawl = 49
        PurchaseForeignCurrency = 35
        SellForeignCurrency = 38
    End Enum

End Module
