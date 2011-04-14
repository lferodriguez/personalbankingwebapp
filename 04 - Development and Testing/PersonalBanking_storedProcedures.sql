USE [PersonalBanking]
GO

/****** Object:  StoredProcedure [dbo].[accountBalancePerWebUser]    Script Date: 04/13/2011 18:24:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[accountBalancePerWebUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[accountBalancePerWebUser]
GO

/****** Object:  StoredProcedure [dbo].[addAccountTransaction]    Script Date: 04/13/2011 18:24:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[addAccountTransaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[addAccountTransaction]
GO

/****** Object:  StoredProcedure [dbo].[addDoubleAccountTransaction]    Script Date: 04/13/2011 18:24:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[addDoubleAccountTransaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[addDoubleAccountTransaction]
GO

USE [PersonalBanking]
GO

/****** Object:  StoredProcedure [dbo].[accountBalancePerWebUser]    Script Date: 04/13/2011 18:24:20 ******/
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

/****** Object:  StoredProcedure [dbo].[addAccountTransaction]    Script Date: 04/13/2011 18:24:20 ******/
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

/****** Object:  StoredProcedure [dbo].[addDoubleAccountTransaction]    Script Date: 04/13/2011 18:24:20 ******/
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


