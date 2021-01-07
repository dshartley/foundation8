USE [f23-data]
GO

DROP TABLE [dbo].[RelativeConnections]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RelativeConnections](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FromRelativeMemberID] [nvarchar](30) NOT NULL,
	[ToRelativeMemberID] [nvarchar](30) NOT NULL,
	[ConnectionContractType] [int] NOT NULL,
	[DateActioned] [datetime] NOT NULL,
	[DateLastActive] [datetime] NOT NULL DEFAULT ('1/1/1900'),
	[ConnectionStatus] [int] NOT NULL DEFAULT ((0)),
	[ApplicationID] [nvarchar](30) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_RelativeConnections] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


