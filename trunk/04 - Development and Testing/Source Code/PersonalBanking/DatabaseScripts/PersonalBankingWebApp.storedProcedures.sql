USE [PersonalBanking]
GO

/****** Object:  StoredProcedure [dbo].[accountBalancePerWebUser]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[accountBalancePerWebUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[accountBalancePerWebUser]
GO

/****** Object:  StoredProcedure [dbo].[accountTypeBalancePerWebUser]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[accountTypeBalancePerWebUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[accountTypeBalancePerWebUser]
GO

/****** Object:  StoredProcedure [dbo].[addAccountTransaction]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[addAccountTransaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[addAccountTransaction]
GO

/****** Object:  StoredProcedure [dbo].[addDoubleAccountTransaction]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[addDoubleAccountTransaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[addDoubleAccountTransaction]
GO

/****** Object:  StoredProcedure [dbo].[addEventOneTransaction]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[addEventOneTransaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[addEventOneTransaction]
GO

/****** Object:  StoredProcedure [dbo].[addEventTwoTransactions]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[addEventTwoTransactions]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[addEventTwoTransactions]
GO

/****** Object:  StoredProcedure [dbo].[authenticateUser]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[authenticateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[authenticateUser]
GO

/****** Object:  StoredProcedure [dbo].[CreditCarDBalanceAndParametersPerWebUser]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreditCarDBalanceAndParametersPerWebUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreditCarDBalanceAndParametersPerWebUser]
GO

/****** Object:  StoredProcedure [dbo].[queryLastEvents]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[queryLastEvents]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[queryLastEvents]
GO

/****** Object:  StoredProcedure [dbo].[searchEvent]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[searchEvent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[searchEvent]
GO

/****** Object:  StoredProcedure [dbo].[stateOfIncomePerWebUserPerPeriod]    Script Date: 01/15/2012 13:39:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[stateOfIncomePerWebUserPerPeriod]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[stateOfIncomePerWebUserPerPeriod]
GO

USE [PersonalBanking]
GO

/****** Object:  StoredProcedure [dbo].[accountBalancePerWebUser]    Script Date: 01/15/2012 13:39:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[accountBalancePerWebUser]
(
	@webUser int
)
as
set nocount on
select 
deposits.number,
deposits.accountType,
deposits.symbol,
(deposits.Deposits-withdrawls.withdrawls) as Balance
from
	(
		select
			ac.account,
			ac.number, 
			actc.name as "AccountType", 
			cc.symbol,sum(at.value) as "Deposits"
		from
			account ac,
			accountTypeCatalog actc,
			currencyCatalog cc,
			accountTransaction at,
			TransactionConcept atc,
			webuseraccounts wac
		where
		ac.type = actc.type and
		ac.currency = cc.currency and
		ac.account = at.account and 
		at.TransactionConcept = atc.TransactionConcept and
		wac.account = ac.account and
		wac.webUser = @webUser and
		atc.TransactionConceptType = 1
	group by
		ac.account,ac.number, actc.name, cc.symbol
	) deposits
	inner join
		(
		select
			ac.account,
			ac.number, 
			actc.name as "AccountType", 
			cc.symbol,sum(at.value) as "withdrawls"
		from
			account ac,
			accountTypeCatalog actc,
			currencyCatalog cc,
			accountTransaction at,
			TransactionConcept atc,
			webuseraccounts wac
		where
		ac.type = actc.type and
		ac.currency = cc.currency and
		ac.account = at.account and 
		at.TransactionConcept = atc.TransactionConcept and
		wac.account = ac.account and
		wac.webUser = @webUser and
		atc.TransactionConceptType = 2
	group by
		ac.account,ac.number, actc.name, cc.symbol
	) withdrawls
on
	deposits.account = withdrawls.account



GO

/****** Object:  StoredProcedure [dbo].[accountTypeBalancePerWebUser]    Script Date: 11/09/2012 19:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



create procedure [dbo].[accountTypeBalancePerWebUser]
(@webUser as int, @AccountStatesToReview as smallint)
as
	set nocount on
		select AccountTypeDescription, currencySymbol, SUM(balance) as total from cnsAccountBalances 
		where webUser = @webUser and accountState = @AccountStatesToReview
		group by AccountTypeDescription, CurrencySymbol 

GO

/****** Object:  StoredProcedure [dbo].[addAccountTransaction]    Script Date: 01/15/2012 13:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[addAccountTransaction]
(
	@trasanctionConcept smallint,
	@account int,
	@transactionDate datetime,
	@value money,
	@webUserComments varchar(1000)
)
as
	set nocount on
		
		insert into accountTransaction
		(transactionConcept,account,transactionDate,value,timeStamp,webUserComments)
		values
		(@trasanctionConcept,@account,@transactionDate,@value,getdate(),@webUserComments)	
		if (@@error>0) 
		begin
			select -1 as accountTransaction
		end
		else
		begin
			select @@identity as accountTransaction
		end


GO

/****** Object:  StoredProcedure [dbo].[addDoubleAccountTransaction]    Script Date: 01/15/2012 13:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


create procedure [dbo].[addDoubleAccountTransaction]
(
	@sourceAccount int,
	@sourceTransactionConcept smallint,
	@sourceValue as money,
	@destinationAccount int,
	@destinationTransactionConcept smallint,	
	@destinationValue as money,
	@transactionDate datetime,	
	@webUserComments varchar(1000)
)
as
	set nocount on
		
		declare @firstTransactionId int
		declare @secondTransactionId int
		
		begin tran tranadat
		
		insert into accountTransaction
		(transactionConcept,account,transactionDate,value,timeStamp,webUserComments)
		values
		(@sourceTransactionConcept,@sourceAccount,@transactionDate,@sourcevalue,getdate(),@webUserComments)	
		
		if (@@error>0) 
		begin
			select @firstTransactionId = -1
			rollback tran tranadat			
		end
		else
		begin
			select @firstTransactionId = @@identity
				
				insert into accountTransaction
				(transactionConcept,account,transactionDate,value,timeStamp,webUserComments)
				values
				(@destinationTransactionConcept,@destinationAccount,@transactionDate,@destinationValue ,getdate(),@webUserComments)
				if (@@error>0) 
				begin
					select @secondTransactionId = -1
					rollback tran tranadat			
				end
				else
				begin
					select @secondTransactionId = @@identity
					commit tran tranadat
					select @firstTransactionId as "FirstID",@secondTransactionId  as "SecondId"
				end	
			end


GO

/****** Object:  StoredProcedure [dbo].[addEventOneTransaction]    Script Date: 01/15/2012 13:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[addEventOneTransaction]
(@pWebUser as int,
@pTransactionReference as int,
@pEventLevel as smallint,
@pSource as varchar(50)
)
AS
	declare @eventMessage varchar(2000)
	declare @name varchar(50)
	declare @transactionDate as datetime
	declare @transactionSystemDate as datetime
	declare @value as money
	declare @concept as varchar(50)
	declare @accountDescript as varchar(500)
	declare @conceptType as varchar(50)
	declare @currencySymbol as varchar(5)
	
	select 
	@name = w.firstName, 
	@transactionDate = at.transactionDate, 
	@transactionSystemDate = at.timeStamp,
	@value = at.value, 
	@concept = t.name, 
	@accountDescript = ar.accountDescription,
	@conceptType = tp.name,
	@currencySymbol = cc.symbol
	from 
	AccountTransaction at,
	webUser w,
	webUserAccounts wa,
	cnsAccountsResume ar,
	TransactionConcept t,
	transactionConceptType tp,
	currencyCatalog cc
	where
	wa.account = at.account and 
	wa.webUser = @pwebUser and
	w.webUser = wa.webUser and
	at.AccountTransaction = @pTransactionReference and
	ar.account = at.account and
	at.transactionConcept = t.TransactionConcept and
	tp.TransactionConceptType = t.TransactionConceptType and
	ar.currency = cc.currency 
	
	select @eventMessage = convert(varchar,@transactionSystemDate,101) + ': ' +@name + ' made a ' + @conceptType
							+ ' as ' + @concept 
							+ ' on: ' + convert(varchar,  @transactionDate,  101)
							+ ', value: ' + @currencySymbol + '. ' + convert(varchar, @value) + '. Account: ' + @accountDescript
							+ ' . PersonalBanking Reference: ' +  CONVERT(varchar,@pTransactionReference)
	insert into webUserEvent values (@pWebUser,@pSource,GETDATE(),@eventMessage,@pEventLevel,GETDATE())

GO

/****** Object:  StoredProcedure [dbo].[addEventTwoTransactions]    Script Date: 01/15/2012 13:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[addEventTwoTransactions]
(@pWebUser as int,
@pFirstTransactionReference as int,
@pSecondTransactionReference as int,
@pEventLevel as smallint,
@pSource as varchar(50)
)
AS
	declare @eventMessage varchar(2000)
	declare @name varchar(50)
	declare @transactionDate as datetime
	declare @transactionSystemDate as datetime
	declare @value as money
	declare @concept as varchar(50)
	declare @accountDescript as varchar(500)
	declare @conceptType as varchar(50)
	declare @currencySymbol as varchar(5)
	
	select 
	@name = w.firstName, 
	@transactionDate = at.transactionDate, 
	@transactionSystemDate = at.timeStamp,
	@value = at.value, 
	@concept = t.name, 
	@accountDescript = ar.accountDescription,
	@conceptType = tp.name,
	@currencySymbol = cc.symbol
	from 
	AccountTransaction at,
	webUser w,
	webUserAccounts wa,
	cnsAccountsResume ar,
	TransactionConcept t,
	transactionConceptType tp,
	currencyCatalog cc
	where
	wa.account = at.account and 
	wa.webUser = @pwebUser and
	w.webUser = wa.webUser and
	at.AccountTransaction = @pFirstTransactionReference and
	ar.account = at.account and
	at.transactionConcept = t.TransactionConcept and
	tp.TransactionConceptType = t.TransactionConceptType and
	ar.currency = cc.currency 
	
	select @eventMessage = convert(varchar,@transactionSystemDate,101) + ': ' +@name + ' made a ' + @conceptType
							+ ' as ' + @concept 
							+ ' on: ' + convert(varchar,  @transactionDate,  101)
							+ ', value: ' + @currencySymbol + '. ' + convert(varchar, @value) + '. Account: ' + @accountDescript
							+ ' . PersonalBanking Reference: ' +  CONVERT(varchar,@pFirstTransactionReference)
	
	select 
	@name = w.firstName, 
	@transactionDate = at.transactionDate, 
	@transactionSystemDate = at.timeStamp,
	@value = at.value, 
	@concept = t.name, 
	@accountDescript = ar.accountDescription,
	@conceptType = tp.name,
	@currencySymbol = cc.symbol
	from 
	AccountTransaction at,
	webUser w,
	webUserAccounts wa,
	cnsAccountsResume ar,
	TransactionConcept t,
	transactionConceptType tp,
	currencyCatalog cc
	where
	wa.account = at.account and 
	wa.webUser = @pwebUser and
	w.webUser = wa.webUser and
	at.AccountTransaction = @pSecondTransactionReference and
	ar.account = at.account and
	at.transactionConcept = t.TransactionConcept and
	tp.TransactionConceptType = t.TransactionConceptType and
	ar.currency = cc.currency 
	
	select @eventMessage = @eventMessage + ', and also made a ' + @conceptType
							+ ' as ' + @concept 
							+ ' on: ' + convert(varchar,  @transactionDate,  101)
							+ ', value: ' + @currencySymbol + '. ' + convert(varchar, @value) + '. Account: ' + @accountDescript
							+ ' . PersonalBanking Reference: ' +  CONVERT(varchar,@pSecondTransactionReference)
							
	insert into webUserEvent values (@pWebUser,@pSource,GETDATE(),@eventMessage,@pEventLevel,GETDATE())

GO

/****** Object:  StoredProcedure [dbo].[authenticateUser]    Script Date: 01/15/2012 13:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[authenticateUser]
(
@pEmail as varchar (150),
@pPassword as varchar(150)
)as 
	select * from webUser where email =  @pEmail and password = @pPassword
	and enabled = 'Y'

GO

/****** Object:  StoredProcedure [dbo].[CreditCarDBalanceAndParametersPerWebUser]    Script Date: 01/15/2012 13:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE procedure [dbo].[CreditCarDBalanceAndParametersPerWebUser]
(@webUser as int)
AS
select 
 ab.CurrencySymbol,ab.number,ab.BankName, ab.Balance, 
(select aa.value from accountAttributes aa where aa.account = ab.account and aa.attribute = 1) as MaxCredit,
(select aa.value from accountAttributes aa where aa.account = ab.account and aa.attribute = 2) as PayDay,
(select aa.value from accountAttributes aa where aa.account = ab.account and aa.attribute = 3) as CutDay,
(ab.Balance + (select aa.value from accountAttributes aa where aa.account = ab.account and aa.attribute = 1)) as available
from cnsAccountBalances ab
where AccountType = 3 and AccountState = 1 
and webUser = @webUser 
order by currencySymbol, BankName ASC


GO

/****** Object:  StoredProcedure [dbo].[queryLastEvents]    Script Date: 01/15/2012 13:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[queryLastEvents]
as
select top 10 webUserEvent,eventMessage  from webUserEvent 
order by eventDate DESC

GO

/****** Object:  StoredProcedure [dbo].[searchEvent]    Script Date: 01/15/2012 13:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[searchEvent]
(
	@pStartDate as datetime, 
	@pEndDate as datetime,
	@pMessage as varchar(25) = null
)
as
	if @pMessage is null
	begin
	select * from cnsEvents where eventDate between @pStartDate and @pEndDate + ' 23:59:59'
	order by eventDate ASC
	end
	else
	begin
		select * from cnsEvents where 
		eventMessage like '%' + @pMessage + '%'
		and eventDate between @pStartDate and @pEndDate + ' 23:59:59'
		order by eventDate ASC
	end 
		

GO

/****** Object:  StoredProcedure [dbo].[stateOfIncomePerWebUserPerPeriod]    Script Date: 01/15/2012 13:39:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



create procedure [dbo].[stateOfIncomePerWebUserPerPeriod]
(	@webUser as int,
	@startDate as datetime,
	@endDate as datetime
)
as 
	set nocount on
	
	select
		tcft.transactionConceptFlowType, tcft.name,cc.currency,cc.symbol, SUM(at.value) as total
	from 
		transactionConceptFlowType tcft,
		TransactionConcept tc,
		AccountTransaction at,
		account a,
		webUserAccounts wa,
		currencyCatalog cc
		where 
			at.transactionConcept = tc.TransactionConcept and
			tc.transactionConceptFlowType = tcft.transactionConceptFlowType and
			tc.isConsiderableInConceptFlowType = 'Y' and
			at.account = a.Account and 
			a.Account = wa.account and 
			a.currency = cc.currency and
			wa.webUser = @webUser and 
			at.transactionDate between @startDate and @endDate
group by 
	tcft.transactionConceptFlowType,tcft.name, cc.currency, cc.symbol


GO

