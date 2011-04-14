USE [PersonalBanking]
GO

/****** Object:  View [dbo].[cnsAccountBalances]    Script Date: 04/13/2011 18:23:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[cnsAccountBalances]'))
DROP VIEW [dbo].[cnsAccountBalances]
GO

/****** Object:  View [dbo].[cnsAccountsResume]    Script Date: 04/13/2011 18:23:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[cnsAccountsResume]'))
DROP VIEW [dbo].[cnsAccountsResume]
GO

/****** Object:  View [dbo].[cnsAccountTransactionsPerUser]    Script Date: 04/13/2011 18:23:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[cnsAccountTransactionsPerUser]'))
DROP VIEW [dbo].[cnsAccountTransactionsPerUser]
GO

/****** Object:  View [dbo].[cnsAccountTransactionsPerUserUSDollars]    Script Date: 04/13/2011 18:23:49 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[cnsAccountTransactionsPerUserUSDollars]'))
DROP VIEW [dbo].[cnsAccountTransactionsPerUserUSDollars]
GO

USE [PersonalBanking]
GO

/****** Object:  View [dbo].[cnsAccountBalances]    Script Date: 04/13/2011 18:23:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[cnsAccountBalances] as 
select
	Deposits.Account,
	Deposits.AccountName,
	Deposits.AccountType,
	deposits.number,
	Deposits.webUser, 
	Deposits.AccountTypeDescription,
	Deposits.BankName,
	deposits.currency, 
	Deposits.CurrencySymbol,
	Deposits.AccountState,
	Deposits.AccountStateName,
	Deposits.AccountBankSituation,
	 Deposits.AccountBankSituationName,
	(Deposits.Amount - Withdrawls.Amount) as "Balance"
from 		
		(select
			ac.account,
			ac.number,
			ac.name as "AccountName",
			bc.name as "BankName", 
			actc.type as "AccountType",
			actc.name as "AccountTypeDescription",
			tct.TransactionConceptType,
			tct.name as "TransactionConceptTypeDescription",
			cc.currency,
			cc.symbol as "CurrencySymbol",
			wua.webUser as "webUser",
			accSc.accountState,
			accSc.name as "AccountStateName",
			absc.accountBankSituation,
			absc.name as "AccountBankSituationName",
			sum(at.value) as "Amount"
		from
			account ac,
			accountTypeCatalog actc,
			currencyCatalog cc,
			accountTransaction at,
			TransactionConcept tc,
			TransactionConceptType tct,
			bankCatalog bc,
			webUserAccounts wua,
			accountStateCatalog accSc,
			accountBankSituationCatalog absc
		where
			ac.bank = bc.bank and
			ac.type = actc.type and
			ac.currency = cc.currency and
			ac.account = at.account and 
			ac.Account = wua.account and
			at.TransactionConcept = tc.TransactionConcept and
			tc.TransactionConceptType= tct.TransactionConceptType and
			tc.TransactionConceptType = 1 and 
			ac.accountState = accSc.accountState and
			ac.accountBankSituation = absc.accountBankSituation		
		group by
			ac.account,ac.number,bc.name, actc.type, actc.name, ac.name,
			cc.symbol,tct.TransactionConceptType,tct.name,wua.webUser,cc.currency
			, accsc.accountState, accSc.name, absc.accountBankSituation, absc.name 
		) as Deposits,
		(select
			ac.account,
			ac.number,
			ac.name as "AccountName",
			bc.name as "BankName", 
			actc.type as "AccountType",
			actc.name as "AccountTypeDescription",
			tct.TransactionConceptType,
			tct.name as "TransactionConceptTypeDescription",
			cc.currency,
			cc.symbol as "CurrencySymbol",
			wua.webUser as "webUser",
			accSc.accountState,
			accSc.name as "AccountStateName",
			absc.accountBankSituation,
			absc.name as "AccountBankSituationName",
			sum(at.value) as "Amount"
		from
			account ac,
			accountTypeCatalog actc,
			currencyCatalog cc,
			accountTransaction at,
			TransactionConcept tc,
			TransactionConceptType tct,
			bankCatalog bc,
			webUserAccounts wua,
			accountStateCatalog accSc,
			accountBankSituationCatalog absc
		where
			ac.bank = bc.bank and
			ac.type = actc.type and
			ac.currency = cc.currency and
			ac.account = at.account and 
			ac.Account = wua.account and
			at.TransactionConcept = tc.TransactionConcept and
			tc.TransactionConceptType= tct.TransactionConceptType and
			tc.TransactionConceptType = 2 and 
			ac.accountState = accSc.accountState and
			ac.accountBankSituation = absc.accountBankSituation
		group by
			ac.account,ac.number,bc.name, actc.type, actc.name, ac.name,
			cc.symbol,tct.TransactionConceptType,tct.name,wua.webUser,cc.currency
			, accsc.accountState, accSc.name, absc.accountBankSituation, absc.name 
		) as Withdrawls
where
	Deposits.Account = Withdrawls.Account 



GO

/****** Object:  View [dbo].[cnsAccountsResume]    Script Date: 04/13/2011 18:23:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE view [dbo].[cnsAccountsResume] as
select 
	a.account,
	a.type,
	a.currency,	
	wua.webUser,
	a.accountState,
	a.accountbanksituation,  
	bc.name + ' (' + a.number + ' ' + a.name + ') (' + c.symbol + ') (' + atc.name  + ')' as accountDescription	
	from 
		account a, 
		webUserAccounts wua, 
		bankCatalog bc, 
		currencyCatalog c, 
		accountTypeCatalog atc
	where 
		a.Account = wua.account and
		bc.bank = a.bank and c.currency = a.currency and atc.type = a.type 


GO

/****** Object:  View [dbo].[cnsAccountTransactionsPerUser]    Script Date: 04/13/2011 18:23:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[cnsAccountTransactionsPerUser] as
select 
	wu.lastname + ',' + wu.firstname as Cliente,
	Year(tra.transactionDate) as TransactionYear,
	month(tra.transactionDate) as TransactionMonth,
	day(tra.transactionDate) as TransactionDay,
	tra.transactionDate,
	acc.number as AccountNumber,
	at.name as AccountType,
	cur.symbol as Currency,
	bk.name as Bank,
	tc.name as Concept,
	tcft.name as FlowType,
	tc.isConsiderableInConceptFlowType,
	case tct.transactionConceptType 
		when 1 then tra.value
		else 0.00
	end as Deposits,
	case tct.transactionConceptType
		when 2 then tra.value
		else 0.00
	end as Withdrawls
from 
	accountTransaction tra,
	account acc,
	accountTypeCatalog at,
	bankCatalog bk,
	TransactionConcept tc,
	TransactionConceptType tct,
	currencyCatalog cur,
	webUserAccounts wua,
	webUser wu,
	transactionConceptFlowType tcft
where 
tra.account = acc.account and
acc.currency = cur.currency and
acc.currency  = 1 and
acc.type = at.type and
acc.bank = bk.bank and
tra.transactionConcept = tc.transactionConcept and
tc.transactionConceptType = tct.transactionConceptType and
wua.account = acc.account and
wua.webuser = wu.webuser and
tc.transactionConceptFlowType = tcft.transactionConceptFlowType

GO

/****** Object:  View [dbo].[cnsAccountTransactionsPerUserUSDollars]    Script Date: 04/13/2011 18:23:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[cnsAccountTransactionsPerUserUSDollars] as
select 
	wu.lastname + ',' + wu.firstname as Cliente,
	Year(tra.transactionDate) as TransactionYear,
	month(tra.transactionDate) as TransactionMonth,
	day(tra.transactionDate) as TransactionDay,
	tra.transactionDate,
	acc.number as AccountNumber,
	at.name as AccountType,
	cur.symbol as Currency,
	bk.name as Bank,
	tc.name as Concept,
	tcft.name as FlowType,
	tc.isConsiderableInConceptFlowType,
	case tct.transactionConceptType 
		when 1 then tra.value
		else 0.00
	end as Deposits,
	case tct.transactionConceptType
		when 2 then tra.value
		else 0.00
	end as Withdrawls
from 
	accountTransaction tra,
	account acc,
	accountTypeCatalog at,
	bankCatalog bk,
	TransactionConcept tc,
	TransactionConceptType tct,
	currencyCatalog cur,
	webUserAccounts wua,
	webUser wu,
	transactionConceptFlowType tcft
where 
tra.account = acc.account and
acc.currency = cur.currency and
acc.currency  = 2 and
acc.type = at.type and
acc.bank = bk.bank and
tra.transactionConcept = tc.transactionConcept and
tc.transactionConceptType = tct.transactionConceptType and
wua.account = acc.account and
wua.webuser = wu.webuser and
tc.transactionConceptFlowType = tcft.transactionConceptFlowType

GO


