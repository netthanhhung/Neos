create table tblParamNationalite (
   NationaliteID       nvarchar(20)        not null,
   Libele   	       nvarchar(20)        null,
   constraint PK_tblParamNationalite primary key (NationaliteID)
)
go

insert into tblParamNationalite (NationaliteID, Libele) 
select distinct Nationalite, Nationalite from tblcandidat where Nationalite <> ''
go

create table tblParamLangue (
   LangueID       nvarchar(25)        not null,
   Libele   	  nvarchar(25)        null,
   constraint PK_tblParamLangue primary key (LangueID)
)
go

insert into tblParamLangue (LangueID, Libele) 
select distinct AutreLangue, AutreLangue from tblCandidatEval where AutreLangue <> ''
go

--Change the length of column CodePays of table tblCandidat to 3 char. Because the column PaysID of table tblParamPays has length = 3.
alter table tblCandidat alter column CodePays nvarchar(3) null
go
--add column createddate to table tblCandidatDocument
alter table tblCandidatDocument add CreatedDate datetime null
go
alter table tblCandidatDocument add AbsoluteURL nvarchar(255) null
go
alter table tblCandidatDocument add ContentType nvarchar(50) null
go
alter table dbo.tblSociete alter column FormeJuridique varchar(35) null
go

----------------------------------start:deployed on 13/11/2008-----------------------------------------
alter table dbo.tblParamUtilisateurs add [Password] nvarchar(255) null
go
CREATE TABLE [dbo].[tblParamPermissions](
	[PermissionCode] [nvarchar](100) NOT NULL,
	[PermissionDescription] [nvarchar](4000) NULL,
	constraint PK_tblParamPermissions primary key (PermissionCode)	
 )
go 
insert into [dbo].[tblParamPermissions] values ('VIEWALLACTIONS','Right to visualize the actions of other users')
go
insert into [dbo].[tblParamPermissions] values ('SENDPRESENTATION','Right to send presentation of a candidate to a company')
go
CREATE TABLE [dbo].[tblParamUserPermissions](
	[UserID] [nvarchar](3) NOT NULL,
	[PermissionCode] [nvarchar](100) NOT NULL
 )
go 
ALTER TABLE [dbo].[tblParamUserPermissions]  ADD  CONSTRAINT [tblParamUserPermissions_FK00] FOREIGN KEY([UserID])
	REFERENCES [dbo].[tblParamUtilisateurs] ([UserID])
go 
ALTER TABLE [dbo].[tblParamUserPermissions]  ADD  CONSTRAINT [tblParamUserPermissions_FK01] FOREIGN KEY([PermissionCode])
	REFERENCES [dbo].[tblParamPermissions] ([PermissionCode])
----------------------------------/end: deployed on 13/11/2008-----------------------------------------
----------------------------------/start: implement on 25/11/2008-----------------------------------------
alter table dbo.tblParamLocations alter column Hierarchie int null

go

CREATE TABLE [dbo].[tblNotification](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](3) NULL,
	[Type] [int] NULL,
	[Subject] [nvarchar](4000) NULL,
	[Content] [nvarchar](4000) NULL,
	[Unread] [bit] NULL,
	[RemindDate] [datetime] NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT PK_tblNotification PRIMARY KEY (MessageID asc)
)
go

-- Mail Stephane (28.11.08) : drop trigger
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('T_tblParamNiveauEtude_DTrig') AND OBJECTPROPERTY(id, 'IsTrigger') = 1)
DROP TRIGGER T_tblParamNiveauEtude_DTrig 
GO 

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('T_tblParamNiveauEtude_UTrig') AND OBJECTPROPERTY(id, 'IsTrigger') = 1)
DROP TRIGGER T_tblParamNiveauEtude_UTrig 

GO 

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('T_tblParamFormation_DTrig') AND OBJECTPROPERTY(id, 'IsTrigger') = 1)
DROP TRIGGER T_tblParamFormation_DTrig
GO 

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id('T_tblParamFormation_UTrig') AND OBJECTPROPERTY(id, 'IsTrigger') = 1)
DROP TRIGGER T_tblParamFormation_UTrig 
GO 
alter table tblCandidatDocument add Type nvarchar(10) null
go
alter table dbo.tblSocietelogo alter column logopath nvarchar(255) null
go


-- Mail Stephane (14.01.09) : Invoices : factoring.
alter table tblSocieteAdressesFacturation add FactoringCode int null
go

alter table Invoices add Factoring  bit null
go