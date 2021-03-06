
/****** Object:  Table [dbo].[tblSocieteDocument]    Script Date: 04/14/2009 14:58:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSocieteDocument](
	[DocumentID] [int] IDENTITY(1,1) NOT NULL,
	[SocieteID] [int] NULL,
	[NomDoc] [nvarchar](150) NULL,
	[CheminDoc] [nvarchar](150) NULL,
	[CreatedDate] [datetime] NULL,
	[AbsoluteURL] [nvarchar](255) NULL,
	[ContentType] [nvarchar](50) NULL,
	[Type] [nvarchar](10) NULL
) ON [PRIMARY]
go 

alter table dbo.tblSocieteAdressesFacturation alter column FactoringCode nvarchar(1000) null
go
