USE [PersonalBanking]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_account_accountTypeCatalog]') AND parent_object_id = OBJECT_ID(N'[dbo].[account]'))
ALTER TABLE [dbo].[account] DROP CONSTRAINT [FK_account_accountTypeCatalog]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_account_bankCatalog]') AND parent_object_id = OBJECT_ID(N'[dbo].[account]'))
ALTER TABLE [dbo].[account] DROP CONSTRAINT [FK_account_bankCatalog]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_account_currencyCatalog]') AND parent_object_id = OBJECT_ID(N'[dbo].[account]'))
ALTER TABLE [dbo].[account] DROP CONSTRAINT [FK_account_currencyCatalog]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_acctStateCat_acct]') AND parent_object_id = OBJECT_ID(N'[dbo].[account]'))
ALTER TABLE [dbo].[account] DROP CONSTRAINT [fk_acctStateCat_acct]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_bankSitCat_acct]') AND parent_object_id = OBJECT_ID(N'[dbo].[account]'))
ALTER TABLE [dbo].[account] DROP CONSTRAINT [fk_bankSitCat_acct]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_account_accountAttribute]') AND parent_object_id = OBJECT_ID(N'[dbo].[accountAttributes]'))
ALTER TABLE [dbo].[accountAttributes] DROP CONSTRAINT [fk_account_accountAttribute]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_attribute_accountAttribute]') AND parent_object_id = OBJECT_ID(N'[dbo].[accountAttributes]'))
ALTER TABLE [dbo].[accountAttributes] DROP CONSTRAINT [fk_attribute_accountAttribute]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountTransaction_account]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountTransaction]'))
ALTER TABLE [dbo].[AccountTransaction] DROP CONSTRAINT [FK_AccountTransaction_account]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_AccountTransaction_TransactionConcept]') AND parent_object_id = OBJECT_ID(N'[dbo].[AccountTransaction]'))
ALTER TABLE [dbo].[AccountTransaction] DROP CONSTRAINT [FK_AccountTransaction_TransactionConcept]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_transactionConcept_transactionConceptFlowType]') AND parent_object_id = OBJECT_ID(N'[dbo].[TransactionConcept]'))
ALTER TABLE [dbo].[TransactionConcept] DROP CONSTRAINT [fk_transactionConcept_transactionConceptFlowType]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TransactionConcept_TransactionConceptType]') AND parent_object_id = OBJECT_ID(N'[dbo].[TransactionConcept]'))
ALTER TABLE [dbo].[TransactionConcept] DROP CONSTRAINT [FK_TransactionConcept_TransactionConceptType]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_webUserAccounts_account]') AND parent_object_id = OBJECT_ID(N'[dbo].[webUserAccounts]'))
ALTER TABLE [dbo].[webUserAccounts] DROP CONSTRAINT [FK_webUserAccounts_account]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_webUserAccounts_webUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[webUserAccounts]'))
ALTER TABLE [dbo].[webUserAccounts] DROP CONSTRAINT [FK_webUserAccounts_webUser]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_webUserEvent_eventLevCatalog]') AND parent_object_id = OBJECT_ID(N'[dbo].[webUserEvent]'))
ALTER TABLE [dbo].[webUserEvent] DROP CONSTRAINT [fk_webUserEvent_eventLevCatalog]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[fk_webUserEvent_webUser]') AND parent_object_id = OBJECT_ID(N'[dbo].[webUserEvent]'))
ALTER TABLE [dbo].[webUserEvent] DROP CONSTRAINT [fk_webUserEvent_webUser]
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[account]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[account]') AND type in (N'U'))
DROP TABLE [dbo].[account]
GO

/****** Object:  Table [dbo].[accountAttributes]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[accountAttributes]') AND type in (N'U'))
DROP TABLE [dbo].[accountAttributes]
GO

/****** Object:  Table [dbo].[accountBankSituationCatalog]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[accountBankSituationCatalog]') AND type in (N'U'))
DROP TABLE [dbo].[accountBankSituationCatalog]
GO

/****** Object:  Table [dbo].[accountStateCatalog]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[accountStateCatalog]') AND type in (N'U'))
DROP TABLE [dbo].[accountStateCatalog]
GO

/****** Object:  Table [dbo].[AccountTransaction]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AccountTransaction]') AND type in (N'U'))
DROP TABLE [dbo].[AccountTransaction]
GO

/****** Object:  Table [dbo].[accountTypeCatalog]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[accountTypeCatalog]') AND type in (N'U'))
DROP TABLE [dbo].[accountTypeCatalog]
GO

/****** Object:  Table [dbo].[attributes]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[attributes]') AND type in (N'U'))
DROP TABLE [dbo].[attributes]
GO

/****** Object:  Table [dbo].[bankCatalog]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[bankCatalog]') AND type in (N'U'))
DROP TABLE [dbo].[bankCatalog]
GO

/****** Object:  Table [dbo].[currencyCatalog]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[currencyCatalog]') AND type in (N'U'))
DROP TABLE [dbo].[currencyCatalog]
GO

/****** Object:  Table [dbo].[EventLevelCatalog]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EventLevelCatalog]') AND type in (N'U'))
DROP TABLE [dbo].[EventLevelCatalog]
GO

/****** Object:  Table [dbo].[TransactionConcept]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransactionConcept]') AND type in (N'U'))
DROP TABLE [dbo].[TransactionConcept]
GO

/****** Object:  Table [dbo].[transactionConceptFlowType]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[transactionConceptFlowType]') AND type in (N'U'))
DROP TABLE [dbo].[transactionConceptFlowType]
GO

/****** Object:  Table [dbo].[TransactionConceptType]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TransactionConceptType]') AND type in (N'U'))
DROP TABLE [dbo].[TransactionConceptType]
GO

/****** Object:  Table [dbo].[webUser]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[webUser]') AND type in (N'U'))
DROP TABLE [dbo].[webUser]
GO

/****** Object:  Table [dbo].[webUserAccounts]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[webUserAccounts]') AND type in (N'U'))
DROP TABLE [dbo].[webUserAccounts]
GO

/****** Object:  Table [dbo].[webUserEvent]    Script Date: 01/15/2012 13:37:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[webUserEvent]') AND type in (N'U'))
DROP TABLE [dbo].[webUserEvent]
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[account]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[account](
	[Account] [int] IDENTITY(1,1) NOT NULL,
	[number] [varchar](20) NOT NULL,
	[bank] [smallint] NOT NULL,
	[currency] [smallint] NOT NULL,
	[type] [smallint] NOT NULL,
	[timestamp] [datetime] NOT NULL,
	[name] [varchar](50) NULL,
	[accountState] [smallint] NULL,
	[accountBankSituation] [smallint] NULL,
 CONSTRAINT [PK_account] PRIMARY KEY CLUSTERED 
(
	[Account] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[accountAttributes]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[accountAttributes](
	[attribute] [smallint] NOT NULL,
	[account] [int] NOT NULL,
	[value] [varchar](50) NOT NULL,
	[timeStamp] [datetime] NOT NULL,
 CONSTRAINT [pk_account_attribute] PRIMARY KEY CLUSTERED 
(
	[attribute] ASC,
	[account] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[accountBankSituationCatalog]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[accountBankSituationCatalog](
	[accountBankSituation] [smallint] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[timeStamp] [datetime] NOT NULL,
 CONSTRAINT [PK_bankSituationsCatalog] PRIMARY KEY CLUSTERED 
(
	[accountBankSituation] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[accountStateCatalog]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[accountStateCatalog](
	[accountState] [smallint] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[timeStamp] [datetime] NOT NULL,
 CONSTRAINT [PK_accountStateCatalog] PRIMARY KEY CLUSTERED 
(
	[accountState] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[AccountTransaction]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AccountTransaction](
	[AccountTransaction] [int] IDENTITY(1,1) NOT NULL,
	[transactionConcept] [smallint] NOT NULL,
	[account] [int] NOT NULL,
	[transactionDate] [datetime] NOT NULL,
	[value] [money] NOT NULL,
	[timeStamp] [datetime] NOT NULL,
	[webUserComments] [varchar](1000) NULL,
 CONSTRAINT [PK_AccountTransaction] PRIMARY KEY CLUSTERED 
(
	[AccountTransaction] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[accountTypeCatalog]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[accountTypeCatalog](
	[type] [smallint] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_accountTypeCatalog] PRIMARY KEY CLUSTERED 
(
	[type] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[attributes]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[attributes](
	[attribute] [smallint] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[timeStamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[attribute] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[bankCatalog]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[bankCatalog](
	[bank] [smallint] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[timeStamp] [datetime] NOT NULL,
 CONSTRAINT [PK_bankCatalog] PRIMARY KEY CLUSTERED 
(
	[bank] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[currencyCatalog]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[currencyCatalog](
	[currency] [smallint] NOT NULL,
	[symbol] [varchar](5) NOT NULL,
	[name] [varchar](25) NOT NULL,
	[timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_currencyCatalog] PRIMARY KEY CLUSTERED 
(
	[currency] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[EventLevelCatalog]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EventLevelCatalog](
	[levelId] [smallint] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[recordDate] [datetime] NOT NULL,
 CONSTRAINT [PK_EventLevelCatalog] PRIMARY KEY CLUSTERED 
(
	[levelId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[TransactionConcept]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TransactionConcept](
	[TransactionConcept] [smallint] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[TransactionConceptType] [smallint] NOT NULL,
	[timeStamp] [datetime] NOT NULL,
	[transactionConceptFlowType] [smallint] NULL,
	[isConsiderableInConceptFlowType] [varchar](1) NULL,
 CONSTRAINT [PK_TransactionConcept] PRIMARY KEY CLUSTERED 
(
	[TransactionConcept] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[transactionConceptFlowType]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[transactionConceptFlowType](
	[transactionConceptFlowType] [smallint] NOT NULL,
	[name] [varchar](25) NOT NULL,
	[modified] [datetime] NOT NULL,
 CONSTRAINT [PK_transactionConceptFlowType\] PRIMARY KEY CLUSTERED 
(
	[transactionConceptFlowType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[TransactionConceptType]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TransactionConceptType](
	[TransactionConceptType] [smallint] NOT NULL,
	[name] [varchar](25) NOT NULL,
	[timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_TransactionConceptType] PRIMARY KEY CLUSTERED 
(
	[TransactionConceptType] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[webUser]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[webUser](
	[webUser] [int] NOT NULL,
	[email] [varchar](150) NOT NULL,
	[password] [varchar](150) NOT NULL,
	[firstName] [varchar](50) NOT NULL,
	[lastName] [varchar](50) NOT NULL,
	[enabled] [char](1) NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[timeStamp] [datetime] NOT NULL,
 CONSTRAINT [PK_webUser] PRIMARY KEY CLUSTERED 
(
	[webUser] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[webUserAccounts]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[webUserAccounts](
	[webUser] [int] NOT NULL,
	[account] [int] NOT NULL,
	[timeStamp] [datetime] NOT NULL,
 CONSTRAINT [PK_webUserAccounts] PRIMARY KEY CLUSTERED 
(
	[webUser] ASC,
	[account] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [PersonalBanking]
GO

/****** Object:  Table [dbo].[webUserEvent]    Script Date: 01/15/2012 13:37:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[webUserEvent](
	[webUserEvent] [int] IDENTITY(1,1) NOT NULL,
	[webUser] [int] NOT NULL,
	[source] [varchar](50) NOT NULL,
	[eventDate] [datetime] NOT NULL,
	[eventMessage] [varchar](5000) NOT NULL,
	[levelId] [smallint] NOT NULL,
	[recordDate] [datetime] NOT NULL,
 CONSTRAINT [PK_webUserEvent] PRIMARY KEY CLUSTERED 
(
	[webUserEvent] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[account]  WITH CHECK ADD  CONSTRAINT [FK_account_accountTypeCatalog] FOREIGN KEY([type])
REFERENCES [dbo].[accountTypeCatalog] ([type])
GO

ALTER TABLE [dbo].[account] CHECK CONSTRAINT [FK_account_accountTypeCatalog]
GO

ALTER TABLE [dbo].[account]  WITH CHECK ADD  CONSTRAINT [FK_account_bankCatalog] FOREIGN KEY([bank])
REFERENCES [dbo].[bankCatalog] ([bank])
GO

ALTER TABLE [dbo].[account] CHECK CONSTRAINT [FK_account_bankCatalog]
GO

ALTER TABLE [dbo].[account]  WITH CHECK ADD  CONSTRAINT [FK_account_currencyCatalog] FOREIGN KEY([currency])
REFERENCES [dbo].[currencyCatalog] ([currency])
GO

ALTER TABLE [dbo].[account] CHECK CONSTRAINT [FK_account_currencyCatalog]
GO

ALTER TABLE [dbo].[account]  WITH CHECK ADD  CONSTRAINT [fk_acctStateCat_acct] FOREIGN KEY([accountState])
REFERENCES [dbo].[accountStateCatalog] ([accountState])
GO

ALTER TABLE [dbo].[account] CHECK CONSTRAINT [fk_acctStateCat_acct]
GO

ALTER TABLE [dbo].[account]  WITH CHECK ADD  CONSTRAINT [fk_bankSitCat_acct] FOREIGN KEY([accountBankSituation])
REFERENCES [dbo].[accountBankSituationCatalog] ([accountBankSituation])
GO

ALTER TABLE [dbo].[account] CHECK CONSTRAINT [fk_bankSitCat_acct]
GO

ALTER TABLE [dbo].[accountAttributes]  WITH CHECK ADD  CONSTRAINT [fk_account_accountAttribute] FOREIGN KEY([account])
REFERENCES [dbo].[account] ([Account])
GO

ALTER TABLE [dbo].[accountAttributes] CHECK CONSTRAINT [fk_account_accountAttribute]
GO

ALTER TABLE [dbo].[accountAttributes]  WITH CHECK ADD  CONSTRAINT [fk_attribute_accountAttribute] FOREIGN KEY([attribute])
REFERENCES [dbo].[attributes] ([attribute])
GO

ALTER TABLE [dbo].[accountAttributes] CHECK CONSTRAINT [fk_attribute_accountAttribute]
GO

ALTER TABLE [dbo].[AccountTransaction]  WITH CHECK ADD  CONSTRAINT [FK_AccountTransaction_account] FOREIGN KEY([account])
REFERENCES [dbo].[account] ([Account])
GO

ALTER TABLE [dbo].[AccountTransaction] CHECK CONSTRAINT [FK_AccountTransaction_account]
GO

ALTER TABLE [dbo].[AccountTransaction]  WITH CHECK ADD  CONSTRAINT [FK_AccountTransaction_TransactionConcept] FOREIGN KEY([transactionConcept])
REFERENCES [dbo].[TransactionConcept] ([TransactionConcept])
GO

ALTER TABLE [dbo].[AccountTransaction] CHECK CONSTRAINT [FK_AccountTransaction_TransactionConcept]
GO

ALTER TABLE [dbo].[TransactionConcept]  WITH CHECK ADD  CONSTRAINT [fk_transactionConcept_transactionConceptFlowType] FOREIGN KEY([transactionConceptFlowType])
REFERENCES [dbo].[transactionConceptFlowType] ([transactionConceptFlowType])
GO

ALTER TABLE [dbo].[TransactionConcept] CHECK CONSTRAINT [fk_transactionConcept_transactionConceptFlowType]
GO

ALTER TABLE [dbo].[TransactionConcept]  WITH CHECK ADD  CONSTRAINT [FK_TransactionConcept_TransactionConceptType] FOREIGN KEY([TransactionConceptType])
REFERENCES [dbo].[TransactionConceptType] ([TransactionConceptType])
GO

ALTER TABLE [dbo].[TransactionConcept] CHECK CONSTRAINT [FK_TransactionConcept_TransactionConceptType]
GO

ALTER TABLE [dbo].[webUserAccounts]  WITH CHECK ADD  CONSTRAINT [FK_webUserAccounts_account] FOREIGN KEY([account])
REFERENCES [dbo].[account] ([Account])
GO

ALTER TABLE [dbo].[webUserAccounts] CHECK CONSTRAINT [FK_webUserAccounts_account]
GO

ALTER TABLE [dbo].[webUserAccounts]  WITH CHECK ADD  CONSTRAINT [FK_webUserAccounts_webUser] FOREIGN KEY([webUser])
REFERENCES [dbo].[webUser] ([webUser])
GO

ALTER TABLE [dbo].[webUserAccounts] CHECK CONSTRAINT [FK_webUserAccounts_webUser]
GO

ALTER TABLE [dbo].[webUserEvent]  WITH CHECK ADD  CONSTRAINT [fk_webUserEvent_eventLevCatalog] FOREIGN KEY([levelId])
REFERENCES [dbo].[EventLevelCatalog] ([levelId])
GO

ALTER TABLE [dbo].[webUserEvent] CHECK CONSTRAINT [fk_webUserEvent_eventLevCatalog]
GO

ALTER TABLE [dbo].[webUserEvent]  WITH CHECK ADD  CONSTRAINT [fk_webUserEvent_webUser] FOREIGN KEY([webUser])
REFERENCES [dbo].[webUser] ([webUser])
GO

ALTER TABLE [dbo].[webUserEvent] CHECK CONSTRAINT [fk_webUserEvent_webUser]
GO


