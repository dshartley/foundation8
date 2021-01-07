USE [f23-data]
GO

DROP TABLE [dbo].[RelativeConnectionRequests]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RelativeConnectionRequests](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FromRelativeMemberID] [nvarchar](30) NOT NULL,
	[ToRelativeMemberID] [nvarchar](30) NOT NULL,
	[RequestType] [int] NOT NULL,
	[DateActioned] [datetime] NOT NULL,
	[RequestStatus] [int] NOT NULL,
	[ApplicationID] [nvarchar](30) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_RelativeConnectionRequests] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


