USE [f23-data]
GO

DROP TABLE [dbo].[RelativeInteractions]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RelativeInteractions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FromRelativeMemberID] [nvarchar](30) NOT NULL,
	[ToRelativeMemberID] [nvarchar](30) NOT NULL,
	[InteractionType] [int] NOT NULL,
	[DateActioned] [datetime] NOT NULL,
	[InteractionStatus] [int] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[ApplicationID] [nvarchar](30) NOT NULL DEFAULT (''),
 CONSTRAINT [PK_RelativeInteractions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


