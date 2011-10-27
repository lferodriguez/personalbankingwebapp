/*
Updates October 25, 2011
*/
/* 1. Fix Transaction Concepts */
update TransactionConcept set isConsiderableInConceptFlowType = 'N' where TransactionConcept = 22
update TransactionConcept set transactionConceptFlowType = 2 where TransactionConcept = 55
/* 1. Add Parameters for account*/
create table attributes(
	attribute smallint primary key,
	name varchar(50) not null,	
	timeStamp datetime not null
)
create table accountAttributes(
	attribute smallint not null,
	account int not null,
	value varchar(50) not null,
	timeStamp datetime not null,
	constraint pk_account_attribute primary key (attribute,account),
	constraint fk_account_accountAttribute foreign key (account) references account(account),
	constraint fk_attribute_accountAttribute foreign key (attribute) references attributes(attribute)
)
insert into attributes values (1,'CREDIT AMOUNT',GETDATE());
insert into attributes values (2,'PAYMENT DAY',GETDATE());
insert into attributes values (3,'CUT DAY',GETDATE());
insert into accountAttributes values (1,16,'10500.00',GETDATE());
insert into accountAttributes values (2,16,'15',GETDATE());
insert into accountAttributes values (3,16,'21',GETDATE());
insert into accountAttributes values (1,19,'7000.00',GETDATE());
insert into accountAttributes values (2,19,'1',GETDATE());
insert into accountAttributes values (3,19,'6',GETDATE());
GO

create procedure accountTypeBalancePerWebUser
(@webUser as int)
as
	set nocount on
		select AccountTypeDescription, currencySymbol, SUM(balance) as total from cnsAccountBalances 
		where webUser = @webUser
		group by AccountTypeDescription, CurrencySymbol 
go

create procedure stateOfIncomePerWebUserPerPeriod
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
go

CREATE procedure CreditCarDBalanceAndParametersPerWebUser
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
go
