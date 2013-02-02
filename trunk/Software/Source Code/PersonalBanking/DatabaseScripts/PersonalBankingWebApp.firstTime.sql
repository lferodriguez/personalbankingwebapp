/* First Time Script */
use PersonalBanking 
/*Required Catalogs*/
insert into accountBankSituationCatalog values (1,'ACTIVATED',GETDATE());
insert into accountBankSituationCatalog values (2,'CANCELED',GETDATE());
insert into accountBankSituationCatalog values (3,'BLOCKED',GETDATE());
insert into accountStateCatalog values (1,'ENABLED',GETDATE());
insert into accountStateCatalog values (2,'DISABLED',GETDATE());
insert into accountTypeCatalog values (1,'CHECKS',GETDATE());
insert into accountTypeCatalog values (2,'SAVINGS',GETDATE());
insert into accountTypeCatalog values (3,'CREDIT CARD',GETDATE());
insert into accountTypeCatalog values (4,'LOAN',GETDATE());
insert into accountTypeCatalog values (5,'CERTIFICATE OF DEPOSITS',GETDATE());
insert into TransactionConceptType values (1,'DEPOSITS',GETDATE());
insert into TransactionConceptType values (2,'WITHDRAWALS',GETDATE());
insert into transactionConceptFlowType values (1,'INCOME',GETDATE());
insert into transactionConceptFlowType values (2,'EXPENSE',GETDATE());
insert into transactionConceptFlowType values (3,'NONE',GETDATE());
insert into TransactionConcept values (53,'LOAN SUBSCRIPTION',1	, getdate(),2,'N'); -- LOAN FEE
insert into TransactionConcept values (54,'CREDIT CARD PAYMENT',1	, getdate(),2,'N'); -- TO CREDIT CARD
insert into TransactionConcept values (15,'CREDIT CARD PARTIAL PAYMENT',2, getdate(),2,'N'); -- TO CREDIT CARD IN ACCOUNT
insert into TransactionConcept values (57,'PARTIAL LOAN SUBSCRIPTION',2	, getdate(),2,'Y'); -- LOAN FEE IN ACCOUNT
insert into TransactionConcept values (48,'VIA ELECTRONIC TRANSFER DEPOSIT',1	, getdate(),3,'N');
insert into TransactionConcept values (49,'VIA ELECTRONIC TRANSFER WITHDRAWL',2	, getdate(),3,'N');
insert into TransactionConcept values (35,'PURCHASE OF FOREIGN CURRENCY',2	, getdate(),	2,'N');
insert into TransactionConcept values (38,'SELL OF FOREIGN CURRENCY',2	, getdate(),1,'N');
insert into TransactionConcept values (22,'BANK LOAN',2, getdate(),2,'N'); -- LOAN
insert into attributes values (1,'CREDIT AMOUNT',GETDATE());
insert into attributes values (2,'PAYMENT DAY',GETDATE());
insert into attributes values (3,'CUT DAY',GETDATE());
insert into attributes values (4,'EXPIRATION DATE',GETDATE())
insert into currencyCatalog values (1,'Q','QUETZALES',GETDATE());
insert into currencyCatalog values (2,'US $','US DOLLARS', GETDATE());
insert into EventLevelCatalog values (1,'INFORMATION',GETDATE());
insert into EventLevelCatalog values (2,'WARNING',GETDATE());
insert into EventLevelCatalog values (3,'ERROR',GETDATE());
/*Custom Data for User*/
insert into bankCatalog values (1,'TEST BANK',GETDATE());
insert into transactionConcept values (1,'PERSONAL INCOME',1,GETDATE(),1,'Y');
insert into transactionConcept values (2,'HOUSEKEEPING SERVICES',2,GETDATE(),2,'Y');
insert into transactionConcept values (3,'MOBILE PHONE',2,GETDATE(),2,'Y');
insert into webUser values (1,'tests@tests.com','3773053717633237266390375763','Jhon','Connor','Y',GETDATE(),GETDATE()); -- test
insert into account values ('0000000001',1,1,1,getdate(),'CONNOR, JHON',1,1);
insert into account values ('0000000002',1,2,1,getdate(),'CONNOR, JHON',1,1);
insert into account values ('0000000003',1,1,2,getdate(),'CONNOR, JHON',1,1);
insert into account values ('1234123412341234',1,1,3,getdate(),'CONNOR, JHON',1,1);
insert into account values ('1234123412341234',1,2,3,getdate(),'CONNOR, JHON',1,1);
insert into account values ('123412341234',1,1,4,getdate(),'CONNOR, JHON',1,1);
insert into account values ('123456789',1,1,5,GETDATE(),'CONNOR, JHON',1,1);
insert into webUserAccounts values (1,1,GETDATE());
insert into webUserAccounts values (1,2,GETDATE());
insert into webUserAccounts values (1,3,GETDATE());
insert into webUserAccounts values (1,4,GETDATE());
insert into webUserAccounts values (1,5,GETDATE());
insert into webUserAccounts values (1,6,GETDATE());
insert into webUserAccounts values (1,7,GETDATE());
insert into accountAttributes values (1,4,10500.00, getdate());
insert into accountAttributes values (2,4,15, getdate());
insert into accountAttributes values (3,4,7, getdate());
insert into accountAttributes values (4,7,'2012/07/28',GETDATE()); -- Format Date YYYY/mm/dd
insert into accountAttributes values (4,4,'2012/07/28',GETDATE()); -- Format Date YYYY/mm/dd
/*Demo of Transaction*/		
insert into AccountTransaction values (1,1,GETDATE(),100.00,getdate(),'DEMO TRANSACTION 1 FOR CHECKS ACCOUNT');
insert into AccountTransaction values (3,1,GETDATE(),100.00,getdate(),'DEMO TRANSACTION 1 FOR CHECKS ACCOUNT');
insert into AccountTransaction values (1,2,GETDATE(),100.01,getdate(),'DEMO TRANSACTION 1 FOR CHECKS ACCOUNT US $');
insert into AccountTransaction values (3,2,GETDATE(),100.01,getdate(),'DEMO TRANSACTION 1 FOR CHECKS ACCOUNT US $');
insert into AccountTransaction values (1,3,GETDATE(),100.02,getdate(),'DEMO TRANSACTION 1 FOR SAVINGS ACCOUNT ');
insert into AccountTransaction values (3,3,GETDATE(),99.50,getdate(),'DEMO TRANSACTION 1 FOR SAVINGS ACCOUNT ');
insert into AccountTransaction values (3,4,GETDATE(),100.03,getdate(),'DEMO TRANSACTION 1 FOR CREDIT CARD ');
insert into AccountTransaction values (54,4,GETDATE(),0.00,getdate(),'DEMO TRANSACTION 1 FOR CREDIT CARD ');
insert into AccountTransaction values (3,5,GETDATE(),100.04,getdate(),'DEMO TRANSACTION 1 FOR CREDIT CARD ');
insert into AccountTransaction values (54,5,GETDATE(),0.00,getdate(),'DEMO TRANSACTION 1 FOR CREDIT CARD ');
insert into AccountTransaction values (22,6,GETDATE(),10000.55,getdate(),'DEMO TRANSACTION 1 FOR A BANK LOAN OPPENING ');
insert into AccountTransaction values (53,6,GETDATE(),0.00,getdate(),'DEMO TRANSACTION 1 FOR A BANK LOAN OPPENING ');
